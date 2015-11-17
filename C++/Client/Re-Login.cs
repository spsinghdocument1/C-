using AMS.Profile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Re_Login : Form
    {
        List<Task> tasks = new List<Task>();
        public Re_Login()
        {
            InitializeComponent();
            Global.Instance.NNFPassword = "";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var config = new Config { GroupName = null };     

          string orderip = config.GetValue("appSettings", "NNFConIp", null);
           string  tbPort= config.GetValue("appSettings", "NNFConSUBPort", null);

            TcpClient tcpSocket = null;
            try
            {
                tcpSocket = new TcpClient(orderip, Convert.ToInt32(tbPort));
            }
            catch (SocketException SE)
            {
                MessageBox.Show("Message~~> " + SE.Message, "Server is Stopped", MessageBoxButtons.OK, MessageBoxIcon.Information);
               if(tcpSocket!=null)
                tcpSocket.Close();
                return;
            }

            Global.Instance.Relogin = true;
            Global.Instance.NNFPassword = textBox1.Text;
         //   if (NNFHandler.flag ==  false)
            NNFHandler.Instance._socketfun();
            NNFInOut.Instance.SIGN_ON_REQUEST_IN();
            Thread.Sleep(2000);
         
          //  tasks.Add(Task.Factory.StartNew(() =>
          //  {
          //      Task.Factory.StartNew(() => NNFHandler.Instance.Subscriber());
          //  }
          //));

            //Task.WaitAll(tasks.ToArray());
            Global.Instance.ReloginFarmloader = true;
            this.Close();
            ///
           
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" Login Failed ", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Dispose();
            Environment.Exit(1);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)       
        {
            if(e.KeyCode ==  Keys.Enter)
            {
                btnLogin_Click(sender, e);
            
            }
        }
    }
}
