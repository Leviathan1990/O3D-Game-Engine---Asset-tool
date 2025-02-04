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
        private Random random = new Random();
        private List<Bitmap> images;

        public Form2()
        {
            InitializeComponent();
            LoadImagesFromResources();
            LoadRandomImage();
        }

        public ProgressBar ProgressBar2
        {
            get { return progressBar2; }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            progressBar2.Value = 0;
        }

        private void LoadImagesFromResources()
        {
            images = new List<Bitmap>
            { 
            Properties.Resources.letöltés,
            Properties.Resources.letöltés__1_
            };
        }

        private void LoadRandomImage()
        {
            if (images.Count > 0) 
            {
                int index = random.Next(images.Count);
                pictureBox1.Image = images[index];
            }

            else
            {
                MessageBox.Show("Could not find images for Outforce Worker!", "Error",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
