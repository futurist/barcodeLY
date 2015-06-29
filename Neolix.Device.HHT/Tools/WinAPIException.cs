using System;
using System.Runtime.InteropServices;

namespace Neolix.Device.Tools
{
    /// <summary>
    /// Exception class for OpenNETCF WinAPI classes
    /// </summary>
    public class WinAPIException : Exception
    {
        private int win32Error;

        public WinAPIException(string Message)
            : base(Message + " " + GetErrorMessage(Marshal.GetLastWin32Error()))
        {
            this.win32Error = Marshal.GetLastWin32Error();
        }

        public WinAPIException(Exception ex)
            : base(ex.Message)
        {
            this.win32Error = 0;
        }

        public WinAPIException(string Message, int ErrorCode)
            : base(Message + " " + GetErrorMessage(ErrorCode))
        {
            this.win32Error = ErrorCode;
        }

        public int Win32Error
        {
            get
            {
                return win32Error;
            }
        }

        /// <summary>
        /// Specifies aspects of the formatting process and how to interpret the lpSource parameter.
        /// </summary>
        /// <remarks>The low-order byte of dwFlags specifies how the function handles line breaks in the output buffer.
        /// The low-order byte can also specify the maximum width of a formatted output line.</remarks>
        [Flags]
        public enum FormatMessageFlags : int
        {
            /// <summary>
            /// The function allocates a buffer large enough to hold the formatted message, and places a pointer to the allocated buffer at the address specified by lpBuffer.
            /// </summary>
            AllocateBuffer = 0x00000100,

            /// <summary>
            /// Insert sequences in the message definition are to be ignored and passed through to the output buffer unchanged.
            /// </summary>
            IgnoreInserts = 0x00000200,

            /// <summary>
            /// Specifies that lpSource is a pointer to a null-terminated message definition.
            /// </summary>
            FromString = 0x00000400,

            /// <summary>
            /// Specifies that lpSource is a module handle containing the message-table resource(s) to search.
            /// </summary>
            FromHModule = 0x00000800,

            /// <summary>
            /// Specifies that the function should search the system message-table resource(s) for the requested message.
            /// </summary>
            FromSystem = 0x00001000,

            /// <summary>
            /// Specifies that the Arguments parameter is not a va_list structure, but instead is just a pointer to an array of 32-bit values that represent the arguments.
            /// </summary>
            ArgumentArray = 0x00002000,

            /// <summary>
            /// Use the <b>MaxWidthMask</b> constant and bitwise Boolean operations to set and retrieve this maximum width value.
            /// </summary>
            MaxWidthMask = 0x000000FF,
        }

        [DllImport("coredll", EntryPoint = "FormatMessageW", SetLastError = false)]
        internal static extern int FormatMessage(FormatMessageFlags dwFlags, int lpSource, int dwMessageId,
                                                 int dwLanguageId, out IntPtr lpBuffer, int nSize, int[] Arguments);

        internal static string GetErrorMessage(int ErrNo)
        {
            IntPtr pBuffer;
            int nLen = FormatMessage(FormatMessageFlags.FromSystem | FormatMessageFlags.AllocateBuffer, 0, ErrNo, 0,
                                     out pBuffer, 0, null);
            if (nLen == 0) //Failed
            {
                return string.Format("Error {0} (0x{0:X})", ErrNo);
            }
            string sMsg = Marshal.PtrToStringUni(pBuffer, nLen);

            Marshal.FreeHGlobal(pBuffer);

            return sMsg;
        }
    }
}