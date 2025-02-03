/*  Outforce ThemeManager.cs
 * 
 *  The Outforce O3D Engine Asset Tool.
 *  Designed by: Krisztian Kispeti
 *  Location: Kaposvár, HU.
 *  Contact: admin@theoutforce.hu
 *  Website: www.theoutforce.hu
 */

using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using AssetTool;

public static class ThemeManager
{
    // Configuration file path
    private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dat/Core/Visual/", "ThemeConfig.cfg");

    // Save a key-value pair to the config file
    public static void SaveConfig(string key, string value)
    {
        var lines = new List<string>();
        if (File.Exists(ConfigPath))
        {
            lines.AddRange(File.ReadAllLines(ConfigPath));
        }

        bool found = false;
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].StartsWith(key + "=", StringComparison.OrdinalIgnoreCase))
            {
                lines[i] = string.Format("{0}={1}", key, value);
                found = true;
                break;
            }
        }

        if (!found)
        {
            lines.Add(string.Format("{0}={1}", key, value));
        }

        string directory = Path.GetDirectoryName(ConfigPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllLines(ConfigPath, lines);
    }

    // Load a key-value pair from the config file
    public static string LoadConfig(string key, string defaultValue)
    {
        if (!File.Exists(ConfigPath))
        {
            SaveConfig(key, defaultValue); // Create file with default value
            return defaultValue;
        }

        string[] lines = File.ReadAllLines(ConfigPath);
        foreach (string line in lines)
        {
            if (line.StartsWith(key + "=", StringComparison.OrdinalIgnoreCase))
            {
                return line.Split('=')[1].Trim();
            }
        }
        return defaultValue;
    }

    //  Apply Light Mode
    public static void ApplyLightMode(Form form)
    {
        form.BackColor = SystemColors.Control;
        form.ForeColor = SystemColors.WindowText;

        foreach (Control control in form.Controls)
        {
            ApplyLightModeToControl(control);
        }
    }
    private static void ApplyLightModeToControl(Control control)
    {
        // MenuStrip and ToolStripMenuItem 
        if (control is MenuStrip)
        {
            MenuStrip menuStrip = (MenuStrip)control;

            // MenuStrip default colors
            menuStrip.BackColor = SystemColors.Control;
            menuStrip.ForeColor = SystemColors.ControlText;

            // ToolStripMenuItem tem coloring
            foreach (ToolStripItem item in menuStrip.Items)
            {
                item.BackColor = SystemColors.Control;
                item.ForeColor = SystemColors.ControlText;
            }
        }
        // SplitContainer 
        else if (control is SplitContainer)
        {
            SplitContainer splitContainer = (SplitContainer)control;
            splitContainer.Panel1.BackColor = SystemColors.Control;
            splitContainer.Panel2.BackColor = SystemColors.Control;
        }
        // TreeView 
        else if (control is TreeView)
        {
            TreeView treeView = (TreeView)control;
            treeView.BackColor = SystemColors.Control;
            treeView.ForeColor = Color.Black;
        }
        // TextBox and RichTextBox 
        else if (control is TextBox || control is RichTextBox)
        {
            control.BackColor = SystemColors.Control;
            control.ForeColor = Color.Black;
        }
        // TabControl and GroupBox 
        else if (control is TabControl || control is GroupBox)
        {
            control.BackColor = SystemColors.Control;
            control.ForeColor = SystemColors.ControlText;
        }
        // Default controls
        else
        {
            control.BackColor = SystemColors.Control;
            control.ForeColor = SystemColors.ControlText;
        }

        // Childnode recursively
        if (control.HasChildren)
        {
            foreach (Control child in control.Controls)
            {
                ApplyLightModeToControl(child);
            }
        }
    }

    //  Apply Dark Mode
    public static void ApplyDarkMode(Form form)
    {
        form.BackColor = Color.Black;
        form.ForeColor = Color.White;

        foreach (Control control in form.Controls)
        {
            ApplyDarkModeToControl(control);
        }
    }
    private static void ApplyDarkModeToControl(Control control)
    {
        // MenuStrip and ToolStripMenuItem
        if (control is MenuStrip)
        {
            MenuStrip menuStrip = (MenuStrip)control;

            // MenuStrip default colors
            menuStrip.BackColor = Color.Black;
            menuStrip.ForeColor = Color.White;

            // ToolStripMenuItem item coloring
            foreach (ToolStripItem item in menuStrip.Items)
            {
                item.BackColor = Color.Black;
                item.ForeColor = Color.White;
            }
        }
        // SplitContainer 
        else if (control is SplitContainer)
        {
            SplitContainer splitContainer = (SplitContainer)control;
            splitContainer.Panel1.BackColor = Color.Black;
            splitContainer.Panel2.BackColor = Color.Black;
        }
        // TreeView 
        else if (control is TreeView)
        {
            TreeView treeView = (TreeView)control;
            treeView.BackColor = Color.Black;
            treeView.ForeColor = Color.White;
        }
        // TextBox and RichTextBox 
        else if (control is TextBox || control is RichTextBox)
        {
            control.BackColor = Color.Black;
            control.ForeColor = SystemColors.Window;
        }
        // TabControl and GroupBox 
        else if (control is TabControl || control is GroupBox)
        {
            control.BackColor = Color.Black;
            control.ForeColor = Color.White;
        }
        // RadioButton, CheckBox and Label 
        else if (control is RadioButton || control is CheckBox || control is Label)
        {
            control.BackColor = Color.Black;
            control.ForeColor = Color.White;
        }

        // Default controls
        else
        {
            control.BackColor = Color.Black;
            control.ForeColor = Color.White;
        }

        // Child controls recursively...
        if (control.HasChildren)
        {
            foreach (Control child in control.Controls)
            {
                ApplyDarkModeToControl(child);
            }
        }
    }

    //  Apply Default mode
    public static void ApplyDefaultMode(Form form)
    {
        //  Default Form colors
        form.BackColor  =    SystemColors.Control;
        form.ForeColor  =    Color.Black;

        //  Set colors of each controls
        foreach (Control control in form.Controls)
        {
            ApplyDefaultModeToControl(control);
        }        
    }
    private static void ApplyDefaultModeToControl(Control control)
    {
        if (control is RichTextBox || control is TreeView || control is TextBox || control is SplitContainer || control is ToolStripTextBox || control is TabControl)
        {
            control.BackColor = SystemColors.ControlDark;
            control.ForeColor = Color.Black;
        }

        else if (control is TreeView)
        {
            TreeView treeView       =   (TreeView)control;
            treeView.BackColor      =   SystemColors.ControlDark;
            treeView.ForeColor      =   Color.Black;
            treeView.BorderStyle    =   BorderStyle.None;
        }

        else if (control is MenuStrip)
        {
            MenuStrip menuStrip = (MenuStrip)control;
            menuStrip.BackColor = SystemColors.ControlDark;
            menuStrip.ForeColor = Color.Black;

            foreach (ToolStripItem item in menuStrip.Items)
            {
                item.BackColor = SystemColors.ControlDark;
                item.ForeColor = Color.Black;
            }
        }
        else
        {
            control.BackColor = SystemColors.ControlDark;
            control.ForeColor = Color.Black;
        }

        if (control.HasChildren)
        {
            foreach (Control child in control.Controls)
            {
                ApplyDefaultModeToControl(child);
            }
        }
    }


    
}