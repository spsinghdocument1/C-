using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Spread
{
    public abstract class SpreadContract
    {
        public const string Token1 = "Token1";
        public const string Token2 = "Token2";
        public const string BuySell1 = "BuySell1";
        public const string BuySell2 = "BuySell2";

        public const string InstrumentName1 = "InstrumentName1";
        public const string Symbol1 = "Symbol1";
        public const string OrderId = "OrderId";
        public const string Series1 = "Series1";
        public const string ExpiryDate1 = "ExpiryDate1";
        public const string StrikePrice1 = "StrikePrice1";
        public const string OptionType1 = "OptionType1";
        public const string CALevel1 = "CALevel1";
        public const string InstrumentName2 = "InstrumentName2";
        public const string Symbol2 = "Symbol2";
        public const string Series2 = "Series2";
        public const string ExpiryDate2 = "ExpiryDate2";
        public const string StrikePrice2 = "StrikePrice2";
        public const string OptionType2 = "OptionType2";
        public const string CALevel2 = "CALevel2";
        public const string ReferencePrice = "ReferencePrice";
        public const string DayLowPriceDiffRange = "DayLowPriceDiffRange";
        public const string DayHighPriceDiffRange = "DayHighPriceDiffRange";
        public const string OpLowPriceDiffRange = "OpLowPriceDiffRange";
        public const string OpHighPriceDiffRange = "OpHighPriceDiffRange";
        public const string BoardLotQuantity1 = "BoardLotQuantity1";
        public const string MinimumLotQuantity1 = "MinimumLotQuantity1";
        public const string TickSize1 = "TickSize1";
        public const string BoardLotQuantity2 = "BoardLotQuantity2";
        public const string MinimumLotQuantity2 = "MinimumLotQuantity2";
        public const string TickSize2 = "TickSize2";
        public const string Eligibility = "Eligibility";
        public const string DeleteFlag = "DeleteFlag";
        public const string UnixExpiry1 = "ExpTime1";
        public const string UnixExpiry2 = "ExpTime2";
        public const string TradedVolume = "TradedVolume";
        public const string Symbol = "Symbol";
        public const string Exchange = "Exchange";
        public const string ExpiryDate = "ExpiryDate";
        public const string LastTradeTime = "LastTradeTime";
        public const string LastUpdateTime = "LastUpdateTime";
        public const string PerChange = "PerChange";
        public const string OrderType = "OrderType";
        public const string BidQ = "BidQ";
        public const string Bid = "Bid";
        public const string TotalBuyQty = "TotalBuyQty";
        public const string AskQ = "AskQ";
        public const string Ask = "Ask";
        public const string PercentChange = "%Change";
        public const string TotalSellQty = "TotalSellQty";
        public const string LTP = "LTP";
        public const string TotalQtyTraded = "TotalQtyTraded";
        public const string LTQ = "LTQ";
        public const string AverageTradedPrice = "AverageTradedPrice";
        public const string StrikePrice = "StrikePrice";
        public const string Multiplier = "Multiplier";
        public const string DprHigh = "DprHigh";
        public const string DprLow = "DprLow";
        public const string PriceTick = "PriceTick";
        public const string OpenPrice = "OpenPrice";
        public const string HighPrice = "HighPrice";
        public const string LowPrice = "LowPrice";
        public const string ClosePrice = "ClosePrice";
        public const string ATP = "ATP";
        public const string TotalTradedValue = "TotalTradedValue";
        public const string LastActiveTime = "LastActiveTime";

        //Order Details
        public const string Buy_SellIndicator = "Buy/Sell";
        public const string FillPrice = "FillPrice";
        public const string FillNumber = "FillNumber";
        public const string Volume = "Volume";
        public const string Status = "Status";
        public const string AccountNumber = "AccountNumber";
        public const string BookType = "BookType";
        public const string BranchId = "BranchId";
        public const string BrokerId = "BrokerId";
        public const string CloseoutFlag = "CloseoutFlag";
        public const string DisclosedVolumeRemaining = "DisclosedVolumeRemaining";
        public const string EntryDateTime = "EntryDateTime";

        public const string filler = "filler";
        public const string GoodTillDate = "GoodTillDate";
        public const string LastModified = "LastModified";
        public const string LogTime = "LogTime";

        public const string Modified_CancelledBy = "Modified_CancelledBy";
        public const string nnffield = "nnffield";
        public const string Open_Close = "Open_Close";
        public const string RejectReason = "RejectReason";
        public const string Pro_ClientIndicator = "Pro_ClientIndicator";
        public const string ReasonCode = "ReasonCode";
        public const string Settlor = "Settlor";
        public const string TraderId = "TraderId";
        public const string TransactionCode = "TransactionCode";
        public const string UserId = "UserId";
        public const string VolumeFilledToday = "VolumeFilledToday";
        public const string Unique_id = "UniqueId";
        public const string TotalVolumeRemaining = "TotalVolumeRemaining";
        public const string Price1 = "Price1";
        public const string Price2 = "Price2";
        public const string Price = "Price";
        public const string Price_Diff = "Price_Diff";
        public const string BidQ_leg2 = "BidQ_leg2";
        public const string AskQ_leg2 = "AskQ_leg2";
        public const string ChangeIndicator = "Indicator";

    }
}
