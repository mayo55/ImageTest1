using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Windows.Forms;

namespace ImageTest1
{
    public class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.ThreadException += Application_ThreadException;

            // 設定読み込み
            Config.LoadConfig();

            GlobalInfo.BorderWidth = Config.BorderWidth;
            GlobalInfo.PreLoadNumber = Config.PreLoadNumber;
            GlobalInfo.MaxThreads = Config.MaxThreads;
            GlobalInfo.InterpolationMode = Config.InterpolationMode;

            Form1 form1 = FormManager.GetNewForm();
            form1.init();

            string[] commandArgs = new string[args.Length];
            Array.Copy(args, commandArgs, args.Length);

            if (!string.IsNullOrEmpty(Config.CommandLine))
            {
                string[] configArgs = Config.CommandLine.Split(' ');
                commandArgs = new string[args.Length + configArgs.Length];
                Array.Copy(configArgs, commandArgs, configArgs.Length);
                Array.Copy(args, 0, commandArgs, configArgs.Length, args.Length);
            }

            string filename = null;
            string arg = null;
            for (int i = 0; i <= commandArgs.Length - 1; i++)
            {
                arg = commandArgs[i];

                string firstChar = arg.Substring(0, 1);
                if (firstChar == "/" || firstChar == "-")
                {
                    string optionStr = arg.Substring(1);
                    switch (optionStr)
                    {
                        case "sfa":
                            form1.SortMode = SortModeType.FilenameAsc;
                            break;

                        case "sfd":
                            form1.SortMode = SortModeType.FilenameDesc;
                            break;

                        case "sta":
                            form1.SortMode = SortModeType.TimeAsc;
                            break;

                        case "std":
                            form1.SortMode = SortModeType.TimeDesc;
                            break;

                        case "p1":
                            form1.ProgramMode = ProgramModeType.OneWindow;
                            form1.fileManager.showPagesPerScreen = 1;
                            break;

                        case "p2":
                            form1.ProgramMode = ProgramModeType.Mihiraki;
                            form1.fileManager.showPagesPerScreen = 2;
                            break;

                        case "p3":
                            form1.ProgramMode = ProgramModeType.Sanmen;
                            form1.fileManager.showPagesPerScreen = 3;
                            break;

                        case "p4":
                            form1.ProgramMode = ProgramModeType.Zengo;
                            form1.fileManager.showPagesPerScreen = 1;
                            break;

                        case "p6":
                            form1.ProgramMode = ProgramModeType.MihirakiZengo;
                            form1.fileManager.showPagesPerScreen = 2;
                            break;

                        case "wm":
                            form1.WatchMode = true;
                            form1.SortMode = SortModeType.TimeAsc;
                            break;
                    }

                    if (optionStr.StartsWith("sn"))
                    {
                        string screenStr = optionStr.Substring(2);
                        int screenNumber = int.Parse(screenStr);
                        form1.InitialScreenNumber = screenNumber;
                    }

                    if (optionStr.StartsWith("pl"))
                    {
                        string preLoadStr = optionStr.Substring(2);
                        int preLoadNumber = int.Parse(preLoadStr);
                        GlobalInfo.PreLoadNumber = preLoadNumber;
                    }

                    if (optionStr.StartsWith("mt"))
                    {
                        string maxThreadsStr = optionStr.Substring(2);
                        int maxThreads = int.Parse(maxThreadsStr);
                        GlobalInfo.MaxThreads = maxThreads;
                    }

                    if (optionStr.StartsWith("im"))
                    {
                        string interpolationModeStr = optionStr.Substring(2);
                        int interpolationMode = int.Parse(interpolationModeStr);
                        GlobalInfo.InterpolationMode = interpolationMode;
                    }

                    if (optionStr.StartsWith("b"))
                    {
                        form1.ShowBorder = true;
                        if (optionStr.Length >= 2)
                        {
                            string borderWidthStr = optionStr.Substring(1);
                            int borderWidth = int.Parse(borderWidthStr);
                            GlobalInfo.BorderWidth = borderWidth;
                        }
                    }

                    if (optionStr.StartsWith("bn"))
                    {
                        string bufferNumberStr = optionStr.Substring(2);
                        int bufferNumber = int.Parse(bufferNumberStr);
                        GlobalInfo.BufferNumber = bufferNumber;
                    }

                    if (optionStr == "spn")
                    {
                        form1.ShowPageNumber = true;
                    }

                    continue;
                }

                filename = arg;
            }

            // ファイル名・フォルダ名の指定がなく、コンフィグにパスの設定がある場合にはそのパスをセットする
            if (filename == null)
            {
                if (!String.IsNullOrEmpty(Config.InitialFolder))
                {
                    filename = Config.InitialFolder;
                    filename = filename.Replace("{MyPictures}", System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
                }
            }

            if (form1.ProgramMode == ProgramModeType.Sanmen)
            {
                form1.fileManager.InitSanmen();
                form1.filename = form1.fileManager.filename;
            }

            if (GlobalInfo.MaxThreads > 0)
            {
                ThreadPool.SetMaxThreads(GlobalInfo.MaxThreads, 1000);
                ThreadPool.SetMinThreads(GlobalInfo.MaxThreads, 1);
            }

            if (form1.WatchMode == true)
            {
                form1.fileManager.filename = null;
                form1.fileManager.dirname = filename;
                form1.fileWatcher.dir = filename;
                form1.fileWatcher.Start();
            }
            else
            {
                if (Directory.Exists(filename))
                {
                    // 引数がディレクトリ
                    form1.fileManager.filename = null;
                    form1.fileManager.dirname = filename;
                    form1.fileManager.MakeFileInfoList();
                    filename = form1.fileManager.GetFileInfoListFullName(0);
                    form1.fileManager.filename = filename;
                    form1.filename = filename;
                    if (filename != null)
                    {
                        FileInfo fi = new FileInfo(filename);
                        form1.fileManager.dirname = fi.DirectoryName;
                    }
                }
                else
                {
                    string ext = Path.GetExtension(filename);
                    if (String.Compare(ext, ".zip", true) == 0)
                    {
                        // 引数がzipファイル
                        form1.FlagZipFile = true;
                        form1.Zip = ZipFile.Open(filename, ZipArchiveMode.Read);
                        ZipArchiveEntry entry = form1.Zip.Entries[0];

                        form1.fileManager.filename = entry.FullName;
                        form1.fileManager.dirname = null;
                        form1.filename = entry.FullName;
                    }
                    else
                    {
                        // 引数が画像ファイル
                        form1.fileManager.filename = filename;
                        form1.filename = filename;
                        FileInfo fi = new FileInfo(filename);
                        form1.fileManager.dirname = fi.DirectoryName;
                    }
                }
            }

            Application.Run(form1);
        }

        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                Console.WriteLine(e.Exception.Message);
            }
            finally
            {
                Application.Exit();
            }
        }

    }
}
