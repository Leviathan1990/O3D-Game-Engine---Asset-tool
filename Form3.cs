/*  Program configuration panel.
 *  
 *  The Outforce    O3D Engine Asset Tool.
 *  Designed by:    Krisztian Kispeti
 *  Location:       Kaposvár, HU.
 *  Contact:        
 */

using System;
using System.IO;
using System.Windows.Forms;

namespace AssetTool
{
    public partial class Form3 : Form
    {
        private Form1 _form1;

        public Form3(Form1 form1)
        {
            InitializeComponent();
            _form1 = form1;
            string theme = ThemeManager.LoadConfig("Theme", "Light");
            if (theme == "Dark")
            {
                DarkModeR.Checked = true;
            }

            else if (theme == "Light")
            {
                LightModeR.Checked = true;
            }
            else
            {
                ThemeDefault.Checked = true;
            }

        }

        public TextBox ArchivePathTextBox => ArchivePath;
        public TextBox BackUpTextBox => BackUpPath;
        public TextBox AssetPathTextBox => AssetPath;
        public TextBox TempFolder => TemporaryFldr;

        private void Form3_Load(object sender, EventArgs e)
        {
            string theme = ThemeManager.LoadConfig("Theme", "Light");

            if (theme == "Dark")
            {
                DarkModeR.Checked = true;
            }
            else if (theme == "Light")
            {
                LightModeR.Checked = true;
            }
            else
            {
                ThemeDefault.Checked = true;
            }

            ThemeManager.ApplyLightMode(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string configFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dat");
            string configFilePath = Path.Combine(configFolderPath, "config.cfg");

            if (!Directory.Exists(configFolderPath))
            {
                Directory.CreateDirectory(configFolderPath);
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(configFilePath))
                {
                    writer.WriteLine($"ArchivePath={ArchivePath.Text}");
                    writer.WriteLine($"BackUpPath={BackUpPath.Text}");
                    writer.WriteLine($"AssetPath={AssetPath.Text}");
                    writer.WriteLine($"TemporaryFldr={TemporaryFldr.Text}");
                }

                MessageBox.Show("Configuration saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the configuration: {ex.Message}");
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)    //  Set theme Light
        {
            if (LightModeR.Checked)
            {
                SetTheme("Light");
            }
        }
        private void SetTheme(string theme)
        {
            ThemeManager.SaveConfig("Theme", theme);
            //  Apply selected theme on each forms
            foreach (Form openForm in Application.OpenForms)
            {
                if (theme == "Dark")
                {
                    ThemeManager.ApplyDarkMode(openForm);
                }
                else if (theme == "Light")
                {
                    ThemeManager.ApplyLightMode(openForm);
                }
                else // Default mód
                {
                    ThemeManager.ApplyDefaultMode(openForm);
                }
            }

        }       //  Theme manager
        private void DarkModeR_CheckedChanged(object sender, EventArgs e)
        {
            if (DarkModeR.Checked)
            {
                SetTheme("Dark");
            }
        }   //  Set theme Dark

        private void ThemeDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (ThemeDefault.Checked)
            {
                SetTheme("Default");
            }
        }   //  Set theme default

        public string ExtractionPath
        {
            get { return AssetPath.Text; }
        }

    }
}
