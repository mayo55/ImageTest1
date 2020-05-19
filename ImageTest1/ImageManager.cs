using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace ImageTest1
{
    public class ImageManager
    {

        private static ReaderWriterLock imageBufferLock = new ReaderWriterLock();
        private static Dictionary<string, ImageBuffer> imageBuffer = new Dictionary<string, ImageBuffer>();

        private static ReaderWriterLock expandImageBufferLock = new ReaderWriterLock();
        private static Dictionary<ExpandInfo, ImageBuffer> expandImageBuffer = new Dictionary<ExpandInfo, ImageBuffer>(new ExpandInfoComparer());

        public static void AddImage(string filename, int width, int height, Bitmap image)
        {
            //Console.WriteLine(DateTime.Now + " AddImage(" + filename + ",) start")

            if (filename == null)
            {
                return;
            }

            // 登録中、登録済みならリターン
            if (GetImageStatus(filename) != ImageBufferStatus.None)
            {
                return;
            }

            try
            {
                imageBufferLock.AcquireWriterLock(Timeout.Infinite);

                if (imageBuffer.ContainsKey(filename))
                {
                    return;
                }

                if (GlobalInfo.BufferNumber != -1 && imageBuffer.Count >= GlobalInfo.BufferNumber)
                {
                    RemoveOldestImage();
                }

                ImageBuffer imageBufferValue = new ImageBuffer(ImageBufferStatus.Stored, width, height, image);
                imageBuffer.Add(filename, imageBufferValue);
            }
            finally
            {
                imageBufferLock.ReleaseWriterLock();
            }

            //Console.WriteLine(DateTime.Now + " AddImage(" + filename + ",) end")
        }

        public static void RemoveImage(string filename)
        {
            if (filename == null)
            {
                return;
            }

            try
            {
                imageBufferLock.AcquireWriterLock(Timeout.Infinite);

                if (!imageBuffer.ContainsKey(filename))
                {
                    return;
                }

                imageBuffer.Remove(filename);
            }
            finally
            {
                imageBufferLock.ReleaseWriterLock();
            }
        }

        public static Bitmap GetImage(string filename)
        {
            //Console.WriteLine(DateTime.Now + " GetImage(" + filename + ",) start")

            if (filename == null)
            {
                return null;
            }

            Bitmap image = null;

            try
            {
                imageBufferLock.AcquireWriterLock(Timeout.Infinite);

                if (imageBuffer.ContainsKey(filename))
                {
                    ImageBuffer imageBufferValue = imageBuffer[filename];
                    image = (Bitmap)(imageBufferValue.Image.Clone());
                }
            }
            finally
            {
                imageBufferLock.ReleaseWriterLock();
            }

            //Console.WriteLine(DateTime.Now + " GetImage(" + filename + ",) end")
            return image;
        }

        public static ImageBufferStatus GetImageStatus(string filename)
        {
            //Console.WriteLine(DateTime.Now + " GetImageStatus(" + filename + ",) start")

            if (filename == null)
            {
                return ImageBufferStatus.None;
            }

            ImageBufferStatus status = ImageBufferStatus.None;

            try
            {
                imageBufferLock.AcquireReaderLock(Timeout.Infinite);

                if (imageBuffer.ContainsKey(filename))
                {
                    status = imageBuffer[filename].Status;
                }
            }
            finally
            {
                imageBufferLock.ReleaseReaderLock();
            }

            //Console.WriteLine(DateTime.Now + " GetImageStatus(" + filename + ",) end")
            return status;
        }

        public static ImageBuffer GetImageBuffer(string filename)
        {
            //Console.WriteLine(DateTime.Now + " GetImageBuffer(" + filename + ",) start")

            if (filename == null)
            {
                return null;
            }

            ImageBuffer buffer = null;

            try
            {
                imageBufferLock.AcquireReaderLock(Timeout.Infinite);

                if (imageBuffer.ContainsKey(filename))
                {
                    buffer = imageBuffer[filename];
                }
            }
            finally
            {
                imageBufferLock.ReleaseReaderLock();
            }

            //Console.WriteLine(DateTime.Now + " GetImageBuffer(" + filename + ",) end")
            return buffer;
        }

        public static void AddExpandImage(ExpandInfo expandInfo, Bitmap image)
        {
            //Console.WriteLine(DateTime.Now + " AddExpandImage(" + expandInfo.ToString() + ",) start")

            if (expandInfo == null || expandInfo.Filename == null)
            {
                return;
            }

            // 登録中、登録済みならリターン
            if (GetExpandImageStatus(expandInfo) != ImageBufferStatus.None)
            {
                return;
            }

            try
            {
                expandImageBufferLock.AcquireWriterLock(Timeout.Infinite);

                if (expandImageBuffer.ContainsKey(expandInfo))
                {
                    return;
                }

                if (GlobalInfo.BufferNumber != -1 && expandImageBuffer.Count >= GlobalInfo.BufferNumber)
                {
                    RemoveOldestExpandImage();
                }

                ImageBuffer expandImageBufferValue = new ImageBuffer(ImageBufferStatus.Stored, expandInfo.Width, expandInfo.Height, image);
                expandImageBuffer[expandInfo] = expandImageBufferValue;
            }
            finally
            {
                expandImageBufferLock.ReleaseWriterLock();
            }

            //Console.WriteLine(DateTime.Now + " AddExpandImage(" + expandInfo.ToString() + ",) end")
        }

        public static void RemoveExpandImage(string filename)
        {
            if (filename == null)
            {
                return;
            }

            try
            {
                expandImageBufferLock.AcquireWriterLock(Timeout.Infinite);

                List<ExpandInfo> expandInfoList = new List<ExpandInfo>();

                // ファイル名が一致するものを全て削除する
                foreach (ExpandInfo expandInfo in expandImageBuffer.Keys)
                {
                    // ファイル名が一致する場合には削除する
                    if (expandInfo.Filename == filename)
                    {
                        // コレクションのforeach中にRemoveするとInvalidOperationExceptionが発生するため、記憶しておいてforeach外でRemoveする
                        expandInfoList.Add(expandInfo);
                    }
                }

                foreach (ExpandInfo expandInfo in expandInfoList)
                {
                    expandImageBuffer.Remove(expandInfo);
                }
            }
            finally
            {
                expandImageBufferLock.ReleaseWriterLock();
            }
        }

        public static void Remove(string filename)
        {
            ImageManager.RemoveImage(filename);
            ImageManager.RemoveExpandImage(filename);
        }

        public static Bitmap GetExpandImage(ExpandInfo expandInfo)
        {
            //Console.WriteLine(DateTime.Now + " GetExpandImage(" + expandInfo.ToString() + ",) start")

            if (expandInfo == null || expandInfo.Filename == null)
            {
                return null;
            }

            Bitmap image = null;

            try
            {
                expandImageBufferLock.AcquireWriterLock(Timeout.Infinite);

                if (expandImageBuffer.ContainsKey(expandInfo))
                {
                    ImageBuffer expandImageBufferValue = expandImageBuffer[expandInfo];
                    image = expandImageBufferValue.Image;
                }
            }
            finally
            {
                expandImageBufferLock.ReleaseWriterLock();
            }

            //Console.WriteLine(DateTime.Now + " GetExpandImage(" + expandInfo.ToString() + ",) end")
            return image;
        }

        public static ImageBufferStatus GetExpandImageStatus(ExpandInfo expandInfo)
        {
            //Console.WriteLine(DateTime.Now + " GetExpandImageStatus(" + expandInfo.ToString() + ",) start")

            if (expandInfo == null || expandInfo.Filename == null)
            {
                return ImageBufferStatus.None;
            }

            ImageBufferStatus status = ImageBufferStatus.None;

            try
            {
                expandImageBufferLock.AcquireReaderLock(Timeout.Infinite);

                if (expandImageBuffer.ContainsKey(expandInfo))
                {
                    status = expandImageBuffer[expandInfo].Status;
                }
            }
            finally
            {
                expandImageBufferLock.ReleaseReaderLock();
            }

            //Console.WriteLine(DateTime.Now + " GetExpandImageStatus(" + expandInfo.ToString() + ",) end")
            return status;
        }

        public static ImageBuffer GetExpandImageBuffer(ExpandInfo expandInfo)
        {
            //Console.WriteLine(DateTime.Now + " GetExpandImageBuffer(" + expandInfo.ToString() + ",) start")

            if (expandInfo == null || expandInfo.Filename == null)
            {
                return null;
            }

            ImageBuffer buffer = null;

            try
            {
                expandImageBufferLock.AcquireReaderLock(Timeout.Infinite);

                if (expandImageBuffer.ContainsKey(expandInfo))
                {
                    buffer = expandImageBuffer[expandInfo];
                }
            }
            finally
            {
                expandImageBufferLock.ReleaseReaderLock();
            }

            //Console.WriteLine(DateTime.Now + " GetExpandImageBuffer(" + expandInfo.ToString() + ",) end")
            return buffer;
        }

        private static void RemoveOldestImage()
        {
            DateTime oldestTime = DateTime.MaxValue;
            string oldestFilename = null;

            if (imageBuffer.Count == 0)
            {
                return;
            }

            foreach (string filename in imageBuffer.Keys)
            {
                ImageBuffer oneImageBuffer = imageBuffer[filename];
                DateTime registerTime = oneImageBuffer.RegisterTime;
                if (registerTime < oldestTime)
                {
                    oldestTime = registerTime;
                    oldestFilename = filename;
                }
            }

            Bitmap bmp = imageBuffer[oldestFilename].Image;
            bmp.Dispose();

            imageBuffer.Remove(oldestFilename);
        }

        private static void RemoveOldestExpandImage()
        {
            DateTime oldestTime = DateTime.MaxValue;
            ExpandInfo oldestExpandInfo = null;

            if (expandImageBuffer.Count == 0)
            {
                return;
            }

            foreach (ExpandInfo expandInfo in expandImageBuffer.Keys)
            {
                ImageBuffer oneExpandImageBuffer = expandImageBuffer[expandInfo];
                DateTime registerTime = oneExpandImageBuffer.RegisterTime;
                if (registerTime < oldestTime)
                {
                    oldestTime = registerTime;
                    oldestExpandInfo = expandInfo;
                }
            }

            Bitmap bmp = expandImageBuffer[oldestExpandInfo].Image;
            bmp.Dispose();

            expandImageBuffer.Remove(oldestExpandInfo);
        }

        private static void ClearImageBuffer()
        {
            try
            {
                imageBufferLock.AcquireWriterLock(Timeout.Infinite);

                imageBuffer.Clear();
            }
            finally
            {
                imageBufferLock.ReleaseWriterLock();
            }
        }

        private static void ClearExpandImageBuffer()
        {
            try
            {
                expandImageBufferLock.AcquireWriterLock(Timeout.Infinite);

                expandImageBuffer.Clear();
            }
            finally
            {
                expandImageBufferLock.ReleaseWriterLock();
            }
        }

        public static void Clear()
        {
            ClearImageBuffer();
            ClearExpandImageBuffer();
        }

        private static void RenameKeyImageBuffer(string oldFilename, string newFilename)
        {
            try
            {
                imageBufferLock.AcquireWriterLock(Timeout.Infinite);

                if (imageBuffer.ContainsKey(oldFilename))
                {
                    ImageBuffer imageBufferValue = imageBuffer[oldFilename];
                    imageBuffer.Remove(oldFilename);
                    if (imageBufferValue != null)
                    {
                        imageBuffer.Add(newFilename, imageBufferValue);
                    }
                }
            }
            finally
            {
                imageBufferLock.ReleaseWriterLock();
            }
        }

        private static void RenameKeyExpandImageBuffer(string oldFilename, string newFilename)
        {
            try
            {
                expandImageBufferLock.AcquireWriterLock(Timeout.Infinite);

                List<ExpandInfo> expandInfoList = new List<ExpandInfo>();
                List<ImageBuffer> imageBufferList = new List<ImageBuffer>();

                // ファイル名が一致するものを全て削除する
                foreach (ExpandInfo expandInfo in expandImageBuffer.Keys)
                {
                    // ファイル名が一致する場合には削除する
                    if (expandInfo.Filename == oldFilename)
                    {
                        // コレクションのforeach中にRemoveするとInvalidOperationExceptionが発生するため、記憶しておいてforeach外でRemoveする
                        expandInfoList.Add(expandInfo);
                        imageBufferList.Add(expandImageBuffer[expandInfo]);
                    }
                }

                foreach (ExpandInfo expandInfo in expandInfoList)
                {
                    expandImageBuffer.Remove(expandInfo);
                }

                for (int i = 0; i < expandInfoList.Count; i++)
                {
                    ExpandInfo expandInfo = expandInfoList[i];
                    ImageBuffer imagebuffer = imageBufferList[i];

                    expandInfo.Filename = newFilename;
                    expandImageBuffer[expandInfo] = imagebuffer;
                }
            }
            finally
            {
                expandImageBufferLock.ReleaseWriterLock();
            }
        }

        public static void RenameKey(string oldFilename, string newFilename)
        {
            RenameKeyImageBuffer(oldFilename, newFilename);
            RenameKeyExpandImageBuffer(oldFilename, newFilename);
        }

    }
}
