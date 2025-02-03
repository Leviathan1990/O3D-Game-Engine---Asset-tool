/*  Outforce Console.cs
 * 
 *  The Outforce O3D Engine Asset Tool.
 *  Designed by: Krisztian Kispeti
 *  Location: Kaposvár, HU.
 *  Contact: admin@theoutforce.hu
 *  Website: www.theoutforce.hu
 */

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Configuration;
using OutforceFileStruct;
using AssetTool;

namespace InitConsole
{
    public class TinaConsole
    {
        private Form3 frm3;
        private RichTextBox reporter;
        private TextBox commander;
        private Dictionary<string, string> commands;
       
        private string boxFileName;
        private BoxItem selectedItem;
        private string outputDirectory;
        private string tutorialColor;

        public TinaConsole(TextBox commander, RichTextBox reporter)
        {
            this.commander = commander;
            this.reporter = reporter;
            InitializeCommands();
            this.commander.KeyDown += Commander_KeyDown;

            //  Load the actual tutorialColor value from .cfg file
            this.tutorialColor = ThemeManager.LoadConfig("TutorialEditorFColor","0");
        }

        //  Keydown 'enter' to send the command
        private void Commander_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string command = commander.Text.Trim();
                if (!string.IsNullOrEmpty(command))
                {
                    ExecuteCommand(command, null);
                    commander.Clear(); // Clear commander  text.
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        
        private void InitializeCommands()
        {
            //CONSOLE 
            commands = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            commands.Add("startgame",           "Starts the game.");
            commands.Add("time",                "Displays the current time.");
            commands.Add("help",                "Displays a list of available commands.");
            commands.Add("about",               "Displays product information");
            commands.Add("clear",               "Clear console");
            commands.Add("restart",             "Restart application");

            //  Program For Tutorial form.
            commands.Add("DModeInfo",           "Display Dmode value for Tutorial form");
            
            //FOR MAP EDITOR information.oms

            commands.Add("si_Map_Descrption",   "A description of the map to play on si_Map_Description(str Desc);");
            commands.Add("si_Map_Author",       "Author of the map si_Map_Author(str AuthorName);");
            commands.Add("si_Map_Player",       "The number of players this map was designed for si_Map_Players(int NumPlayers);");
            commands.Add("si_Map_Width",        "The width of the selected map si_Map_Width(int Width);");
            commands.Add("si_Map_Height",       "The height of the selected map si_Map_Width(int Height);");

            // FOR MAP EDITOR: init.oms

            commands.Add("M_SetGameScene_rect", "M_SetGameSceneRect  Sets and allocates the game scene. Measuered in world units. M_SetGameSceneRect( int x1, int z1, int x2, int z2 );");
            commands.Add("scene_bgcolor_red",   "Sets the red component of the background color. scene_bgcolor_red(int Color);");
            commands.Add("scene_bgcolor_green", "Sets the green component of the background color. scene_bgcolor_green(int Color);");
            commands.Add("scene_bgcolor_blue",  "Sets the blue component of the background color. scene_bgcolor_blue(int Color)");
            commands.Add("M_Resources",         "Sets the number of resources for the active player. M_Resources(int Resources);");
            commands.Add("STARTMP3",            "StartMP3(str FileName);");

            //FOR MAP EDITOR extra

            commands.Add("chkmap",              "Check for missing files in MapData.box loaded to listBox1 component;");
            commands.Add("buildmap",            "Builds a MapData.box archive with the given file(s) in the listBox1 component;");

            //External tools

            commands.Add("radstart",            "Start Rad Video tools program");
        }

        public void ExecuteCommand(string command, TextBox pathTextBox)
        {
            if (command.Equals("time", StringComparison.OrdinalIgnoreCase))
            {
                DisplayCurrentTime();
            }
            else if (command.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                DisplayHelp();
            }
            else if (command.Equals("about", StringComparison.OrdinalIgnoreCase))
            {
                DisplayAbout();
            }

            else if (command.Equals("clear", StringComparison.OrdinalIgnoreCase)) 
            {
                ConsoleClear();
            }
            else if (command.Equals("restart", StringComparison.OrdinalIgnoreCase))
            {
                Restart();
            }

            else if (command.Equals("DModeInfo", StringComparison.OrdinalIgnoreCase))
            {
                DModeInfo();
            }

                //FOR MAP EDITOR: information.oms

            else if (command.Equals("si_Map_Description", StringComparison.OrdinalIgnoreCase))
            {
                si_Map_Description();
            }

            else if (command.Equals("si_Map_Author", StringComparison.OrdinalIgnoreCase))
            {
                si_Map_Author();
            }

            else if (command.Equals("si_Map_Player", StringComparison.OrdinalIgnoreCase))
            {
                si_Map_Player();
            }

            else if (command.Equals("si_Map_Width", StringComparison.OrdinalIgnoreCase))
            {
                si_Map_Width();
            }

            else if (command.Equals("si_Map_Height", StringComparison.OrdinalIgnoreCase))
            {
                si_Map_Height();
            }

                // FOR MAP EDITOR: init.oms

            else if (command.Equals("M_SetGameScene_rect", StringComparison.OrdinalIgnoreCase))
            {
                M_SetGameScene_rect();
            }

            else if (command.Equals("scene_bgcolor_red", StringComparison.OrdinalIgnoreCase))
            {
                scene_bgcolor_red();
            }
            else if (command.Equals("scene_bgcolor_green", StringComparison.OrdinalIgnoreCase))
            {
                scene_bgcolor_green();
            }
            else if (command.Equals("scene_bgcolor_bule", StringComparison.OrdinalIgnoreCase))
            {
                scene_bgcolor_blue();
            }
            else if (command.Equals("M_Resources", StringComparison.OrdinalIgnoreCase))
            {
                M_Resources();
            }
            else if (command.Equals("STARTMP3", StringComparison.OrdinalIgnoreCase))
            {
                STARTMP3();
            }

                //      For external programs
            else if (command.Equals("radstart", StringComparison.OrdinalIgnoreCase))
            {
                radstart();
            }

            else
            {
                AppendOutput("Unknown command: {command}");
            }
        }

        private void DisplayCurrentTime()
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            AppendOutput("Current time: " + currentTime);
        }

