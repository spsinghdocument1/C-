using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AMS.Profile;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Net.Sockets;
using Client.LZO_NanoData;

namespace Client
{

    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        public void btnConnect_Click(object sender, EventArgs e)
        {
           
            // var config = new Config { GroupName = null };
            //if(!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"login.txt")))
            //{
            //     string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"login.txt");
            //                File.WriteAllText(destPath,"try");
            //     string login = DateTime.Now.ToString();
            //    config.SetValue("Login", Convert.ToString(0), login);
            //}
           
               
            //    var iforms = config.GetValue("Login", Convert.ToString(0));
            //    DateTime dt = Convert.ToDateTime(Convert.ToString(iforms));

            //  // var date = dt.AddDays(3);
            //   var diff = (DateTime.Today-dt).Days;
            //   //var diff = (date- dt).Days;
            //    if (diff >= 3)
            //    {
            //        this.DialogResult = DialogResult.Cancel;
            //        return;



            //    }
            
           
                Global.Instance.NNFPassword = txtPassword.Text;
                List<Task> tasks = new List<Task>();
                NNFInOut.Instance.SIGN_ON_REQUEST_IN();
                if (cbDownHistory.CheckState == CheckState.Checked)
                {
                    StartDownload();
                    //File.Copy(@"\\192.168.168.36\share\vSphere\vSphere.exe", "E:\\abc.xml", true);
                }
              
         Thread.Sleep(8000);
         //Global.Instance.SignInStatus = true;

            //   LzoNanoData.Instance.UDPReciever();   
           //  UDP_Reciever.Instance.UDPReciever(); 
                if (Global.Instance.SignInStatus)
                {
                    this.DialogResult = DialogResult.OK;
                    
                }
                else

                {
                    this.DialogResult = DialogResult.Cancel;
                                      
                }

                

            
        }

