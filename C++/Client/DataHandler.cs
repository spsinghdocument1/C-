using System;
//using ZeroMQ;
using System.Text;
using System.Linq;
using Structure;
using System.Threading.Tasks;

namespace Client
{
    public class DataHandler
    {
        //static bool SignInstatus;
      //  ZmqContext context;
       // ZmqSocket SubData;
        //public delegate void dll (short TransactionCode,byte[] buffer);
        //public static event dll dllevent;

        public event EventHandler<ReadOnlyEventArgs<FinalPrice>> OnDataUpdate;

        private static readonly DataHandler instance = new DataHandler();
        public static DataHandler Instance
        {
            get
            {
                return instance;
            }
        }

        internal void InitConnection()
        {
            //context = ZmqContext.Create();
           // SubData = context.CreateSocket(SocketType.SUB);
            //SubData.Connect("tcp://" + Global.Instance.DataConIp + ":" + Global.Instance.DataConSUBPort);
           // Subscribe();
            //Subscriber();
            //
        }


        public void Subscribe()
        {

           // SubData.SubscribeAll();

          //  SubData.Subscribe(Encoding.Unicode.GetBytes("35068"));
           // SubData.Subscribe(Encoding.Unicode.GetBytes("35072"));
            //SubData.Subscribe(Encoding.Unicode.GetBytes("35074"));
           // SubData.Subscribe(Encoding.Unicode.GetBytes("35078"));
            //SubData.Subscribe(Encoding.Unicode.GetBytes("35080"));
           // SubData.Subscribe(Encoding.Unicode.GetBytes("35082"));
          //  SubData.Subscribe(Encoding.Unicode.GetBytes("35084"));
           // SubData.Subscribe(Encoding.Unicode.GetBytes("35088"));
          //  SubData.Subscribe(Encoding.Unicode.GetBytes("35092"));
            


        }
      
        public void Subscriber()
        {
            Task.Factory.StartNew(() =>
            {
                //	int i = 1;
                while (true)
                {
                 //   byte[] buffer = new byte[1024];
                 // //  string MsgT = SubData.Receive(Encoding.Unicode);
                 ////   int bufferSize = SubData.Receive(buffer);
                 //   //buffer = buffer.Skip(0).Take(bufferSize).ToArray();
                 //   // Console.WriteLine(MsgT);
                 //   if (bufferSize > 0)
                 //   {

                 //       //Add rawdeserialize here after unpacking send data using invoke written below
                 //       // this.OnDataArrived.Invoke()

                 //       FinalPrice FP = (FinalPrice)DataPacket.RawDeserialize(buffer, typeof(FinalPrice));
                 //       Holder.holderData.TryAdd(FP.Token, FP);
                 //       OnDataUpdate.Raise(OnDataUpdate, OnDataUpdate.CreateReadOnlyArgs(FP));

                    //}
                }
            }
                );


        }


    }
}

