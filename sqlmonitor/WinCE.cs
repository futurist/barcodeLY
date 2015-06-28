using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;

namespace sqlmonitor
{
    /*
     * HANDLE = IntPtr
     * LPCTSTR = string
     * DWORD = UInt32
    */

    public class WinCEConst
    {
        public const int ERROR_SUCCESS = 0;
        public const int MAX_ADAPTER_DESCRIPTION_LENGTH = 128;
        public const int ERROR_BUFFER_OVERFLOW = 111;
        public const int MAX_ADAPTER_NAME_LENGTH = 256;
        public const int MAX_ADAPTER_ADDRESS_LENGTH = 8;
        public const int MIB_IF_TYPE_OTHER = 1;
        public const int MIB_IF_TYPE_ETHERNET = 6;
        public const int MIB_IF_TYPE_TOKENRING = 9;
        public const int MIB_IF_TYPE_FDDI = 15;
        public const int MIB_IF_TYPE_PPP = 23;
        public const int MIB_IF_TYPE_LOOPBACK = 24;
        public const int MIB_IF_TYPE_SLIP = 28;
        public const UInt32 STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        public const UInt32 SECTION_QUERY = 0x0001;
        public const UInt32 SECTION_MAP_WRITE = 0x0002;
        public const UInt32 SECTION_MAP_READ = 0x0004;
        public const UInt32 SECTION_MAP_EXECUTE = 0x0008;
        public const UInt32 SECTION_EXTEND_SIZE = 0x0010;
        public const UInt32 SECTION_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SECTION_QUERY |
            SECTION_MAP_WRITE |
            SECTION_MAP_READ |
            SECTION_MAP_EXECUTE |
            SECTION_EXTEND_SIZE);
        public const UInt32 FILE_MAP_ALL_ACCESS = SECTION_ALL_ACCESS;

        public const int CREATE_NEW = 1;
        public const int CREATE_ALWAYS = 2;
        public const int OPEN_EXISTING = 3;
        public const int OPEN_ALWAYS = 4;
        public const int TRUNCATE_EXISTING = 5;

        public const UInt32 GENERIC_READ = 0x80000000;
        public const UInt32 GENERIC_WRITE = 0x40000000;
        public const UInt32 GENERIC_EXECUTE = 0x20000000;
        public const UInt32 GENERIC_ALL = 0x10000000;

        public const int FILE_SHARE_READ = 0x00000001;
        public const int FILE_SHARE_WRITE = 0x00000002;

        public const int FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        public const int FILE_ATTRIBUTE_NORMAL = 0x00000080;
        public const int FILE_FLAG_RANDOM_ACCESS = 0x10000000;
        public const int SEC_COMMIT = 0x8000000;

