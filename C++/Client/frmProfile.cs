using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Client
{
    public partial class frmProfile : Form
    {
        public frmProfile()
        {
            InitializeComponent();
        }


        public string GetProfileName()
        {
            return cmbprofile.Text;
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            if (cmbprofile.Text == "")
            {
                MessageBox.Show("Please Fill Profile Name");
                return;
            }
            var settings = new XmlWriterSettings {Indent = true};

            XmlWriter writer = XmlWriter.Create(Application.StartupPath + Path.DirectorySeparatorChar + "Profiles" + Path.DirectorySeparatorChar + cmbprofile.Text + ".xml",
                settings);

            writer.WriteStartDocument();

            writer.WriteStartElement("Columns");

            foreach (String itm in lbxSecondary.Items)
            {
                string encodedXml = itm;
                writer.WriteStartElement("Column");
                writer.WriteAttributeString("Input", encodedXml);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteEndDocument();

            writer.Flush();

            writer.Close();

            DialogResult = DialogResult.OK;
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (lbxPrimary.SelectedIndex > -1)
            {
                if (!Equals(lbxPrimary.SelectedItem, null))
                {
                    lbxSecondary.Items.Add(lbxPrimary.SelectedItem);
                    lbxPrimary.Items.Remove(lbxPrimary.SelectedItem);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void frmProfile_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(Application.StartupPath+ Path.DirectorySeparatorChar  +"Profiles"))
            {
                foreach (string Sname in Directory.GetFiles(Application.StartupPath +Path.DirectorySeparatorChar +"Profiles", "*.xml"))
                {
                    cmbprofile.Items.Add(Path.GetFileNameWithoutExtension(Sname));
                }
            }
            else
                Directory.CreateDirectory(Application.StartupPath +Path.DirectorySeparatorChar +"Profiles");
            cmbprofile.SelectedIndex = cmbprofile.Items.IndexOf("abc");
        }

        private void cmbprofile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath +Path.DirectorySeparatorChar +"Profiles"+Path.DirectorySeparatorChar + cmbprofile.Text + ".xml"))
            {
                var settings = new XmlReaderSettings {IgnoreWhitespace = true, IgnoreComments = true};
                lbxSecondary.Items.Clear();
                using (
                    XmlReader reader =
                        XmlReader.Create(Application.StartupPath + Path.DirectorySeparatorChar + "Profiles" + Path.DirectorySeparatorChar + cmbprofile.Text + ".xml", settings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && "Column" == reader.LocalName)
                        {
                            reader.MoveToFirstAttribute();
                            lbxSecondary.Items.Add(reader.Value);
                            lbxPrimary.Items.Remove(reader.Value);
                        }
                    }
                }
            }
        }

        private void btnremove_Click(object sender, EventArgs e)
        {
            if (lbxSecondary.SelectedIndex > -1)
            {
                if (!Equals(lbxSecondary.SelectedItem, null))
                {
                    lbxPrimary.Items.Add(lbxSecondary.SelectedItem);
                    lbxSecondary.Items.Remove(lbxSecondary.SelectedItem);
                   
                }
            }
        }
     
        private void lbxSecondary_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void lbxSecondary_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void lbxSecondary_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void lbxSecondary_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnremove_Click(sender, e);
        }

        private void lbxPrimary_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnadd_Click(sender, e);
        }
    }
}