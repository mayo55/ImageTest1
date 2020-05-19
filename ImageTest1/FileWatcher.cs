using System;
using System.IO;
using System.Threading;

namespace ImageTest1
{
    public class FileWatcher
    {
        public string dir { get; set; }

        private FileSystemWatcher fileSystemWatcher;

        private Form1 form1;

        public FileWatcher(Form1 form1)
        {
            this.form1 = form1;
        }

        public void Start()
        {
            fileSystemWatcher = new FileSystemWatcher(dir, "*.*");
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            fileSystemWatcher.IncludeSubdirectories = false;
            fileSystemWatcher.SynchronizingObject = form1;
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;

            fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            fileSystemWatcher.EnableRaisingEvents = false;
            fileSystemWatcher.Dispose();
            fileSystemWatcher = null;
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //Console.WriteLine(e.FullPath + ", " + e.ChangeType);

            form1.fileManager.SetLatestFile();
        }

    }
}
