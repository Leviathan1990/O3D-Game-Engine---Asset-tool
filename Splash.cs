using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetTool
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }

        private async void Splash_Load(object sender, EventArgs e)
        {
            string changelogPath = @"Changelog.txt";
            string linkPath = @"Dat\Program\Links.txt";
            string webNewsUrl = "https://raw.githubusercontent.com/Leviathan1990/O3D-Game-Engine---Asset-tool/main/News";                                             // Live news
            string ProgVersionURL = "https://raw.githubusercontent.com/Leviathan1990/O3D-Game-Engine---Asset-tool/main/Info";                               // Program version from Github

            if (File.Exists(changelogPath) && File.Exists(linkPath))            //  Check: Files are exists
            {
                try
                {
                    richTextBox2.Text = File.ReadAllText(changelogPath);        //  Load the content of the Changelog.txt file to richTextBox2
                    richTextBox3.Text = File.ReadAllText(linkPath);             //  Load the content of the Links.txt to richTextBox3

                    using (HttpClient client = new HttpClient())
                    {
                        string webNewsContent = await client.GetStringAsync(webNewsUrl);
                        string VresionURL = await client.GetStringAsync(ProgVersionURL);
                        richTextBox4.Text = webNewsContent;
                        richTextBox1.Text = VresionURL;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unknown error has occured. Can't read one of the files, or fetch web content.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("One or both of the files cannot be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}