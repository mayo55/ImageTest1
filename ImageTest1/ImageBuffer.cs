using System;
using System.Drawing;

namespace ImageTest1
{
    public class ImageBuffer
    {

        public ImageBufferStatus Status { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Bitmap Image { get; set; }
        public DateTime RegisterTime { get; set; }

        public ImageBuffer(ImageBufferStatus status, int width, int height, Bitmap image)
        {
            this.Status = status;
            this.width = width;
            this.height = height;
            this.Image = image;
            this.RegisterTime = DateTime.Now;
        }
    }
}
