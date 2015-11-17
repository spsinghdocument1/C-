

//////////////////////////////////				NANOMQ   		///////////////////////////////////

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


namespace Client
{
    class UDP_Reciever
    {
        private static readonly UDP_Reciever instance = new UDP_Reciever("tcp://" + Global.Instance.DataConIp + ":" + Global.Instance.DataConSUBPort);
        public static UDP_Reciever Instance
        {
            get
            {
                return instance;
            }
        }
        public CancellationTokenSource cts = new CancellationTokenSource();
        private List<int> _iSubscribe = new List<int>();
        struct sendData
        {
            public int Token, Bid1, Ask1, LTP;

        }
        public event EventHandler<ReadOnlyEventArgs<FinalPrice>> OnDataArrived;
        public event EventHandler<ReadOnlyEventArgs<SYSTEMSTATUS>> OnStatusChange;
        internal event EventHandler<ReadOnlyEventArgs<string>> OnDataStatusChange;
     
        private int _countdata = 0;
        private int _countolddatadata = 0;
        int BufferSize = 16;
        SubscribeSocket subscriber = null;
        private string DATAAddress;
        public UDP_Reciever(string DataAddress)
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


        ~UDP_Reciever()
        {

            _iSubscribe.Clear();
            cts.Cancel();
            //subscriber.Disconnect (DATAAddress);
            if (subscriber != null)
            {
                subscriber.Dispose();
            }

        }
        internal Int32 Subscribe
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

                        int Token = BitConverter.ToInt32(buffer, 0);

                        FinalPrice _obj = (FinalPrice)DataPacket.RawDeserialize(buffer.Skip(4).Take(buffer.Length - 4).ToArray(), typeof(FinalPrice));



                        //			Console.WriteLine("Received");
                        //		if(_obj.Token==37454)
                        //			Console.Title="Token: "+_obj.Token+", Bid: "+_obj.MAXBID+", Ask: "+_obj.MINASK+" LTP: "+_obj.LTP;
                        //		else// if (_obj.Token==66039)
                        //		Console.WriteLine("Token {0} Bid {1} Ask {2} LTP {3}",_obj.Token,_obj.MAXBID,_obj.MINASK,_obj.LTP);

                        if (_obj.Token == 111)
                        {
                            Console.WriteLine(" SomeThing Wrong in DATA Server");
                            this.OnDataStatusChange.Raise(OnDataStatusChange, OnDataStatusChange.CreateReadOnlyArgs("START"));
                            //	OnDataError.Invoke();
                            continue;
                        }

                        if (_iSubscribe.Contains(_obj.Token))
                        {
                            this.OnDataStatusChange.Raise(OnDataStatusChange, OnDataStatusChange.CreateReadOnlyArgs("START"));
                            OnDataArrived.Raise(OnDataArrived, OnDataArrived.CreateReadOnlyArgs(_obj));
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


//////////////////////////////////				LZO   		///////////////////////////////////


 
/*
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


namespace Client
{
    class UDP_Reciever
    {
        private static readonly UDP_Reciever instance = new UDP_Reciever();
        public static UDP_Reciever Instance
        {
            get
            {
                return instance;
            }
        }
        private int _countdata = 0;
        private int _countolddatadata = 0;
       private  CancellationTokenSource cts=new CancellationTokenSource();

		private List<int> _iSubscribe = new List<int> ();
        public event EventHandler<ReadOnlyEventArgs<SYSTEMSTATUS>> OnStatusChange;
        internal event EventHandler<ReadOnlyEventArgs<string>> OnDataStatusChange;
      
		struct sendData
		{
			public int Token,Bid1,Ask1,LTP;
		}

        internal event EventHandler<ReadOnlyEventArgs<FinalPrice>> OnDataArrived;

        int BufferSize=16;


		~UDP_Reciever()
		{
			_iSubscribe.Clear ();
			cts.Cancel ();
		}
		internal Int32 Subscribe
		{
			//	get { return mSeqNumber; }

			set
			{
				if (!_iSubscribe.Contains (value)) 
				{
					_iSubscribe.Add (value);
				}

			}
		}

        System.Timers.Timer timerforchecklogin;
        private  void timers_datacheck()
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

        internal void UDPReciever(String LanIP ="127.0.0.1",string McastIp ="239.70.70.21",int port=5050)  //10821)   //  internal void UDPReciever(String LanIP = "127.0.0.1", string McastIp = "233.1.2.5", int port = 34330)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.ExclusiveAddressUse = false;
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, Convert.ToInt32(port));

            s.Bind(ipep);

            IPAddress ip = IPAddress.Parse(McastIp);

            s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip, IPAddress.Parse(LanIP)));

            byte[] r_req = new byte[BufferSize];

            try
            {
                
                timerforchecklogin = new System.Timers.Timer();
                timerforchecklogin.Interval = 30000;
                timerforchecklogin.Start();
                timerforchecklogin.Elapsed += timerforchecklogin_Elapsed;
                this.OnDataStatusChange.Raise(OnDataStatusChange, OnDataStatusChange.CreateReadOnlyArgs("STOP"));
                //Task.Run(() =>
                    Task.Factory.StartNew(()=> 
                {
                   
                   
                        while (!cts.IsCancellationRequested)
                        {
                            int size = s.Receive(r_req);
                            if (size==0)
                            {
                                this.OnDataStatusChange.Raise(OnDataStatusChange, OnDataStatusChange.CreateReadOnlyArgs("STOP"));
                            }
                           
                            if (size > 0)
                            {
                                if (_countdata==1)
                                {
                                    this.OnDataStatusChange.Raise(OnDataStatusChange, OnDataStatusChange.CreateReadOnlyArgs("START"));
                                }
                                ++_countdata;
							FinalPrice  _obj = (FinalPrice)DataPacket.RawDeserialize (r_req, typeof(FinalPrice));	
							//Console.WriteLine("Token {0} Bid {1} Ask {2} LTP {3}",_obj.Token,_obj.MAXBID,_obj.MINASK,_obj.LTP);

								if(_iSubscribe.Contains(_obj.Token))
							   	{
                                    OnDataArrived.Raise(OnDataArrived, OnDataArrived.CreateReadOnlyArgs(_obj));
								}
                            }

                            cts.Token.ThrowIfCancellationRequested();
                        }
                       
                    
                }, cts.Token);
                
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("Cancellation invoked");
            }
            catch (AggregateException e)
            {
                if (e.InnerException is OperationCanceledException)
                {

                    if (s != null)
                        if (s.Connected)
                        {
                            s.Shutdown(SocketShutdown.Both);
                            s.Close();
                        }

                    
                }
                else
                {
                    Console.WriteLine("Some unexpected exception ");
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Raised " + Ex.Message);
            }
            

        }
   }
}
*/
