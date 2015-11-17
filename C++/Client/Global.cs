using System;
using AMS.Profile;
using System.Data;
using System.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Structure;
using System.Threading;
using CashData;
using LzoNseFO;
using System.Windows.Forms;
using Client.Spread;


namespace Client
{
    public struct TradeTrac
    {

        public int PF_ID;
        public string SYMBOL;
        public int B_S;
        public int QTy;
        public double ACTUALPRICE;
        public double Given_Price_Sell;
        public double Given_Price_Buy;
        public string TIME;
    }
    public enum orderStatus
    {
        Open,
        Modified,
        Cancel,
        Traded,
        Rejected
    }
    
    enum SYSTEMSTATUS
    {
        NONE = 0,
        LOGGEDIN = 1,
        LOGGEDOUT = 2,
        DATARUNNING = 3,
        DATASTOPPED = 4,
        PASSERROR=5,
        PASSEXPIRE=6

    }
    enum ClassType
    {
        MARKETWTCH=0,
        SPREAD=1,
        INDICES=2
    }
    public sealed class Global
    {
        internal string DataConIp;
        internal string DataConSUBPort;
        internal string NNFConIp;
        internal string NNFConPUBPort;
        internal string NNFConSUBPort;
        internal string LanIp;
        internal string LZOIP;
        internal string LZOPORT;
        internal string INDEXIP;
        internal string INDEXPORT;
        internal string C_Type;
        internal int ClientId;
        internal string NNFPassword;
        internal string Net_File;
        internal bool SignInStatus;
        internal bool Pass_bool;
        internal bool warningvar;
        internal bool Fopairbool;
        internal bool Relogin;
        internal bool stop_all;
        internal bool Best_Bid;
        internal Int16 WTC_cnt;
        internal bool ReloginFarmloader;
        internal bool write;
        internal Int32 interval;
        internal DataRow dr;
        internal DataTable OrdetTable;
        internal DataTable Server_SpreadTable;

        internal DataTable TradeTracker;
        internal DataTable Child_Index;
        internal string APPTYPE = "";
        internal static frmLogin loginfrmobj = null;
        private static readonly Global instance = new Global();
        internal  static DateTime LastTime = System.DateTime.Now;
        internal static bool flag =  false;
        internal ConcurrentDictionary<int, Double> MTMDIct = new ConcurrentDictionary<int, Double>();
        internal ConcurrentDictionary<string, int> Ratio = new ConcurrentDictionary<string, int>();
        internal ConcurrentDictionary<string,string>Child_Index_Dict = new ConcurrentDictionary<string,string>();
        internal ConcurrentDictionary<long, ClassType> Data_With_Nano = new ConcurrentDictionary<long, ClassType>();
        internal ConcurrentDictionary<int, TradeTrac> TradeTrac_dict = new ConcurrentDictionary<int, TradeTrac>();
        internal ConcurrentDictionary<int, int> _Netbook = new ConcurrentDictionary<int, int>();
       // internal ConcurrentDictionary<string, DataGridViewRow> _SprdwatchDict = new ConcurrentDictionary<string, DataGridViewRow>();
        internal Dictionary<string, DataGridViewRow> _SprdwatchDict = new Dictionary<string, DataGridViewRow>();
        internal Dictionary<string, DataGridViewRow> _IndexwatchDict = new Dictionary<string, DataGridViewRow>();
        public static Global Instance
        {
            get
            {
                return instance;
            }
        }
       
      //  Thread thDataFilter;
        internal xSocket CashSock = new xSocket();
        internal LzoCashData cashDataSection; 
        public ConcurrentDictionary<int, int> BoardLotDict = new ConcurrentDictionary<int, int>();
        private Global()
        {
            OrdetTable = new DataTable();
            OrdetTable = ReadyOrderBook();

            TradeTracker = new DataTable();
            TradeTracker = ReadyDatatable();
            Child_Index = new DataTable();
            Child_Index = child_index_Datatable();
            var config = new Config { GroupName = null };
            DataConIp = config.GetValue("appSettings", "DataConIp", null);
            DataConSUBPort = config.GetValue("appSettings", "DataConSUBPort", null);
            NNFConIp = config.GetValue("appSettings", "NNFConIp", null);
            NNFConPUBPort = config.GetValue("appSettings", "NNFConPUBPort", null);
            NNFConSUBPort = config.GetValue("appSettings", "NNFConSUBPort", null);
            LanIp = config.GetValue("appSettings", "LanIp", null);
            ClientId =Convert.ToInt32(config.GetValue("appSettings", "ClientId", null));
            LZOIP = config.GetValue("appSettings", "LZOIP", null);
            LZOPORT = config.GetValue("appSettings", "LZOPORT", null);
            INDEXIP = config.GetValue("appSettings", "INDEXIP", null);
            INDEXPORT = config.GetValue("appSettings", "INDEXPORT", null);
            APPTYPE = config.GetValue("appSettings", "APPTYPE", null);
            interval = Convert.ToInt32(config.GetValue("appSettings", "Interval", null));



            cashDataSection = new LzoCashData();
            CashSock.DataArrival += cashDataSection.OnDataArrival;

            //pData = new Primary();

           // FoSock.DataArrival += pData.OnDataArrival;
         //   thDataFilter = new Thread(new ThreadStart(StartMyThread));
           // thDataFilter.Start();

            //	NNFPassword=config.GetValue("Profile","NNFPassword",null);

            //Console.WriteLine("DataConIp :" + DataConIp);
            //Console.WriteLine("DataConSUBPort :" + DataConSUBPort);
            //Console.WriteLine("NNFConIp :" + NNFConIp);
            //Console.WriteLine("NNFConPUBPort :" + NNFConPUBPort);
            //Console.WriteLine("NNFConSUBPort :" + NNFConSUBPort);
            //Console.WriteLine("ClientId :" + ClientId);
        }
        DataTableFIlter _DTFIlter;

