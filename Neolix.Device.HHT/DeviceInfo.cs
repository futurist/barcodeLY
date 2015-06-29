using Microsoft.Win32;
using Neolix.Device;
using Neolix.Device.Tools;

namespace Neolix.Device
{
    public static class DeviceInfo
    {
        private static string serial;
        private static string osVersion;

        public static string OsVersion
        {
            get
            {
                if (string.IsNullOrEmpty(osVersion))
                {
                    osVersion = GetOsVersion();
                }
                return osVersion;
            }
        }

        //public static string Serial
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(serial))
        //        {
        //            serial = GetSN();
        //        }
        //        return serial;
        //    }
        //}

        //private static string GetSN()
        //{
        //    byte[] tmp = new byte[32];
        //    Gpc.IOCtrl(IOCTL.IOCTL_READ_SN, null, tmp);
        //    return HexCon.GetString(tmp);
        //}

        public static void WriteSN(string sn)
        {
            byte[] tmp = HexCon.GetBytes(sn);
            Gpc.IOCtrl(IOCTL.IOCTL_WRITE_SN, tmp, null);
        }

        private static string GetOsVersion()
        {
            string osPath = @"ControlPanel\OEMINFORMATION";
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(osPath);
            string ret = registryKey.GetValue("SOFT_VERSION") as string;
            return ret;
        }
    }
}