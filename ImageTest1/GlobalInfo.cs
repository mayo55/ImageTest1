using System.IO.Compression;

namespace ImageTest1
{
    public class GlobalInfo
    {

        public static int BorderWidth = Config.BorderWidth;
        public static int PreLoadNumber = 0;
        public static int MaxThreads = 1;
        public static int InterpolationMode = 0;
        public static int BufferNumber = -1;
        public static bool FlagInterlocking = false;
        public static bool FlagChangeScreen = false;

        public static bool Loading = false;
    }
}
