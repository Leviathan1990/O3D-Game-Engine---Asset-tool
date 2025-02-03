/*  Outforce BoxBuilder.cs
 * 
 *  The Outforce O3D Engine Asset Tool.
 *  Designed by: Krisztian Kispeti
 *  Location: Kaposvár, HU.
 *  Contact: admin@theoutforce.hu
 *  Website: www.theoutforce.hu
 */

using OutforceFileStruct;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetTool
{
    public partial class BoxBuilder : Form
    {
        private MConsolecs reporterForm = new MConsolecs();
        private ImageList imageList;
        public BoxBuilder()
        {
            InitializeComponent();
        }

        private void BoxBuilder_Load(object sender, EventArgs e)
        {
            string theme = ThemeManager.LoadConfig("Theme", "Light");
            if (theme == "Dark")
            {
                ThemeManager.ApplyDarkMode(this);
            }
            else if (theme == "Light")
            {
                ThemeManager.ApplyLightMode(this);
            }
            else
            {
                ThemeManager.ApplyDefaultMode(this);
            }

            TViewSpecs.SetImageList(treeView1);
            treeView1.Nodes.Clear();
            UpdateFileSizeLabel();
            textBox1.Clear();
            radioButton1.Checked = true;
        }

        private void LogToRichTextBox(string message)
        {
            richTextBox1.AppendText(message + Environment.NewLine);
            richTextBox1.ScrollToCaret();  // Automatically scroll to the latest entry
        }

        private void AddDirectoryToTreeView(string dirPath, TreeNodeCollection nodes)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath);

            TreeNode dirNode = new TreeNode(dir.Name) { Tag = dir.FullName };   // Add folder
            nodes.Add(dirNode);

            foreach (var subDir in dir.GetDirectories())                        // Add subfolders recursively.
            {
                AddDirectoryToTreeView(subDir.FullName, dirNode.Nodes);
            }

            foreach (var file in dir.GetFiles())                                // Add files for folders.
            {
                TreeNode fileNode = new TreeNode(file.Name) { Tag = file.FullName };
                dirNode.Nodes.Add(fileNode);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (treeView1.Nodes.Count > 0)
            {

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Box files (*.box)|*.box|All files (*.*)|*.*";
                    saveFileDialog.Title = "Save a Box Archive";
                    saveFileDialog.FileName = textBox1.Text + ".box";                       // Default name.

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string archiveName = saveFileDialog.FileName;

                        using (FileStream fs = new FileStream(archiveName, FileMode.Create))
                        {
                            using (BinaryWriter writer = new BinaryWriter(fs))
                            {
                                WriteFilesToArchive(writer, treeView1.Nodes);               //  Writing treeView1 to the archive
                            }
                        }
                        MessageBox.Show("Archive successfully created", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //reporterForm.AddSuccess(string.Format("Archive: '{0}' successfully created, and exported to path: '{1}'",Path.GetFileNameWithoutExtension(archiveName),archiveName));
                        
                        UpdateFileSizeLabel();                                              //  Updataing toolstriplabel
                    }
                }
            }
            else
            {
                MessageBox.Show("Can not find any files in treeView. Add some file(s) first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void WriteFilesToArchive(BinaryWriter writer, TreeNodeCollection nodes)
        {
            List<(string filename, long offset, int size, bool isDirectory)> metaDataList = new List<(string, long, int, bool)>();

            WriteContent(writer, nodes, metaDataList);          // Writing file and folder metadata and content

            long directoryOffset = writer.BaseStream.Position;  // Writing metadata
            writer.Write(metaDataList.Count);                   // Writing the number of files and folders

            foreach (var metaData in metaDataList)
            {
                writer.Write(Encoding.ASCII.GetBytes(metaData.filename));
                writer.Write((byte)0);                          // Null terminator
                writer.Write((int)metaData.offset);
                writer.Write((int)metaData.size);

                LogToRichTextBox($"Metadata: {metaData.filename}, Offset: {metaData.offset}, Size: {metaData.size}");
            }
            writer.Write((int)directoryOffset);                 // Writing directory offset
            LogToRichTextBox("Directory offset written: " + directoryOffset);
        }

        private void WriteContent(BinaryWriter writer, TreeNodeCollection nodes, List<(string filename, long offset, int size, bool isDirectory)> metaDataList)
        {
            foreach (TreeNode node in nodes)
            {
                string nodePath = node.Tag.ToString();

                // If folder
                if (node.Nodes.Count > 0)
                {
                    long currentOffset = writer.BaseStream.Position;
                    metaDataList.Add((node.FullPath, currentOffset, 0, true));  // Folder metadata

                    LogToRichTextBox($"Folder added (virtual): {node.FullPath}, Offset: {currentOffset}");

                    WriteContent(writer, node.Nodes, metaDataList); // Recursively write the folder's content (subfolders and files)
                }
                // If file
                else if (File.Exists(nodePath))
                {
                    try
                    {
                        long fileOffset = writer.BaseStream.Position;
                        byte[] fileData = File.ReadAllBytes(nodePath);

                        if (fileData.Length > 0)
                        {
                            writer.Write(fileData);  // Writing file content
                            LogToRichTextBox($"File added: {node.FullPath}, Size: {fileData.Length}, Offset: {fileOffset}");

                            metaDataList.Add((node.FullPath, fileOffset, fileData.Length, false));  // File metadata
                        }
                        else
                        {
                            LogToRichTextBox($"Error: {node.FullPath} file is empty or unreadable.");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogToRichTextBox($"Error occurred while {node.FullPath} writing file: {ex.Message}");
                    }
                }
                else
                {
                    LogToRichTextBox($"File or folder does not exist: {nodePath}");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (var newFolderDialog = new NodeName(true))
            {
                if (newFolderDialog.ShowDialog() == DialogResult.OK)
                {
                    string newFolderName = newFolderDialog.FolderName;
                    string fullPath = Path.Combine(Application.StartupPath, newFolderName);  // Full path to the folder

                    TreeNode newNode = new TreeNode(newFolderName) { Tag = fullPath };
                    treeView1.Nodes.Add(newNode);  // Add the new folder to the root level

                    LogToRichTextBox($"Folder added: {fullPath}");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                openFileDialog.Title = "Select Files";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        string fileName = Path.GetFileName(filePath);
                        TreeNode fileNode = new TreeNode(fileName)
                        {
                            Tag = filePath,
                            ImageKey = TViewSpecs.GetImageKey(filePath),
                            SelectedImageKey = TViewSpecs.GetImageKey(filePath)
                        };

                        if (treeView1.SelectedNode != null)
                        {
                            treeView1.SelectedNode.Nodes.Add(fileNode);
                        }
                        else
                        {
                            treeView1.Nodes.Add(fileNode);
                        }
                    }

                    UpdateFileSizeLabel();
                    UpdateFileCountLabel();

                    if (radioButton2.Checked)
                    {
                        CheckMissingFiles();
                    }
                }
            }
        }

        private int CountFiles(TreeNodeCollection nodes)
        {
            int count = 0;
            foreach (TreeNode node in nodes)
            {
                if (node.Nodes.Count == 0)
                {
                    count++;
                }

                else
                {
                    count += CountFiles(node.Nodes);
                }
            }
            return count;
        }

        private void UpdateSelectedFileSizeLabel()
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Nodes.Count == 0) // Ellenőrzi, hogy fájl van-e kiválasztva
            {
                string filePath = treeView1.SelectedNode.Tag.ToString();
                if (File.Exists(filePath))
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    toolStripLabel6.Text = $": {fileInfo.Length} bytes";
                }
                else
                {
                    toolStripLabel6.Text = "Selected file not found.";
                }
            }
            else
            {
                toolStripLabel6.Text = "No file selected.";
            }
        }

        private void UpdateFileCountLabel()
        {
            int fileCount = CountFiles(treeView1.Nodes);
            toolStripLabel4.Text = $"{fileCount}";
        }

        private void UpdateFileSizeLabel()
        {
            long totalSize = GetTotalFileSize(treeView1.Nodes);
            toolStripLabel2.Text = $"{totalSize} Kbyte";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            using (var newFolderDialog = new NodeName(false))
            {
                if (newFolderDialog.ShowDialog() == DialogResult.OK)
                {
                    string childNodeName = newFolderDialog.ChildNodeName;


                    string fullPath;
                    if (treeView1.SelectedNode != null)
                    {
                        fullPath = Path.Combine(treeView1.SelectedNode.Tag.ToString(), childNodeName);
                    }
                    else
                    {
                        fullPath = childNodeName;
                    }


                    TreeNode newNode = new TreeNode(childNodeName) { Tag = fullPath };

                    if (treeView1.SelectedNode != null)
                    {

                        treeView1.SelectedNode.Nodes.Add(newNode);
                    }
                    else
                    {

                        treeView1.Nodes.Add(newNode);
                    }
                }
            }
        }

        private long GetTotalFileSize(TreeNodeCollection nodes)
        {
            long totalSize = 0;

            foreach (TreeNode node in nodes)
            {
                if (node.Nodes.Count > 0)
                {
                    totalSize += GetTotalFileSize(node.Nodes);
                }
                else                                                                    //  If file
                {
                    string filePath = node.Tag.ToString();                              //  Full file path
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.Exists)
                    {
                        totalSize += fileInfo.Length / 1024;                            //  Add file size
                    }
                }
            }

            return totalSize;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateSelectedFileSizeLabel();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            textBox1.Clear();
            UpdateFileSizeLabel();
        }

        //MapData building mode. This part of the code has nothing to do with all the above functions!

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                MessageBox.Show("MapData.box archive builder info:\n\n Builder mode set to Skirmish.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Check the required files
                List<string> requiredFiles = new List<string> { "Radar.bik", "background.bik", "init.oms", "information.oms" };
                List<string> missingFiles = new List<string>();

                foreach (string file in requiredFiles)
                {
                    if (!IsFilePresentInTreeView(file, treeView1.Nodes))
                    {
                        missingFiles.Add(file);
                    }
                }

                // If there are missing files, display them in the richTextBox1
                if (missingFiles.Count > 0)
                {
                    richTextBox1.Clear();
                    richTextBox1.AppendText("Missing files:\n");
                    foreach (string missingFile in missingFiles)
                    {
                        richTextBox1.AppendText(missingFile + "\n");
                    }
                }
                else
                {
                    richTextBox1.Clear();
                    richTextBox1.AppendText("All required files are present.\n");
                }
            }
            else
            {
                MessageBox.Show("*.box archive builder mode\n has been set to Normal mode.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            textBox1.ReadOnly = radioButton2.Checked;
            textBox1.Text = "MapData";
        }

        private bool IsFilePresentInTreeView(string fileName, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Nodes.Count > 0)
                {
                    if (IsFilePresentInTreeView(fileName, node.Nodes))
                        return true;
                }
                else if (node.Text.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckMissingFiles()
        {
            List<string> requiredFiles = new List<string> { "Radar.bik", "background.bik", "init.oms", "information.oms" };
            List<string> missingFiles = new List<string>();

            foreach (string file in requiredFiles)
            {
                if (!IsFilePresentInTreeView(file, treeView1.Nodes))
                {
                    missingFiles.Add(file);
                }
            }

            // Updating richTextBox1 with missing files.
            if (missingFiles.Count > 0)
            {
                richTextBox1.Clear();
                richTextBox1.AppendText("Missing files:\n");
                foreach (string missingFile in missingFiles)
                {
                    richTextBox1.AppendText(missingFile + "\n");
                }
            }
            else
            {
                richTextBox1.Clear();
                richTextBox1.AppendText("All required files are present.\n");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Check if a node is selected in the TreeView
            if (treeView1.SelectedNode != null)
            {
                // Check if the selected node is a file (and not a folder)
                if (treeView1.SelectedNode.Nodes.Count == 0)  // It's a file if there are no child nodes
                {
                    LogToRichTextBox($"File deleted: {treeView1.SelectedNode.FullPath}");
                    treeView1.SelectedNode.Remove();  // Remove the file
                }
                else
                {
                    MessageBox.Show("The selected item is a folder, not a file. Use the appropriate button to delete folders.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("No file selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Check if a node is selected in the TreeView
            if (treeView1.SelectedNode != null)
            {
                // Check if the selected node is a folder or subfolder
                if (treeView1.SelectedNode.Nodes.Count >= 0) // It can be a folder even if it's empty (Nodes.Count == 0)
                {
                    // Warning about deleting the folder and its contents
                    DialogResult result = MessageBox.Show($"Are you sure you want to delete the folder '{treeView1.SelectedNode.Text}' and all its contents?", "Delete Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        LogToRichTextBox($"Folder deleted: {treeView1.SelectedNode.FullPath}");
                        treeView1.SelectedNode.Remove();  // Remove the folder and its contents
                    }
                }
                else
                {
                    MessageBox.Show("The selected item is not a folder or subfolder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("No folder selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }
    }
}

// Important: This code is well-implemented. No need for major changes.
// Only MapData building mode needed to be improved.
