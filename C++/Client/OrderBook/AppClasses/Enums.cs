using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBook.Enums
{
    class Enums
    {
        public enum BuySell
        {
            BUY = 1,
            SELL = 2,
        }
        public enum OrderType
        {
            Market=1,
            Limit=2,
            Stop=3,
            StopLimit=4,
            PreOpenOrder='P'
        }

        public enum orderStatus
        {
            Open,
            Modified,
            Cancel,
            Traded,
            Rejected
        }

       
        public enum ExecTransType
        {
            NEW=0,
            CANCEL=1,
            CORRECT=2,
            STATUS=3
        }
        public enum ExecType
        {
            NEW = 0,
            PARTIAL_FILL=1,
            FILL=2,
            DONE_FOR_DAY=3,
            CANCELED=4,
            REPLACE=5,
            PENDING_CANCEL=6,
            STOPPED=7,
            REJECTED=8,
            SUSPENDED=9,
            PENDING_NEW='A',
            RESTATED='D',
            PENDING_REPLACE='E',
            Sync='S'
        }
      
       


    }
}