        private void DisplayHelp()
        {
            AppendOutput("Available commands:");
            foreach (var kvp in commands)
            {
                AppendOutput(string.Format("{0}: {1}", kvp.Key, kvp.Value));
            }
        }

        private void DisplayAbout()
{
    // Reading actual assembly
    Assembly assembly = Assembly.GetExecutingAssembly();

    // Reading Attributes
    string productName = ((AssemblyProductAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyProductAttribute))).Product ?? "Unknown Product";
    string version = assembly.GetName().Version.ToString();
    string developer = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute))).Company ?? "Unknown Developer";

    // Display informations
    reporter.AppendText(string.Format("Product information:\n{0} {1}\nDeveloped by: {2}\n", productName, version, developer));

}

        private void ConsoleClear() 
        {
            reporter.Clear();
        }
        private void Restart()
        {
            Application.Restart();
        }

        //  Form3 "Tutorial" form related sh!t
        private void DModeInfo()
        {
            reporter.AppendText(string.Format("Dmode value for Tutorial form is: {0}\n", tutorialColor));
            //  If dark mode is enabled, show an error message.
            if (tutorialColor == "1")
            {
                AppendOutput(string.Format("Warning: Dark mode is enabled. Make sure text color is adjusted!"));
            }
        }

        //  FOR MAP EDITOR: information.oms
        private void si_Map_Description() 
        {
            reporter.Clear();
            AppendOutput("si_Map_Description(char); | A description of the map to play on si_Map_Description(str Desc).");
        }

        private void si_Map_Author()
        {
            reporter.Clear();
            reporter.AppendText("si_Map_Author(); | Author of the map si_Map_Author(str AuthorName).");
        }

        private void si_Map_Player()
        {
            reporter.Clear();
            reporter.AppendText("si_Map_Player(); | The number of players this map was designed for si_Map_Players(int NumPlayers).");
        }

        private void si_Map_Width()
        {
            reporter.Clear();
            reporter.AppendText("si_Map_Height(); | The width of the selected map si_Map_Width(int Width).");
        }

        private void si_Map_Height()
        {
            reporter.Clear();
            reporter.AppendText("si_Map_Height(); | The height of the selected map si_Map_Width(int Width).");
        }

        //      FOR MAP EDITOR: init.oms
        private void M_SetGameScene_rect()
        {
            reporter.Clear();
            reporter.AppendText("M_SetGameScene_rect(); | M_SetGameSceneRect Sets and allocates the game scene. Measuered in world units. M_SetGameSceneRect( int x1, int z1, int x2, int z2 );");
        }

        private void scene_bgcolor_red()
        {
            reporter.Clear();
            reporter.AppendText("scene_bgcolor_red(); | Sets the red component of the background color. scene_bgcolor_red(int Color);");
        }
        private void scene_bgcolor_green()
        {
            reporter.Clear();
            reporter.AppendText("scene_bgcolor_green(); | Sets the red component of the background color. scene_bgcolor_green(int Color);");
        }
        private void scene_bgcolor_blue()
        {
            reporter.Clear();
            reporter.AppendText("scene_bgcolor_blue(); | Sets the red component of the background color. scene_bgcolor_blue(int Color);");
        }

        private void M_Resources()
        {
            reporter.Clear();
            reporter.AppendText("M_Resources(); | Sets the number of resources for the active player. M_Resources(int Resources);");
        }

        private void STARTMP3()
        {
            reporter.Clear();
            reporter.AppendText("STARTMP3(); | StartMP3(str FileName);");
        }

        //      For external programs

        private void radstart()
        {
            try
            {
                string toolsPath = Path.Combine(Application.StartupPath, "Tools");
                string radToolsPath = Path.Combine(toolsPath, "radvideo.exe");
                if (File.Exists(radToolsPath))
                {
                    Process.Start(radToolsPath);
                    AppendOutput("RAD Tools started: " + radToolsPath);
                }
                else
                {
                    AppendOutput("RAD Tools executable not found: " + radToolsPath);
                }
            }
            catch (Exception ex)
            {
                AppendOutput("Error starting RAD Tools: " + ex.Message);
            }
        }

        //      FOR CONSOLE APPENDOUT
        private void AppendOutput(string message)
        {
            if (this.reporter != null)
            {
                this.reporter.SelectionColor = Color.OrangeRed; // Success messages in blue
                this.reporter.Font = new Font("Arial", 10);
                this.reporter.AppendText(">> Console: " + message + Environment.NewLine);
                this.reporter.ScrollToCaret(); // Scroll to the bottom
            }
        }
    }
}
