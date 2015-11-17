using Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Client
{
    class GStruct
    {
    }
    #region NSE FO Struct

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct cCompData
    {
        public short iCompLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 506)]
        public byte[] buffer;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 2)]
    public struct Data
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] cNetId;
        public short iNoPackets;
        public cCompData data;
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 2)]
    public struct MS_BCAST_INDICES_7207
    {
        public MESSAGE_HEADER BcastHeader;
        public short NumberOfRecords;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
        public MS_INDICES_7207[] Indices;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct MESSAGE_HEADER
    {
        public Int16 iApiTcode;
        public Int16 iApiFuncId;
        public Int32 LogTime;
        public short AlphaChar;
        public short TransactionCode;
        public short ErrorCode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] TimeStamp;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] TimeStamp1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] TimeStamp2;
        public short MessageLength;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 2)]
    public struct MBP_INFORMATION
    {
        public Int32 Quantity;
        public Int32 Price;
        public short NumberOfOrders;
        public short BbBuySellFlag;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 2)]
    public struct INTERACTIVE_ONLY_MBP
    {
        public int Token;
        public short BookType;
        public short TradingStatus;
        public int VolumeTradedToday;
        public int LastTradedPrice;
        public byte NetChangeIndicator;
        public int NetPriceChangeFromClosingPrice;
        public int LastTradeQuantity;
        public int LastTradeTime;
        public int AverageTradePrice;
        public short AuctionNumber;
        public short AuctionStatus;
        public short InitiatorType;
        public int InitiatorPrice;
        public int InitiatorQuantity;
        public int AuctionPrice;
        public int AuctionQuantity;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
        public MBP_INFORMATION[] RecordBuffer;
        public short BbTotalBuyFlag;
        public short BbTotalSellFlag;
        public double TotalBuyQuantity;
        public double TotalSellQuantity;
        public short Indicator;
        public int ClosingPrice;
        public int OpenPrice;
        public int HighPrice;
        public int LowPrice;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 2)]
    public struct BROADCAST_ONLY_MBP
    {
        public MESSAGE_HEADER mHeader;
        public short NoOfRecords;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
        public INTERACTIVE_ONLY_MBP[] Data;
    }

    /// <summary>
    /// BCAST_INDUSTRY_INDEX_UPDATE (
    /// 
    /// ) 
    /// </summary>

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    public struct INDUSTRY_INDICES
    {
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 15)]
        public string IndustryName;
        public int IndexValue;
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    public struct MS_BCAST_VCT_MSGS
    {
        /* 1) BC_OPEN_MSG (6511). This is sent when the market is opened.
	   2) BC_CLOSE_MSG (6521). This is sent when the market is closed.
	   3) BC_PRE_OR_POST_DAY_MSG (6531). This is sent when the market is preopened.
	   4) BC_PRE_OPEN_ENDED (6571). This is sent when the pre-open period ends.
	   5) EQUAL BC_POSTCLOSE_MSG (6522). This is sent when the Market is in Postclose session.*/
        //Packet Length: 320 bytes
        
        public BCAST_HEADER BcastHeader;
        public long Token;
        public SEC_INFO SecInfo;
        public short MarketType;
        public short ST_BCAST_DESTINATION;
        public short BroadcastMessageLength;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 239)]
        public string BroadcastMessage;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    public struct BCAST_HEADER
    {
        public int Reserved;
        public int LogTime;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 2)]
        public string AlphaChar;
        public short TransCode;
        public short ErrorCode;
        public int BCSeqNo;
        public int Reserved1;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 8)]
        public string TimeStamp2;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 8)]
        public string Filler2;
        public short MessageLength;
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 2)]
    public struct BCAST_INDUSTRY_INDEX_UPDATE_7203
    {
        public MESSAGE_HEADER mHeader;
        public short NoOfRecs;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
        public INDUSTRY_INDICES[] sIndustry;
    }

    /// <summary>
    /// MS_TICKER_TRADE_DATA(7202)
    /// </summary>
    /// <param name="Data"></param>
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi, Pack = 2)]
    public struct MS_INDICES_7207
    {
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 21)]
        public string IndexName;
        public int IndexValue;
        public int HighIndexValue;
        public int LowIndexValue;
        public int OpeningIndex;
        public int ClosingIndex;
        public int PercentChange;
        public int YearlyHigh;
        public int YearlyLow;
        public int NoOfUpmoves;
        public int NoOfDownmoves;
        public double MarketCapitalisation;
        public byte NetChangeIndicator;
        public byte Reserved;
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 2)]
    public struct ST_TICKER_INDEX_INFO
    {
        public int Token;
        public short MarketType;
        public int FillPrice;
        public int FillVolume;
        public int OpenInterest;
        public int DayHiOI;
        public int DayLoOI;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 2)]
    public struct MS_TICKER_TRADE_DATA_7202
    {
        public MESSAGE_HEADER mHeader;
        public short NumberRecords;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 17, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
        public ST_TICKER_INDEX_INFO[] indexinfo;
    }

    /// <summary>
    /// For Spread Calculation structure
    /// </summary>
    /// <param name="Data"></param>


    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 2)]
    public struct MbpDepth
    {
        public short NoOrders;
        public int Volume;
        public int Price;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 2)]
    public struct TotalOrderVolume
    {
        public double Buy;
        public double Sell;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 2)]
    public struct MS_SPD_MKT_INFO_7211
    {
        public MESSAGE_HEADER mHeader;
        public int Token1;
        public int Token2;
        public short MbpBuy;
        public short MbpSell;
        public int LastActiveTime;
        public int TradedVolume;
        public double TotalTradedValue;

        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
        public MbpDepth[] mbpBuys;

        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
        public MbpDepth[] mbpSells;

        public TotalOrderVolume totalOrdVolume;
        public int OpenPriceDifference;
        public int DayHighPriceDifference;
        public int DayLowPriceDifference;
        public int LastTradedPriceDifference;
        public int LastUpdateTime;
    }

    #endregion

    struct SendData
    {
        public int Token, Bid1, Ask1, LTP;
    }

}
