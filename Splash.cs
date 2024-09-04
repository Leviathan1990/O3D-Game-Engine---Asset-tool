/*  Welcome screen code
 * 
 *  The Outforce O3D Game Engine Asset Tool.
 *  Designed by: Krisztian Kispeti
 *  Location: Kaposvár, HU.
 *  Contact:
 */
using System;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProgBarMechanism;

namespace AssetTool
{
    public partial class Splash : Form
    {
        private Form2 form2;

        public Splash()
        {
            InitializeComponent();
            form2 = new Form2();
        }

        private async void Splash_Load(object sender, EventArgs e)
        {
            form2.TopMost = true;
            form2.Show();
            form2.BringToFront();

            Worker.InitializeProgressBar(form2.ProgressBar2, 100);

            string changelogPath = @"Changelog.txt";
            string linkPath = @"Dat\Program\Links.txt";
            string webNewsUrl = "https://raw.githubusercontent.com/Leviathan1990/O3D-Game-Engine---Asset-tool/main/News";
            string ProgVersionURL = "https://raw.githubusercontent.com/Leviathan1990/O3D-Game-Engine---Asset-tool/main/Info";

            if (File.Exists(changelogPath) && File.Exists(linkPath))
            {
                try
                {
                    //Loading files and updating progressBar
                    richTextBox2.Text = File.ReadAllText(changelogPath);
                    Worker.UpdateProgressBar(form2.ProgressBar2, 25);

                    richTextBox3.Text = File.ReadAllText(linkPath);
                    Worker.UpdateProgressBar(form2.ProgressBar2, 25);

                    using (HttpClient client = new HttpClient())
                    {
                        string webNewsContent = await client.GetStringAsync(webNewsUrl);
                        richTextBox4.Text = webNewsContent;
                        Worker.UpdateProgressBar(form2.ProgressBar2, 25);

                        string VersionContent = await client.GetStringAsync(ProgVersionURL);
                        richTextBox1.Text = VersionContent;
                        Worker.UpdateProgressBar(form2.ProgressBar2, 25);
                    }

                    await Task.Delay(600);

                    // Check progressBar finnished loading, then close form2.
                    if (form2.ProgressBar2.Value >= form2.ProgressBar2.Maximum)
                    {
                        form2.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unknown error has occured. \n Can't read one of the files, or fetch web content.\n Check your internet connection! \n Program works in offline mode now.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    form2.Close();
                }
            }
            else
            {
                MessageBox.Show("One or both of the files cannot be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                form2.Close();
            }
        }
    }
}
