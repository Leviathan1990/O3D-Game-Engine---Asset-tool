using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AssetTool
{
    public static class TViewSpecs
    {
        private static ImageList imageList;
        public const string FolderClosedImageKey = "folder_closed";
        public const string FolderOpenImageKey = "folder_open";

        static TViewSpecs()
        {
            imageList = new ImageList();
            imageList.ImageSize = new Size(25, 25);
            //  Open-Close folder
            imageList.Images.Add(FolderClosedImageKey, Properties.Resources.icons8_file_folder_32);
            imageList.Images.Add(FolderOpenImageKey, Properties.Resources.icons8_opened_folder_32);
            //  Image files
            imageList.Images.Add(".bmp", Properties.Resources.icons8_bmp_32);
            imageList.Images.Add(".jpg", Properties.Resources.icons8_jpg_32);
            imageList.Images.Add(".png", Properties.Resources.icons8_png_32);
            //  Box archive files
            imageList.Images.Add(".box", Properties.Resources.icons8_box_32);
            //  Script files
            imageList.Images.Add(".cfg", Properties.Resources.icons8_script_32);
            imageList.Images.Add(".ai", Properties.Resources.icons8_ai_32);
            imageList.Images.Add(".oms", Properties.Resources.icons8_script_32);
            //  Font file
            imageList.Images.Add(".cof", Properties.Resources.icons8_font_32);
            //  Media fules
            imageList.Images.Add(".bik", Properties.Resources.icons8_video_32);
            imageList.Images.Add(".mp3", Properties.Resources.icons8_speaker_32);
            //  Asset container main project file
            imageList.Images.Add(".opf", Properties.Resources.icons8_shipping_container_32);
            //  O3D engine files
            imageList.Images.Add(".gui", Properties.Resources.icons8_gui_48);
            imageList.Images.Add(".GUI", Properties.Resources.icons8_gui_48);
            //  id file
            imageList.Images.Add(".id", Properties.Resources.icons8_id_32);

            //  Microsoft Visual Sourcasafe
            imageList.Images.Add(".scc", Properties.Resources.icons8_file_32);
            //  JASC Brows file
            imageList.Images.Add(".jbf", Properties.Resources.icons8_paint_32);

            //  Asset container: Classes section files
            imageList.Images.Add(".CBaseClass", Properties.Resources.progicon);
            imageList.Images.Add(".CGridMember", Properties.Resources.Dummy_Groups_and_Views_BG);
            imageList.Images.Add(".CUnit", Properties.Resources.build_icon_hawk);
            imageList.Images.Add(".CUnitWeapon", Properties.Resources.HM_LightLaserTurret);
            imageList.Images.Add(".unknown", Properties.Resources.build);
        }

        public static void SetImageList(TreeView treeView)
        {
            treeView.ImageList = imageList;
        }

        public static string GetImageKey(string fileName)
        {
            string extension = Path.GetExtension(fileName)?.ToLower();
            if (string.IsNullOrEmpty(extension))
            {
                return FolderClosedImageKey;
            }
            switch (extension)
            {
                case ".bmp":
                    return ".bmp";
                case ".jpg":
                case ".jpeg":
                    return ".jpg";
                case ".png":
                    return ".png";
                case ".box":
                    return ".box";
                case ".cfg":
                    return ".cfg";
                case ".ai":
                    return ".ai";
                case ".oms":
                    return ".oms";
                case ".cof":
                    return ".cof";
                case ".bik":
                    return ".bik";
                case ".mp3":
                    return ".mp3";
                case ".opf":
                    return ".opf";
                case ".gui":
                    return ".gui";
                case ".id":
                    return ".id";
                case ".jbf":
                    return ".jbf";
                case ".scc":
                    return ".scc";
                case ".CBaseClass":
                    return ".CBaseClass";
                case ".unknown":
                    return ".unknown";
                case ".CGridMember":
                    return ".CGridMember";
                case ".CUnit":
                    return ".CUnit";
                case ".CUnitWeapon":
                    return ".CUnitWeapon";
                default:

                    return FolderClosedImageKey; // Default image key
            }
        }
    }
}