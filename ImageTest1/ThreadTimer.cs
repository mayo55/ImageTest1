using System.Threading;

namespace ImageTest1
{
    public class ThreadTimer
    {

        private static TimerCallback timerDelegate = new TimerCallback(MyClock);
        private static Timer timer;

        private static object lockObj = new object();
        public static void Run()
        {
            if (timer == null)
            {
                timer = new Timer(timerDelegate, null, 0, 100);
            }
        }

        public static void MyClock(object o)
        {
            lock (lockObj)
            {
                Form1 form = FormManager.GetFirstForm();

                if (form == null)
                {
                    return;
                }

                if (form.fileManager.filename == null)
                {
                    return;
                }

                if (GlobalInfo.Loading)
                {
                    return;
                }

                // 次以降のファイルを先読み
                for (int i = 1; i <= GlobalInfo.PreLoadNumber; i++)
                {
                    string forwardFilename = form.fileManager.GetForwardFilename(i);
                    if (forwardFilename != null)
                    {
                        //form.LoadFileOneCache(nextFilename)
                        ThreadManager.LoadFileOneCacheArg arg = new ThreadManager.LoadFileOneCacheArg(form, forwardFilename);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadManager.LoadFileOneCache), arg);
                    }
                }
            }
        }

    }
}