        private DataTable ReadyDatatable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PF_ID", typeof(string));
            dt.Columns.Add("SYMBOL", typeof(string));
            dt.Columns.Add("B/S", typeof(string));
            dt.Columns.Add("QTY", typeof(string));
            dt.Columns.Add("ACTUALPRICE", typeof(string));
            dt.Columns.Add("GIVENPRICESEll", typeof(string));
            dt.Columns.Add("GIVENPRICEBUY", typeof(string));
            dt.Columns.Add("TIME", typeof(string));
            return dt;
        }

        private DataTable child_index_Datatable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(SpreadContract.Symbol, typeof(string));
            dt.Columns.Add(SpreadContract.Price, typeof(string));
            dt.Columns.Add(SpreadContract.ChangeIndicator, typeof(string));
            dt.Columns.Add(SpreadContract.PercentChange, typeof(string));
            dt.Columns.Add(SpreadContract.ClosePrice, typeof(string));
            return dt;
        }
        private void StartMyThread()
        {
            _DTFIlter = new DataTableFIlter();

        }
        public DataColumn GetColumn(string ColumnName, string DataType, Boolean KeepNull = true,
           Boolean KeepUnique = false)
        {
            DataColumn dc = null;
            dc = new DataColumn();
            dc.AllowDBNull = KeepNull;
            dc.ColumnName = ColumnName.Trim();
            
            dc.DataType = Type.GetType("System." + DataType);
            dc.Unique = KeepUnique;
            //dc.Caption  = "Abc" + ColumnName;
            return dc;
        }

        public long ReturnValue(MessageType STYPE, string nnf)
        {
            return Convert.ToInt64(Convert.ToByte(STYPE) + nnf);
        }
      



        public DataTable ReadyOrderBook()
        {
            DataTable dt = new DataTable();
      
            dt.Columns.Add("InstrumentName", typeof(String));
            dt.Columns.Add("FullName", typeof(String));
            dt.Columns.Add("Symbol", typeof(String));
            dt.Columns.Add("TokenNo", typeof(String));   // int32
            dt.Columns.Add("Buy_SellIndicator", typeof(String));
            dt.Columns.Add("OptionType", typeof(String));
            dt.Columns.Add("StrikePrice", typeof(Int32));
            dt.Columns.Add("Price", typeof(String));  //  double
            dt.Columns.Add("FillPrice", typeof(String));
            dt.Columns.Add("FillNumber", typeof(long));
            dt.Columns.Add("Volume", typeof(String));
            dt.Columns.Add("Status", typeof(String));         
            dt.Columns.Add("AccountNumber", typeof(String));
            dt.Columns.Add("BookType", typeof(String));
            dt.Columns.Add("BranchId", typeof(Int16));
            dt.Columns.Add("BrokerId", typeof(String));
            dt.Columns.Add("CloseoutFlag", typeof(String));
            dt.Columns.Add("ExpiryDate", typeof(String));
            dt.Columns.Add("DisclosedVolume", typeof(Int32));
            dt.Columns.Add("DisclosedVolumeRemaining", typeof(Int32));
            dt.Columns.Add("EntryDateTime", typeof(String));
            dt.Columns.Add("filler", typeof(Int32));
            dt.Columns.Add("GoodTillDate", typeof(String));
            dt.Columns.Add("LastModified", typeof(String));
            dt.Columns.Add("LogTime", typeof(DateTime));
            dt.Columns.Add("Modified_CancelledBy", typeof(Char));
            dt.Columns.Add("nnffield", typeof(Int64));
            dt.Columns.Add("Open_Close", typeof(String));
            dt.Columns.Add("OrderNumber", typeof(String));
            dt.Columns.Add("RejectReason", typeof(String));
            dt.Columns.Add("Pro_ClientIndicator", typeof(Int16));
            dt.Columns.Add("ReasonCode", typeof(Int16));
            dt.Columns.Add("Settlor", typeof(String));
            dt.Columns.Add("TimeStamp1", typeof(string));
            dt.Columns.Add("TimeStamp2", typeof(string));
            dt.Columns.Add("TotalVolumeRemaining", typeof(Int32));
            dt.Columns.Add("TraderId", typeof(Int32));
            dt.Columns.Add("TransactionCode", typeof(Int16));
            dt.Columns.Add("UserId", typeof(Int32));
            dt.Columns.Add("VolumeFilledToday", typeof(string));
            dt.Columns.Add("Unique_id", typeof(string));
            dt.PrimaryKey = new[] { dt.Columns["Unique_id"] };
            return dt;
        }

        //public DataTable ReadyOrderBook()
        //{
        //    DataTable dtOrderBook = new DataTable();
        //    dtOrderBook.TableName = "OrderBook";
        //    dtOrderBook.Columns.Add(GetColumn("AccountNumber", "String"));
        //    dtOrderBook.Columns.Add(GetColumn("BookType", "String"));

        //    dtOrderBook.Columns.Add(GetColumn("BranchId", "Int16"));
        //    dtOrderBook.Columns.Add(GetColumn("BrokerId", "String"));
        //    dtOrderBook.Columns.Add(GetColumn("Buy_SellIndicator", "String"));
        //    dtOrderBook.Columns.Add(GetColumn("CloseoutFlag", "String"));
        //    dtOrderBook.Columns.Add(GetColumn("CompetitorPeriod", "Int16"));

        //    dtOrderBook.Columns.Add(GetColumn("ExpiryDate", "Int32"));
        //    dtOrderBook.Columns.Add(GetColumn("InstrumentName", "String"));
        //    dtOrderBook.Columns.Add(GetColumn("OptionType", "String"));
        //    dtOrderBook.Columns.Add(GetColumn("StrikePrice", "Int32"));
        //    dtOrderBook.Columns.Add(GetColumn("Symbol", "String"));


        //    dtOrderBook.Columns.Add(GetColumn("cOrdFiller", "String"));
        //    dtOrderBook.Columns.Add(GetColumn("CounterPartyBrokerId", "String"));
        //    dtOrderBook.Columns.Add(GetColumn("DisclosedVolume", "Int32"));
        //    dtOrderBook.Columns.Add(GetColumn("DisclosedVolumeRemaining", "Int32"));
        //    dtOrderBook.Columns.Add(GetColumn("EntryDateTime", "Int32"));
        //    dtOrderBook.Columns.Add(GetColumn("LastModified", "Int32"));
        //    dtOrderBook.Columns.Add(GetColumn("MinimumFill_AONVolumel", "Int32"));
        //    dtOrderBook.Columns.Add(GetColumn("mkt_replay", "Double"));


        //    dtOrderBook.Columns.Add(GetColumn("Modified_CancelledBy", "String"));
        //    dtOrderBook.Columns.Add(GetColumn("nnffield", "Double"));
        //    dtOrderBook.Columns.Add(GetColumn("Open_Close", "String"));
        //    dtOrderBook.Columns.Add(GetColumn("OrderNumber", "Double"));
        //    dtOrderBook.Columns.Add(GetColumn("OrderType", "Int16"));
        //    dtOrderBook.Columns.Add(GetColumn("ParticipantType", "String"));
        //    dtOrderBook.Columns.Add(GetColumn("Price", "Int32"));

        //    dtOrderBook.Columns.Add(GetColumn("Pro_ClientIndicator", "String"));
        //    dtOrderBook.Columns.Add(GetColumn("ReasonCode", "String"));
        //    dtOrderBook.Columns.Add(GetColumn("SettlementPeriod", "Int16"));
        //    dtOrderBook.Columns.Add(GetColumn("Settlor", "String"));
        //    dtOrderBook.Columns.Add(GetColumn("SolicitorPeriod", "Int16"));
        //    dtOrderBook.Columns.Add(GetColumn("TokenNo", "Int32"));
        //    dtOrderBook.Columns.Add(GetColumn("TotalVolumeRemaining", "Int32"));
        //    dtOrderBook.Columns.Add(GetColumn("TraderId", "Int32"));
        //    dtOrderBook.Columns.Add(GetColumn("TriggerPrice", "Int32"));
        //    dtOrderBook.Columns.Add(GetColumn("Volume", "Int32"));
        //    dtOrderBook.Columns.Add(GetColumn("VolumeFilledToday", "Int32"));

        //    return dtOrderBook;

        //  //  dtOrderBook.PrimaryKey = new[] { dtOrderBook.Columns["ExcOrderID"] 
        //    }
        }



/*
    public class EditorFontData : ConfigurationSection
    {
        public EditorFontData()
        {
        }

        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("size")]

        public float Size
        {
            get { return (float)this["size"]; }
            set { this["size"] = value; }
        }

        [ConfigurationProperty("style")]
        public int Style
        {
            get { return (int)this["style"]; }
            set { this["style"] = value; }
        }
    }
    
    */

}
