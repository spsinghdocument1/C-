


using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using Structure;
using Client;
using System.Net.NetworkInformation;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Collections.Concurrent;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Linq;
using Client.Spread;



namespace Client
{
    public class NNFInOut
    {


        private static readonly NNFInOut instance = new NNFInOut();
        public static NNFInOut Instance
        {
            get
            {
                return instance;
            }
        }
      

       internal event EventHandler<ReadOnlyEventArgs<SYSTEMSTATUS>> OnStatusChange;
       internal event EventHandler<ReadOnlyEventArgs<string>> OnDataStatusChange;
       internal event EventHandler<ReadOnlyEventArgs<string>> OnDataAPPTYPEStatusChange;
       

        public static long OrderNoGet
        {
            get
            {

                string str = DateTime.Now.ToString("yyMMddHHmmssff") + "001";
                return Convert.ToInt64(str);
            }

        }

        

        ////////////////////////////////////////////////////////////////////////.............ALL NNF OUT..........//////////////////////////////////////////////////////


        #region NNF OUT Messages
        private void timerforchecklogin_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
          
            if (DateTime.Compare(Global.LastTime.AddSeconds(Global.Instance.interval), DateTime.Now) < 0)
            { 
                NNFHandler.flag = false;
                NNFHandler.Instance._CTS.Cancel();
                timerforchecklogin.Stop();                               
                OnStatusChange.Raise(OnStatusChange, OnStatusChange.CreateReadOnlyArgs(SYSTEMSTATUS.LOGGEDOUT));
            }
        }


