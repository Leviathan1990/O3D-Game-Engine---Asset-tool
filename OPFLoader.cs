/*  *.OPF archive Loader code
 *  
 *  The Outforce O3D Engine Asset Tool.
 *  Designed by: Krisztian Kispeti
 *  Location: Kaposvár, HU.
 *  Contact:
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OutforceopfFileStruct
{
    public class OPFLoader
    {
        public static OutforceopfArchive LoadOPFArchive(string filePath)
        {
            OutforceopfArchive archive = new OutforceopfArchive();
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read)))
                {
                    if (reader.BaseStream.Length < 224 || !CheckHeader(reader))         // Check header information
                    {
                        throw new Exception("Invalid OPF file format.");
                    }
                    reader.BaseStream.Seek(224, SeekOrigin.Begin);                      // Skip header information
                    int numImages = reader.ReadInt32() +6;                              // Read image directory
                    for (int i = 0; i < numImages; i++)
                    {
                        long startOffset = reader.BaseStream.Position;

                        short nameLength = reader.ReadInt16();                          // 2 - Filename Length
                        string filename = ReadString(reader, nameLength) + ".jpg";

                        if (filename[0] == '\0')
                        {
                            filename = $"Image_{i}.jpg";
                            reader.BaseStream.Seek(startOffset + 17, SeekOrigin.Begin);
                            int remainingPadding = reader.ReadInt32();
                            reader.BaseStream.Seek(remainingPadding, SeekOrigin.Current);
                        }
                        else
                        {
                            nameLength = reader.ReadInt16();                             // 2 - Source File Path Length
                            SkipBytes(reader, nameLength);                               // Skip Source File Path

                            nameLength = reader.ReadInt16();                             // 2 - Drive Length
                            SkipBytes(reader, nameLength);                               // Skip Drive

                            reader.BaseStream.Seek(70, SeekOrigin.Current);              // Skip the rest of the image header

                            int remainingPadding = reader.ReadInt32();
                            reader.BaseStream.Seek(remainingPadding, SeekOrigin.Current);
                        }

                        int length = reader.ReadInt32();                                 // 4 - File Length
                        byte[] fileData = reader.ReadBytes(length);

                        OPFImage image = new OPFImage
                        {
                            Filename = AppendExtensionIfNeeded(filename, fileData),
                            FileData = fileData
                        };
                        archive.Images.Add(image);
                    }

                    // SCRIPTS

                    int numScripts = reader.ReadInt32();                                //  Read script directory
                    for (int i = 0; i < numScripts; i++)
                    {
                        short nameLength = reader.ReadInt16();                          //  2 - Filename Length
                        string filename = ReadString(reader, nameLength) + ".unknown";

                        byte[] fileData = reader.ReadBytes(899);                        //  899 - File Data

                        OPFScript script = new OPFScript
                        {
                            Filename = AppendExtensionIfNeeded(filename, fileData),
                            FileData = fileData
                        };
                        archive.Scripts.Add(script);
                    }

                    // Read classes section directory
                    int numClasses = reader.ReadInt32() * 2;                            // *2 ?
                    for (int i = 0; i < numClasses; i++)
                    {
                        long offset = reader.BaseStream.Position;

                        short nameLength = reader.ReadInt16();                          // 2 - Class Type Length
                        string classType = ReadString(reader, nameLength);

                        reader.BaseStream.Seek(9, SeekOrigin.Current);                  // Skip unknown bytes
                        
                        nameLength = reader.ReadInt16();                                // 2 - Filename Length
                        string filename = ReadString(reader, nameLength) + "." + classType;

                        reader.BaseStream.Seek(100, SeekOrigin.Current);                // Skip remaining unknown bytes

                        long currentOffset = reader.BaseStream.Position;
                        while (currentOffset < reader.BaseStream.Length)
                        {
                            reader.BaseStream.Seek(currentOffset, SeekOrigin.Begin);
                            if (reader.ReadInt32() == 16256 && reader.ReadInt32() == 16256 && reader.ReadInt32() == 16256 && reader.ReadInt32() == 0)
                            {
                                break;
                            }
                            currentOffset++;
                        }
                        long length = currentOffset - offset;
                        byte[] fileData = reader.ReadBytes((int)length);
                        OPFClass classObj = new OPFClass
                        {
                            Filename = AppendExtensionIfNeeded(filename, fileData),
                            FileData = fileData
                        };
                        archive.Classes.Add(classObj);
                    }
                    
                    //reader.Close();
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error: File not found. {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error: IO exception. {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
           
            return (archive);
        }

        private static bool CheckHeader(BinaryReader reader)                            //  Check for "Outforce Packed Content" header
        {
            byte[] headerBytes = Encoding.UTF7.GetBytes("Outforce Packed Content");         
            byte[] fileHeader = reader.ReadBytes(headerBytes.Length);
            return StructuralComparisons.StructuralEqualityComparer.Equals(headerBytes, fileHeader);
        }

        private static string ReadString(BinaryReader reader, short length)
        {
            byte[] bytes = reader.ReadBytes(length);
            return Encoding.UTF7.GetString(bytes);                                      //  Because of UTF8 can not handle Swedish characters like: ä
        }

        private static void SkipBytes(BinaryReader reader, int count)
        {
            reader.BaseStream.Seek(count, SeekOrigin.Current);
        }

        private static string AppendExtensionIfNeeded(string filename, byte[] fileData)
        {
            if (!HasExtension(filename))                                                //  Check if the file already has an extension
            {
                string fileType = DetermineFileType(fileData);                          //  Determine the file type based on the file content
                switch (fileType)                                                       //  Append the appropriate extension based on the detected file type
                {
                    case "jpg":
                        filename += ".jpg";
                        break;
                    case "unknown":
                        filename += ".unknown";
                        break;
                    case "CBaseClass":
                        filename += ".CBaseClass";
                        break;
                    case "CGridMember":
                        filename += ".CGridMember";
                        break;
                    case "CUnit":
                        filename += ".CUnit";
                        break;
                    case "CUnitWeapon":
                        filename += ".CUnitWeapon";
                        break;
                    // Add more cases as needed for other file types
                    default:
                        break;
                }
            }
            return filename;
        }

        private static bool HasExtension(string filename)
        {
            return filename.Contains(".");
        }

        private static string DetermineFileType(byte[] fileData)
        {
            if (fileData.Length >= 2 && fileData[0] == 0xFF && fileData[1] == 0xD8)
            {
                return "jpg";           // JPEG format
            }
            else if (fileData.Length >= 2 && fileData[0] == 'C' && fileData[1] == 'B')
            {
                return "CBaseClass";    // CBaseClass format
            }
            else if (fileData.Length >= 2 && fileData[0] == 'C' && fileData[1] == 'G')
            {
                return "CGridMember";   // CGridMember format
            }
            else if (fileData.Length >= 2 && fileData[0] == 'C' && fileData[1] == 'U')
            {
                return "CUnit";         // CUnit format
            }
            else if (fileData.Length >= 2 && fileData[0] == 'C' && fileData[1] == 'W')
            {
                return "CUnitWeapon";   // CUnitWeapon format
            }
            else
            {
                return "unknown";       // Unknown format
            }
        }
    }

    public class OutforceopfArchive
    {
        public List<OPFImage> Images { get; set; } = new List<OPFImage>();
        public List<OPFScript> Scripts { get; set; } = new List<OPFScript>();
        public List<OPFClass> Classes { get; set; } = new List<OPFClass>();
    }

    public class OPFImage
    {
        public string Filename { get; set; }
        public byte[] FileData { get; set; }
    }

    public class OPFScript
    {
        public string Filename { get; set; }
        public byte[] FileData { get; set; }
    }

    public class OPFClass
    {
        public string Filename { get; set; }
        public byte[] FileData { get; set; }
    }
}

// Notes
// I used UTF7 inspite of UTF8 that can handle Swedish characters
// Still there's a problem with the provided *.opf archive structure.
// We can open and extract the content of the *.opf archive, but
// the program can only see less then 800 files out of the almost
// 2000 files stored in the archive...
// Image files (345 images) are ok, script files (410) (.UNKNOWNS) are ok
// The classes secltion files are the issues...
// We need to fix the structure...