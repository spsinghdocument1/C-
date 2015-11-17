using System;
//using ZeroMQ;
using System.Text;

using Structure;
//using Packet;
using System.Net;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using NNanomsg.Protocols;
using System.Data;


namespace Client
{
   
    public class NNFHandler
    {

//////////
       

        private PublishSocket _publishSocket = null;

        private long _CID = 0;
        private short _STGID = 0;
        private CancellationTokenSource _ctsCancellationTokenSource;
        private bool _AmIServer = false;

        public RequestSocket _requestSocket = null;
        public ReplySocket _replySocket = null;

        public SubscribeSocket _subscribeSocket = null;
        public SubscribeSocket _dataSubscribeSocket = null;
        public event EventHandler<ReadOnlyEventArgs<byte[]>> OnLogin;
        public event EventHandler<ReadOnlyEventArgs<byte[]>> OnHeartBeat;
        public event EventHandler<ReadOnlyEventArgs<byte[]>> OnOrderUpdate;
        public event EventHandler<ReadOnlyEventArgs<byte[]>> OnStartRequest;
        public event EventHandler<ReadOnlyEventArgs<byte[]>> OnStopRequest;
        public event EventHandler<ReadOnlyEventArgs<byte[]>> OnTraderUpdate;
        public event EventHandler<ReadOnlyEventArgs<byte[]>> OnPreviewUpdate;
        public event EventHandler<ReadOnlyEventArgs<byte[]>> OnNewOrder;

        
        ////////////

            private static readonly NNFHandler _instance = new NNFHandler();
            public static NNFHandler Instance
            {
                get
                {
                    return _instance;
                }
            }

            internal event EventHandler<ReadOnlyEventArgs<SYSTEMSTATUS>> OnStatusChange;
            internal event EventHandler<ReadOnlyEventArgs<HeartBeatInfo>> OnStatusChangeHeartBeatInfo;

            

            

        static bool SignInstatus;
       // static ZmqContext context;
      //  public static ZmqSocket PubNNF;
       // public static ZmqSocket SubNNF;
        public delegate void dll(short TransactionCode, byte[] buffer);
        public static event dll dllevent;
       

        PublishSocket publisher = null;
        SubscribeSocket subscriber = null;

        public int cout = 0;
      


        public delegate void RaiseEventDelegate(byte[] buffer);

        public delegate void RaiseEventStatusDelegate(string Value);

        public static event RaiseEventDelegate eOrderORDER_CONFIRMATION_TR;
        public static event RaiseEventDelegate eOrderORDER_ERROR_TR;
        public static event RaiseEventDelegate eOrderORDER_MOD_REJECT_TR;
        public static event RaiseEventDelegate eOrderORDER_CANCEL_REJECT_TR;
        public static event RaiseEventDelegate eOrderORDER_MOD_CONFIRMATION_TR;
        public static event RaiseEventDelegate eOrderORDER_CXL_CONFIRMATION_TR;
        public static event RaiseEventDelegate eOrderPRICE_CONFIRMATION_TR;
        
        public static event RaiseEventDelegate eOrderTRADE_CONFIRMATION_TR;
        public static event RaiseEventDelegate eOrderORDER_ERROR_OUT;
        public static event RaiseEventDelegate eOrderPRICE_CONFIRMATION;
        public static event RaiseEventDelegate eOrderORDER_CONFIRMATION_OUT;
        public static event RaiseEventDelegate eOrderFREEZE_TO_CONTROL;
        public static event RaiseEventDelegate eOrderORDER_MOD_CONFIRM_OUT;
        public static event RaiseEventDelegate eOrderORDER_MOD_REJ_OUT;
        public static event RaiseEventDelegate eOrderORDER_CANCEL_CONFIRM_OUT;
        public static event RaiseEventDelegate eOrderBATCH_ORDER_CANCEL;
        public static event RaiseEventDelegate eOrderORDER_CXL_REJ_OUT;
        public static event RaiseEventDelegate eOrderTRADE_ERROR;
        public static event RaiseEventDelegate eOrderTRADE_CANCEL_OUT;

        public static event RaiseEventDelegate eOrderTWOL_ORDER_CONFIRMATION;
        public static event RaiseEventDelegate eOrderTWOL_ORDER_ERROR;
        public static event RaiseEventDelegate eOrderTWOL_ORDER_CXL_CONFIRMATION;

        public static event RaiseEventDelegate eOrderTRADED_OUT;

