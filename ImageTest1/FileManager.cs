using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace ImageTest1
{
    public class FileManager
    {
        public FileInfo[] fileInfoList { get; set; }

        // 表示ファイルのインデックス、ファイル名。複数枚表示時は左端のファイル。
        public int fileInfoIndex { get; set; }
        public string filename { get; set; }
        public string dirname { get; set; }

        // 表示枚数：正数なら左側の表示枚数、負数なら絶対値が右側の表示枚数
        public int showPages { get; set; }

        public int showPagesPerScreen { get; set; }

        public string destDir { get; set; }

        private Form1 form1;

        public FileManager(Form1 form1)
        {
            this.form1 = form1;
        }

        /// <summary>
        /// ファイルをコピーする
        /// </summary>
        /// <param name="dest"></param>
        public void CopyToDestination(string dest)
        {
            if (string.IsNullOrWhiteSpace(dest))
            {
                FormManager.ShowErrorMessage2("コピー先の設定がありません。");
            }
            else
            {
                FileInfo fi = new FileInfo(filename);
                string destFilename = Path.Combine(dest, fi.Name);
                if (File.Exists(destFilename))
                {
                    FormManager.ShowErrorMessage2("コピー先ファイル：'" + destFilename + "'が既に存在するためコピーを行ないません。");
                    return;
                }

                try
                {
                    FileSystem.CopyFile(filename, destFilename);
                }
                catch (Exception ex)
                {
                    FormManager.ShowErrorMessage2("コピー失敗：'" + destFilename + "' " + ex.Message);
                    return;
                }

                FormManager.ShowMessage2("コピーしました：'" + destFilename + "'");
            }
        }

        /// <summary>
        /// フルパスファイル名をクリップボードにコピーする
        /// </summary>
        public void CopyFullName()
        {
            Clipboard.SetText(filename);

            FormManager.ShowMessage("クリップボードにフルパスファイル名をコピーしました：'" + filename + "'");
        }

        /// <summary>
        /// ファイルをクリップボードにコピーする
        /// </summary>
        public void CopyFile()
        {
            System.Collections.Specialized.StringCollection files = new System.Collections.Specialized.StringCollection();
            files.Add(filename);
            Clipboard.SetFileDropList(files);

            FormManager.ShowMessage("クリップボードにファイルをコピーしました：'" + filename + "'");
        }

        /// <summary>
        /// エクスプローラ起動
        /// </summary>
        public void SelectExplorer()
        {
            Process.Start("explorer", "/select,\"" + filename + "\"");
        }

        /// <summary>
        /// コマンド1起動
        /// </summary>
        public void ExecCommand1()
        {
            ExecCommand(Config.ExecCommand1);
        }

        /// <summary>
        /// コマンド2起動
        /// </summary>
        public void ExecCommand2()
        {
            ExecCommand(Config.ExecCommand2);
        }

        /// <summary>
        /// コマンド3起動
        /// </summary>
        public void ExecCommand3()
        {
            ExecCommand(Config.ExecCommand3);
        }

        /// <summary>
        /// コマンド4起動
        /// </summary>
        public void ExecCommand4()
        {
            ExecCommand(Config.ExecCommand4);
        }

        /// <summary>
        /// コマンド5起動
        /// </summary>
        public void ExecCommand5()
        {
            ExecCommand(Config.ExecCommand5);
        }

        /// <summary>
        /// 複数ファイル移動後コマンド起動
        /// </summary>
        public void ExecCommandAfterMoveFiles(string destDir)
        {
            this.destDir = destDir;
            ExecCommand(Config.ExecCommandAfterMoveFiles);
            this.destDir = "";
        }

        /// <summary>
        /// コマンド起動
        /// </summary>
        /// <param name="command"></param>
        private void ExecCommand(string command)
        {
            string commandName = null;
            string commandArgs = null;

            if (command != "")
            {
                GetCommandNameAndArgs(command, ref commandName, ref commandArgs);

                if (commandArgs == null)
                {
                    try
                    {
                        Process.Start(commandName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("コマンドの実行に失敗しました。\nコマンド：" + commandName + "\n" + ex.Message, "コマンド実行エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        Process.Start(commandName, commandArgs);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("コマンドの実行に失敗しました。\nコマンド：" + commandName + "\n引数：" + commandArgs + "\n" + ex.Message, "コマンド実行エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// コマンド名と引数の取得
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandName"></param>
        /// <param name="commandArgs"></param>
        private void GetCommandNameAndArgs(string command, ref string commandName, ref string commandArgs)
        {
            // {filename}を含む場合は、"＜フルパスファイル名＞"(ダブルクォーテーション付き)と置換する
            string replaceCommand = ReplaceCommandValue(command);

            if (replaceCommand[0] == '"')
            {
                // コマンド名にダブルクォーテーションあり
                int secondQuotPos = replaceCommand.IndexOf('"', 1);
                if (secondQuotPos < 0)
                {
                    // 2番目のダブルクォーテーションなし(先頭のダブルクォーテーションを除いて全てコマンド名とする)
                    commandName = replaceCommand.Substring(1);
                    commandArgs = null;
                }
                else
                {
                    // 2番目のダブルクォーテーションあり
                    // 両側のダブルクォーテーションを省いてコマンド名とする
                    commandName = replaceCommand.Substring(1, secondQuotPos - 1);

                    int firstSpacePos = replaceCommand.IndexOf(' ', secondQuotPos + 1);
                    if (firstSpacePos < 0)
                    {
                        // 2番目のダブルクォーテーションの後に半角スペースなし(全てコマンド名)
                        commandName = replaceCommand;
                        commandArgs = null;
                    }
                    else
                    {
                        // 2番目のダブルクォーテーションの後に半角スペースあり(コマンド引数あり)
                        commandName = replaceCommand.Substring(0, firstSpacePos);
                        commandArgs = replaceCommand.Substring(firstSpacePos + 1);
                    }
                }
            }
            else
            {
                // コマンド名にダブルクォーテーションなし
                int firstSpacePos = replaceCommand.IndexOf(' ');
                if (firstSpacePos < 0)
                {
                    // 半角スペースなし(全てコマンド名)
                    commandName = replaceCommand;
                    commandArgs = null;
                }
                else
                {
                    // 半角スペースあり(コマンド引数あり)
                    commandName = replaceCommand.Substring(0, firstSpacePos);
                    commandArgs = replaceCommand.Substring(firstSpacePos + 1);
                }
            }
        }

        private string ReplaceCommandValue(string command)
        {
            string retCommand = command;

            retCommand = retCommand.Replace("{ImageTest1}", "\"" + Assembly.GetExecutingAssembly().Location + "\"");
            retCommand = retCommand.Replace("{ImageTest1-Dir}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            retCommand = retCommand.Replace("{filename}", "\"" + filename + "\"");
            retCommand = retCommand.Replace("{destDir}", "\"" + destDir + "\"");
            retCommand = retCommand.Replace("{MyPictures}", System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));

            return retCommand;
        }

        public void MakeFileInfoListIfNotExist()
        {
            if (fileInfoList != null)
            {
                return;
            }

            MakeFileInfoList();
        }

        public void MakeFileInfoList()
        {
            if (form1.FlagZipFile)
            {
                return;
            }

            switch (form1.SortMode)
            {
                case SortModeType.FilenameAsc:
                    fileInfoList = FileUtil.GetFilesOrderByFilename(dirname);
                    break;
                case SortModeType.FilenameDesc:
                    fileInfoList = FileUtil.GetFilesOrderByFilename(dirname);
                    Array.Reverse(fileInfoList);
                    break;
                case SortModeType.TimeAsc:
                    fileInfoList = FileUtil.GetFilesOrderByDate(dirname);
                    break;
                case SortModeType.TimeDesc:
                    fileInfoList = FileUtil.GetFilesOrderByDate(dirname);
                    Array.Reverse(fileInfoList);
                    break;
            }

            if (fileInfoList.Length <= 0)
            {
                fileInfoIndex = -1;
                return;
            }

            if (filename == null)
            {
                fileInfoIndex = 0;
                return;
            }

            FileInfo fi = new FileInfo(filename);
            fileInfoIndex = Array.FindIndex(fileInfoList, (FileInfo fInfo) => fInfo.Name == fi.Name);
        }

        public string GetBeforeFilename(string filename)
        {
            if (form1.FlagZipFile)
            {
                for (int i = 0; i < form1.Zip.Entries.Count; i++)
                {
                    ZipArchiveEntry entry = form1.Zip.Entries[i];
                    if (entry.FullName == filename)
                    {
                        if (i >= 1)
                        {
                            return form1.Zip.Entries[i - 1].FullName;
                        }
                        return null;
                    }
                }
                return null;
            }
            else
            {
                MakeFileInfoListIfNotExist();
                string beforeFilename = null;
                if (filename != null)
                {
                    FileInfo fi = new FileInfo(filename);
                    int beforeIndex = Array.FindIndex(fileInfoList, (FileInfo fInfo) => fInfo.Name == fi.Name) - 1;

                    if (beforeIndex >= 0)
                    {
                        beforeFilename = System.IO.Path.Combine(fi.DirectoryName, fileInfoList[beforeIndex].Name);
                    }
                }

                return beforeFilename;
            }
        }

        public string GetNextFilename()
        {
            return GetNextFilename(filename);
        }

        public string GetNextFilename(string filename)
        {
            if (form1.FlagZipFile)
            {
                for (int i = 0; i < form1.Zip.Entries.Count; i++)
                {
                    ZipArchiveEntry entry = form1.Zip.Entries[i];
                    if (entry.FullName == filename)
                    {
                        if (i < form1.Zip.Entries.Count - 1)
                        {
                            return form1.Zip.Entries[i + 1].FullName;
                        }
                        return null;
                    }
                }
                return null;
            }
            else
            {
                MakeFileInfoListIfNotExist();
                string nextFilename = null;
                if (filename != null)
                {
                    FileInfo fi = new FileInfo(filename);
                    int nextIndex = Array.FindIndex(fileInfoList, (FileInfo fInfo) => fInfo.Name == fi.Name) + 1;

                    if (nextIndex < fileInfoList.Length)
                    {
                        nextFilename = System.IO.Path.Combine(fi.DirectoryName, fileInfoList[nextIndex].Name);
                    }
                }

                return nextFilename;
            }
        }

        public string GetBackwardFilename(int subNum)
        {
            if (form1.FlagZipFile)
            {
                for (int i = 0; i < form1.Zip.Entries.Count; i++)
                {
                    ZipArchiveEntry entry = form1.Zip.Entries[i];
                    if (entry.FullName == filename)
                    {
                        if (i >= subNum)
                        {
                            return form1.Zip.Entries[i - subNum].FullName;
                        }
                        return null;
                    }
                }
                return null;
            }
            else
            {
                MakeFileInfoListIfNotExist();
                string prevFilename = null;
                FileInfo fi = new FileInfo(filename);
                int prevIndex = Array.FindIndex(fileInfoList, (FileInfo fInfo) => fInfo.Name == fi.Name) - subNum;

                if (prevIndex >= 0)
                {
                    prevFilename = System.IO.Path.Combine(fi.DirectoryName, fileInfoList[prevIndex].Name);
                }

                return prevFilename;
            }
        }

        public string GetForwardFilename(int addNum)
        {
            if (form1.FlagZipFile)
            {
                for (int i = 0; i < form1.Zip.Entries.Count; i++)
                {
                    ZipArchiveEntry entry = form1.Zip.Entries[i];
                    if (entry.FullName == filename)
                    {
                        if (i < form1.Zip.Entries.Count - addNum)
                        {
                            return form1.Zip.Entries[i + addNum].FullName;
                        }
                        return null;
                    }
                }
                return null;
            }
            else
            {
                MakeFileInfoListIfNotExist();
                string nextFilename = null;
                FileInfo fi = new FileInfo(filename);
                int nextIndex = Array.FindIndex(fileInfoList, (FileInfo fInfo) => fInfo.Name == fi.Name) + addNum;

                if (nextIndex < fileInfoList.Length)
                {
                    nextFilename = System.IO.Path.Combine(fi.DirectoryName, fileInfoList[nextIndex].Name);
                }

                return nextFilename;
            }
        }

        public void InitSanmen()
        {
            if (filename == null)
            {
                return;
            }

            MakeFileInfoListIfNotExist();
            FileInfo fi = new FileInfo(filename);
            int nextIndex = Array.FindIndex(fileInfoList, (FileInfo fInfo) => fInfo.Name == fi.Name) + 1;

            if (nextIndex < fileInfoList.Length)
            {
                filename = fileInfoList[nextIndex].FullName;
                fileInfoIndex = nextIndex;
                showPages = 2;
            }
            else
            {
                showPages = 1;
            }

        }

        public void SetLatestFile()
        {
            MakeFileInfoList();

            fileInfoIndex = fileInfoList.Length - 1;
            filename = GetFullName();

            if (form1.ProgramMode == ProgramModeType.Sanmen)
            {
                form1.fileManager.showPages = 3;
            }
            else if (form1.ProgramMode == ProgramModeType.Mihiraki)
            {
                form1.fileManager.showPages = 2;
            }
            else
            {
                form1.fileManager.showPages = 1;
            }

            form1.filename = filename;
            form1.Invoke(new Form1.ChangeFileDelegate(form1.ChangeFile));
        }

        public int GetFileInfoListLength()
        {
            if (form1.FlagZipFile)
            {
                return form1.Zip.Entries.Count;
            }
            else
            {
                MakeFileInfoListIfNotExist();
                return fileInfoList.Length;
            }
        }

        public string GetFullName()
        {
            return GetFileInfoListFullName(fileInfoIndex);
        }

        public string GetFileInfoListFullName(int index)
        {
            if (form1.FlagZipFile)
            {
                if (fileInfoIndex >= 0)
                {
                    return form1.Zip.Entries[index].FullName;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (fileInfoIndex >= 0)
                {
                    return fileInfoList[index].FullName;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// zipファイルエントリ取得。
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ZipArchiveEntry GetZipEntry(string filename)
        {
            if (form1.FlagZipFile)
            {
                for (int i = 0; i < form1.Zip.Entries.Count; i++)
                {
                    ZipArchiveEntry entry = form1.Zip.Entries[i];
                    if (entry.FullName == filename)
                    {
                        return form1.Zip.Entries[i];
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 複数ファイル移動。
        /// </summary>
        /// <param name="destPath"></param>
        /// <returns></returns>
        public bool MoveFiles(string destPath)
        {
            MakeFileInfoListIfNotExist();
            for (int i = 0; i < fileInfoList.Length; i++)
            {
                FileInfo fi = fileInfoList[i];

                if (fi.Exists)
                {
                    string destFile = Path.Combine(destPath, fi.Name);

                    try
                    {
                        fi.MoveTo(destFile);
                    }
                    catch (Exception ex)
                    {
                        form1.message = "'" + fi.Name + "'で例外が発生しました。一度終了させて下さい。\n" + ex.Message;
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// ファイル名をページ番号にリネームする。
        /// </summary>
        public void RenamePage()
        {
            int firstPageNumber = form1.FirstPageNumber;
            string addChar = "_";
            bool flagAddChar = false;

            MakeFileInfoListIfNotExist();

            // 新しいファイル名のファイルが存在するか確認
            for (int i = 0; i < fileInfoList.Length; i++)
            {
                FileInfo fi = fileInfoList[i];
                string oldFilename = fi.Name;
                int pageNumber = firstPageNumber + i;
                string pageNumberStr = String.Format("{0:D3}", pageNumber);
                string newFilename = Path.Combine(fi.DirectoryName, pageNumberStr + fi.Extension);

                if (File.Exists(newFilename))
                {
                    flagAddChar = true;
                    break;
                }
            }

            // ファイル名がぶつかる場合には、ファイル名(拡張子の手前)に"_"を追加して一度リネームする
            if (flagAddChar)
            {
                for (int i = 0; i < fileInfoList.Length; i++)
                {
                    FileInfo fi = fileInfoList[i];
                    string oldFilename = fi.Name;
                    int pageNumber = firstPageNumber + i;
                    string pageNumberStr = String.Format("{0:D3}", pageNumber);
                    string newFilename = Path.Combine(fi.DirectoryName, pageNumberStr + addChar + fi.Extension);

                    if (File.Exists(newFilename))
                    {
                        form1.SetMessage("'" + newFilename + "'が存在するため、リネーム処理を行うことができません。");
                        return;
                    }
                }

                for (int i = 0; i < fileInfoList.Length; i++)
                {
                    FileInfo fi = fileInfoList[i];
                    string oldFilename = fi.Name;
                    int pageNumber = firstPageNumber + i;
                    string pageNumberStr = String.Format("{0:D3}", pageNumber);
                    string newFilename = Path.Combine(fi.DirectoryName, pageNumberStr + addChar + fi.Extension);

                    try
                    {
                        // リネーム
                        fi.MoveTo(newFilename); // fileInfoListのFileInfoのファイル名も書き換わる
                    }
                    catch (Exception ex)
                    {
                        form1.SetMessage("'" + fi.Name + "'のリネームに失敗しました。：\n" + ex.Message);
                    }

                    // 読み込み済み画像のキーを変更する。
                    ImageManager.RenameKey(oldFilename, newFilename);
                }
            }

            // リネーム
            for (int i = 0; i < fileInfoList.Length; i++)
            {
                FileInfo fi = fileInfoList[i];
                string oldFilename = fi.Name;   // 上でリネームしている場合はリネーム済みのファイル名
                int pageNumber = firstPageNumber + i;
                string pageNumberStr = String.Format("{0:D3}", pageNumber);
                string newFilename = Path.Combine(fi.DirectoryName, pageNumberStr + fi.Extension);

                try
                {
                    // リネーム
                    fi.MoveTo(newFilename);
                }
                catch (Exception ex)
                {
                    form1.SetMessage("'" + fi.Name + "'のリネームに失敗しました。：\n" + ex.Message);
                }

                // 読み込み済み画像のキーを変更する。
                ImageManager.RenameKey(oldFilename, newFilename);
            }

            form1.SetMessage("'" + dirname + "'内のファイルのリネームを終了しました。");
        }

    }
}
