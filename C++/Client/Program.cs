using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;
using System.Diagnostics;
using Structure;
using System.Runtime.InteropServices;
using System.Threading;
using AMS.Profile;
using System.Windows.Forms;
using System.Data;

namespace Client
{

    internal class MainClass
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");
        [STAThread]
        private static void Main()
        {

            //MessageBox.Show("size :" + System.Runtime.InteropServices.Marshal.SizeOf(new C_OrderReject()).ToString());
            //return;

            //    byte[] buffer = DataPacket.RawSerialize(new FOPAIR()
            //    {
            //        PORTFOLIONAME = 55,
            //        TokenNear = 55,
            //        TokenFar = 77
            //    }
            //);

            //    var v = (FOPAIR)DataPacket.RawDeserialize(buffer, typeof(FOPAIR));

            //Application.Run(new MDIParent1());
           //return;
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MDIParent1());
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("only one instance at a time");
            }

            //var frmServer = new Form1();
            //var frmClient = new Form2Client();
            //frmServer.Show();
            //frmClient.Show();
            // MDIParent1 obj = new MDIParent1();
            // obj.Text =Global.Instance.ClientId+" =>Running Form";
            // Show the instance of the form modally.
            // obj.ShowDialog();
           
            // MessageBox.Show("MS_OE_REQUEST_TR :"+System.Runtime.InteropServices.Marshal.SizeOf(new MS_OE_REQUEST_TR()).ToString());
            // MessageBox.Show("MS_OE_RESPONSE_TR :"+System.Runtime.InteropServices.Marshal.SizeOf(new MS_OE_RESPONSE_TR()).ToString());
            // MessageBox.Show("MS_OM_REQUEST_TR :"+System.Runtime.InteropServices.Marshal.SizeOf(new MS_OM_REQUEST_TR()).ToString());
            /*   MessageBox.Show(System.Runtime.InteropServices.Marshal.SizeOf(new MS_SPD_OE_REQUEST()).ToString());
               MessageBox.Show(System.Runtime.InteropServices.Marshal.SizeOf(new MS_SPD_LEG_INFO()).ToString());
               MessageBox.Show(System.Runtime.InteropServices.Marshal.SizeOf(new ST_ORDER_FLAGS()).ToString());
               MessageBox.Show(System.Runtime.InteropServices.Marshal.SizeOf(new CONTRACT_DESC()).ToString());
               MessageBox.Show(System.Runtime.InteropServices.Marshal.SizeOf(new Message_Header()).ToString());
               MessageBox.Show(System.Runtime.InteropServices.Marshal.SizeOf(new ST_ORDER_FLAGS()).ToString());
               MessageBox.Show(System.Runtime.InteropServices.Marshal.SizeOf(new ADDITIONAL_ORDER_FLAGS()).ToString());*/
            // Application.Run(new MDIParent1());
            //Application.Run(obj);

        }

    }
  
}
