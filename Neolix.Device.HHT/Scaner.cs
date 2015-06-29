using System;
using System.Threading;
using Neolix.Device.Tools;

namespace Neolix.Device
{
    public class Scaner
    {
        private class ScanDevice : StreamInterfaceDriver
        {
            public ScanDevice()
                : base("SCN0:")
            {
            }

            internal new void Open()
            {
                Open(FileAccess.Read | FileAccess.Write, FileShare.None);
            }
        }

        private static readonly ScanDevice scanDevice = new ScanDevice();

        public delegate void ScanerDataReceivedHandler(object sender, string code);

        public event ScanerDataReceivedHandler ScanerDataReceived;

        public static void InvokeOnScan(string code)
        {
            if (sender == null)
            {
                return;
            }
            ScanerDataReceivedHandler handler = sender.ScanerDataReceived;
            if (handler != null)
            {
                handler(sender, code);
            }
        }

        private static readonly object s_scanerLock = new object();

        public void Open()
        {
            lock (s_scanerLock)
            {
                if (!isOpen)
                {
                    scanDevice.Open();
                    isOpen = true;
                }
                if (scanDevice.IsOpen)
                {
                    if (scanThread == null)
                    {
                        scanThread = new Thread(ReadScanerData);
                        scanThread.Start();
                    }
                }
            }
        }

        public static void Close()
        {
            lock (s_scanerLock)
            {
                try
                {
                    quitscanEvent.Set();
                    if (isOpen)
                    {
                        scanDevice.Close();
                    }
                }
                catch
                {
                }

            }
        }

        static bool isOpen = false;

        public static bool IsOpen
        {
            get
            {
                return isOpen;
            }
        }

        private static Scaner sender;
        public bool Read()
        {
            if (!isOpen)
            {
                return false;
            }
            bContinuousRead = false;
            sender = this;
            scanEvent.Set();
            return true;
        }


        private static void ReadScanerData()
        {
            TimeSpan ts = TimeSpan.FromSeconds(3);

            while (scanDevice.IsOpen)
            {
                //scanEvent.WaitOne();
                IntPtr[] handles = new IntPtr[]
                                       {
                                           scanEvent.Handle,
                                           quitscanEvent.Handle
                                       };
                int ret = NativeMethods.WaitForMultipleObjects((uint) handles.Length, 
                    handles,
                    false, 
                    uint.MaxValue);

                if (ret != 0)
                {
                    return;
                }
                if (!scanDevice.IsOpen)
                {
                    break;
                }

                scanEvent.Reset();

                bScanReadding = true;


                do
                {
                    DateTime begin = DateTime.Now;

                    while (scanDevice.IsOpen)
                    {
                        try
                        {
                            int bytesRead;
                            byte[] result = scanDevice.Read(0x40, out bytesRead); //0x20 0x40

                            /*
                            for (int index = 0; index <= 10; index++)
                            {
                                if (bytesRead > 0)
                                {
                                    int morethan32number = 34; //USPS ServeHawbCode                                 
                                    string code = HexCon.GetString(result);
                                    if (code.Length == morethan32number - 32 || code.Length == 32 || code.Length == morethan32number + morethan32number - 32) //re-read
                                        result = scanDevice.Read(64, out bytesRead);                                  
                                    else
                                        break;
                                }
                            }
                            */

                            if (bytesRead > 0)
                            {
                                string code = HexCon.GetString(result);
                                if (!string.IsNullOrEmpty(code))
                                {
                                    InvokeOnScan(code);
                                }
                                break;
                            }
                            else
                            {
                                if ((DateTime.Now - begin) >= ts && !bContinuousRead)
                                {
                                    break;
                                }
                            }
                        }
                        catch
                        {

                        }
                    }

                } while (bContinuousRead);
                bScanReadding = false;

            }

        }

        public void ContinuousRead()
        {
            if (!isOpen)
            {
                return;
            }

            bContinuousRead = true;
            sender = this;
            scanEvent.Set();

        }

        public void StopContinuousRead()
        {
            bContinuousRead = false;
            scanEvent.Reset();

        }

        public bool IsContinuousReading
        {
            get { return bScanReadding && bContinuousRead; }
        }

        private static bool bScanReadding = false;
        private static EventWaitHandle scanEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        private static EventWaitHandle quitscanEvent = new EventWaitHandle(false, EventResetMode.ManualReset);

        private static bool bContinuousRead = false;
        private static Thread scanThread;
    }
}