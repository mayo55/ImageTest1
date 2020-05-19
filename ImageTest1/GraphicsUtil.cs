namespace ImageTest1
{
    public class GraphicsUtil
    {

        public static System.Drawing.Drawing2D.InterpolationMode GetInterpolationMode(int mode)
        {
            System.Drawing.Drawing2D.InterpolationMode interpolationMode = 0;

            switch (mode)
            {
                case 0:
                    interpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                    break;
                case 1:
                    interpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                    break;
                case 2:
                    interpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    break;
                case 3:
                    interpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                    break;
                case 4:
                    interpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                    break;
                case 5:
                    interpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    break;
                case 6:
                    interpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                    break;
                case 7:
                    interpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    break;
                default:
                    interpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                    break;
            }

            return interpolationMode;
        }

    }
}
