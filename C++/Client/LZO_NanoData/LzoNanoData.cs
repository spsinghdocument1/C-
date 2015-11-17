using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Structure;
//using ZeroMQ;
using NNanomsg.Protocols;


namespace Client.LZO_NanoData
{
    class LzoNanoData
    {
          private static readonly LzoNanoData instance = new LzoNanoData("tcp://" + Global.Instance.LZOIP + ":" + Global.Instance.LZOPORT);
        public static LzoNanoData Instance
        {
            get
            {
                return instance;
            }
        }
        public CancellationTokenSource cts = new CancellationTokenSource();
        private List<Int64> _iSubscribe = new List<Int64>();
        struct sendData
        {
            public int Token, Bid1, Ask1, LTP;

        }
        
        public event EventHandler<ReadOnlyEventArgs<SYSTEMSTATUS>> OnStatusChange;
        internal event EventHandler<ReadOnlyEventArgs<string>> OnDataStatusChange;

        public event EventHandler<ReadOnlyEventArgs<INTERACTIVE_ONLY_MBP>> OnDataChange;
        public event EventHandler<ReadOnlyEventArgs<MS_SPD_MKT_INFO_7211>> OnSpreadDataChange;
        public INTERACTIVE_ONLY_MBP Data;
        public MS_SPD_MKT_INFO_7211 SpreadData_7211;
     
        private int _countdata = 0;
        private int _countolddatadata = 0;
        int BufferSize = 1024;
        SubscribeSocket subscriber = null;
        private string DATAAddress;
        public LzoNanoData(string DataAddress)
        {
            DATAAddress = DataAddress;
            subscriber = new SubscribeSocket();
            subscriber.Options.ReconnectInterval = new TimeSpan(0, 0, 1);
            subscriber.Connect(DataAddress);
            Console.WriteLine("NANOMQ UDP_Reciever Start DataAddress: " + DataAddress);
        }
        System.Timers.Timer timerforchecklogin;
        private void timers_datacheck()
        {
            timerforchecklogin = new System.Timers.Timer();
            timerforchecklogin.Interval = 30000;
            timerforchecklogin.Start();
            //  timerforchecklogin.Elapsed += timerforchecklogin_Elapsed;
        }
        void timerforchecklogin_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (_countolddatadata >= _countdata)
                {
                    this.OnDataStatusChange.Raise(OnDataStatusChange, OnDataStatusChange.CreateReadOnlyArgs("STOP"));

                }
                else
                {
                    this.OnDataStatusChange.Raise(OnDataStatusChange, OnDataStatusChange.CreateReadOnlyArgs("START"));
                    _countolddatadata = _countdata;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }


        ~LzoNanoData()
        {

            _iSubscribe.Clear();
            cts.Cancel();
            //subscriber.Disconnect (DATAAddress);
            if (subscriber != null)
            {
                subscriber.Dispose();
            }

        }
        internal Int64 Subscribe
        {
            //	get { return mSeqNumber; }

            set
            {
                if (!_iSubscribe.Contains(value))
                {
                    _iSubscribe.Add(value);
                    if (subscriber != null)
                        subscriber.Subscribe(BitConverter.GetBytes(value));
                }

            }
        }



        internal Int32 UnSubscribe
        {
            //	get { return mSeqNumber; }

            set
            {
                if (_iSubscribe.Contains(value))
                {
                    _iSubscribe.Remove(value);
                    if (subscriber != null)
                        subscriber.Unsubscribe(BitConverter.GetBytes(value));
                }

            }
        }



        internal void UDPReciever()
        {
            
            try
            {
                timerforchecklogin = new System.Timers.Timer();
                timerforchecklogin.Interval = 30000;
                timerforchecklogin.Start();
                timerforchecklogin.Elapsed += timerforchecklogin_Elapsed;
                Task.Factory.StartNew(() =>
                {
                
                    while (true)
                    {

                        //		string address = subscriber.Receive (Encoding.Unicode);
                       //	    byte[] buffer = new byte[512];
                      //	    int bufferSize = subscriber.Receive (buffer);
                        
                        var buffer = subscriber.Receive();
                        if (buffer == null)
                        {
                            this.OnDataStatusChange.Raise(OnDataStatusChange, OnDataStatusChange.CreateReadOnlyArgs("STOP"));
                            continue;
                        }
                        //Data = (INTERACTIVE_ONLY_MBP)DataPacket.RawDeserialize(buffer.Skip(4).ToArray(), typeof(INTERACTIVE_ONLY_MBP));
                        long TokenName = BitConverter.ToInt64(buffer, 0);

                        if (Global.Instance.Data_With_Nano.ContainsKey(TokenName))
                        {
                            switch(Global.Instance.Data_With_Nano[TokenName])
                            { 
                                case ClassType.MARKETWTCH:

                                Data = (INTERACTIVE_ONLY_MBP)DataPacket.RawDeserialize(buffer.Skip(8).ToArray(), typeof(INTERACTIVE_ONLY_MBP));
                                OnDataChange.Raise(OnDataChange, OnDataChange.CreateReadOnlyArgs(Data));
                                    break;

                                case ClassType.SPREAD:

                                    SpreadData_7211 = (MS_SPD_MKT_INFO_7211)DataPacket.RawDeserialize(buffer.Skip(8).ToArray(), typeof(MS_SPD_MKT_INFO_7211));
                                    OnSpreadDataChange.Raise(OnSpreadDataChange, OnSpreadDataChange.CreateReadOnlyArgs(SpreadData_7211));
                                    break;
                            }
                        }
                        
                    }
                });
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("Cancellation invoked");
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Some unexpected exception ");
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Raised " + Ex.StackTrace);
            }
        }
    }
}
