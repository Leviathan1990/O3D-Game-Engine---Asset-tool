using System;
using System.IO;
using System.Windows.Forms;

namespace AssetTool
{
    public static class LoadVisualConfig
    {
        public static void LoadConfiguration(Form3 form3)
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dat/config.cfg");

            try
            {
                Configuration.LoadConfiguration(configFilePath,
                                                form3.ArchivePathTextBox,
                                                form3.BackUpTextBox,
                                                form3.AssetPathTextBox,
                                                form3.TempFolder);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading configuration: {ex.Message}", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void LoadTheme(Form3 form3)
        {
            string theme = ThemeManager.LoadConfig("Theme", "Light");

            ApplyTheme.SetTheme(form3); // Apply theme for the Form3.

            //  Set radioButton properly based on the saved theme
            switch (theme)
            {
                case "Dark":
                    form3.DarkModeRadio.Checked = true;
                    break;
                case "Light":
                    form3.LightModeRadio.Checked = true;
                    break;
                default:
                    form3.DefaultModeRadio.Checked = true;
                    break;
            }
        }
    }
}
