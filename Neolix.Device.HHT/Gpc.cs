using System;
using System.Diagnostics;
using Neolix.Device.Tools;

namespace Neolix.Device
{
    public static class Gpc
    {
        private class GpcDevice : StreamInterfaceDriver
        {
            internal GpcDevice()
                : base("GPC1:")
            {
            }

            public void Open()
            {
                Open(FileAccess.Read | FileAccess.Write, FileShare.ReadWrite);
            }
        }

        public static void IOCtrl(uint nIoCtrl)
        {
            IOCtrl(nIoCtrl, null, null);
        }

        public static void IOCtrl(uint controlCode, byte[] inData, byte[] outData)
        {
            using (GpcDevice device = new GpcDevice())
            {
                device.Open();
                try
                {
                    device.DeviceIoControl(controlCode, inData, outData);
                }
                catch
                {
                }
            }
        }

        public static void LockTouchScreen(bool bLock)
        {
            if (bLock)
            {
                IOCtrl(IOCTL.IOCTL_BKL_OFF);
                IOCtrl(IOCTL.IOCTL_TOUCH_LOCK);
            }
            else
            {
                IOCtrl(IOCTL.IOCTL_TOUCH_UNLOCK);
                IOCtrl(IOCTL.IOCTL_BKL_ON);
            }
        }

        public static void SetModemPowerState(bool bOn)
        {
            IOCtrl(bOn ? IOCTL.IOCTL_ModemPowerUp_EN : IOCTL.IOCTL_ModemPowerOff_EN);
        }

        public static void SetWifiPower(bool bOn)
        {
            IOCtrl(bOn ? IOCTL.IOCTL_WIFI_ON : IOCTL.IOCTL_WIFI_OFF);
        }
    }

}