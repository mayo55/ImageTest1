using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace ImageTest1
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class Config
    {

        public static int numberOfScreen { get; set; }

        /// <summary>
        /// ディスプレイ情報リスト。
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Rectangle[] ScreenInfo { get; set; }

        public static int[] LeftList { get; set; }
        public static int[] RightList { get; set; }
        public static int[] UpList { get; set; }
        public static int[] DownList { get; set; }

        public static int horizontallyLongScreen { get; set; }
        public static int verticallyLongScreen { get; set; }

        /// <summary>
        /// 拡大縮小率リスト。
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double[] ResizeList { get; set; }

        public static string destDir1 { get; set; }
        public static string destDir2 { get; set; }
        public static string destDir3 { get; set; }
        public static string destDir4 { get; set; }
        public static string destDir5 { get; set; }
        public static string destDir6 { get; set; }
        public static string destDir7 { get; set; }
        public static string destDir8 { get; set; }
        public static string destDir9 { get; set; }

        public static bool Maximize { get; set; }
        public static int MoveValueSmall { get; set; }
        public static int MoveValueLarge { get; set; }
        public static int BorderWidth { get; set; }
        public static string CommandLine { get; set; }
        public static int PreLoadNumber { get; set; }
        public static int MaxThreads { get; set; }
        public static int InterpolationMode { get; set; }
        public static int BufferNumber { get; set; }
        
        public static string ExecCommand1 { get; set;  }
        public static int ClearAfterExecCommand1 { get; set; }
        public static string ExecCommand2 { get; set;  }
        public static int ClearAfterExecCommand2 { get; set; }
        public static string ExecCommand3 { get; set; }
        public static int ClearAfterExecCommand3 { get; set; }
        public static string ExecCommand4 { get; set; }
        public static int ClearAfterExecCommand4 { get; set; }
        public static string ExecCommand5 { get; set; }
        public static int ClearAfterExecCommand5 { get; set; }

        public static string InterlockFolder { get; set; }
        public static int InterlockScreenNumber { get; set; }

        public static string InitialFolder { get; set; }
        public static string ExecCommandAfterMoveFiles { get; set; }

        public static int MoveAfterCopy { get; set; }

        public static int ChangeLeftRight { get; set; }

        public static void LoadConfig()
        {
            KeyManager.Initalize();

            Config.destDir1 = Properties.Settings.Default.DestinationDirectory1;
            Config.destDir2 = Properties.Settings.Default.DestinationDirectory2;
            Config.destDir3 = Properties.Settings.Default.DestinationDirectory3;
            Config.destDir4 = Properties.Settings.Default.DestinationDirectory4;
            Config.destDir5 = Properties.Settings.Default.DestinationDirectory5;
            Config.destDir6 = Properties.Settings.Default.DestinationDirectory6;
            Config.destDir7 = Properties.Settings.Default.DestinationDirectory7;
            Config.destDir8 = Properties.Settings.Default.DestinationDirectory8;
            Config.destDir9 = Properties.Settings.Default.DestinationDirectory9;

            if (Properties.Settings.Default.ScreenInfoAuto == "0")
            {
                // 画面情報マニュアル(app.configから読み込み)

                int numberOfScreen = 0;
                int.TryParse(Properties.Settings.Default.NumberOfScreen, out numberOfScreen);
                if (numberOfScreen <= 0)
                {
                    MessageBox.Show("NumberOfScreen設定異常");
                    throw new Exception("NumberOfScreen設定異常");
                }
                Config.numberOfScreen = numberOfScreen;

                string screenInformationsStr = Properties.Settings.Default.ScreenInfomations;
                string[] screenInfoStrs = screenInformationsStr.Split('/');
                if (screenInfoStrs.Length != numberOfScreen)
                {
                    MessageBox.Show("ScreenInfomations設定異常");
                    throw new Exception("ScreenInfomations設定異常");
                }
                Config.ScreenInfo = new Rectangle[numberOfScreen];
                for (int i = 0; i < screenInfoStrs.Length; i++)
                {
                    string[] screenValueStr = screenInfoStrs[i].Trim().Split(',');
                    if (screenValueStr.Length != 4)
                    {
                        MessageBox.Show("ScreenInfomations設定異常");
                        throw new Exception("ScreenInfomations設定異常");
                    }

                    int x = 0;
                    int y = 0;
                    int w = 0;
                    int h = 0;
                    int.TryParse(screenValueStr[0], out x);
                    int.TryParse(screenValueStr[1], out y);
                    int.TryParse(screenValueStr[2], out w);
                    int.TryParse(screenValueStr[3], out h);
                    Config.ScreenInfo[i] = new Rectangle(x, y, w, h);
                }

                string leftListStr = Properties.Settings.Default.LeftDestinations;
                string[] leftValueStr = leftListStr.Split(',');
                if (leftValueStr.Length != numberOfScreen)
                {
                    MessageBox.Show("LeftDestinations設定異常");
                    throw new Exception("LeftDestinations設定異常");
                }
                Config.LeftList = new int[numberOfScreen];
                for (int i = 0; i < numberOfScreen; i++)
                {
                    int.TryParse(leftValueStr[i], out Config.LeftList[i]);
                }

                string rightListStr = Properties.Settings.Default.RightDestinations;
                string[] rightValueStr = rightListStr.Split(',');
                if (rightValueStr.Length != numberOfScreen)
                {
                    MessageBox.Show("RightDestinations設定異常");
                    throw new Exception("RightDestinations設定異常");
                }
                Config.RightList = new int[numberOfScreen];
                for (int i = 0; i < numberOfScreen; i++)
                {
                    int.TryParse(rightValueStr[i], out Config.RightList[i]);
                }

                string upListStr = Properties.Settings.Default.UpDestinations;
                string[] upValueStr = upListStr.Split(',');
                if (upValueStr.Length != numberOfScreen)
                {
                    MessageBox.Show("UpDestinations設定異常");
                    throw new Exception("UpDestinations設定異常");
                }
                Config.UpList = new int[numberOfScreen];
                for (int i = 0; i < numberOfScreen; i++)
                {
                    int.TryParse(upValueStr[i], out Config.UpList[i]);
                }

                string downListStr = Properties.Settings.Default.DownDestinations;
                string[] downValueStr = downListStr.Split(',');
                if (downValueStr.Length != numberOfScreen)
                {
                    MessageBox.Show("DownDestinations設定異常");
                    throw new Exception("DownDestinations設定異常");
                }
                Config.DownList = new int[numberOfScreen];
                for (int i = 0; i < numberOfScreen; i++)
                {
                    int.TryParse(downValueStr[i], out Config.DownList[i]);
                }
            }
            else
            {
                // 画面情報オート(Screen.AllScreensから取得)

                Config.numberOfScreen = Screen.AllScreens.Length;

                Config.ScreenInfo = new Rectangle[numberOfScreen];
                for (int i = 0; i < numberOfScreen; i++)
                {
                    Config.ScreenInfo[i] = new Rectangle(
                        Screen.AllScreens[i].Bounds.X,
                        Screen.AllScreens[i].Bounds.Y,
                        Screen.AllScreens[i].Bounds.Width,
                        Screen.AllScreens[i].Bounds.Height
                        );
                }

                Config.LeftList = new int[numberOfScreen];
                if (numberOfScreen == 1)
                {
                    // 無効
                    Config.LeftList[0] = -1;
                }
                else
                {
                    // 例：5画面の場合、1,2,3,4,0とする
                    for (int i = 0; i < (numberOfScreen - 1); i++)
                    {
                        Config.LeftList[i] = i + 1;
                    }
                    Config.LeftList[numberOfScreen - 1] = 0;
                }

                Config.RightList = new int[numberOfScreen];
                if (numberOfScreen == 1)
                {
                    // 無効
                    Config.RightList[0] = -1;
                }
                else
                {
                    // 例：5画面の場合、4,0,1,2,3とする
                    Config.RightList[0] = numberOfScreen - 1;
                    for (int i = 1; i < numberOfScreen; i++)
                    {
                        Config.RightList[i] = i - 1;
                    }
                }

                // 上と下は全て無効にする
                Config.UpList = new int[numberOfScreen];
                Config.DownList = new int[numberOfScreen];
                for (int i = 0; i < numberOfScreen; i++)
                {
                    Config.UpList[i] = -1;
                    Config.DownList[i] = -1;
                }
            }

            string resizeListStr = Properties.Settings.Default.ResizeList;
            string[] resizeValueStr = resizeListStr.Split(',');
            if (resizeValueStr.Length <= 0)
            {
                MessageBox.Show("ResizeList設定異常");
                throw new Exception("ResizeList設定異常");
            }
            Config.ResizeList = new double[resizeValueStr.Length];
            for (int i = 0; i < resizeValueStr.Length; i++)
            {
                double.TryParse(resizeValueStr[i], out Config.ResizeList[i]);
            }

            bool maximize = false;
            bool.TryParse(Properties.Settings.Default.Maximize, out maximize);
            Maximize = maximize;

            int moveValueSmall = 0;
            int.TryParse(Properties.Settings.Default.MoveValueSmall, out moveValueSmall);
            MoveValueSmall = moveValueSmall;

            int moveValueLarge = 0;
            int.TryParse(Properties.Settings.Default.MoveValueLarge, out moveValueLarge);
            MoveValueLarge = moveValueLarge;

            int borderWidth = 0;
            int.TryParse(Properties.Settings.Default.BorderWidth, out borderWidth);
            BorderWidth = borderWidth;

            CommandLine = Properties.Settings.Default.CommandLine;

            int preLoadNumber = 0;
            int.TryParse(Properties.Settings.Default.PreLoadNumber, out preLoadNumber);
            PreLoadNumber = preLoadNumber;

            int maxThreads = 0;
            int.TryParse(Properties.Settings.Default.MaxThreads, out maxThreads);
            MaxThreads = maxThreads;

            int interpolationMode = 0;
            int.TryParse(Properties.Settings.Default.InterpolationMode, out interpolationMode);
            InterpolationMode = interpolationMode;

            int bufferNumber = 0;
            int.TryParse(Properties.Settings.Default.BufferNumber, out bufferNumber);
            BufferNumber = bufferNumber;

            int horizontallyLongScreen = 0;
            int.TryParse(Properties.Settings.Default.HorizontallyLongScreen, out horizontallyLongScreen);
            Config.horizontallyLongScreen = horizontallyLongScreen;

            int verticallyLongScreen = 0;
            int.TryParse(Properties.Settings.Default.VerticallyLongScreen, out verticallyLongScreen);
            Config.verticallyLongScreen = verticallyLongScreen;

            ExecCommand1 = Properties.Settings.Default.ExecCommand1;

            int clearAfterExecCommand1 = 0;
            int.TryParse(Properties.Settings.Default.ClearAfterExecCommand1, out clearAfterExecCommand1);
            ClearAfterExecCommand1 = clearAfterExecCommand1;

            ExecCommand2 = Properties.Settings.Default.ExecCommand2;

            int clearAfterExecCommand2 = 0;
            int.TryParse(Properties.Settings.Default.ClearAfterExecCommand2, out clearAfterExecCommand2);
            ClearAfterExecCommand2 = clearAfterExecCommand2;

            ExecCommand3 = Properties.Settings.Default.ExecCommand3;

            int clearAfterExecCommand3 = 0;
            int.TryParse(Properties.Settings.Default.ClearAfterExecCommand3, out clearAfterExecCommand3);
            ClearAfterExecCommand3 = clearAfterExecCommand3;

            ExecCommand4 = Properties.Settings.Default.ExecCommand4;

            int clearAfterExecCommand4 = 0;
            int.TryParse(Properties.Settings.Default.ClearAfterExecCommand4, out clearAfterExecCommand4);
            ClearAfterExecCommand4 = clearAfterExecCommand4;

            ExecCommand5 = Properties.Settings.Default.ExecCommand5;

            int clearAfterExecCommand5 = 0;
            int.TryParse(Properties.Settings.Default.ClearAfterExecCommand5, out clearAfterExecCommand5);
            ClearAfterExecCommand5 = clearAfterExecCommand5;

            InterlockFolder = Properties.Settings.Default.InterlockFolder;

            int interlockScreenNumber = 0;
            int.TryParse(Properties.Settings.Default.InterlockScreenNumber, out interlockScreenNumber);
            InterlockScreenNumber = interlockScreenNumber;

            InitialFolder = Properties.Settings.Default.InitialFolder;

            int moveAfterCopy = 0;
            int.TryParse(Properties.Settings.Default.MoveAfterCopy, out moveAfterCopy);
            MoveAfterCopy = moveAfterCopy;

            int changeLeftRight = 0;
            int.TryParse(Properties.Settings.Default.ChangeLeftRight, out changeLeftRight);
            ChangeLeftRight = changeLeftRight;

            ExecCommandAfterMoveFiles = Properties.Settings.Default.ExecCommandAfterMoveFiles;

            KeyManager.AddKeyEvents(Properties.Settings.Default.DoMaximize, Operation.DoMaximize);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoMaximizeModeChange, Operation.DoMaximizeModeChange);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoRotateRight, Operation.DoRotateRight);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoRotateLeft, Operation.DoRotateLeft);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoExpansion, Operation.DoExpansion);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoReduce, Operation.DoReduce);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoPrevFile, Operation.DoPrevFile);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoNextFile, Operation.DoNextFile);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoPrevFileChangable, Operation.DoPrevFileChangable);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoNextFileChangable, Operation.DoNextFileChangable);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoFirstFile, Operation.DoFirstFile);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoLastFile, Operation.DoLastFile);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoSkipPrevFile, Operation.DoSkipPrevFile);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoSkipNextFile, Operation.DoSkipNextFile);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoIncrementPageNumber, Operation.DoIncrementPageNumber);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoDecrementPageNumber, Operation.DoDecrementPageNumber);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoClose, Operation.DoClose);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoMoveLeftSmall, Operation.DoMoveLeftSmall);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoMoveRightSmall, Operation.DoMoveRightSmall);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoMoveUpSmall, Operation.DoMoveUpSmall);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoMoveDownSmall, Operation.DoMoveDownSmall);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoMoveLeftLarge, Operation.DoMoveLeftLarge);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoMoveRightLarge, Operation.DoMoveRightLarge);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoMoveUpLarge, Operation.DoMoveUpLarge);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoMoveDownLarge, Operation.DoMoveDownLarge);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoMoveLeftScreen, Operation.DoMoveLeftScreen);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoMoveRightScreen, Operation.DoMoveRightScreen);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoMoveUpScreen, Operation.DoMoveUpScreen);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoMoveDownScreen, Operation.DoMoveDownScreen);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoNoCopyFile, Operation.DoNoCopyFile);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoNoCopyFileReverse, Operation.DoNoCopyFileReverse);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoCopyFile1, Operation.DoCopyFile1);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoCopyFile2, Operation.DoCopyFile2);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoCopyFile3, Operation.DoCopyFile3);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoCopyFile4, Operation.DoCopyFile4);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoCopyFile5, Operation.DoCopyFile5);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoCopyFile6, Operation.DoCopyFile6);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoCopyFile7, Operation.DoCopyFile7);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoCopyFile8, Operation.DoCopyFile8);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoCopyFile9, Operation.DoCopyFile9);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoSortModeFilenameAsc, Operation.DoSortModeFilenameAsc);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoSortModeFilenameDesc, Operation.DoSortModeFilenameDesc);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoSortModeTimeAsc, Operation.DoSortModeTimeAsc);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoSortModeTimeDesc, Operation.DoSortModeTimeDesc);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoCopyFullName, Operation.DoCopyFullName);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoCopyFile, Operation.DoCopyFile);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoSelectExplorer, Operation.DoSelectExplorer);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoExecCommand1, Operation.DoExecCommand1);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoExecCommand2, Operation.DoExecCommand2);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoExecCommand3, Operation.DoExecCommand3);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoExecCommand4, Operation.DoExecCommand4);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoExecCommand5, Operation.DoExecCommand5);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoReload, Operation.DoReload);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoSinglePage, Operation.DoSinglePage);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoTwoPages, Operation.DoTwoPages);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoInterlock, Operation.DoInterlock);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoMoveFiles, Operation.DoMoveFiles);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoRenamePage, Operation.DoRenamePage);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoShowPage, Operation.DoShowPage);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoThreePages, Operation.DoThreePages);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoWatchMode, Operation.DoWatchMode);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoOpenFile, Operation.DoOpenFile);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoOpenDirectory, Operation.DoOpenDirectory);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoChangeFocus, Operation.DoChangeFocus);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoFrontBack, Operation.DoFrontBack);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoShowBorder, Operation.DoShowBorder);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoChangeLeftRight, Operation.DoChangeLeftRight);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoTwoPagesFrontBack, Operation.DoTwoPagesFrontBack);
            KeyManager.AddKeyEvents(Properties.Settings.Default.DoConfig, Operation.DoConfig);
        }

        /// <summary>
        /// 指定座標のディスプレイ番号取得。
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetScreenNumber(Point point)
        {
            for (int i = 0; i <= Config.ScreenInfo.Length - 1; i++)
            {
                Rectangle si = Config.ScreenInfo[i];
                if (si.Contains(point))
                {
                    return i;
                }
            }

            // 範囲外はプライマリディスプレイとする
            return 0;
        }

    }
}
