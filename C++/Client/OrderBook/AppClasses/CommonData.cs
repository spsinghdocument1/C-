using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderBook.GUI;
using OrderBook.AppClasses;

namespace OrderBook.CommonData
{
    class CommonData
    {
        public static FrmOrderBook frmOrderBook;
        public static DataTable dtOrderBook;


        public static string LoginId;

        static CommonData()
        {
            OrderTableMethods.CreateOrderTable();
        }
     

    }
}
