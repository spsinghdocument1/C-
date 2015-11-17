using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using NNanomsg;
using NNanomsg.Protocols;

namespace Client
{
    public static class Test_PubSub
    {
        const string InprocAddress = "inproc://127.0.0.1:6519";
        const int DataSize = TestConstants.DataSize, BufferSize = 1024 * 4, Iter = TestConstants.Iterations * 10;
        public static void Execute()
        {
            Console.WriteLine("Executing pubsub test ");
            int receiveCount = 0;
            var clientThread = new Thread(
                () =>
                {
                    var subscriber = new SubscribeSocket();
                    subscriber.Connect(InprocAddress);
                    subscriber.Subscribe("TestMessage");

                    byte[] streamOutput = new byte[BufferSize];
                    while (true)
                    {
                        var sw = Stopwatch.StartNew();
                        for (int i = 0; i < Iter; i++)
                        {
                            int read = 0;
                            streamOutput = subscriber.Receive();
                            read = streamOutput.Length;
                            //using (var stream = subscriber.ReceiveStream())
                            //    while (stream.Length != stream.Position)
                            //    {
                            //        read += stream.Read(streamOutput, 0, streamOutput.Length);
                            //        var message = Encoding.ASCII.GetString(streamOutput, 0, read);
                            //        Trace.Assert(message.StartsWith("TestMessage"));
                            //        
                            //        break;
                            //    }

                            ++receiveCount;
                        }
                        sw.Stop();
                        var secondsPerSend = sw.Elapsed.TotalSeconds / (double)Iter;
                        Console.WriteLine("Time {0} us, {1} per second, {2} mb/s ",
                            (int)(secondsPerSend * 1000d * 1000d),
                            (int)(1d / secondsPerSend),
                            (int)(DataSize * 2d / (1024d * 1024d * secondsPerSend)));
                    }
                });
            clientThread.Start();


            {
                var publisher = new PublishSocket();
                publisher.Bind(InprocAddress);
                Thread.Sleep(100);
                var sw = Stopwatch.StartNew();
                int sendCount = 0;
                var text = "TestMessage" + new string('q', 10);
                var data = Encoding.ASCII.GetBytes(text);
                while (sw.Elapsed.TotalSeconds < 10)
                {
                    publisher.Send(data);
                    ++sendCount;
                }
                Thread.Sleep(100);
                clientThread.Abort();

                Console.WriteLine("Send count {0} receive count {1}", sendCount, receiveCount);
                publisher.Dispose();
            }
        }
    }

    class Test_Listener
    {
        public static void Execute()
        {
            Console.WriteLine("Executing Listener test");

            const string inprocAddress = "tcp://127.0.0.1:6522";
            const string unusedAddress = "tcp://127.0.0.1:6521";

            byte[] buffer1;
            byte[] buffer2;

            var clientThread = new Thread(
                () =>
                {
                    var req1 = NN.Socket(Domain.SP, Protocol.REQ);
                    NN.Connect(req1, unusedAddress);
                    var req = NN.Socket(Domain.SP, Protocol.REQ);
                    NN.Connect(req, inprocAddress);
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                    NN.Send(req, BitConverter.GetBytes((int)42), SendRecvFlags.NONE);
                    NN.Recv(req, out buffer1, SendRecvFlags.NONE);
                    Debug.Assert(BitConverter.ToInt32(buffer1, 0) == 77);
                    Console.WriteLine("Response: " + BitConverter.ToInt32(buffer1, 0));
                });
            clientThread.Start();

            var unused = NN.Socket(Domain.SP, Protocol.REP);
            NN.Bind(unused, unusedAddress);
            var rep = NN.Socket(Domain.SP, Protocol.REP);
            NN.Bind(rep, inprocAddress);

            var listener = new NanomsgListener();
            listener.AddSocket(unused);
            listener.AddSocket(rep);
            listener.ReceivedMessage += delegate(int s)
            {
                NN.Recv(s, out buffer2, SendRecvFlags.NONE);
                Console.WriteLine("Message: " + BitConverter.ToInt32(buffer2, 0));
                NN.Send(s, BitConverter.GetBytes((int)77), SendRecvFlags.NONE);
            };

            while (true)
            {
                listener.Listen(TimeSpan.FromMinutes(30));
            }
        }
    }

    class Test_PushPull
    {

        static byte[] _clientData, _serverData;
        const string InprocAddress = "inproc://pushpull_test", InprocAddressReverse = "inproc://pushpull_test_reverse";
       const int DataSize = TestConstants.DataSize, BufferSize = 1024 * 4, Iter = TestConstants.Iterations;

        public static void Execute()
        {
            Console.WriteLine("Executing PushPull test");


            _clientData = new byte[DataSize];
            _serverData = new byte[DataSize];
            var r = new Random();
            r.NextBytes(_clientData);
            r.NextBytes(_serverData);


            var clientThread = new Thread(
                () =>
                {
                    var req = new PushSocket();
                    req.Connect(InprocAddress);

                    var revreq = new PullSocket();
                    revreq.Bind(InprocAddressReverse);
                   

                    byte[] streamOutput = new byte[BufferSize];
                    while (true)
                    {
                        var sw = Stopwatch.StartNew();
                        for (int i = 0; i < Iter; i++)
                        {
                            var result = req.SendImmediate(_clientData);
                            Trace.Assert(result);
                            int read = 0;
                            using (var stream = revreq.ReceiveStream())
                                while (stream.Length != stream.Position)
                                    read += stream.Read(streamOutput, 0, streamOutput.Length);
                            Trace.Assert(read == _serverData.Length);
                        }
                        sw.Stop();
                        var secondsPerSend = sw.Elapsed.TotalSeconds / (double)Iter;
                        Console.WriteLine("PushPull Time {0} us, {1} per second, {2} mb/s ",
                            (int)(secondsPerSend * 1000d * 1000d),
                            (int)(1d / secondsPerSend),
                            (int)(DataSize * 2d / (1024d * 1024d * secondsPerSend)));
                    }
                });
            clientThread.Start();

            {
                var rep = new PullSocket();
                rep.Bind(InprocAddress);

                var revrep = new PushSocket();
                revrep.Connect(InprocAddressReverse);

                byte[] streamOutput = new byte[BufferSize];

                var sw = Stopwatch.StartNew();
                while (sw.Elapsed.TotalSeconds < 10)
                {
                    int read = 0;
                    using (var stream = rep.ReceiveStream())
                        while (stream.Length != stream.Position)
                            read += stream.Read(streamOutput, 0, streamOutput.Length);
                    revrep.SendImmediate(_serverData);
                }

                clientThread.Abort();
            }

        }
    }

    public static class TestConstants
    {
        public const int
            Iterations = 10000,
            DataSize = 4 * 1024;//1024 * 100;

    }
}
