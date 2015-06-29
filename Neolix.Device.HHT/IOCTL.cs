namespace Neolix.Device
{
    public static class IOCTL
    {
        #region definitions

        private const uint FILE_ANY_ACCESS = 0;
        private const uint METHOD_BUFFERED = 0;

        private static uint CTL_CODE(uint deviceType, uint function, uint method, uint access)
        {
            return (((deviceType) << 16) | ((access) << 14) | ((function) << 2) | (method));
        }

        private const uint QNWW_DEVICE_TYPE = 0x8800;

        //led1-8¶ÔÓ¦¼üÅÌ°å×óÏÂ½Çµ½ÓÒÉÏ½Ç

        /// <summary>
        /// led1 ÁÁ
        /// </summary>
        internal static uint IOCTL_LED1_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x801, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        ///led1 Ãð
        /// </summary>
        internal static uint IOCTL_LED1_UN = CTL_CODE(QNWW_DEVICE_TYPE, 0x802, METHOD_BUFFERED, FILE_ANY_ACCESS);

        internal static uint IOCTL_LED2_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x803, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_LED2_UN = CTL_CODE(QNWW_DEVICE_TYPE, 0x804, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_LED3_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x805, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_LED3_UN = CTL_CODE(QNWW_DEVICE_TYPE, 0x806, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_LED4_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x807, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_LED4_UN = CTL_CODE(QNWW_DEVICE_TYPE, 0x808, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_LED5_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x809, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_LED5_UN = CTL_CODE(QNWW_DEVICE_TYPE, 0x810, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_LED6_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x811, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_LED6_UN = CTL_CODE(QNWW_DEVICE_TYPE, 0x812, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_LED7_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x813, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_LED7_UN = CTL_CODE(QNWW_DEVICE_TYPE, 0x814, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_LED8_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x815, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_LED8_UN = CTL_CODE(QNWW_DEVICE_TYPE, 0x816, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// modem power on
        /// </summary>
        internal static uint IOCTL_ModemPowerUp_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x817, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// modem power down
        /// </summary>
        internal static uint IOCTL_ModemPowerOff_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x818, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// TD power on
        /// </summary>
        internal static uint IOCTL_TD_PWN_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x819, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// TD power down
        /// </summary>
        internal static uint IOCTL_TD_PWN_UN = CTL_CODE(QNWW_DEVICE_TYPE, 0x820, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// ÉÁ¹âµÆÉÁÒ»ÏÂ
        /// </summary>
        internal static uint IOCTL_FLASH_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x821, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// other led on
        /// </summary>
        internal static uint IOCTL_OLED_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x822, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// other led off
        /// </summary>
        internal static uint IOCTL_OLED_UN = CTL_CODE(QNWW_DEVICE_TYPE, 0x823, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// ÂÌµÆÁÁ
        /// </summary>
        internal static uint IOCTL_GLED_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x825, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// ÂÌµÆÃð
        /// </summary>
        internal static uint IOCTL_GLED_UN = CTL_CODE(QNWW_DEVICE_TYPE, 0x826, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// À¶µÆÁÁ
        /// </summary>
        internal static uint IOCTL_BLED_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x827, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// À¶µÆÃð
        /// </summary>
        internal static uint IOCTL_BLED_UN = CTL_CODE(QNWW_DEVICE_TYPE, 0x828, METHOD_BUFFERED, FILE_ANY_ACCESS);

        internal static uint IOCTL_MADA_EN = CTL_CODE(QNWW_DEVICE_TYPE, 0x829, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_MADA_UN = CTL_CODE(QNWW_DEVICE_TYPE, 0x830, METHOD_BUFFERED, FILE_ANY_ACCESS);

        internal static uint IOCTL_GetModemPower_OK = CTL_CODE(QNWW_DEVICE_TYPE, 0x831, METHOD_BUFFERED, FILE_ANY_ACCESS);

        internal static uint IOCTL_Modem_Reset = CTL_CODE(QNWW_DEVICE_TYPE, 0x832, METHOD_BUFFERED, FILE_ANY_ACCESS);

        internal static uint IOCTL_BKL_ON = CTL_CODE(QNWW_DEVICE_TYPE, 0x850, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_BKL_OFF = CTL_CODE(QNWW_DEVICE_TYPE, 0x851, METHOD_BUFFERED, FILE_ANY_ACCESS);

        internal static uint IOCTL_WRITE_SN = CTL_CODE(QNWW_DEVICE_TYPE, 0x869, METHOD_BUFFERED, FILE_ANY_ACCESS);
        internal static uint IOCTL_READ_SN = CTL_CODE(QNWW_DEVICE_TYPE, 0x868, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// ´ò¿ªwifi
        /// </summary>
        internal static uint IOCTL_WIFI_ON = CTL_CODE(QNWW_DEVICE_TYPE, 0x852, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// ¹Ø±Õwifi
        /// </summary>
        internal static uint IOCTL_WIFI_OFF = CTL_CODE(QNWW_DEVICE_TYPE, 0x853, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// ´¥ÃþÆÁ¼ÓËø
        /// </summary>
        internal static uint IOCTL_TOUCH_LOCK = CTL_CODE(QNWW_DEVICE_TYPE, 0x854, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// ´¥ÃþÆÁ½âËø
        /// </summary>
        internal static uint IOCTL_TOUCH_UNLOCK = CTL_CODE(QNWW_DEVICE_TYPE, 0x855, METHOD_BUFFERED, FILE_ANY_ACCESS);

        #endregion
    }
}