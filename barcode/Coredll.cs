using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace barcode
{
    public static class Coredll
    {

        [Flags()]
        internal enum FullScreenFlags : int
        {
            SwHide = 0,
            ShowTaskbar = 0x1,
            HideTaskbar = 0x2,
            ShowSipButton = 0x4,
            HideSipButton = 0x8,
            SwRestore = 9,
            ShowStartIcon = 0x10,
            HideStartIcon = 0x20

        }

        internal enum Hwnd : int
        {
            HWND_TOP = 0,
            HWND_BOTTOM = 1,
            HWND_TOPMOST = -1,
            HWND_NOTOPMOST = -2
        }


        /***
         * not use at present

        /// <summary>
        /// The function retrieves the handle to the top-level
        /// window whose class name and window name match
        /// the specified strings. This function does not search child windows.
        /// </summary>
        /// <param name="lpClass"></param>
        /// <param name="lpWindow"></param>
        /// <returns></returns>
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern IntPtr FindWindowW(string lpClass, string lpWindow);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hwnd, int state);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndAfter,
                                                                          int xPos, int yPos, int cX, int cY, int wFlage);

        [DllImport("coredll.dll")]
        public static extern int GetSystemMetrics(int smIndex);

        [DllImport("coredll.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(Spi uiAction, uint uiParam, IntPtr pvParam, Spif fWinIni);

        [DllImport("coredll.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(Spi uiAction, uint uiParam, String pvParam, Spif fWinIni);

        [DllImport("coredll.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(Spi uiAction, uint uiParam, ref AnimationInfo pvParam, Spif fWinIni);

        [DllImport("coredll.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
        public static extern bool SystemParametersInfoGet(uint action, uint param, ref uint vparam, uint init);

        [DllImport("coredll.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
        public static extern bool SystemParametersInfoSet(uint action, uint param, uint vparam, uint init);

        /// <summary>
        /// to find whether you are running on a Smartphone or a Pocket PC
        /// </summary>
        /// <param name="uiAction"></param>
        /// <param name="uiParam"></param>
        /// <param name="pvParam"></param>
        /// <param name="fWinIni"></param>
        /// <returns></returns>
        [DllImport("Coredll.dll", EntryPoint = "SystemParametersInfoW", CharSet = CharSet.Unicode)]
        public static extern int SystemParametersInfo4Strings(uint uiAction, uint uiParam, System.Text.StringBuilder pvParam, uint fWinIni);


         * 
         * */
        


        /******
         * https://andocs.wordpress.com/2009/04/12/windows-ce-full-screen-applications/
         * /
         * 
         public static void StartFullScreen( Control control )
            {
                  IntPtr hWndInputPanel = Coredll.FindWindowW( "SipWndClass" , null );
                  IntPtr hWndSipButton = Coredll.FindWindowW( "MS_SIPBUTTON", null );
                  if( hWndInputPanel != null ) Coredll.ShowWindow( hWndInputPanel, SW_HIDE );
                  IntPtr hWndTaskBar = Coredll.FindWindowW( "HHTaskBar", null );
 
                  IntPtr hWnd = control.Handle;
 
                  if( hWndTaskBar != null ) Coredll.ShowWindow( hWndTaskBar, (int) Sw.SW_HIDE );
                  if( hWndInputPanel != null ) Coredll.ShowWindow( hWndInputPanel, (int) Sw.SW_HIDE );
                  if( hWndSipButton != null ) Coredll.ShowWindow( hWndSipButton, (int) Sw.SW_HIDE );
 
                  Coredll.SetWindowPos(
                        hWnd, (IntPtr) ( (int)Hwnd.HWND_TOPMOST ), 0, 0,
                        Coredll.GetSystemMetrics( (int) SystemMetric.SM_CXSCREEN ),
                        Coredll.GetSystemMetrics( (int) SystemMetric.SM_CYSCREEN ),
                        (int) Swp.SWP_SHOWWINDOW );
            }
 
            public static void StopFullScreen( Control control )
            {
                  //IntPtr hWndInputPanel = Coredll.FindWindowW( "SipWndClass" , null );
                  IntPtr hWndSipButton = Coredll.FindWindowW( "MS_SIPBUTTON", null );
                  IntPtr hWndTaskBar = Coredll.FindWindowW( "HHTaskBar", null );
 
                  IntPtr hWnd = control.Handle;
 
                  Rect rtDesktop = new Rect( );
 
                  if( hWndTaskBar != null )
                  {
                        Coredll.ShowWindow( hWndTaskBar, (int) Sw.SW_SHOW );
                  }
                  //Never forcibly show the input panel
                  //if( hWndSipButton != null )
                  //{
                  //    Coredll.ShowWindow( hWndSipButton, Sw.SW_SHOW );
                  //}
 
                  IntPtr rectPtr = Marshal.AllocHGlobal( Marshal.SizeOf( rtDesktop ) );
 
                  if( Coredll.SystemParametersInfo( Spi.SPI_GETWORKAREA, 0, rectPtr, Spif.None ) ) )
                  {
                        Rect rect = (Rect) Marshal.PtrToStructure( rectPtr, typeof( Rect ) );
 
                        Coredll.SetWindowPos( hWnd, (IntPtr) ( (int) Hwnd.HWND_TOPMOST ), 0, 0, rect.right – rect.left, rect.bottom – rect.top, (int) Swp.SWP_SHOWWINDOW );
                  }
                  Marshal.FreeHGlobal( rectPtr );
            }
        */


    }
}
