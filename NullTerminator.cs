// NullTerminator.cs

using System.Collections.Generic;
using System.IO;
using System.Text;

public static class NullTerminator
{
    public static string ReadNullTerminatedString(BinaryReader reader)
    {
        List<byte> bytes = new List<byte>();
        byte b;
        while ((b = reader.ReadByte()) != 0)
        {
            bytes.Add(b);
        }

        try
        {
            return Encoding.UTF7.GetString(bytes.ToArray());
        }
        
        catch
        {
            return Encoding.ASCII.GetString(bytes.ToArray());
        }
    }
    
}
