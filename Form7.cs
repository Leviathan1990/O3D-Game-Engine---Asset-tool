/*  Script editor code
 *  
 *  The Outforce O3D Engine Asset Tool.
 *  Designed by:        Krisztian Kispeti
 *  Location:           Kaposvár, HU.
 *  Contact:
 */

//  Warning, this scriptool has an extra external .cs file, omseditor.cs

using OutforceFileStruct;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using omseditor;

namespace AssetTool
{
    public partial class Form7 : Form
    {
        private OpenFileDialog ofdScripter = new OpenFileDialog();
        private SaveFileDialog ScripterSaver = new SaveFileDialog();

        public RichTextBox RichTextBox1
        {
            get { return RichTextBox1; }
        }

        public Form7()
        {
            InitializeComponent();
            ScripterSaver.Title = "Save script file";
            ScripterSaver.Filter = "Configuration files (*.cfg)|*.cfg|OMS files (*.oms)|*.oms|AI files (*.ai)|*.ai";
            ScripterSaver.DefaultExt = "oms";
            ScripterSaver.FileName = "";
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void SetRichTextBoxContent(string content)
        {
            richTextBox1.Text = content;
        }

        public void SetFileInfo(string filename, uint size)
        {
            toolStripLabel2.Text = filename;
            toolStripLabel4.Text = $"{size} bytes";
        }

        private void Form7_Load(object sender, EventArgs e)
        {
        }
        private void UpdateCharacterCount(string text)
        {
            int characterCount = text.Length;
            toolStripLabel6.Text = "" + characterCount.ToString();
        }

        private void UpdateLineCount(string text)
        {
            int lineCount = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
            toolStripLabel8.Text = "" + lineCount.ToString();
        }

        private void omsOrCfgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ScripterSaver.ShowDialog() == DialogResult.OK)
            {
                string filePath = ScripterSaver.FileName;
                try
                {
                    File.WriteAllText(filePath, richTextBox1.Text, Encoding.ASCII);
                    MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            string originalText = richTextBox1.Text;
            if (toolStripButton5.Selected)
            {
                string extractedText = "";
                string[] lines = originalText.Split('\n');
                bool insideMultilineComment = false;
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();                               //  Filter empty row and spaces from the beginning and end...
                    if (string.IsNullOrEmpty(trimmedLine))                          //  Filter empty row
                    {
                        continue;
                    }
                    if (!insideMultilineComment && trimmedLine.StartsWith("//"))    //  Check, hogy kezdődik-e a sor egysoros kommenttel
                    {
                        continue;
                    }
                    if (!insideMultilineComment && trimmedLine.StartsWith("/*"))    //  Ellenőrzi, hogy kezdődik-e a sor többsoros kommenttel
                    {
                        insideMultilineComment = true;
                        continue;
                    }
                    if (insideMultilineComment && trimmedLine.EndsWith("*/"))       //  Ellenőrzi, hogy végződik-e a többsoros komment
                    {
                        insideMultilineComment = false;
                        continue;
                    }
                    if (!insideMultilineComment)                                    //  Ha semmilyen komment nem található a sorban, hozzáadjuk a kimeneti szöveghez
                    {
                        int index = trimmedLine.IndexOf("//");                      //  Remove all the "//" and the text behind it
                        if (index != -1)
                        {
                            trimmedLine = trimmedLine.Substring(0, index);
                        }
                        extractedText += trimmedLine + "\n";
                    }
                }
                extractedText = extractedText.TrimEnd();                            //  Remove the last unnecessary line-break
                richTextBox1.Text = extractedText;
                UpdateCharacterCount(extractedText);
                UpdateLineCount(extractedText);
            }
            else
            {
                richTextBox1.Text = originalText;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)             //  Eventhander function for toolStripButton3
        {
            if (string.IsNullOrEmpty(richTextBox1.Text))                            //  Check whether richtextBox1 is empty or not
            {
                MessageBox.Show("RichtextBox is empty, Please load a file to treeView component, then select a readable-format file to display it on another form.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string textContent = richTextBox1.Text;                                 //  Read richTextBox1 content as text
            byte[] byteContent = Encoding.ASCII.GetBytes(textContent);              //  Convert texts to bytes
            StringBuilder byteDisplay = new StringBuilder();                        //  Conver bytes to readable format
            foreach (byte b in byteContent)
            {
                byteDisplay.Append(b.ToString("X2") + " ");
            }
            richTextBox1.Clear();
            richTextBox1.AppendText(byteDisplay.ToString());                        //  Clean up richtextBox1 and display its content in byte format

            UpdateCharacterCount(textContent);
            UpdateLineCount(textContent);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)             //  Eventhandler for toolStripButton4 click
        {
            if (string.IsNullOrEmpty(richTextBox1.Text))                            //  Check whether richtextBox1 is empty
            {
                MessageBox.Show("RichTextBox1 is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string byteContent = richTextBox1.Text;                                 //  Read the content to byte format (with separated hexadecimal format with spaces.)

            try                                                                     //  Remove spaces and convert hexadecimal numbers to bytes.
            {
                byte[] bytes = byteContent.Split(' ')
                                          .Where(hex => !string.IsNullOrEmpty(hex))
                                          .Select(hex => Convert.ToByte(hex, 16))
                                          .ToArray();
                string textContent = Encoding.ASCII.GetString(bytes);               //  Convert all the byte back to ASCII text
                richTextBox1.Clear();                                               //  Clean up richTextBox1 component and display text
                richTextBox1.AppendText(textContent);
                UpdateCharacterCount(textContent);
                UpdateLineCount(textContent);
            }
            catch (FormatException)
            {
                MessageBox.Show("Corrupt byte-format! Please use the Show bytes button first, and then this, to convert it back to plain text.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateLineCount(richTextBox1.Text);
            UpdateCharacterCount(richTextBox1.Text);
        }

    }
}

//< --------------------| [0] |--------------------- >

//  IMPORTANT NOTES:
//  Implement: Settings tab as well as configuration loader -from external .cfg/ai/oms files.