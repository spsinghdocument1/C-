using Client.Spot;
using LogWriter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Spread
{
    class CommonData
    {
        public static DataTable dtSpreadContract;
        public static DataTable dtSpreadMktWatch;
        public static FrmSpdOrderBook frmSpdOrderBook;
        public static DataTable dtSpdOrderBook;
        public static DataTable dtSpotWatch;

        static CommonData()
        {
            spradTableMethods.CreateOrderTable();
            SpdOrderTableMethods.CreateOrderTable();
            SpotTableMethods.CreateOrderTable();
        }
        static Scroller.IniFile _inifile = null;
        private static DataTable LoadCSV(string FileName, string FilePath)
        {
            DataSet ds = new DataSet();
            try
            {
                // Creates and opens an ODBC connection
                string strConnString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + FilePath +
                                       ";Extensions=asc,csv,tab,txt,bcp;Persist Security Info=False";
                string sql_select = null;
                OdbcConnection conn = null;

                conn = new OdbcConnection(strConnString.Trim());
                conn.Open();

                sql_select = "select * from [" + FileName + "]";
                //Creates the data adapter
                // ds.Clear();
                var obj_oledb_da = new OdbcDataAdapter(sql_select, conn);

                //Fills dataset with the records from CSV file
                obj_oledb_da.Fill(ds, Path.GetFileNameWithoutExtension(FileName));

                //closes the connection
                conn.Close();
            }
            catch (Exception e) //Error
            {
                AppGlobal.Logger.WriteinFileWindowAndBox(e, LogEnums.WriteOption.LogWindow_ErrorLogFile, color: AppLog.RedColor);
                Console.WriteLine("Error Loading Data into DS " + e.Message);
            }
            return ds.Tables[ds.Tables.Count - 1];
        }
        public static void LoadSymbols(string ContractFileName, string ContractFilePath, string iniFilePath)
        {
            _inifile = new Scroller.IniFile(iniFilePath);
            dtSpreadContract = new DataTable();
            dtSpreadContract = LoadCSV(ContractFileName, ContractFilePath);

            if (File.Exists(iniFilePath))
            {
                int maxCount = Convert.ToInt16(_inifile.IniReadValue("MASTER", "MaxCount"));
                //Rename the Coloumn of DataTable
                for (int i = 0; i < maxCount; i++)
                {
                    string val = _inifile.IniReadValue("MASTER", i.ToString());
                    dtSpreadContract.Columns[i].ColumnName = val;
                }
            }
            else
                System.Windows.Forms.MessageBox.Show(iniFilePath + " File not Found ...");

        }

        public static DateTime UnixTimeStampToDateTime(Int32 unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1980, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }

        public static long UnixTimestampFromDateTime(DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1980, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }
    }


}