        private void StartDownload(int Section = 0, string FileType = "")
        {

            pBarProgress.Value = 0;
            string remoteUri = "http://192.168.168.97:5252/files/";
            string fileName = "001.xml", myStringWebResource = null;
            // Create a new WebClient instance.
            WebClient myWebClient = new WebClient();
          //  myWebClient.DownloadFileCompleted += Completed;
            myWebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            myWebClient.DownloadProgressChanged += ProgressChanged;
            myWebClient.DownloadFileCompleted += Completed;           
            myStringWebResource = remoteUri + fileName;
            pBarProgress.Value = 1;
            myWebClient.DownloadFile(myStringWebResource, fileName);
            pBarProgress.Value = 100;
         
            return;

            #region  comment
            //String FilePath = "";
            //if (Section == 0)
            //{
            //    FilePath = IniFIle.IniReadValue("DOWNLOAD", "SERVER").Trim();
            //}
            //else
            //{
            //    FilePath = IniFIle.IniReadValue("DOWNLOAD", "HISTORY").Trim() + "//" +
            //               DateTime.Today.ToString("yyyy//MM//dd//") + Section + "//" + Section + FileType + ".csv";
            //}

            //if (FilePath.Length > 0)
            //{
             //   var webClient = new WebClient();
            
                //frmdown = new frmDownloadProgress();
               // frmdown.lbl_ItemName.Text = "Contract files :";
                //webClient.DownloadFileCompleted += Completed;
                //  webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
              //  webClient.DownloadProgressChanged += ProgressChanged;
             ///   webClient.DownloadFileCompleted += Completed;
             ///   AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
             //   NetworkCredential myCredentials = new NetworkCredential("other", "123456");

             //   WindowsIdentity idnt = new WindowsIdentity("other", "123456");

               // WindowsImpersonationContext context = idnt.Impersonate();
            //    File.Copy(@"\\192.168.168.97\share e\contract.txt", "E:\\abc211.txt", true);




            // File.Copy("E:\\02022015\\001.xml", "E:\\abc.xml", true);
            #endregion

        }
          public void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            pBarProgress.Value = e.ProgressPercentage;
        }

        public void Completed(object sender, AsyncCompletedEventArgs e)
        {
            Close();
        }
    
        /*
        private void frmLogin_Load(object sender, EventArgs e)
        {
        //EditorFontData configData =  new EditorFontData();
          EditorFontData configData = (EditorFontData)ConfigurationManager.GetSection("EditorSettings");
          var v = configData.Style;
          configData.Name = "Arial";
          configData.Size = 20;
          configData.Style = 2;
          Configuration config =ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.None);
        //You need to remove the old settings object before you can replace it
          config.Sections.Remove("EditorSettings");
       //with an updated one
          config.Sections.Add("EditorSettings", configData);
       //Write the new configuration data to the XML file
          config.Save();
          var v2 = configData.Style;
      //Get the application configuration file.
     //System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
    //Get the collection of the section groups.
   //ConfigurationSectionGroupCollection sectionGroups = config.SectionGroups;
  //Show the configuration values
 //ShowSectionGroupCollectionInfo(sectionGroups);






                foreach (string key in ConfigurationManager.AppSettings)
                {
                    string value = ConfigurationManager.AppSettings[key];
                    Console.WriteLine("Key: {0}, Value: {1}", key, value);
                }
                string fval = System.Configuration.ConfigurationManager.AppSettings["DataConIp"].ToString();
              
          


        }



        static void ShowSectionGroupCollectionInfo( ConfigurationSectionGroupCollection sectionGroups)
        {
            ClientSettingsSection clientSection;
            SettingValueElement value;

            foreach (ConfigurationSectionGroup group in sectionGroups)
            // Loop over all groups
            {
                if (!group.IsDeclared)
                    // Only the ones which are actually defined in app.config
                    continue;

                Console.WriteLine("Group {0}", group.Name);

                // get all sections inside group
                foreach (ConfigurationSection section in group.Sections)
                {
                    clientSection = section as ClientSettingsSection;
                    Console.WriteLine("\tSection: {0}", section);

                    if (clientSection == null)
                        continue;


                    foreach (SettingElement set in clientSection.Settings)
                    {
                        value = set.Value as SettingValueElement;
                        // print out value of each section
                        Console.WriteLine("\t\t{0}: {1}",
                        set.Name, value.ValueXml.InnerText);
                    }
                }
            }
        }
    */

        private void frmLogin_Load(object sender, EventArgs e)
        {
            var config = new Config { GroupName = null };
            tbServerIP.Text = config.GetValue("appSettings", "DataConIp", null);
            tbdataport.Text = config.GetValue("appSettings", "DataConSUBPort", null);

            tbOrderIp.Text = config.GetValue("appSettings", "NNFConIp", null);
            tbPort.Text = config.GetValue("appSettings", "NNFConSUBPort", null);

            txtUserId.Text = config.GetValue("appSettings", "ClientId", null);
            // txtPassword.Text = config.GetValue("Profile", "NNFPassword", null);
          
            Global.Instance.NNFPassword = txtPassword.Text;
            txtPassword.Focus();
            // if avoid  check soket telnet yhen please assign _bval ==  false  
            bool _bval = true;
          
            if (_bval == true)
                return;
            TcpClient tcpSocket = null;
            try
            {
                 tcpSocket = new TcpClient(tbOrderIp.Text, Convert.ToInt32(tbPort.Text));           
            }
            catch (SocketException SE)
            {
                MessageBox.Show("Message~~> " + SE.Message, "Server is Stopped" , MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Dispose();
                Environment.Exit(0);
            }
            finally
            {
               tcpSocket.Close();
            }


        }

            private void txtPassword_KeyDown(object sender, KeyEventArgs e)
            {
                 btnConnect.Enabled = true;
                 if (e.KeyCode == Keys.Enter)
                 {
                     btnConnect_Click(sender, e);
                 }
            }

            private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
            {
               //Global.Instance.warningvar = false;
               // if(Global.Instance.warningvar ==false)

            }

            private void label2_Click(object sender, EventArgs e)
            {

            }

    }
}
