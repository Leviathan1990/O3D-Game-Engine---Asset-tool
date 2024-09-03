using System;
using System.IO;
using System.Windows.Forms;

public static class Configuration
{
    public static void LoadConfiguration(string configFilePath, TextBox ArchivePath, TextBox BackUpPath, TextBox AssetPath, TextBox TemporaryFldr)
    {

        if (File.Exists(configFilePath))
        {
            try
            {
                using (StreamReader reader = new StreamReader(configFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();

                        if (line.StartsWith("ArchivePath="))
                        {
                            ArchivePath.Text = line.Substring("ArchivePath=".Length);
                        }
                        else if (line.StartsWith("BackUpPath="))
                        {
                            BackUpPath.Text = line.Substring("BackUpPath=".Length);
                        }
                        else if (line.StartsWith("AssetPath="))
                        {
                            AssetPath.Text = line.Substring("AssetPath=".Length);
                        }
                        else if (line.StartsWith("TemporaryFldr"))
                        {
                            TemporaryFldr.Text = line.Substring("TemporaryFldr=".Length);
                        }
  
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the configuration: {ex.Message}");
            }
        }
    }

    public static void SaveIconSize(int iconSize)
    {
        string configFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dat");
        string configFilePath = Path.Combine(configFolderPath, "config.cfg");

        if (!Directory.Exists(configFolderPath))
        {
            Directory.CreateDirectory(configFolderPath);
        }

        try
        {
            using (StreamWriter writer = new StreamWriter(configFilePath, true))
            {
                writer.WriteLine($"IconSize={iconSize}");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while saving the icon size: {ex.Message}");
        }
    }
}
