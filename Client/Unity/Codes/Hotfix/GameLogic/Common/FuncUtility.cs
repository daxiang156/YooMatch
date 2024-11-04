namespace ET.Codes.Hotfix.GameLogic.Common
{
    public class FuncUtility
    {
        /// <summary>
        /// 获取平台名字
        /// </summary>
        /// <returns></returns>
        public static string GetPlatformName()
        {
            string platform = "Windows32/";
#if UNITY_ANDROID
            platform = "Android/";
#elif UNITY_IPHONE
            platform = "IOS/";
#endif
            return platform;
        }
        static void FormatColorString(ref string color)
        {
            if (color.Length < 10)
            {
                color = color.Insert(2, "0");
                FormatColorString(ref color);
            }
        }

        public static uint GetUInt32ColorFromString(string color)
        {
            uint value = 0;
            color = color.Insert(0, "0x");
            FormatColorString(ref color);
            for (int i = 0; i < color.Length; i++)
            {
                if (color[i] == '0' || color[i] == 'x')
                    continue;
                if (color[i] >= 'a' && color[i] <= 'f')
                {
                    value += (uint)((10 + color[i] - 'a') * System.Math.Pow(16, 9 - i));
                }
                else if (color[i] >= '0' && color[i] <= '9')
                {
                    value += (uint)((color[i] - '0') * System.Math.Pow(16, 9 - i));
                }
                else if (color[i] >= 'A' && color[i] <= 'F')
                {
                    value += (uint)((10 + color[i] - 'A') * System.Math.Pow(16, 9 - i));
                }
            }
            return value;
        }
    }
}