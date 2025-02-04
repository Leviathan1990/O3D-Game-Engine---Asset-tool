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
        private static MConsolecs reporterForm;
        private TinaConsole tinaConsole;

        public static void OpenReporter()
        {
            if (reporterForm == null || reporterForm.IsDisposed)
            {

                reporterForm = new MConsolecs();
                reporterForm.Show();
            }
            else
            {
                reporterForm.BringToFront();
            }
        }

        public static MConsolecs GetInstance()
        {
            if (reporterForm == null || reporterForm.IsDisposed)
            {
                OpenReporter();
            }
            return reporterForm;
        }

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

        //  Close Console
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        //  Copy to clipboard
        private void button2_Click(object sender, EventArgs e)
        {
            string text = "The Outforce O3D engine tool summary:" + Environment.NewLine + this.Reporter.Text;
            Clipboard.SetText(text);
        }
        //  Clear console
        private void button3_Click(object sender, EventArgs e)
        {
            Reporter.Clear();
        }



    }
}
