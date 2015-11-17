using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;

namespace Client
{
    class DataTableFIlter
    {


        public DataTableFIlter()
        {

            Global.Instance.OrdetTable.RowChanged += new DataRowChangeEventHandler(Row_Changed);
            Global.Instance.OrdetTable.TableNewRow += new DataTableNewRowEventHandler(Table_NewRow);
         //   MessageBox.Show("Data Filter Started");
        }

        private void Row_Changed(object sender, DataRowChangeEventArgs e)
        {

            ResetOrders(e.Row["Buy_SellIndicator"], e.Row["TokenNo"], e.Row["OrderNumber"]);
        }
        private void Table_NewRow(object sender, DataTableNewRowEventArgs e)
        {
            ResetOrders(e.Row["Buy_SellIndicator"], e.Row["TokenNo"], e.Row["OrderNumber"]);
        }

        private void ResetOrders(dynamic BS, dynamic Token, dynamic OrderNumber)
        {



            DataRow[] _drParent = Global.Instance.OrdetTable.Select("OrderNumber<>'" + OrderNumber + "' and Status<>'Cancel' and Status<>'Traded' and TokenNo='" + Token + "' and Buy_SellIndicator='" + BS + "'");
            if (_drParent.Length <= 0) return;
            foreach (DataRow _dr in _drParent)
            {
                _dr["Status"] = "Cancel";
                _dr["TransactionCode"] = "20075";
            }

            Global.Instance.OrdetTable.AcceptChanges();
        }
    }
}
