using System.Threading;

namespace ImageTest1
{
    public class ThreadManager
    {

        public class LoadFileOneCacheArg
        {
            public Form1 Form { get; set; }
            public string Filename { get; set; }

            public LoadFileOneCacheArg(Form1 form, string filename)
            {
                this.Form = form;
                this.Filename = filename;
            }
        }

        public static void LoadFileOneCache(object obj)
        {
            LoadFileOneCacheArg arg = (LoadFileOneCacheArg)obj;
            Form1 form = arg.Form;
            string filename = arg.Filename;

            form.LoadFileOneCache(filename);
        }

    }
}
