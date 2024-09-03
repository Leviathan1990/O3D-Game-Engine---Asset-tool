/*  Find code
 *  
 *  The Outforce O3D Engine Asset Tool.
 *  Designed by: Krisztian Kispeti
 *  Location: Kaposvár, HU.
 *  Contact:
 */

using System;
using System.Linq;
using System.Windows.Forms;

namespace AssetTool
{
    public partial class Form5 : Form
    {
        public event EventHandler<SearchEventArgs> SearchPerformed;
        private string lastSearchTerm = string.Empty;
        private TreeNode lastFoundNode = null;

        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchTerm = comboBox1.Text;
            Form1 form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();

            if (form1 != null)
            {
                System.Windows.Forms.TreeView treeView = form1.GetTreeView();
                bool matchCase = checkBox3.Checked;                                                 //  Match case.
                int foundCount = CountNodes(treeView.Nodes, searchTerm, matchCase);                 //  Determining the number of items.
                toolStripLabel2.Text = $"Items found: {foundCount}";
            }
            else
            {
                toolStripLabel2.Text = "Form1 is not available";
                MessageBox.Show("Form1 is not available!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lastSearchTerm))
            {
                PerformSearch();
            }
            else
            {
                FindNext();
            }
        }

        protected virtual void OnSearchPerformed(SearchEventArgs e)
        {
            SearchPerformed?.Invoke(this, e);
        }

        private void PerformSearch()
        {
            string searchTerm = comboBox1.Text;
            Form1 form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (form1 != null)
            {
                System.Windows.Forms.TreeView treeView = form1.GetTreeView();
                bool matchCase = checkBox3.Checked;                                                 //  Match case.
                bool matchWholeWord = checkBox2.Checked;                                            //  Match whole word only (settings considering).

                TreeNode foundNode = FindNode(treeView.Nodes, searchTerm, matchCase, matchWholeWord);
                if (foundNode != null)
                {
                    treeView.SelectedNode = foundNode;
                    treeView.Focus();
                    toolStripLabel2.Text = $"Items found: 1";
                    lastSearchTerm = searchTerm;
                    lastFoundNode = foundNode;
                }
                else
                {
                    toolStripLabel2.Text = "No items found";
                    MessageBox.Show("Items can not be found.", "Searching", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                toolStripLabel2.Text = "Form1 is not available.";
                MessageBox.Show("Form1 is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private TreeNode FindNode(TreeNodeCollection nodes, string searchTerm, bool matchCase, bool matchWholeWord)
        {
            StringComparison comparisonType = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            foreach (TreeNode node in nodes)
            {
                if (matchWholeWord)
                {
                    if (string.Equals(node.Text, searchTerm, comparisonType))
                    {
                        return node;
                    }
                }
                else
                {
                    if (node.Text.Contains(searchTerm, comparisonType))
                    {
                        return node;
                    }
                }

                TreeNode foundNode = FindNode(node.Nodes, searchTerm, matchCase, matchWholeWord);
                if (foundNode != null)
                {
                    return foundNode;
                }
            }
            return null;
        }

        private int CountNodes(TreeNodeCollection nodes, string searchTerm, bool matchCase)
        {
            int count = 0;
            StringComparison comparisonType = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            foreach (TreeNode node in nodes)
            {
                if (node.Text.Contains(searchTerm, comparisonType))
                {
                    count++;
                }
                count += CountNodes(node.Nodes, searchTerm, matchCase);
            }
            return count;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
                                                                                                    //  Implement logic here
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
                                                                                                    //  Implement logic here
        }

        private void FindNext()
        {
            if (lastFoundNode == null || string.IsNullOrEmpty(lastSearchTerm))
            {
                return;
            }

            Form1 form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (form1 != null)
            {
                System.Windows.Forms.TreeView treeView = form1.GetTreeView();
                bool matchCase = checkBox3.Checked;                                                 //  Considering match case.
                bool backward = checkBox1.Checked;                                                  //  Considering Backward searching.
                bool matchWholeWord = checkBox2.Checked;                                            //  Considering Match whole word only.

                TreeNode nextNode = FindNextNode(treeView.Nodes, lastSearchTerm, matchCase, matchWholeWord, lastFoundNode, backward);

                if (nextNode != null)
                {
                    treeView.SelectedNode = nextNode;
                    treeView.Focus();
                    lastFoundNode = nextNode;
                    toolStripLabel2.Text = $"Items found: {CountNodes(treeView.Nodes, lastSearchTerm, matchCase)}";
                }
                else
                {
                    toolStripLabel2.Text = "No more items found";
                    MessageBox.Show("No more items found.", "Searching", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lastFoundNode = null;
                }
            }
            else
            {
                toolStripLabel2.Text = "Form1 is not available.";
                MessageBox.Show("Form1 is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private TreeNode FindNextNode(TreeNodeCollection nodes, string searchTerm, bool matchCase, bool matchWholeWord, TreeNode startNode, bool backward)
        {
            StringComparison comparisonType = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            bool foundStartNode = false;
            TreeNode foundNode = null;

            if (backward)                                                                           //  If searching backward, then copying nodes to array?
            {
                TreeNode[] nodeArray = new TreeNode[nodes.Count];
                nodes.CopyTo(nodeArray, 0);
                Array.Reverse(nodeArray);                                                           //  Reversing array.
                foreach (TreeNode node in nodeArray)
                {
                    if (foundStartNode)                                                             //  If we found the starting node, then continue searching for further items.
                    {
                        if (matchWholeWord)
                        {
                            if (string.Equals(node.Text, searchTerm, comparisonType))
                            {
                                foundNode = node;
                                break;
                            }
                        }
                        else
                        {
                            if (node.Text.Contains(searchTerm, comparisonType))
                            {
                                foundNode = node;
                                break;
                            }
                        }

                        TreeNode nextNode = FindNextNode(node.Nodes, searchTerm, matchCase, matchWholeWord, startNode, backward);
                        if (nextNode != null)
                        {
                            foundNode = nextNode;
                            break;
                        }
                    }
                    if (node == startNode)                                                          //  If we stll could not find the starting node, check it is a start node.
                    {
                        foundStartNode = true;
                    }
                }
            }
            else                                                                                    //  If searching forward, then searching through nodes.
            {
                foreach (TreeNode node in nodes)                                                    //  If start node has been found, then searching for additional items.
                {
                    if (foundStartNode)
                    {
                        if (matchWholeWord)
                        {
                            if (string.Equals(node.Text, searchTerm, comparisonType))
                            {
                                foundNode = node;
                                break;
                            }
                        }
                        else
                        {
                            if (node.Text.Contains(searchTerm, comparisonType))
                            {
                                foundNode = node;
                                break;
                            }
                        }

                        TreeNode nextNode = FindNextNode(node.Nodes, searchTerm, matchCase, matchWholeWord, startNode, backward);
                        if (nextNode != null)
                        {
                            foundNode = nextNode;
                            break;
                        }
                    }
                    if (node == startNode)                                                          //  If could not find start node, check it is a start node.
                    {
                        foundStartNode = true;
                    }
                }
            }
            return foundNode;                                                                       //  If searching through & no more items left.
        }

        public class SearchEventArgs : EventArgs
        {
            public string SearchTerm { get; }
            public TreeNode FoundNode { get; }

            public SearchEventArgs(string searchTerm, TreeNode foundNode)
            {
                SearchTerm = searchTerm;
                FoundNode = foundNode;
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            lastSearchTerm = comboBox1.Text;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
            }
            else 
            { 
            }
            lastFoundNode = null;
        }
    }
}