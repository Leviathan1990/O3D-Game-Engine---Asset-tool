/*  Outforce Worker Code
 * 
 *  The Outforce O3D Engine Asset Tool.
 *  Designed by: Krisztian Kispeti
 *  Location: Kaposvár, HU.
 *  Contact:
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProgBarMechanism;

namespace AssetTool
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public ProgressBar ProgressBar2
        {
            get { return progressBar2; }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }
    }
}
