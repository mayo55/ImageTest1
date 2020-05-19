using System;
using System.IO;
using System.Linq;

namespace ImageTest1
{
    public class FileUtil
    {

        /// <summary>更新日時順に整列しているファイルの一覧を取得する。</summary>
        /// <param name="Path">対象のフォルダのパス。例："C:\"</param>
        /// <returns>更新日時順に整列しているファイルの一覧</returns>
        public static FileInfo[] GetFilesOrderByDate(string Path)
        {
            FileInfo[] files = GetImageFiles(Path);
            DateTime[] times = new DateTime[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                times[i] = files[i].LastWriteTime;
            }

            Array.Sort(times, files);

            return files;

        }

        /// <summary>ファイル名順に整列しているファイルの一覧を取得する。</summary>
        /// <param name="Path">対象のフォルダのパス。例："C:\"</param>
        /// <returns>ファイル名順に整列しているファイルの一覧</returns>
        public static FileInfo[] GetFilesOrderByFilename(string Path)
        {
            FileInfo[] Files = GetImageFiles(Path);
            string[] Filenames = new string[Files.Length];

            for (int i = 0; i < Files.Length; i++)
            {
                Filenames[i] = Files[i].Name;
            }

            Array.Sort(Filenames, Files);

            return Files;

        }

        private static FileInfo[] GetImageFiles(string Path)
        {
            System.IO.DirectoryInfo oFolder = new System.IO.DirectoryInfo(Path);
            FileInfo[] Files = oFolder.GetFiles("*.png").Concat(oFolder.GetFiles("*.jpg")).Concat(oFolder.GetFiles("*.jpeg")).Concat(oFolder.GetFiles("*.bmp")).Concat(oFolder.GetFiles("*.gif")).ToArray();
            return Files;
        }

    }
}
