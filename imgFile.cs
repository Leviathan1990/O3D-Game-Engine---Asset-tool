using System.Drawing.Imaging;

namespace imgFile
{
    public static class IMGFILE
    {
        public static bool IsImageFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp" || extension == ".gif";
        }

        public static string[] GetSupportedImageFormats()
        {
            return new string[] {
                "Bitmap (*.bmp)",
                "JPEG (*.jpeg)",
                "JPG (*.jpg)",
                "GIF (*.gif)",
                "PNG (*.png)",
                "TIFF (*.tiff)",
                "HEIF (*.heif)",
                "ICON (*.icon)",
                "WMF (*.wmf)"
            };
        }
    }

    public class conversion
    {

        public static Image ConvertTo16Bit(Image originalImage)
        {
            //  Checking the original image is exists
            if (originalImage == null)
            {
                return null;
            }
            //  Create a 16bit bitmap
            Bitmap newImage = new Bitmap(originalImage.Width, originalImage.Height, PixelFormat.Format16bppRgb555);

            //  Copying pixels crom 'originalImage' to 'newImage'.
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height));
            }
            return newImage;
        }

        public static Image ConvertTo32Bit(Image originalImage)
        {
            //  Check original image is exists
            if (originalImage == null)
            {
                return null;
            }

            //  Create a 32bit bitmap
            Bitmap newImage = new Bitmap(originalImage.Width, originalImage.Height, PixelFormat.Format32bppArgb);
            
            //  Copying pixels crom 'originalImage' to 'newImage'.
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height));
            }
            return newImage;
        }

    }
}