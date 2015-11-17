using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Client
{
    public partial class ProfileTrade_Book : Form
    {
        public ProfileTrade_Book()
        {
            InitializeComponent();
        }
        public string GetProfileName()
        {
            return cmbprofile.Text;
        }
        private void ProfileTrade_Book_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Global.Instance.OrdetTable.Columns.Count; i++)
			{
                lbx_Primary.Items.Add(Global.Instance.OrdetTable.Columns[i].ColumnName.ToString());
			}

            if (Directory.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "Trade_Profiles"))
            {
                foreach (string Sname in Directory.GetFiles(Application.StartupPath + Path.DirectorySeparatorChar + "Trade_Profiles", "*.xml"))
                {
                    cmbprofile.Items.Add(Path.GetFileNameWithoutExtension(Sname));
                }
            }
            else
                Directory.CreateDirectory(Application.StartupPath + Path.DirectorySeparatorChar + "Trade_Profiles");

            cmbprofile.SelectedIndex = cmbprofile.Items.IndexOf("new profile");
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            if (cmbprofile.Text == "")
            {
                MessageBox.Show("Please Fill Profile Name");
                return;
            }
            var settings = new XmlWriterSettings { Indent = true };
            XmlWriter writer;
            if(checkBox1.CheckState == CheckState.Checked)
            {
                writer = XmlWriter.Create(Application.StartupPath + Path.DirectorySeparatorChar + "Trade_Profiles" + Path.DirectorySeparatorChar + "TradeDefault.xml",
                settings);
            }
            else
            {
                writer = XmlWriter.Create(Application.StartupPath + Path.DirectorySeparatorChar + "Trade_Profiles" + Path.DirectorySeparatorChar + cmbprofile.Text + ".xml",
               settings);
            }
            writer.WriteStartDocument();

            writer.WriteStartElement("Columns");

            foreach (String itm in lbx_Secondary.Items)
            {
                string encodedXml = itm;
                writer.WriteStartElement("Column");
                writer.WriteAttributeString("Input", encodedXml);
                writer.WriteEndElement();
            }
            if(checkBox1.Checked == true)
            {

            }
            writer.WriteEndElement();

            writer.WriteEndDocument();

            writer.Flush();

            writer.Close();

             DialogResult = DialogResult.OK;

          
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (lbx_Primary.SelectedIndex > -1)
            {
                if (!Equals(lbx_Primary.SelectedItem, null))
                {
                    lbx_Secondary.Items.Add(lbx_Primary.SelectedItem);
                    lbx_Primary.Items.Remove(lbx_Primary.SelectedItem);
                }
            }
        }

        private void btnremove_Click(object sender, EventArgs e)
        {
            if (lbx_Secondary.SelectedIndex > -1)
            {
                if (!Equals(lbx_Secondary.SelectedItem, null))
                {
                    lbx_Primary.Items.Add(lbx_Secondary.SelectedItem);
                    lbx_Secondary.Items.Remove(lbx_Secondary.SelectedItem);

                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //frmGenOrderBook.Instance.profile_load();
            //frmTradeBook.Instance.profile_load();
            this.Close();
        }

        private void cmbprofile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "Trade_Profiles" + Path.DirectorySeparatorChar + cmbprofile.Text + ".xml"))
            {
                var settings = new XmlReaderSettings { IgnoreWhitespace = true, IgnoreComments = true };
             //   lbx_Secondary.Items.Clear();
                using (
                    XmlReader reader =
                        XmlReader.Create(Application.StartupPath + Path.DirectorySeparatorChar + "Trade_Profiles" + Path.DirectorySeparatorChar + cmbprofile.Text + ".xml", settings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && "Column" == reader.LocalName)
                        {
                            reader.MoveToFirstAttribute();
                            lbx_Secondary.Items.Add(reader.Value);
                            lbx_Primary.Items.Remove(reader.Value);
                        }
                    }
                }
            }
        }

        private void lbx_Secondary_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void lbx_Secondary_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void lbx_Secondary_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Unchecked)
            {

                if (Directory.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "TradeDefault_Profiles"))
                {
                    File.Delete(Application.StartupPath + Path.DirectorySeparatorChar + "TradeDefault_Profiles" + Path.DirectorySeparatorChar + "TradeDefault.xml");
                }
            }
            
        }

        private void lbx_Primary_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lbx_Primary.SelectedIndex > -1)
            {
                if (!Equals(lbx_Primary.SelectedItem, null))
                {
                    lbx_Secondary.Items.Add(lbx_Primary.SelectedItem);
                    lbx_Primary.Items.Remove(lbx_Primary.SelectedItem);
                }
            }
        }

        private void lbx_Secondary_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnremove_Click(sender, e);
           
        }
    }
}
