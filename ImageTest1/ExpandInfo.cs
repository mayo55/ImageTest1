namespace ImageTest1
{
    public class ExpandInfo
    {

        public string Filename { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public RotateMode RotMode { get; set; }

        public ExpandInfo(string filename, int width, int height, RotateMode rotMode)
        {
            this.Filename = filename;
            this.Width = width;
            this.Height = height;
            this.RotMode = rotMode;
        }

        public override string ToString()
        {
            return "(" + Filename + "," + Width + "," + Height + "," + RotMode + ")";
        }

    }
}
