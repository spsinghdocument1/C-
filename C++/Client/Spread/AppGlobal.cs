using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogWriter;
using System.Windows.Forms;
using FormatWriter;
using Client.Spot;

namespace Client.Spread
{
    class AppGlobal
    {
        public static frmSpreadMktWatch frmSpreadWatch;
        public static FrmSpdOrderBook frmSpdOBook;
        public static frmSpot frmSpotIndex;
        public static frmMarketDepth frmMarketDpth;
        public static FrmMarketPicture frmMarketPicture;
        public static AppLog AppLogger;
        public static AppErrorLog Logger;
        public static LogFormatter FormatWriter;

        static AppGlobal()
        {
            AppLogger = new AppLog();
            Logger = new AppErrorLog();
            FormatWriter = new LogFormatter(ref Logger);
            AppLogger.GenerateLogfiles(Application.StartupPath + "\\" + Application.ProductName + "\\ApplicationLog", "Trace_" + Application.ProductName);
            Logger.GenerateLogfiles(Application.StartupPath + "\\" + Application.ProductName + "\\ApplicationLog", Application.ProductName);
        }
        public static void DisposeApp()
        {
            AppLogger.CloseLogfiles();
            AppLogger = null;
            Logger.CloseLogfiles();
            Logger = null;
            FormatWriter.dispose();
        }
    }
}
