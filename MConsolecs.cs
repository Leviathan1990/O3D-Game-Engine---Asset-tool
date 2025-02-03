/*  Outforce MConsolecs.cs
 * 
 *  The Outforce O3D Engine Asset Tool.
 *  Designed by: Krisztian Kispeti
 *  Location: Kaposvár, HU.
 *  Contact: admin@theoutforce.hu
 *  Website: www.theoutforce.hu
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using InitConsole;
using OutforceFileStruct;

namespace AssetTool
{
    public partial class MConsolecs : Form
    {
        private List<BoxItem> toc = new List<BoxItem>();
        private TinaConsole tinaConsole;

        public MConsolecs()
        {
            InitializeComponent();

            tinaConsole = new TinaConsole(commander, Reporter);
        }

        //  Message displayer
        public void AddError(string message)
        {
            if (Reporter != null)
            {
                Reporter.SelectionColor = Color.Red;
                Reporter.AppendText(message + Environment.NewLine);
                Reporter.ScrollToCaret();   //  autoscroll
            }
        }
        public void ConsoleMessage(string message)
        {
            if (Reporter != null)
            {
                Reporter.SelectionColor = Color.Orange;
                Reporter.AppendText("Console: " + message + Environment.NewLine);
                Reporter.ScrollToCaret();
            }
        }
        public void AddSuccess(string message)
        {
            if (Reporter != null)
            {
                Reporter.SelectionColor = Color.DarkCyan;
                Reporter.AppendText(message + Environment.NewLine);
                Reporter.ScrollToCaret();
            }
        }


        private void MConsolecs_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text = "The Outforce O3D engine tool summary:" + Environment.NewLine + this.Reporter.Text;
            Clipboard.SetText(text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Reporter.Clear();
        }
    }
}