        public static event RaiseEventStatusDelegate eDataUpdate;
        public static event RaiseEventDelegate eSpreadOrderConfirmation;
        public static event RaiseEventDelegate eSpreadOrderCancelConfirmation;
        public static event RaiseEventDelegate eSpreadOrderTradeonfirmation;
        public static event RaiseEventDelegate eSpreadOrderTradeonfirmationconfirmation;
        public static event RaiseEventDelegate eSpreadOrderModconfirmation;
        public static bool flag = false;
        
        //ORDER_CONFIRMATION_TR
        private NNFHandler()
        {
           // MessageBox.Show("Trying Connect to NNF ");
            //context = ZmqContext.Create();
            _ctsCancellationTokenSource = new CancellationTokenSource();
          /////////
           
            /////

        }
        public CancellationTokenSource _CTS = null;
        public void _socketfun()
        {
        
            _requestSocket = new RequestSocket();
            _requestSocket.Connect("tcp://" + Global.Instance.NNFConIp + ":" + Global.Instance.NNFConPUBPort);

            _subscribeSocket = new SubscribeSocket();
            _subscribeSocket.Connect("tcp://" + Global.Instance.NNFConIp + ":" + Global.Instance.NNFConSUBPort);



            long client_Autoid =Global.Instance.ClientId- 100;
            
           _subscribeSocket.Subscribe(BitConverter.GetBytes((short)MessageType.LOGIN).Concat(BitConverter.GetBytes(Global.Instance.ClientId)).ToArray());
        //    _subscribeSocket.Subscribe(BitConverter.GetBytes((short)MessageType.ORDER).Concat(BitConverter.GetBytes(Global.Instance.ClientId)).ToArray());

           if (Global.Instance.Relogin != true)
            {
                dllevent += new dll(checkCase);

                _CTS = new CancellationTokenSource();

            }

        }
       
        //private void timerforchecklogin_Elapsed(object o ,)
        //{

        //}

        //public int Publisher(MessageType msgType, byte[] buffer)
        //{
        //    //var buf=BitConverter.GetBytes((byte)msgType);
        //    //publisher.Send(buf).Concat(buffer).ToArray());