        System.Timers.Timer timerforchecklogin;
        public void SIGN_ON_REQUEST_OUT(byte[] buffer)  //--2300
        {
            try { 
           
                

            C_SignIn L_in = new C_SignIn();
          //  Console.WriteLine(Marshal.SizeOf(L_in));

               L_in = (C_SignIn)DataPacket.RawDeserialize(buffer, typeof(C_SignIn));
            Console.WriteLine(""+L_in.Status);
            if (L_in.TransectionCode == 2320)
            {
                if (L_in.Status == (short)LogInStatus.UserAlreadyLogOut || L_in.Status == (short)LogInStatus.LogOut)
                {
               
                    Global.Instance.warningvar = true;
                Global.Instance.SignInStatus = false;
                Console.WriteLine("Log Off SuccessFully");
                }

            }
          
            switch (L_in.Status)
            {                     
                case (short)LogInStatus.LogIn:
                    Global.Instance.Pass_bool = false;
                    Global.Instance.SignInStatus = true;
                   // MessageBox.Show("Login Reponse Recieved Successfully...");
                    timerforchecklogin = new System.Timers.Timer();
                    timerforchecklogin.Interval = Global.Instance.interval;
                    timerforchecklogin.Start();
                    timerforchecklogin.Elapsed += timerforchecklogin_Elapsed;
                    OnStatusChange.Raise(OnStatusChange, OnStatusChange.CreateReadOnlyArgs(SYSTEMSTATUS.LOGGEDIN));
                    Global.Instance.Net_File = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}",DateTime.Now);
                    Global.Instance.Net_File = Global.Instance.Net_File + "Net_Position_File";
                    break;
                case (short)LogInStatus.PwdError:
                    Global.Instance.Pass_bool = true;
                    MessageBox.Show("Please Enter Valid Password");         
                    break;
                case (short)LogInStatus.PwdExpire:
                    Global.Instance.Pass_bool = true;
                    MessageBox.Show("Password Expire");                 
                    break;
                case (short)LogInStatus.LogOutStatus:
                    MessageBox.Show("Status LogOut");
                    
                    timerforchecklogin.Stop();
                    NNFHandler.Instance._CTS.Cancel();
                    NNFHandler.flag = false;
                    this.OnStatusChange.Raise(OnStatusChange, OnStatusChange.CreateReadOnlyArgs(SYSTEMSTATUS.LOGGEDOUT));
                    timerforchecklogin.Elapsed -= timerforchecklogin_Elapsed;
                    break;
                case (short)LogInStatus.LogOutbyAdmin:
                    MessageBox.Show("LogOut By Admin");
                    timerforchecklogin.Stop();
                  

                    NNFHandler.Instance._subscribeSocket.Unsubscribe(BitConverter.GetBytes((short)MessageType.ORDER).Concat(BitConverter.GetBytes(Global.Instance.ClientId)).ToArray());
                    NNFHandler.Instance._subscribeSocket.Unsubscribe(BitConverter.GetBytes((short)MessageType.ORDERRej).Concat(BitConverter.GetBytes(Global.Instance.ClientId)).ToArray());
                    NNFHandler.Instance._subscribeSocket.Unsubscribe(BitConverter.GetBytes((short)MessageType.LOGIN).Concat(BitConverter.GetBytes(Global.Instance.ClientId)).ToArray());
                    NNFHandler.Instance._subscribeSocket.Unsubscribe(BitConverter.GetBytes((short)MessageType.MESSAGE).Concat(BitConverter.GetBytes(Global.Instance.ClientId)).ToArray());
                    NNFHandler.Instance._subscribeSocket.Unsubscribe(BitConverter.GetBytes((short)MessageType.HEARTBEAT).Concat(BitConverter.GetBytes(0)).ToArray());

                    NNFHandler.Instance._subscribeSocket.Unsubscribe(BitConverter.GetBytes((short)MessageType.ORDER).Concat(BitConverter.GetBytes(Global.Instance.ClientId - 100)).ToArray());
                    NNFHandler.Instance._subscribeSocket.Unsubscribe(BitConverter.GetBytes((short)MessageType.ORDERRej).Concat(BitConverter.GetBytes(Global.Instance.ClientId - 100)).ToArray());
                    NNFHandler.Instance._subscribeSocket.Unsubscribe(BitConverter.GetBytes((short)MessageType.LOGIN).Concat(BitConverter.GetBytes(Global.Instance.ClientId - 100)).ToArray());
            
                   // NNFHandler.SubNNF.UnsubscribeAll();
                    this.OnStatusChange.Raise(OnStatusChange, OnStatusChange.CreateReadOnlyArgs(SYSTEMSTATUS.LOGGEDOUT));
                    break;
                case (short)LogInStatus.LogOutNoheartbeat:
                  //  timerforchecklogin.Elapsed -= timerforchecklogin_Elapsed;
                    this.OnStatusChange.Raise(OnStatusChange, OnStatusChange.CreateReadOnlyArgs(SYSTEMSTATUS.LOGGEDOUT));                  
                    break;
                case (short)LogInStatus.LogOut:                  
                    NNFHandler.Instance._CTS.Cancel();
                    NNFHandler.flag = false;
                    Global.Instance.SignInStatus = false;
                    Global.Instance.warningvar = true;
                    MessageBox.Show("LogOut Succesfuly", "Log Out");
                    
                    timerforchecklogin.Elapsed -= timerforchecklogin_Elapsed;
                   // new MDIParent1().logout();
                    break;
                case (short)LogInStatus.UserAlreadyLogIn:
                    MessageBox.Show("user Already Login");                  
                 //   this.OnStatusChange.Raise(OnStatusChange, OnStatusChange.CreateReadOnlyArgs(SYSTEMSTATUS.LOGGEDOUT));
                  //  timerforchecklogin.Elapsed -= timerforchecklogin_Elapsed;
                    break;                  
            }

                }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            /*	if (Convert.ToBoolean (L_in.Status)) {
                    Console.WriteLine ("LogIn SuccessFully");
                //	new Login ();
                }
                else
                    Console.WriteLine ("Inavlid LogIn attempted ");
                    */

        }
        //....Order and Trade Management ...TR
        private void logout_win()
        {
            
         
        }



        public void ORDER_ERROR_TR(byte[] buffer) //-- 20231
        {
            MS_OE_RESPONSE_TR obj = (MS_OE_RESPONSE_TR)DataPacket.RawDeserialize(buffer, typeof(MS_OE_RESPONSE_TR));
        }

        public void ON_STOP_NOTIFICATION(byte[] buffer)
        {
            MS_TRADE_INQ_DATA obj = (MS_TRADE_INQ_DATA)DataPacket.RawDeserialize(buffer, typeof(MS_TRADE_INQ_DATA));

        }

        public void ORDER_MOD_REJECT_TR(byte[] buffer) //-- 20042
        {
            MS_OE_RESPONSE_TR obj = (MS_OE_RESPONSE_TR)DataPacket.RawDeserialize(buffer, typeof(MS_OE_RESPONSE_TR));
        }

        public void ORDER_CANCEL_REJECT_TR(byte[] buffer) //-- 20072
        {
            MS_OE_RESPONSE_TR obj = (MS_OE_RESPONSE_TR)DataPacket.RawDeserialize(buffer, typeof(MS_OE_RESPONSE_TR));
        }


        public void ORDER_CONFIRMATION_TR(byte[] buffer) //-- 20073
        {
            MS_OE_RESPONSE_TR obj = (MS_OE_RESPONSE_TR)DataPacket.RawDeserialize(buffer, typeof(MS_OE_RESPONSE_TR));
            Holder.holderOrder.TryAdd(LogicClass.DoubleEndianChange(obj.OrderNumber), new Order((int)_Type.MS_OE_RESPONSE_TR));
            Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber)].mS_OE_RESPONSE_TR = obj;
            Console.WriteLine("Symbol : " + ASCIIEncoding.UTF8.GetString(obj.Contr_dec_tr_Obj.Symbol) +
                               ", TokenNo  : " + IPAddress.HostToNetworkOrder(obj.TokenNo) +
                                ", Order No : " + (long)LogicClass.DoubleEndianChange(obj.OrderNumber) +
                               ", Buy_S : " + IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) +
                               ", Price : " + IPAddress.HostToNetworkOrder(obj.Price) +
                               ", TotalVolumeRemaining : " + IPAddress.HostToNetworkOrder(obj.TotalVolumeRemaining) +
                               ", Volume : " + IPAddress.HostToNetworkOrder(obj.Volume) +
                               ", VolilledToday : " + IPAddress.HostToNetworkOrder(obj.VolumeFilledToday)
                               );

            Console.WriteLine(" Order No : " + LogicClass.DoubleEndianChange(obj.OrderNumber));




            //		ORDER_MOD_IN_TR (LogicClass.DoubleEndianChange(obj.OrderNumber),802200);
            //		Thread.Sleep (1000);
            //		ORDER_CANCEL_IN_TR (LogicClass.DoubleEndianChange (obj.OrderNumber));

        }

        public void ORDER_MOD_CONFIRMATION_TR(byte[] buffer) //-- 20074
        {
            MS_OE_RESPONSE_TR obj = (MS_OE_RESPONSE_TR)DataPacket.RawDeserialize(buffer, typeof(MS_OE_RESPONSE_TR));
            Holder.holderOrder.TryAdd(LogicClass.DoubleEndianChange(obj.OrderNumber), new Order((int)_Type.MS_OE_RESPONSE_TR));
            Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber)].mS_OE_RESPONSE_TR = obj;
            Console.WriteLine(" ORDER_MOD_CONFIRMATION_TR : ");

            Console.WriteLine("Symbol : " + ASCIIEncoding.UTF8.GetString(obj.Contr_dec_tr_Obj.Symbol) +
                               ", TokenNo  : " + IPAddress.HostToNetworkOrder(obj.TokenNo) +
                               ", Order No : " + (long)LogicClass.DoubleEndianChange(obj.OrderNumber) +
                               ", Buy_S : " + IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) +
                               ", Price : " + IPAddress.HostToNetworkOrder(obj.Price) +
                               ", TotalVolumeRemaining : " + IPAddress.HostToNetworkOrder(obj.TotalVolumeRemaining) +
                               ", Volume : " + IPAddress.HostToNetworkOrder(obj.Volume) +
                               ", VolilledToday : " + IPAddress.HostToNetworkOrder(obj.VolumeFilledToday)
                               );


        }
        static int count12 = 0;
        public void ORDER_CXL_CONFIRMATION_TR(byte[] buffer) //-- 20075
        {
            Console.WriteLine("ORDER_CXL_CONFIRMATION_TR ******** count " + ++count12);
            MS_OE_RESPONSE_TR obj = (MS_OE_RESPONSE_TR)DataPacket.RawDeserialize(buffer, typeof(MS_OE_RESPONSE_TR));
            int ch = Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber)].GetType();
            switch (ch)
            {
                case 1:
                    {
                        var ob = new Order((int)_Type.MS_OE_REQUEST);
                        Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.OrderNumber), out ob);
                        break;
                    }
                case 2:
                    {
                        var ob = new Order((int)_Type.MS_OE_RESPONSE_TR);
                        Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.OrderNumber), out ob);
                        break;
                    }
                case 3:
                    {
                        var ob = new Order((int)_Type.MS_SPD_OE_REQUEST);
                        Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.OrderNumber), out ob);
                        break;
                    }
            }
        }
        public void PRICE_CONFIRMATION_TR(byte[] buffer) //-- 20012
        {
            MS_OE_RESPONSE_TR obj = (MS_OE_RESPONSE_TR)DataPacket.RawDeserialize(buffer, typeof(MS_OE_RESPONSE_TR));
        }

        public void TRADE_CONFIRMATION_TR(byte[] buffer) //-- 20222
        {
            var obj = (MS_TRADE_CONFIRM_TR)DataPacket.RawDeserialize(buffer, typeof(MS_TRADE_CONFIRM_TR));
            int ch = 0;
            if (Holder.holderOrder.ContainsKey(LogicClass.DoubleEndianChange(obj.ResponseOrderNumber)))
                ch = Holder.holderOrder[LogicClass.DoubleEndianChange(obj.ResponseOrderNumber)].GetType();
            else
                Console.WriteLine("Error while remove order from dictionary " + (long)LogicClass.DoubleEndianChange(obj.ResponseOrderNumber));
            switch (ch)
            {
                case 1:
                    {
                        var ob = new Order((int)_Type.MS_OE_REQUEST);
                        Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.ResponseOrderNumber), out ob);
                        break;
                    }
                case 2:
                    {
                        var ob = new Order((int)_Type.MS_OE_RESPONSE_TR);
                        Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.ResponseOrderNumber), out ob);
                        break;
                    }
                case 3:
                    {
                        var ob = new Order((int)_Type.MS_SPD_OE_REQUEST);
                        Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.ResponseOrderNumber), out ob);
                        break;
                    }
            }

            Console.WriteLine(
                " Token  : " + IPAddress.HostToNetworkOrder(obj.Token) +
                ", ResponseOrderNumber : " + (long)LogicClass.DoubleEndianChange(obj.ResponseOrderNumber) +
                ", Buy_SellIndicator : " + IPAddress.HostToNetworkOrder(obj.Buy_SellIndicator) +
                ", FillPrice : " + IPAddress.HostToNetworkOrder(obj.FillPrice) +
                ", Price : " + IPAddress.HostToNetworkOrder(obj.Price) +
                ", DisclosedVolumeRemaining : " + IPAddress.HostToNetworkOrder(obj.DisclosedVolumeRemaining) +
                ", DisclosedVolume : " + IPAddress.HostToNetworkOrder(obj.DisclosedVolume) +
                ", VolumeFilledToday : " + IPAddress.HostToNetworkOrder(obj.VolumeFilledToday) +
                ", OriginalVolume : " + IPAddress.HostToNetworkOrder(obj.OriginalVolume) +
                ", RemainingVolume : " + IPAddress.HostToNetworkOrder(obj.RemainingVolume)
                );

        }


        //Order and Trade Management


        public void ORDER_ERROR_OUT(byte[] buffer) //-- 2231
        {
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
        }



        public void PRICE_CONFIRMATION(byte[] buffer) //-- 2012
        {
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
        }

        static int count11 = 0;
        public void ORDER_CONFIRMATION_OUT(byte[] buffer) //-- 2073
        {
            Console.WriteLine("ORDER_CONFIRMATION_OUT ******** count " + ++count11);

            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
            Holder.holderOrder.TryAdd(LogicClass.DoubleEndianChange(obj.OrderNumber), new Order((int)_Type.MS_OE_REQUEST));
            Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber)].mS_OE_REQUEST = obj;

            ORDER_MOD_IN(LogicClass.DoubleEndianChange(obj.OrderNumber), 801100);
            Thread.Sleep(1000);
            //	ORDER_CANCEL_IN (LogicClass.DoubleEndianChange (obj.OrderNumber));
        }
        public void FREEZE_TO_CONTROL(byte[] buffer) //-- 2170
        {
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
        }
        public void ORDER_MOD_REJ_OUT(byte[] buffer) //-- 2042
        {
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
        }

        public void ORDER_MOD_CONFIRM_OUT(byte[] buffer) //-- 2074
        {
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
            Holder.holderOrder.TryAdd(LogicClass.DoubleEndianChange(obj.OrderNumber), new Order((int)_Type.MS_OE_REQUEST));
            Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber)].mS_OE_REQUEST = obj;
        }
        public void ORDER_CANCEL_CONFIRM_OUT(byte[] buffer) //-- 2075
        {
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
            var ob = new Order((int)_Type.MS_OE_REQUEST);
            Holder.holderOrder.TryRemove(LogicClass.DoubleEndianChange(obj.OrderNumber), out ob);
        }

        public void BATCH_ORDER_CANCEL(byte[] buffer) //-- 9002
        {
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));
           
        }

        public void ORDER_CXL_REJ_OUT(byte[] buffer) //-- 2072
        {
            MS_OE_REQUEST obj = (MS_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_OE_REQUEST));

        }


        public void TRADE_ERROR(byte[] buffer) //-- 2223
        {
            MS_TRADE_INQ_DATA obj = (MS_TRADE_INQ_DATA)DataPacket.RawDeserialize(buffer, typeof(MS_TRADE_INQ_DATA));
        }

        public void TRADE_CANCEL_OUT(byte[] buffer) //-- 5441
        {
            MS_TRADE_INQ_DATA obj = (MS_TRADE_INQ_DATA)DataPacket.RawDeserialize(buffer, typeof(MS_TRADE_INQ_DATA));
        }



        public void SP_ORDER_CONFIRMATION(byte[] buffer)//2124
        {

            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
            if (Holder.holderOrder.ContainsKey(LogicClass.DoubleEndianChange(obj.OrderNumber1)))
                return;
            Holder.holderOrder.TryAdd(LogicClass.DoubleEndianChange(obj.OrderNumber1), new Order((int)_Type.MS_SPD_OE_REQUEST));
            Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber1)].mS_SPD_OE_REQUEST = obj;

            Console.WriteLine(
                " Token1  : " + IPAddress.HostToNetworkOrder(obj.Token1) +
                ", OrderNumber1 : " + (long)LogicClass.DoubleEndianChange(obj.OrderNumber1) +
                ", BuySell1 : " + IPAddress.HostToNetworkOrder(obj.BuySell1) +
                ", PriceDiff : " + IPAddress.HostToNetworkOrder(obj.PriceDiff) +
                ", Price1 : " + IPAddress.HostToNetworkOrder(obj.Price1) +
                ", Volume1 : " + IPAddress.HostToNetworkOrder(obj.Volume1) +
                ", TriggerPrice1 : " + IPAddress.HostToNetworkOrder(obj.TriggerPrice1) +
                ", TotalVolRemaining1 : " + IPAddress.HostToNetworkOrder(obj.TotalVolRemaining1)
                );



        }
        public void SP_ORDER_MOD_CON_OUT(byte[] buffer)//2136
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
            Holder.holderOrder[LogicClass.DoubleEndianChange(obj.OrderNumber1)].mS_SPD_OE_REQUEST = obj;

            Console.WriteLine(
                " Token1  : " + IPAddress.HostToNetworkOrder(obj.Token1) +
                ", OrderNumber1 : " + (long)LogicClass.DoubleEndianChange(obj.OrderNumber1) +
                ", BuySell1 : " + IPAddress.HostToNetworkOrder(obj.BuySell1) +
                ", PriceDiff : " + IPAddress.HostToNetworkOrder(obj.PriceDiff) +
                ", Price1 : " + IPAddress.HostToNetworkOrder(obj.Price1) +
                ", Volume1 : " + IPAddress.HostToNetworkOrder(obj.Volume1) +
                ", TriggerPrice1 : " + IPAddress.HostToNetworkOrder(obj.TriggerPrice1) +
                ", TotalVolRemaining1 : " + IPAddress.HostToNetworkOrder(obj.TotalVolRemaining1)
                );
        }

        public void SP_ORDER_MOD_REJ_OUT(byte[] buffer)//2133
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
        }

        public void SP_ORDER_CXL_CONFIRMATION(byte[] buffer)//2130
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));

            MS_SPD_OE_REQUEST o1 = (MS_SPD_OE_REQUEST)obj;

            Console.WriteLine(
                " Token1  : " + IPAddress.HostToNetworkOrder(obj.Token1) +
                ", OrderNumber1 : " + (long)LogicClass.DoubleEndianChange(obj.OrderNumber1) +
                ", BuySell1 : " + IPAddress.HostToNetworkOrder(obj.BuySell1) +
                ", PriceDiff : " + IPAddress.HostToNetworkOrder(obj.PriceDiff) +
                ", Price1 : " + IPAddress.HostToNetworkOrder(obj.Price1) +
                ", Volume1 : " + IPAddress.HostToNetworkOrder(obj.Volume1) +
                ", TriggerPrice1 : " + IPAddress.HostToNetworkOrder(obj.TriggerPrice1) +
                ", TotalVolRemaining1 : " + IPAddress.HostToNetworkOrder(obj.TotalVolRemaining1)
                );
        }


        public void SP_ORDER_CXL_REJ_OUT(byte[] buffer)//2127
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
        }


        public void SP_ORDER_ERROR_out(byte[] buffer)//2154
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
        }


        public void TWOL_ORDER_CONFIRMATION(byte[] buffer) //-- 2125
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
            Console.WriteLine(
                " Token1  : " + IPAddress.HostToNetworkOrder(obj.Token1) +
                ", OrderNumber1 : " + (long)LogicClass.DoubleEndianChange(obj.OrderNumber1) +
                ", BuySell1 : " + IPAddress.HostToNetworkOrder(obj.BuySell1) +
                ", PriceDiff : " + IPAddress.HostToNetworkOrder(obj.PriceDiff) +
                ", Price1 : " + IPAddress.HostToNetworkOrder(obj.Price1) +
                ", Volume1 : " + IPAddress.HostToNetworkOrder(obj.Volume1) +
                ", TriggerPrice1 : " + IPAddress.HostToNetworkOrder(obj.TriggerPrice1) +
                ", TotalVolRemaining1 : " + IPAddress.HostToNetworkOrder(obj.TotalVolRemaining1)
            );

        }

        public void THRL_ORDER_CONFIRMATION(byte[] buffer) //-- 2126
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
        }


        public void TWOL_ORDER_ERROR(byte[] buffer) //-- 2155
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
        }
        public void THRL_ORDER_ERROR(byte[] buffer) //-- 2156
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
        }

        public void TWOL_ORDER_CXL_CONFIRMATION(byte[] buffer) //-- 2131
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
        }

        public void THRL_ORDER_CXL_CONFIRMATION(byte[] buffer) //-- 2132
        {
            MS_SPD_OE_REQUEST obj = (MS_SPD_OE_REQUEST)DataPacket.RawDeserialize(buffer, typeof(MS_SPD_OE_REQUEST));
        }


        #endregion NNF OUT Messages






        ////////////////////////////////////////////////////////////////////////.............ALL NNF IN..........//////////////////////////////////////////////////////



        #region NNF IN Messages

        public void SIGN_ON_REQUEST_IN()  //--2301  
        {

            STGTYPE _APPTYPE = STGTYPE.FOFO;

            if (Global.Instance.APPTYPE == "FOFO")
            {
                _APPTYPE = STGTYPE.FOFO;
            }
            else if (Global.Instance.APPTYPE == "2LIOC")
            {
                _APPTYPE = STGTYPE.TWOLEGOPT;
            }
            else
            {
                MessageBox.Show("Please assign Application Type for the user.", "User Type not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            C_SignIn obj = new C_SignIn()
            {
                TransectionCode = 2300,
                ClintId = Global.Instance.ClientId,
                Password = Global.Instance.NNFPassword,
                StgType = _APPTYPE

            };
            this.OnDataAPPTYPEStatusChange.Raise(OnDataAPPTYPEStatusChange, OnDataAPPTYPEStatusChange.CreateReadOnlyArgs(_APPTYPE.ToString()));
            byte[] buffer = DataPacket.RawSerialize(obj);
            NNFHandler.Instance.Publisher(MessageType.LOGIN, buffer);
        }
        


        public void SIGN_OFF_REQUEST_IN()  //--2320
        {
            C_SignIn obj = new C_SignIn()
            {
                TransectionCode = 2320,
                ClintId = Global.Instance.ClientId,
            };
            byte[] buffer = DataPacket.RawSerialize(obj);
            NNFHandler.Instance.Publisher(MessageType.LOGIN, buffer);
        }

        static int count1 = 0;
        public void BOARD_LOT_IN()  //-- 2000
        {
            Console.WriteLine("BOARD_LOT_IN ******** count " + ++count1);

            C_LotIN obj = new C_LotIN()
            {
                TransectionCode = 2000,
                OrderNo = 0002,
                ClintId = Global.Instance.ClientId,//ORDERC001
                TokenNo = 42882,
                contract_obj = new C_Contract_Desc()
                {
                    InstrumentName = "FUTIDX",
                    Symbol = "NIFTY",
                    ExpiryDate = 1107009000,
                    StrikePrice = -1,
                    OptionType = "XX",
                    //		CALevel = 0,//	
                },
                AccountNumber = "",
                Buy_SellIndicator = 1,
                DisclosedVolume = 100,
                Volume = 100,
                Price = 802000,
                TriggerPrice = 801000,
                Open_Close = Convert.ToByte('O'),
                //	Reasoncode=00,//
            };
            byte[] buffer = DataPacket.RawSerialize(obj);
            NNFHandler.Instance.Publisher(MessageType.ORDER, buffer);
           


        }

        public void ORDER_MOD_IN(double OrderNo, int TriggerPrice)  //-- 2040	
        {
            C_LotIN obj = new C_LotIN()
            {
                TransectionCode = 2040,
                OrderNo = (long)OrderNo,
                ClintId = Global.Instance.ClientId,
                //Holder.holderOrder [OrderNo].TokenNo
                TokenNo = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_REQUEST.TokenNo),

                contract_obj = new C_Contract_Desc()
                {
                    InstrumentName = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_REQUEST.contract_obj.InstrumentName),
                    Symbol = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_REQUEST.contract_obj.Symbol),
                    ExpiryDate = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_REQUEST.contract_obj.ExpiryDate),
                    StrikePrice = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_REQUEST.contract_obj.StrikePrice),
                    OptionType = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_REQUEST.contract_obj.OptionType),
                    //		CALevel = 0,//	
                },
                AccountNumber = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_REQUEST.AccountNumber),
                Buy_SellIndicator = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_REQUEST.Buy_SellIndicator),
                DisclosedVolume = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_REQUEST.DisclosedVolume),
                Volume = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_REQUEST.Volume),
                Price = 802000,
                TriggerPrice = TriggerPrice,
                Open_Close = Holder.holderOrder[OrderNo].mS_OE_REQUEST.Open_Close,

            };
            byte[] buffer = DataPacket.RawSerialize(obj);
            NNFHandler.Instance.Publisher(MessageType.ORDER, buffer);
            

        }
        static int count2 = 0;
        public void ORDER_CANCEL_IN(double OrderNo)  //-- 2070	
        {
            Console.WriteLine("ORDER_CANCEL_IN ******** count " + ++count2);

            C_LotIN obj = new C_LotIN()
            {
                TransectionCode = 2070,
                OrderNo = (long)OrderNo,
                ClintId = Global.Instance.ClientId,
                //Holder.holderOrder [OrderNo].TokenNo
                TokenNo = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_REQUEST.TokenNo),

                contract_obj = new C_Contract_Desc()
                {
                    InstrumentName = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_REQUEST.contract_obj.InstrumentName),
                    Symbol = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_REQUEST.contract_obj.Symbol),
                    ExpiryDate = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_REQUEST.contract_obj.ExpiryDate),
                    StrikePrice = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_REQUEST.contract_obj.StrikePrice),
                    OptionType = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_REQUEST.contract_obj.OptionType),
                    //		CALevel = 0,//	
                },
                AccountNumber = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_REQUEST.AccountNumber),
                Buy_SellIndicator = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_REQUEST.Buy_SellIndicator),
                DisclosedVolume = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_REQUEST.DisclosedVolume),
                Volume = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_REQUEST.Volume),
                Price = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_REQUEST.Price),
                TriggerPrice = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_REQUEST.TriggerPrice),
                Open_Close = Holder.holderOrder[OrderNo].mS_OE_REQUEST.Open_Close,
            };
            byte[] buffer = DataPacket.RawSerialize(obj);
            NNFHandler.Instance.Publisher(MessageType.ORDER, buffer);

        }


        public void BOARD_LOT_IN_TR(int tokenNo, string instrumentName, string symbol, int expiryDate, int strikePrice, string optionType, short buy_SellIndicator, int volume, int price)  //-- 20000
        {

            C_LotIN obj = new C_LotIN()
            {
                TransectionCode = 20000,
                OrderNo = OrderNoGet,
                ClintId = Global.Instance.ClientId,
                TokenNo = tokenNo,
                contract_obj = new C_Contract_Desc()
                {
                    InstrumentName = instrumentName,
                    Symbol = symbol,
                    ExpiryDate = expiryDate,
                    StrikePrice = strikePrice,
                    OptionType = optionType,
                    //		CALevel = 0,//	
                },


                AccountNumber = "",
                Buy_SellIndicator = buy_SellIndicator,
                DisclosedVolume = volume,
                Volume = volume,
                Price = price,
                //  TriggerPrice = 0,//
                Open_Close = Convert.ToByte('O'),
                //	Reasoncode=00,//


            };
            bool flag = true;



            /*	Console.WriteLine (" InstrumentName\n 1.FUTIDX  \n 2. FUTSTK ");
                Char ch = Console.ReadLine ().ToString ().ToUpper ().ToCharArray () [0];
                if (ch == '1')
                    obj.contract_obj.InstrumentName = "FUTIDX";
                if (ch == '2')
                    obj.contract_obj.InstrumentName = "FUTSTK";
                Console.WriteLine (" Symbol ");
                obj.contract_obj.Symbol = Console.ReadLine().ToUpper();			
                Console.WriteLine ("Enter Buy_SellIndicator 1(buy)/2(sell) = ");
                flag= Int16.TryParse(Console.ReadLine(),out obj.Buy_SellIndicator);
                Console.WriteLine ("Enter Token No = ");
                flag= int.TryParse(Console.ReadLine(),out obj.TokenNo);
                Console.WriteLine ("Enter quantity = ");
                flag=int.TryParse(Console.ReadLine(),out obj.Volume);
                obj.DisclosedVolume = obj.Volume;
                Console.WriteLine ("Enter Price = ");
                flag=int.TryParse(Console.ReadLine(),out obj.Price);
            */
         /*   if (!flag || obj.Price == 0)
            {
                Console.WriteLine("Market Order Not allow  ");
                return;
            }
            */
            byte[] buffer = DataPacket.RawSerialize(obj);
            NNFHandler.Instance.Publisher(MessageType.ORDER, buffer);
        }

        //public void ORDER_MOD_IN_TR(double OrderNo,int price )// 20040 
        public void ORDER_MOD_IN_TR(long _OrderNo, int volume, int price)// 20040 
        {
            double OrderNo = Convert.ToDouble(_OrderNo);
            C_LotIN obj = new C_LotIN();
            if (Holder.holderOrder.ContainsKey(OrderNo))
            {
                obj.TransectionCode = 20040;
                obj.OrderNo = (long)OrderNo;
                obj.ClintId = Global.Instance.ClientId;
                obj.TokenNo = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.TokenNo);
                obj.contract_obj = new C_Contract_Desc()
                {
                    InstrumentName = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Contr_dec_tr_Obj.InstrumentName),
                    Symbol = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Contr_dec_tr_Obj.Symbol),
                    ExpiryDate = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Contr_dec_tr_Obj.ExpiryDate),
                    StrikePrice = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Contr_dec_tr_Obj.StrikePrice),
                    OptionType = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Contr_dec_tr_Obj.OptionType),
                    //		CALevel = 0,//	
                };
                obj.AccountNumber = "";
                obj.Buy_SellIndicator = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Buy_SellIndicator);
                obj.Price = price;
                obj.DisclosedVolume = obj.Volume=volume;

                obj.Open_Close = Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Open_Close;

                byte[] buffer = DataPacket.RawSerialize(obj);
                NNFHandler.Instance.Publisher(MessageType.ORDER, buffer);
            }
            else
                Console.WriteLine("Order no. does no exist ");


        }


        public void PRICE_MOD_IN(double OrderNo)// 27013
        {

        }



        public void ORDER_CANCEL_IN_TR(long _OrderNo)  //-- 20070
        {
           // Console.WriteLine("Enter Order No ");
            double OrderNo = Convert.ToDouble(_OrderNo);

            if (Holder.holderOrder.ContainsKey(OrderNo))
            {

                C_LotIN obj = new C_LotIN()
                {
                    TransectionCode = 20070,
                    OrderNo = (long)OrderNo,
                    ClintId = Global.Instance.ClientId,
                    TokenNo = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.TokenNo),
                   
            
                    contract_obj = new C_Contract_Desc()
                    {
                        InstrumentName = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Contr_dec_tr_Obj.InstrumentName),
                        Symbol = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Contr_dec_tr_Obj.Symbol),
                        ExpiryDate = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Contr_dec_tr_Obj.ExpiryDate),
                        StrikePrice = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Contr_dec_tr_Obj.StrikePrice),
                        OptionType = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Contr_dec_tr_Obj.OptionType),
                        //		CALevel = 0,//	
                    },
                    AccountNumber = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.AccountNumber),
                    Buy_SellIndicator = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Buy_SellIndicator),
                    DisclosedVolume = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.DisclosedVolume),
                    Volume = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Volume),
                    Price = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Price),
                    //TriggerPrice = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR),
                    Open_Close = Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Open_Close,

                };

                byte[] buffer = DataPacket.RawSerialize(obj);
                NNFHandler.Instance.Publisher(MessageType.ORDER, buffer);
            }
            else
                Console.WriteLine("Order no. does no exist ");
        }


        public void KILL_switch_in()  //-- 2062
        {
            // Console.WriteLine("Enter Order No ");
            double OrderNo = Convert.ToDouble(0);

            //if (Holder.holderOrder.ContainsKey(OrderNo))
           // {

                C_LotIN obj = new C_LotIN()
                {
                    TransectionCode = 2062,
                    OrderNo = (long)OrderNo,
                    ClintId = Global.Instance.ClientId,
                    TokenNo = IPAddress.HostToNetworkOrder(0),


                    contract_obj = new C_Contract_Desc()
                    {
                        InstrumentName = "",
                        Symbol =" ",//System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR.Contr_dec_tr_Obj.Symbol),
                        ExpiryDate = IPAddress.HostToNetworkOrder(0),
                        StrikePrice = IPAddress.HostToNetworkOrder(0),
                        OptionType = "",
                        //		CALevel = 0,//	
                    },
                    AccountNumber = "",
                    Buy_SellIndicator = IPAddress.HostToNetworkOrder((short)0),
                    DisclosedVolume = IPAddress.HostToNetworkOrder(0),
                    Volume = IPAddress.HostToNetworkOrder(0),
                    Price = IPAddress.HostToNetworkOrder(0),
                    //TriggerPrice = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_OE_RESPONSE_TR),
                    Open_Close =Convert.ToByte(' '),

                };

                byte[] buffer = DataPacket.RawSerialize(obj);
                NNFHandler.Instance.Publisher(MessageType.ORDER, buffer);
           // }
           // else
              //  Console.WriteLine("Order no. does no exist ");
        }



        public void SP_BOARD_LOT_IN(PlaceSpreadOrder spdOrder)   //2100
        {
            C_Spread_lot_In obj = new C_Spread_lot_In()
            {
                TransectionCode = 2100,
                OrderNo = OrderNoGet,
                ClintId = Global.Instance.ClientId,
                TokenNo = spdOrder.Token1,
                ReasonCode = 0,
                contract_obj = new C_Contract_Desc()
                {
                    InstrumentName = spdOrder.SecInfo1.InstrumentName,
                    Symbol = spdOrder.SecInfo1.Symbol,
                    ExpiryDate = spdOrder.SecInfo1.ExpiryDate,
                    StrikePrice = spdOrder.SecInfo1.StrikePrice,
                    OptionType = spdOrder.SecInfo1.OptionType,
                    CALevel = spdOrder.SecInfo1.CALevel,
                },
                AccountNumber = "",
                Buy_SellIndicator = spdOrder.BuySell1,
                Volume = spdOrder.Volume1,
                Price = 0,
                Open_Close = Convert.ToByte('O'),

                obj_leg2 = new C_MS_SPD_LEG_INFO()
                {
                    token = spdOrder.Token2,
                    Price2 = 0,
                    contract_obj = new C_Contract_Desc()
                    {
                        InstrumentName = spdOrder.SecInfo2.InstrumentName,
                        Symbol = spdOrder.SecInfo2.Symbol,
                        ExpiryDate = spdOrder.SecInfo2.ExpiryDate,
                        StrikePrice = spdOrder.SecInfo2.StrikePrice,
                        OptionType = spdOrder.SecInfo2.OptionType,
                        CALevel = spdOrder.SecInfo2.CALevel,
                    },
                    BuySell2 = spdOrder.BuySell2,
                    Volume2 = spdOrder.Volume2,
                    OpenClose2 = Convert.ToByte('O'),
                },

            };

            obj.PriceDiff = spdOrder.Price1;
            byte[] buffer = DataPacket.RawSerialize(obj);
            NNFHandler.Instance.Publisher(MessageType.ORDER, buffer);

        }

        public void SP_ORDER_MOD_IN(long orderno,int Price1,int Volume1)//2118 
        {
            double OrderNo = Convert.ToDouble(orderno);

            if (Holder.holderOrder.ContainsKey(OrderNo))
            {
                C_Spread_lot_In obj = new C_Spread_lot_In()
                {
                    TransectionCode = 2118,
                    OrderNo = (long)OrderNo,
                    ClintId = Global.Instance.ClientId,
                    TokenNo = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.Token1),
                    ReasonCode = IPAddress.HostToNetworkOrder((short)Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.ReasonCode1),
                    contract_obj = new C_Contract_Desc()
                    {
                        InstrumentName = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.ms_oe_obj.InstrumentName),
                        Symbol = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.ms_oe_obj.Symbol),
                        ExpiryDate = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.ms_oe_obj.ExpiryDate),
                        StrikePrice = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.ms_oe_obj.StrikePrice),
                        OptionType = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.ms_oe_obj.OptionType),
                        CALevel = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.ms_oe_obj.CALevel),
                    },
                    AccountNumber = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.AccountNumber1),
                    Buy_SellIndicator = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.BuySell1),
                    Volume = Volume1,
                    Price = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.Price1),
                    PriceDiff = Price1,
                    Open_Close = Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.OpenClose1,
                    obj_leg2 = new C_MS_SPD_LEG_INFO()
                    {
                        token = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.token),
                        contract_obj = new C_Contract_Desc()
                        {
                            InstrumentName = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.InstrumentName),
                            Symbol = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.Symbol),
                            ExpiryDate = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.ExpiryDate),
                            StrikePrice = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.StrikePrice),
                            OptionType = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.Symbol),
                            CALevel = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.CALevel),
                        },
                        BuySell2 = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.BuySell2),
                        Volume2 = Volume1,
                        OpenClose2 = Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.OpenClose2,
                    },
                    //	obj_leg3=,
                    //res=2,
                };


                byte[] buffer = DataPacket.RawSerialize(obj);
                NNFHandler.Instance.Publisher(MessageType.ORDER, buffer);
            }
            else
                Console.WriteLine("Order no. does no exist ");
        }

        public void SP_ORDER_CANCEL_IN(long orderno)//2106 
        {
            Console.WriteLine("Enter Order No ");
            double OrderNo = Convert.ToDouble(orderno);

            if (Holder.holderOrder.ContainsKey(OrderNo))
            {
                C_Spread_lot_In obj = new C_Spread_lot_In()
                {
                    TransectionCode = 2106,
                    OrderNo = (long)OrderNo,
                    ClintId = Global.Instance.ClientId,
                    TokenNo = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.Token1),
                    ReasonCode = IPAddress.HostToNetworkOrder((short)Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.ReasonCode1),
                    contract_obj = new C_Contract_Desc()
                    {
                        InstrumentName = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.ms_oe_obj.InstrumentName),
                        Symbol = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.ms_oe_obj.Symbol),
                        ExpiryDate = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.ms_oe_obj.ExpiryDate),
                        StrikePrice = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.ms_oe_obj.StrikePrice),
                        OptionType = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.ms_oe_obj.OptionType),
                        CALevel = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.ms_oe_obj.CALevel),
                    },
                    AccountNumber = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.AccountNumber1),
                    Buy_SellIndicator = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.BuySell1),
                    Volume = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.Volume1),
                    Price = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.Price1),
                    PriceDiff = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.PriceDiff),
                    Open_Close = Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.OpenClose1,
                    obj_leg2 = new C_MS_SPD_LEG_INFO()
                    {
                        token = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.token),
                        contract_obj = new C_Contract_Desc()
                        {
                            InstrumentName = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.InstrumentName),
                            Symbol = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.Symbol),
                            ExpiryDate = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.ExpiryDate),
                            StrikePrice = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.StrikePrice),
                            OptionType = System.Text.Encoding.UTF8.GetString(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.Symbol),
                            CALevel = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.ms_oe_obj.CALevel),
                        },
                        BuySell2 = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.BuySell2),
                        Volume2 = IPAddress.HostToNetworkOrder(Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.Volume2),
                        OpenClose2 = Holder.holderOrder[OrderNo].mS_SPD_OE_REQUEST.leg2.OpenClose2,
                    },
                    //	obj_leg3=,
                    //res=2,
                };


                byte[] buffer = DataPacket.RawSerialize(obj);
                NNFHandler.Instance.Publisher(MessageType.ORDER, buffer);
            }
            else
                Console.WriteLine("Order no. does no exist ");
        }


        public void TWOL_BOARD_LOT_IN() //-- 2102 
        {
            C_Spread_lot_In obj = new C_Spread_lot_In()
            {
                TransectionCode = 2102,
                OrderNo = OrderNoGet,
                ClintId = Global.Instance.ClientId,
                //	TokenNo = 46182,
                ReasonCode = 0,
                contract_obj = new C_Contract_Desc()
                {
                    //	InstrumentName ="FUTIDX",
                    //		Symbol = "NIFTY",
                    //		ExpiryDate = 1109428200,
                    StrikePrice = -1,
                    OptionType = "XX",
                    CALevel = 0,
                },
                AccountNumber = "",
                //	Buy_SellIndicator = 2,
                //	Volume = 25,
                Price = 0,
                //	PriceDiff=10000,
                Open_Close = Convert.ToByte('O'),
                obj_leg2 = new C_MS_SPD_LEG_INFO()
                {
                    //		token = 42882,
                    contract_obj = new C_Contract_Desc()
                    {
                        //		InstrumentName = "FUTIDX",
                        //		Symbol = "NIFTY",
                        //		ExpiryDate = 1107009000,
                        StrikePrice = -1,
                        OptionType = "XX",
                        CALevel = 0
                    },
                    //		BuySell2 = 1,
                    //		Volume2=25,
                    OpenClose2 = Convert.ToByte('O'),
                },
                //	obj_leg3=,//
                //res=2,//
            };




            bool flag = true;



            Console.WriteLine(" InstrumentName\n 1.FUTIDX  \n 2. FUTSTK ");
            Char ch = Console.ReadLine().ToString().ToUpper().ToCharArray()[0];
            if (ch == '1')
                obj.contract_obj.InstrumentName = obj.obj_leg2.contract_obj.InstrumentName = "FUTIDX";
            if (ch == '2')
                obj.contract_obj.InstrumentName = obj.obj_leg2.contract_obj.InstrumentName = "FUTSTK";
            Console.WriteLine(" Symbol ");
            obj.contract_obj.Symbol = obj.obj_leg2.contract_obj.Symbol = Console.ReadLine().ToUpper();

            Console.WriteLine("Enter L1 Token No = ");
            flag = int.TryParse(Console.ReadLine(), out obj.TokenNo);
            Console.WriteLine("Enter L1 Buy_SellIndicator 1(buy)/2(sell) = ");
            flag = Int16.TryParse(Console.ReadLine(), out obj.Buy_SellIndicator);
            Console.WriteLine("Enter L1 Volume = ");
            flag = int.TryParse(Console.ReadLine(), out obj.Volume);
            Console.WriteLine("Enter L1 ExpiryDate = ");
            flag = int.TryParse(Console.ReadLine(), out obj.contract_obj.ExpiryDate);


            Console.WriteLine("Enter L2 Token No = ");
            flag = int.TryParse(Console.ReadLine(), out obj.obj_leg2.token);
            Console.WriteLine("Enter L2 Buy_SellIndicator 1(buy)/2(sell) = ");
            flag = Int16.TryParse(Console.ReadLine(), out obj.obj_leg2.BuySell2);
            Console.WriteLine("Enter L2 Volume = ");
            flag = int.TryParse(Console.ReadLine(), out obj.obj_leg2.Volume2);
            Console.WriteLine("Enter L2 ExpiryDate = ");
            flag = int.TryParse(Console.ReadLine(), out obj.obj_leg2.contract_obj.ExpiryDate);


            Console.WriteLine("Enter PriceDiff = ");
            flag = int.TryParse(Console.ReadLine(), out obj.PriceDiff);

      /*      if (!flag || obj.PriceDiff == 0)
            {
                Console.WriteLine("Market Order Not allow  ");
                return;
            }
            */


            byte[] buffer = DataPacket.RawSerialize(obj);
            NNFHandler.Instance.Publisher(MessageType.ORDER, buffer);
        }


        public void THRL_BOARD_LOT_IN() //2104   
        {
            C_Spread_lot_In obj = new C_Spread_lot_In()
            {
                TransectionCode = 2104,
                OrderNo = OrderNoGet,
                ClintId = Global.Instance.ClientId,
                TokenNo = 35885,
                ReasonCode = 0,
                contract_obj = new C_Contract_Desc()
                {
                    InstrumentName = "FUTIDX",
                    Symbol = "S&P500",
                    ExpiryDate = 1127053800,
                    StrikePrice = -1,
                    OptionType = "XX",
                    CALevel = 0,
                },
                AccountNumber = "",
                Buy_SellIndicator = 2,
                Volume = 250,
                Price = 0,
                PriceDiff = 100,
                Open_Close = Convert.ToByte('O'),
                obj_leg2 = new C_MS_SPD_LEG_INFO()
                {
                    token = 40351,
                    contract_obj = new C_Contract_Desc()
                    {
                        InstrumentName = "FUTIDX",
                        Symbol = "S&P500",
                        ExpiryDate = 1105885800,
                        StrikePrice = -1,
                        OptionType = "XX",
                        CALevel = 0
                    },
                    BuySell2 = 1,
                    Volume2 = 250,
                    OpenClose2 = Convert.ToByte('O'),
                    res = Convert.ToByte('2'),
                },
                obj_leg3 = new C_MS_SPD_LEG_INFO()
                {
                    token = 44517,
                    contract_obj = new C_Contract_Desc()
                    {
                        InstrumentName = "FUTIDX",
                        Symbol = "S&P500",
                        ExpiryDate = 1119191400,
                        StrikePrice = -1,
                        OptionType = "XX",
                        CALevel = 0
                    },
                    BuySell2 = 1,
                    Volume2 = 250,
                    OpenClose2 = Convert.ToByte('O'),
                    res = Convert.ToByte('V'),
                },
            };

            byte[] buffer = DataPacket.RawSerialize(obj);
            NNFHandler.Instance.Publisher(MessageType.ORDER, buffer);
        }

        public void TRADE_MOD_IN()  //-- 5445
        {

        }


        #endregion NNF IN Messages


    }
}

