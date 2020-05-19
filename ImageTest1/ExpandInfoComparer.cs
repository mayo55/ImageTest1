using System.Collections.Generic;

namespace ImageTest1
{
    public class ExpandInfoComparer : IEqualityComparer<ExpandInfo>
    {

        public bool Equals1(ExpandInfo x, ExpandInfo y)
        {
            return x.Filename == y.Filename && x.Width == y.Width && x.Height == y.Height && x.RotMode == y.RotMode;
        }
        bool IEqualityComparer<ImageTest1.ExpandInfo>.Equals(ExpandInfo x, ExpandInfo y)
        {
            return Equals1(x, y);
        }

        public int GetHashCode1(ExpandInfo x)
        {
            return x.Filename.GetHashCode() ^ x.Width.GetHashCode() ^ x.Height.GetHashCode() ^ x.RotMode.GetHashCode();
        }
        int IEqualityComparer<ImageTest1.ExpandInfo>.GetHashCode(ExpandInfo x)
        {
            return GetHashCode1(x);
        }

    }
}
