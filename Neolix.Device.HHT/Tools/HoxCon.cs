using System;
using System.Text;

namespace Neolix.Device.Tools
{
    internal class HexCon
    {
        //converter hex string to byte and byte to hex string
        
        public static string GetString(byte[] inBytes)
        {
            return GetString(inBytes, Encoding.Default);
        }

        public static string GetString(byte[] inBytes, Encoding encoding)
        {
            if (inBytes == null)
            {
                return string.Empty;
            }
            byte[] tmp = new byte[inBytes.Length + 2];
            Buffer.BlockCopy(inBytes, 0, tmp, 0, inBytes.Length);
            return encoding.GetString(tmp, 0, Array.IndexOf(tmp, (byte)0));
        }

        public static byte[] GetBytes(string inString, Encoding encoding)
        {
            if (inString == null)
            {
                return null;
            }
            return encoding.GetBytes(inString);
        }

        public static byte[] GetBytes(string inString)
        {
            return GetBytes(inString, Encoding.Default);
        }
    }
}