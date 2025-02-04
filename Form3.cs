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
            //  Load visual configurations
            LoadVisualConfig.LoadConfiguration(this);
            //  Apply theme
            ApplyTheme.SetTheme(this);
            //  Load RadioButtons
            LoadVisualConfig.LoadTheme(this);
        }

        public TextBox ArchivePathTextBox => ArchivePath;
        public TextBox BackUpTextBox => BackUpPath;
        public TextBox AssetPathTextBox => AssetPath;
        public TextBox TempFolder => TemporaryFldr;

        public RadioButton LightModeRadio => LightModeR;
        public RadioButton DarkModeRadio => DarkModeR;
        public RadioButton DefaultModeRadio => ThemeDefault;

        private void Form3_Load(object sender, EventArgs e)
        {
            //  Load theme
            LoadVisualConfig.LoadConfiguration(this);
            //  Apply theme
            ApplyTheme.SetTheme(this);
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
                else // Default mode
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
        private void savePathsToolStripMenuItem_Click(object sender, EventArgs e)
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
                MConsolecs.GetInstance().AddError($"An error occurred while saving the configuration: {ex.Message}");
            }
        }

        public string ExtractionPath
        {
            get { return AssetPath.Text; }
        }

    }
}

//  Implement language selector
//  Implement Syntax checker!