        public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        public const int PAGE_READWRITE = 0x04;
        public const int PAGE_READONLY = 0x02;
        public const int FILE_MAP_READ = 0x0004;
        public const int FILE_MAP_WRITE = 0x0002;
        //public const int FILE_MAP_ALL_ACCESS = 0x001f;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct IP_ADDRESS_STRING
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Address;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct IP_ADDR_STRING
    {
        public IntPtr Next;
        public IP_ADDRESS_STRING IpAddress;
        public IP_ADDRESS_STRING IpMask;
        public Int32 Context;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SECURITY_ATTRIBUTES
    {
        public int nLength;
        public IntPtr lpSecurityDescriptor;
        public int bInheritHandle;
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct IP_ADAPTER_INFO
    {
        public IntPtr Next;
        public Int32 ComboIndex;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WinCEConst.MAX_ADAPTER_NAME_LENGTH + 4)]
        public string AdapterName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WinCEConst.MAX_ADAPTER_DESCRIPTION_LENGTH + 4)]
        public string AdapterDescription;
        public UInt32 AddressLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = WinCEConst.MAX_ADAPTER_ADDRESS_LENGTH)]
        public byte[] Address;
        public Int32 Index;
        public UInt32 Type;
        public UInt32 DhcpEnabled;
        public IntPtr CurrentIpAddress;
        public IP_ADDR_STRING IpAddressList;
        public IP_ADDR_STRING GatewayList;
        public IP_ADDR_STRING DhcpServer;
        public bool HaveWins;
        public IP_ADDR_STRING PrimaryWinsServer;
        public IP_ADDR_STRING SecondaryWinsServer;
        public Int32 LeaseObtained;
        public Int32 LeaseExpires;
    }

    [Flags]
    public enum FileMapAccess : uint
    {
        FileMapCopy = 0x0001,
        FileMapWrite = 0x0002,
        FileMapRead = 0x0004,
        FileMapAllAccess = 0x001f,
        fileMapExecute = 0x0020,
    }

    [Flags]
    enum FileMapProtection : uint
    {
        PAGE_READONLY = 0x02,
        PAGE_READWRITE = 0x04,
        PAGE_WRITECOPY = 0x08,       // WinCE not support
        PageExecuteRead = 0x20,
        PageExecuteReadWrite = 0x40,
        SEC_COMMIT = 0x8000000,
        SEC_IMAGE = 0x1000000,       // WinCE not support
        SEC_NOCACHE = 0x10000000,    // WinCE not support
        SEC_RESERVE = 0x4000000,     // WinCE not support
    }


    public class WinCE
    {
        // * WinCE¤U­±¬Ocoredll.dll
        // * Windows¤U­±¬Okeneral32.dll

        [DllImport("iphlpapi.dll", CharSet = CharSet.Auto)]
        public static extern int GetAdaptersInfo(IntPtr pAdapterInfo, ref int pBufOutLen);

        [DllImport("Coredll.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
            );

        [DllImport("Coredll.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFileMapping(
            IntPtr hFile,
            object lpFileMappingAttributes,
            uint flProtect,
            uint dwMaximumSizeHigh,
            uint dwMaximumSizeLow,
            string lpName);

        [DllImport("Coredll.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFileForMapping(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes, // set null
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        // WinCE not support OpenFileMapping
        //[DllImport("coredll.dll", SetLastError = true)]
        //public static extern IntPtr OpenFileMapping(uint dwDesiredAccess, bool bInheritHandle, string lpName);
        public static IntPtr OpenFileMapping(uint dwDesiredAccess, bool bInheritHandle, string lpName)
        {
            //IntPtr t_pHandle = CreateFileForMapping(lpName, WinCEConst.GENERIC_ALL,
            //    WinCEConst.FILE_SHARE_READ | WinCEConst.FILE_SHARE_WRITE, IntPtr.Zero,
            //    WinCEConst.OPEN_EXISTING,
            //    WinCEConst.FILE_ATTRIBUTE_NORMAL | WinCEConst.FILE_FLAG_RANDOM_ACCESS, IntPtr.Zero);
            IntPtr t_pHandle = CreateFileMapping(new IntPtr(-1), null,
                    WinCEConst.PAGE_READWRITE, 0, 0, lpName);
            return t_pHandle;
        }

        [DllImport("Coredll.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("Coredll.dll", SetLastError = true)]
        public static extern IntPtr MapViewOfFile(
            IntPtr hFileMappingObject,
            uint dwDesiredAccess,
            uint dwFileOffsetHigh,
            uint dwFileOffsetLow,
            uint dwNumberOfBytesToMap);

        [DllImport("Coredll.dll", SetLastError = true)]
        public static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);



        [DllImport("Coredll.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        static extern void CopyMemory(IntPtr destination, IntPtr source, uint length);

        [DllImport("Coredll.dll", EntryPoint = "MoveMemory", SetLastError = false)]
        static extern void MoveMemory(IntPtr destination, IntPtr source, uint length);

        [DllImport("Coredll.dll", EntryPoint = "RtlFillMemory", SetLastError = false)]
        static extern void FillMemory(IntPtr destination, uint length, byte fill);




        const string MapName = "HSLY_SQL_FILE";
        const int MapSize = 1024 * 1024;
        static IntPtr m_pMapFile= IntPtr.Zero;
        static IntPtr m_pBuff= IntPtr.Zero;


        public static void createMemFile(string str)
        {

            //str = str.Replace("+", " ");
            //str = Regex.Replace(str, "%([A-Fa-f\\d]{2})", a => "" + Convert.ToChar(Convert.ToInt32(a.Groups[1].Value, 16)));
            
            //Regex rx = new Regex(@"\\[uU]([0-9A-F]{4})");
            //str = rx.Replace(str, delegate(Match match) { return ((char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString(); });

            int t_i4Error = 0;
            try
            {
                if (m_pMapFile == IntPtr.Zero)
                {
                    // create map file
                    m_pMapFile = WinCE.CreateFileMapping(WinCEConst.INVALID_HANDLE_VALUE, null,
                        WinCEConst.PAGE_READWRITE, 0, MapSize, MapName);
                    t_i4Error = Marshal.GetLastWin32Error();

                    // get map file
                    m_pBuff = WinCE.MapViewOfFile(m_pMapFile, WinCEConst.FILE_MAP_ALL_ACCESS, 0, 0, MapSize);
                    t_i4Error = Marshal.GetLastWin32Error();
                }



                //FillMemory(m_pBuff, MapSize, 0);

                // String to byte array
                byte[] x = Encoding.UTF8.GetBytes(str);
                byte[] y = { 0,0,0,0, 0,0,0,0 };

                var z = new byte[x.Length + y.Length];
                x.CopyTo(z, 0);
                y.CopyTo(z, x.Length);

                // write data to map file
                Marshal.Copy(z, 0, m_pBuff, z.Length);

            }
            catch (Exception ex)
            {
                t_i4Error = Marshal.GetLastWin32Error();
                MessageBox.Show("writeMemFile Err: " + ex.Message+ "\n" + ex.StackTrace);
            }
        }


        public static string readMemFile()
        {
            int t_i4Error = 0;
            try
            {
                if (m_pMapFile == IntPtr.Zero)
                {
                    // open map file
                    //m_pMapFile = WinCE.CreateFileMapping(WinCEConst.INVALID_HANDLE_VALUE, null,
                    //    WinCEConst.PAGE_READWRITE, 0, 0, MapName);
                    m_pMapFile = WinCE.OpenFileMapping(WinCEConst.FILE_MAP_ALL_ACCESS, false, MapName);
                    // check error code
                    t_i4Error = Marshal.GetLastWin32Error();

                    // get map file
                    m_pBuff = WinCE.MapViewOfFile(m_pMapFile, WinCEConst.FILE_MAP_ALL_ACCESS, 0, 0, MapSize);
                }

                byte[] bytData = new byte[MapSize];
                // read data from map file
                Marshal.Copy(m_pBuff, bytData, 0, MapSize);
                // change byte array to string

                int i;
                for (i = 0; i < MapSize; ++i)
                {
                    if (bytData[i] == 0)
                    {
                        if ((bytData[i + 1] | bytData[i + 2] | bytData[i + 3] |
                            bytData[i + 4] | bytData[i + 5] | bytData[i + 6] | bytData[i + 7]) == 0)
                        break;
                    }
                }
                
                string RetVal = Encoding.UTF8.GetString(bytData, 0, i);
                // RetVal = Regex.Split(RetVal, "!!!!")[0];
                return RetVal;
            }
            catch (Exception ex)
            {
                t_i4Error = Marshal.GetLastWin32Error();
                //MessageBox.Show("readMemFile Err: " + ex.Message + "\n" + ex.StackTrace);
                return "";
            }
        }

        public static void closeMemFile()
        {
            try
            {
                WinCE.UnmapViewOfFile(m_pBuff);
                WinCE.CloseHandle(m_pMapFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



    
    }
}
