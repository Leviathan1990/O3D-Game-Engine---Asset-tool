/*  *.box archive structure
 *  
 *  The Outforce O3D Engine Asset Tool.
 *  Designed by: Krisztian Kispeti
 *  Location: Kaposvár, HU.
 *  Contact:
 */

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OutforceFileStruct
{

    public struct BoxItem
    {
        public string Filename;
        public uint Offset;
        public uint Size;
        public bool IsDirectory { get; set; }
        public bool IsImage;
        public bool IsReadable;

        public BoxItem(string filename, uint offset, uint size)
        {
            Filename = filename;
            Offset = offset;
            Size = size;
            IsDirectory = IsDirectory;
        }

        public BoxItem()
        { }
    }
}