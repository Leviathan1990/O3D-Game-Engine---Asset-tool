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
        private ImageList imageList;
        public BoxBuilder()
        {
            InitializeComponent();
        }

        private void BoxBuilder_Load(object sender, EventArgs e)
        {
            TViewSpecs.SetImageList(treeView1);
            treeView1.Nodes.Clear();
            UpdateFileSizeLabel();
            textBox1.Clear();
        }

        private void AddDirectoryToTreeView(string dirPath, TreeNodeCollection nodes)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath);

            // Add folder
            TreeNode dirNode = new TreeNode(dir.Name) { Tag = dir.FullName };
            nodes.Add(dirNode);

            // Add subfolders recursively.
            foreach (var subDir in dir.GetDirectories())
            {
                AddDirectoryToTreeView(subDir.FullName, dirNode.Nodes);
            }

            // Add files for folders.
            foreach (var file in dir.GetFiles())
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
                        MessageBox.Show("Archive successfully created","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        UpdateFileSizeLabel();                                              //  Updataing toolstriplabel
                    }
                }
            }
            else
            {
                MessageBox.Show("Can not find any files in treeView. Add some file(s) first!", "Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void WriteFilesToArchive(BinaryWriter writer, TreeNodeCollection nodes)
{
    // List of metadata containing information about files and directories
    List<(string filename, long offset, int size)> metaDataList = new List<(string, long, int)>();

    // Write content and collect metadata
    WriteContent(writer, nodes, metaDataList);

    // Starting offset of metadata
    long directoryOffset = writer.BaseStream.Position;

    // Write the number of files (4 bytes)
    writer.Write(metaDataList.Count);

    // Write all metadata
    foreach (var metaData in metaDataList)
    {
        // Filename (terminated with a null character)
        writer.Write(Encoding.ASCII.GetBytes(metaData.filename));
        writer.Write((byte)0);  // Null terminator at the end of the filename

        // File offset (4 bytes)
        writer.Write((int)metaData.offset);

        // File size (4 bytes)
        writer.Write((int)metaData.size);
    }

    // Finally, write the directory offset (4 bytes)
    writer.Write((int)directoryOffset);
}

// Recursive function to write file and directory contents
private void WriteContent(BinaryWriter writer, TreeNodeCollection nodes, List<(string filename, long offset, int size)> metaDataList)
{
    foreach (TreeNode node in nodes)
    {
        string fullPath = node.Tag.ToString();

        if (Directory.Exists(fullPath)) // Directory handling
        {
            // Recursive call for files and subdirectories within the directory
            WriteContent(writer, node.Nodes, metaDataList);
        }
        else if (File.Exists(fullPath)) // File handling
        {
            FileInfo fileInfo = new FileInfo(fullPath);

            // Write file content to the archive
            long currentOffset = writer.BaseStream.Position;
            using (FileStream fs = new FileStream(fullPath, FileMode.Open))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    writer.Write(buffer, 0, bytesRead);
                }
            }

            // Add file metadata to the list
            metaDataList.Add((node.FullPath, currentOffset, (int)fileInfo.Length));
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

                    TreeNode newNode = new TreeNode(newFolderName) { Tag = newFolderName };             //  Add new folder (node) in the treeView1.
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
                }
            }
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
                        totalSize += fileInfo.Length / 1024;                                   //  Add file size
                    }
                }
            }

            return totalSize;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            textBox1.Clear();
            UpdateFileSizeLabel();
        }
    }
}

//Info: BoxBuilder.cs uses the NullTerminator.cs, BoxStruct.cs and NodeName.cs files


//TODO

// fix&Implement folder and subfolder function!
// Method: *.box archives can store only files and / or files and folders and subfolders
// and some files can be outside of folders and subfolders too... 
