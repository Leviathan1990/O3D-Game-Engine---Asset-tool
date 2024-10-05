using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetTool
{
    public partial class NodeName : Form
    {
        public string FolderName { get; private set; }
        public string ChildNodeName { get; private set; }                           //  Childnode name.

        public NodeName(bool isFolderName)
        {
            InitializeComponent();
            if (isFolderName)                                                       //  If folder name is needed.
            {
                textBoxFolderName.Visible = true;                                   //  TextBox for folder name.
                textBox1.Visible = false;                                           //  Hide textBox for childnode name. (sub folder).
                label2.Visible = false;
                button2.Visible = false;
                this.Text = "New folder (node) name";
            }
            else                                                                    //  If child node name is needed.
            {
                textBox1.Visible = true;                                            //  TextBox for childnode name.
                textBoxFolderName.Visible = false;                                  //  Hide textbox for folder name.
                label1.Visible = false;
                button1.Visible = false;
                this.Text = "New subfolder (childnode) name";
            }
        }

        private void NodeName_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxFolderName.Text))
            {

                FolderName = textBoxFolderName.Text;
                DialogResult = DialogResult.OK;
                Close();
            }

            else
            {
                MessageBox.Show("This field is can not be empty!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {

                ChildNodeName = textBox1.Text; //   Subfolder name
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("This field is can not be empty!");
            }
        
        }
    }
}