        //    //publisher.Send(Encoding.ASCII.GetBytes(msgType));
        //    //publisher.Send(buffer);

           
        //    PubNNF.SendMore(msgType.ToString(), Encoding.Unicode);
        //    return (int)PubNNF.Send(buffer);
        // //   return 1;
        //}


      
        public void Publisher(MessageType _conversationenum, byte[] data)
        {
        _requestSocket.SendImmediate(BitConverter.GetBytes((short)_conversationenum).Concat(BitConverter.GetBytes(Global.Instance.ClientId)).Concat(data).ToArray());
           
        }
        public void Publisher(MsgType _conversationenum, byte[] data)
        {
            _requestSocket.SendImmediate(BitConverter.GetBytes((short)_conversationenum).Concat(BitConverter.GetBytes(Global.Instance.ClientId)).Concat(data).ToArray());

        }
        int i = 1;
        public void RecieveDataAsClient()
        {
            try
            {

                Task.Factory.StartNew(() =>
                {
                    while (!_ctsCancellationTokenSource.IsCancellationRequested)
                    {

                        byte[] _IncomingData = _subscribeSocket.ReceiveImmediate();

                        if (_IncomingData == null) continue;

                        byte[] buffer = _IncomingData.Skip(8).Take(_IncomingData.Length - 8).ToArray();

                        if ((MessageType)BitConverter.ToInt16(_IncomingData, 0) != MessageType.HEARTBEAT)
                        {
                           // cout = cout + 1;
                           // eDataUpdate.Invoke(cout.ToString());
                        }
                        switch ((MessageType)BitConverter.ToInt16(_IncomingData, 0))
                        {

                            case MessageType.ORDER:
                                {
                                     NNFPktCracker(buffer);
                                    break;
                                }
                            case MessageType.ORDERRej:
                                {
                                    ORDERRej(buffer);
                                    break;
                                }
                            case MessageType.LOGIN:
                                {
                                    NNFInOut.Instance.SIGN_ON_REQUEST_OUT(buffer);
                                    
                                    Global.LastTime = System.DateTime.Now;

                                    long client_Autoid = Global.Instance.ClientId - 100;

                                     
                                    _subscribeSocket.Subscribe(
                                        BitConverter.GetBytes((short)MessageType.ORDERRej).Concat(BitConverter.GetBytes(Global.Instance.ClientId)).ToArray());

                                    _subscribeSocket.Subscribe(
                                            BitConverter.GetBytes((short)MessageType.MESSAGE).Concat(BitConverter.GetBytes(Global.Instance.ClientId)).ToArray());

                                    _subscribeSocket.Subscribe(
                                            BitConverter.GetBytes((short)MessageType.HEARTBEAT).Concat(BitConverter.GetBytes(0)).ToArray());



                                    _subscribeSocket.Subscribe(
                                            BitConverter.GetBytes((short)MessageType.ORDER).Concat(BitConverter.GetBytes(client_Autoid)).ToArray());
                                    _subscribeSocket.Subscribe(
                                        BitConverter.GetBytes((short)MessageType.ORDERRej).Concat(BitConverter.GetBytes(client_Autoid)).ToArray());
                                    _subscribeSocket.Subscribe(
                                            BitConverter.GetBytes((short)MessageType.MESSAGE).Concat(BitConverter.GetBytes(client_Autoid)).ToArray());

                                    break;
                                }
                            case MessageType.MESSAGE:
                                {
                                    MESSAGE(buffer);
                                    break;
                                }
                            case MessageType.HEARTBEAT:
                                {
                                    HeartBeat(buffer);
                                    Global.LastTime = System.DateTime.Now;
                                    break;
                                }

                            default:
                                MessageBox.Show("Some invalid packet recieved from Server Message Code " + "Error ");
                                break;

                        }


                        _ctsCancellationTokenSource.Token.ThrowIfCancellationRequested();
                    }



                }, _ctsCancellationTokenSource.Token);
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("Cancellation invoked");
            }
            catch (AggregateException e)
            {
                if (e.InnerException is OperationCanceledException)
                {
                    if (_requestSocket != null)
                        _requestSocket.Dispose();
                    if (_subscribeSocket != null)
                        _subscribeSocket.Dispose();
                }
                else
                {
                    Console.WriteLine("Some unexpected exception ");
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception Raised from Index " + "  " + Ex.Message);
            }
        }
        struct IncomingData
        {
           public  string PacketType;
            public object _obj;
        }


        private ConcurrentQueue<IncomingData> _DataQueue = new ConcurrentQueue<IncomingData>();



           void ORDERRej(byte[] buffer)
        {
            var obj = (C_OrderReject)DataPacket.RawDeserialize(buffer, typeof(C_OrderReject));
            Console.WriteLine("Order Reject By NNF OrderNo " + obj.OrderNo + " Reasoncode" + obj.Reasoncode.ToString());
        }
       
       public void HeartBeat(byte[] buffer)
        {
            HeartBeatInfo obj = (HeartBeatInfo)DataPacket.RawDeserialize(buffer, typeof(HeartBeatInfo));
            //this.OnDataAPPTYPEStatusChange.Raise(OnDataAPPTYPEStatusChange, OnDataAPPTYPEStatusChange.CreateReadOnlyArgs(_APPTYPE.ToString()));
            NNFHandler.Instance.OnStatusChangeHeartBeatInfo.Raise(NNFHandler.Instance.OnStatusChangeHeartBeatInfo, NNFHandler.Instance.OnStatusChangeHeartBeatInfo.CreateReadOnlyArgs(obj));
           Global. LastTime = DateTime.Now;

          Publisher(MessageType.HEARTBEAT,BitConverter.GetBytes(Global.Instance.ClientId));
         // Task.Factory.StartNew(() => LogWriterClass.logwritercls.logs("mgtdata", "hartbit"));
          //  Publisher(Structure.MsgTypeC.HEARTBEAT.ToString() + Global.Instance.ClientId, Encoding.ASCII.GetBytes(Global.Instance.ClientId));
        }

   internal  void MESSAGE(byte[] buffer)
        {
           
          
          //  MessageBox.Show("Message Received By NNF : " + Encoding.ASCII.GetString(buffer));
          //  Console.WriteLine("Message Received By NNF : " + Encoding.ASCII.GetString(buffer));

        }

        #region NNFPktCracker

        public void NNFPktCracker(byte[] buffer)
        {
            short TransactionCode = 0;
            var MsgHeader = (Message_Header)DataPacket.RawDeserialize(buffer.Skip(0).Take(40).ToArray(), typeof(Message_Header));
            TransactionCode = IPAddress.HostToNetworkOrder(MsgHeader.TransactionCode);
            dllevent.Invoke(TransactionCode, buffer);
        }

        static void checkCase(short TransactionCode, byte[] buffer)
        {
        //  Task.Factory.StartNew(()=>  LogWriterClass.logwritercls.logs("Transactioncode","Transaction Code : " +   TransactionCode.ToString() + "   Buffer size  :" + buffer.Length.ToString()));
            switch (TransactionCode)
            {
                case 2212:
                    NNFInOut.Instance.ON_STOP_NOTIFICATION(buffer);
                    break;

                case 20231:
                    eOrderORDER_ERROR_TR.Invoke(buffer);
                    break;

                case 20042:
                    eOrderORDER_MOD_REJECT_TR.Invoke(buffer);
                    break;

                case 20072:
                    eOrderORDER_CANCEL_REJECT_TR.Invoke(buffer);
                    break;
                case 20073:
                    //obj.ORDER_CONFIRMATION_TR(buffer);
                    eOrderORDER_CONFIRMATION_TR.Invoke(buffer);
                    break;

                case 20074:
                   // obj.ORDER_MOD_CONFIRMATION_TR(buffer);
                    eOrderORDER_MOD_CONFIRMATION_TR.Invoke(buffer);
                    break;

                case 20075:
                    //obj.ORDER_CXL_CONFIRMATION_TR(buffer);
                    eOrderORDER_CXL_CONFIRMATION_TR.Invoke(buffer);
                    break;


                case 20012:
                    eOrderPRICE_CONFIRMATION_TR.Invoke(buffer);
                    break;

                case 20222:
                    NNFHandler.eSpreadOrderTradeonfirmationconfirmation.Invoke(buffer);
                    eOrderTRADE_CONFIRMATION_TR.Invoke(buffer);
                    
                    break;

                case 2231:
                    eOrderORDER_ERROR_OUT.Invoke(buffer);
                    break;

                case 2012:
                    eOrderPRICE_CONFIRMATION.Invoke(buffer);
                    break;

                case 2073:
                    eOrderORDER_CONFIRMATION_OUT.Invoke(buffer);
                    break;

                case 2170:
                    eOrderFREEZE_TO_CONTROL.Invoke(buffer);
                    break;

                case 2074:
                    eOrderORDER_MOD_CONFIRM_OUT.Invoke(buffer);
                    break;

                case 2042:
                    eOrderORDER_MOD_REJ_OUT.Invoke(buffer);
                    break;

                case 2075:
                    eOrderORDER_CANCEL_CONFIRM_OUT.Invoke(buffer);
                    break;

                case 9002:
                    eOrderBATCH_ORDER_CANCEL.Invoke(buffer);
                    break;

                case 2072:
                    eOrderORDER_CXL_REJ_OUT.Invoke(buffer);
                    break;

                case 2223:
                    eOrderTRADE_ERROR.Invoke(buffer);
                    break;

                case 5441:
                    eOrderTRADE_CANCEL_OUT.Invoke(buffer);
                    break;

                case 2124:
                    Task.Factory.StartNew(() => eSpreadOrderConfirmation.Invoke(buffer));
                   // NNFInOut.Instance.SP_ORDER_CONFIRMATION(buffer);
                    break;

                case 2136:
                    Task.Factory.StartNew(() => eSpreadOrderModconfirmation.Invoke(buffer));
                  // NNFInOut.Instance.SP_ORDER_MOD_CON_OUT(buffer);
                    break;

                case 2130:
                  //  NNFInOut.Instance.SP_ORDER_CXL_CONFIRMATION(buffer);
                    Task.Factory.StartNew(() => eSpreadOrderCancelConfirmation.Invoke(buffer));
                    break;

                case 2133:
                    NNFInOut.Instance.SP_ORDER_MOD_REJ_OUT(buffer);
                    break;

                case 2127:
                    NNFInOut.Instance.SP_ORDER_CXL_REJ_OUT(buffer);
                    break;

                case 2154:
                    NNFInOut.Instance.SP_ORDER_ERROR_out(buffer);
                    break;

                case 2125:
                   // eOrderTWOL_ORDER_CONFIRMATION.Invoke(buffer);
                    Task.Factory.StartNew(() => eSpreadOrderCancelConfirmation.Invoke(buffer)); 
                   break;

                case 2155:
                    eOrderTWOL_ORDER_ERROR.Invoke(buffer);
                    break;

                case 2131:
                    eOrderTWOL_ORDER_CXL_CONFIRMATION.Invoke(buffer);
                    break;

                case 2126:
                    NNFInOut.Instance.THRL_ORDER_CONFIRMATION(buffer);
                    break;

                case 2156:
                    NNFInOut.Instance.THRL_ORDER_ERROR(buffer);
                    break;

                case 2132:
                    NNFInOut.Instance.THRL_ORDER_CXL_CONFIRMATION(buffer);
                    break;

              

                default:
                    Console.WriteLine("Invalid TransactionCode : " + TransactionCode);
                    break;
            }
        }
        #endregion NNFPktCracker

    }
}

