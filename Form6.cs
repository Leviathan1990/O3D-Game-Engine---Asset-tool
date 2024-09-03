/*  Image Inspector code
 *  
 *  The Outforce O3D Engine Asset Tool.
 *  Designed by:        Krisztian Kispeti
 *  Location:           Kaposvár, HU.
 *  Contact:
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using OutforceFileStruct;
using System.Configuration;
using System.Drawing.Text;
using System.Diagnostics.Eventing.Reader;

namespace AssetTool
{
    public partial class Form6 : Form
    {
        public string TextBox1Text
        {
            get { return (textBox1.Text); }
            set { textBox1.Text = value; }
        }
        private string selectedFormat = "";
        private int rotationAngleX = 0;                                                             //  Variable to store the rotation for angle X
        private int rotationAngleY = 0;                                                             //  Variable to store the rotation for angle Y  

        public PictureBox PictureBox2
        {
            get { return pictureBox2; }
        }

        public Form6()
        {
            InitializeComponent();

            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(imgFile.IMGFILE.GetSupportedImageFormats());      //  imgFime.cs
            // this.comboBox1.Items.AddRange(new object[] { "Bitmap (*.bmp)", "JPEG (*.jpeg)", "JPG (*.jpg)", "GIF (*.gif)", "PNG (*.png)", "TIFF (*.tiff)", "HEIF (*.heif)","ICON (*.icon)", "WMF (*.wmf)" // Original
            //});   // Original

            this.KeyDown += new KeyEventHandler(Form6_KeyDown);
            this.KeyPreview = true;
        }

        private void Form6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X && pictureBox2.Image != null)
            {
                rotationAngleX = (rotationAngleX + 90) % 360;                               // Rotate around X axis
                pictureBox2.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pictureBox2.Refresh();
            }
            else if (e.KeyCode == Keys.Y && pictureBox2.Image != null)
            {
                rotationAngleY = (rotationAngleY + 90) % 360;                               // Rotate around Y axis
                pictureBox2.Image.RotateFlip(RotateFlipType.Rotate90FlipY);
                pictureBox2.Refresh();
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void SetToolstripLabels(string fileName, uint offset, uint size)
        {
            toolStripLabel2.Text = fileName;
            toolStripLabel4.Text = offset.ToString();
            toolStripLabel6.Text = size.ToString();
        }

        public void UpdateImageDimensions()
        {
            if (pictureBox2.Image != null)
            {
                numericUpDown1.Value = pictureBox2.Image.Width;
                numericUpDown2.Value = pictureBox2.Image.Height;
            }
            else
            {
                numericUpDown1.Value = 0;
                numericUpDown2.Value = 0;
            }
        }

        public void SetPictureBoxImage(string imagePath)
        {
            try
            {
                using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    pictureBox2.Image = Image.FromStream(fs);
                    pictureBox2.SizeMode = PictureBoxSizeMode.Normal;
                    ImageFormat format = pictureBox2.Image.RawFormat;

                    foreach (var item in comboBox1.Items)
                    {
                        string itemText = item.ToString();
                        if (itemText.Contains(GetImageFormatDescription(format)))
                        {
                            comboBox1.SelectedItem = item;
                            break;
                        }
                    }
                }

                UpdateImageDimensions();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.SizeMode = PictureBoxSizeMode.Normal;
        }

        private void autoSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void strectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void centerImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        private void imageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image == null)
            {
                MessageBox.Show("No image file loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (numericUpDown1.Text == "")                                                          // Check that have filename and size
            {
                MessageBox.Show("File name can not be empty! Name your image file with out extension!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int imageWidth = 0, imageHeight = 0;
            if (!int.TryParse(numericUpDown1.Text, out imageWidth) || !int.TryParse(numericUpDown2.Text, out imageHeight))
            {
                MessageBox.Show("Invalid image sizes.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();                                   // Open SavefileDialog
            saveFileDialog.Filter = "Bitmap (*.bmp)|*.bmp|JPEG (*.jpeg)|*.jpeg|JPEG (*.jpg)|*.jpg|GIF (*.gif)|*.gif|PNG (*.png)|*.png|TIFF (*.tiff)|*.tiff";
            saveFileDialog.FilterIndex = comboBox1.SelectedIndex + 1;                               // Selected format index in ComboBox
            saveFileDialog.FileName = textBox1.Text;                                                // Set filename

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ImageFormat format = GetImageFormatFromComboBox(saveFileDialog.FilterIndex);        // Save image in the selected
                if (format != null)
                {
                    try
                    {
                        Bitmap bmp = new Bitmap(pictureBox2.Image, imageWidth, imageHeight);        // Create new Bitmap with the set size
                        bmp.Save(saveFileDialog.FileName, format);                                  // Save image in the set format
                        MessageBox.Show("Image saved and exported!", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An unknown error has occured while saving the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Can not find format for the selected index.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private ImageFormat GetImageFormatFromComboBox(int index)
        {
            switch (index)
            {
                case 1: return ImageFormat.Bmp;
                case 2: return ImageFormat.Jpeg;
                case 3: return ImageFormat.Jpeg;
                case 4: return ImageFormat.Gif;
                case 5: return ImageFormat.Png;
                case 6: return ImageFormat.Tiff;
                case 7: return ImageFormat.Heif;
                case 8: return ImageFormat.Icon;
                case 9: return ImageFormat.Wmf;
                default: return null;
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Refresh();
            pictureBox2.Update();
            UpdateImageDimensions();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedFormat = comboBox1.SelectedItem.ToString();
        }

        private string GetImageFormatDescription(ImageFormat format)
        {
            foreach (var prop in typeof(ImageFormat).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))  //  Get description about file format
            {
                ImageFormat imgFormat = (ImageFormat)prop.GetValue(null, null);
                if (imgFormat.Guid == format.Guid)
                {
                    return prop.Name + " (*" + prop.GetValue(null).ToString().ToLower() + ")";
                }
            }
            return "";
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image.RotateFlip(RotateFlipType.Rotate90FlipX);
                pictureBox2.Refresh();
                UpdateImageDimensions();                                                            //  Update textBox that stores image size X:90.
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image.RotateFlip(RotateFlipType.Rotate90FlipY);
                pictureBox2.Refresh();
                UpdateImageDimensions();                                                            //  Update textBox that stores image size Y:90.
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image.RotateFlip(RotateFlipType.Rotate180FlipX);
                pictureBox2.Refresh();
                UpdateImageDimensions();                                                            //  Update textBox that stores image size X:180.
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image.RotateFlip(RotateFlipType.Rotate270FlipX);
                pictureBox2.Refresh();
                UpdateImageDimensions();                                                            //  Update textBox that stores image size X:270.
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image.RotateFlip(RotateFlipType.Rotate180FlipY);
                pictureBox2.Refresh();
                UpdateImageDimensions();                                                            //  Update textBox that stores image size Y:180.
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image.RotateFlip(RotateFlipType.Rotate270FlipY);
                pictureBox2.Refresh();
                UpdateImageDimensions();                                                            //  Update textBox that stores image size Y:270.
            }
        }

        private void bitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                Image originalImage = pictureBox2.Image;
                Image convertedImage = imgFile.conversion.ConvertTo16Bit(originalImage);
                pictureBox2.Image = convertedImage;
            }
        }

        private void bitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                Image originalImage = pictureBox2.Image;
                Image convertedImage = imgFile.conversion.ConvertTo32Bit(originalImage);
                pictureBox2.Image = convertedImage;
            }
        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left && pictureBox2.Image != null)
            {
                if (pictureBox2.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    pictureBox2.SizeMode = PictureBoxSizeMode.Normal;
                }
                else
                {
                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }

            else if (me.Button == MouseButtons.Right && pictureBox2.Image != null)
            {
                pictureBox2.SizeMode |= PictureBoxSizeMode.CenterImage;
            }
        }

        private void imageInspectorUsageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Program usage with mouse:\n\n\t Double left click: Strect Image\n\t Double left click again: Normal mode.\n\t Right click: Center image\n\n Program usage with keyboard:\n\n\t X: Rotate X:90 \n\t Y: Rotate Y:90",
           "Product information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {

            label7.Text = $"{e.X}";
            label9.Text = $"{e.Y}";
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            label7.Text = "";
            label9.Text = "";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)                                                          //  Check if the pictures is loaded.
            {
                int originalWidth = pictureBox2.Image.Width;
                int originalHeight = pictureBox2.Image.Height;

                if (checkBox1.Checked)
                {
                    decimal aspectRatio = (decimal)originalWidth / originalHeight;                  //  Keep aspect ratio.
                    if (numericUpDown1.Value != originalWidth)                                      //  If loaded image width is being modified, then set the height too.
                    {
                        numericUpDown2.Value = (decimal)(numericUpDown1.Value / aspectRatio);
                    }
                    else if (numericUpDown2.Value != originalHeight)                                //  If loaded image height is being modified, then set the width too.
                    {
                        numericUpDown1.Value = (decimal)(numericUpDown2.Value * aspectRatio);
                    }
                }
                else
                {
                    //  If aspect ratio if off, then won't modify anything, default values will be used.
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)                                                                  //  Update loaded image width by keep aspect ratio [ON].
            {
                decimal aspectRatio = (decimal)pictureBox2.Image.Width / pictureBox2.Image.Height;
                numericUpDown2.Value = (decimal)(numericUpDown1.Value / aspectRatio);
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                decimal aspectRatio = (decimal)pictureBox2.Image.Width / pictureBox2.Image.Height;  //  Update loaded image height by keep aspect ratio [ON].
                numericUpDown1.Value = (decimal)(numericUpDown2.Value * aspectRatio);
            }
        }


    }
}

//  Upcoming fixes here:
//  1.) Err: If I left double click when the displayed image mode is Zoom, the program stopped working. [Investigation in progress] 



//  <--------------------------------------------|O|-------------------------------------------->

//  Status: 1 issue needs to be done