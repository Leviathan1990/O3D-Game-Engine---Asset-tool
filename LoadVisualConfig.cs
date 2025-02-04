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
    }
}
