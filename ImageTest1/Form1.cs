using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageTest1
{
    public partial class Form1
    {
        /// <summary>
        /// 読み込んだ画像。
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Bitmap bmp { get; set; }
        public Bitmap bmp2 { get; set; }
        public Bitmap bmp3 { get; set; }

        /// <summary>
        /// 中央座標。
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Point bmpCenter { get; set; }

        private Point mousePoint;
        public int mouseValue { get; set; }

        /// <summary>
        /// 表示モード
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public WindowMode winMode { get; set; }

        /// <summary>
        /// 回転モード
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public RotateMode rotMode { get; set; }

        /// <summary>
        /// 通常表示(＝非最大化)の位置とサイズ。拡大倍率加味される。
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Rectangle normalRect { get; set; }

        public string message { get; set; }
        public Font messageFont { get; set; }
        public Brush messageForegroundBrush { get; set; }
        public Brush messageBackgroundBrush { get; set; }
        public string message2 { get; set; }
        public Font message2Font { get; set; }
        public Brush message2ForegroundBrush { get; set; }
        public Brush message2BackgroundBrush { get; set; }
        public int message2Height { get; set; }
        public int message2Count { get; set; }

        public Font pageNumberFont { get; set; }
        public Brush pageNumberForegroundBrush { get; set; }
        public Brush pageNumberBackgroundBrush { get; set; }

        /// <summary>
        /// 現在のディスプレイ情報。
        /// </summary>
        /// <remarks></remarks>

        private Size nowScreenSize;
        public string filename { get; set; }
        public string beforeFilename { get; set; }
        public string beforeBeforeFilename { get; set; }
        public string nextFilename { get; set; }


        private bool maximize = false;
        //Private drawnSizeLock As New ReaderWriterLock()
        //Private drawnSize As New Size(0, 0)

        public MaximizeMode maximizeMode { get; set; }

        public ProgramModeType ProgramMode = ProgramModeType.OneWindow;
        public SortModeType SortMode = SortModeType.FilenameAsc;
        public int InitialScreenNumber = 0;
        public bool WatchMode = false;
        public int FirstPageNumber = 1;
        public bool FlagZipFile = false;
        public ZipArchive Zip = null;
        public bool ShowPageNumber = false;
        public bool ShowBorder = false;

        public FileManager fileManager = null;
        public FileWatcher fileWatcher = null;

        public delegate void ChangeFileDelegate();

        bool keyDownExecute = false;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            winMode = WindowMode.Normal;
            rotMode = RotateMode.Normal;
            normalRect = new Rectangle();
            message = null;
            messageFont = new Font("ＭＳ ゴシック", 16);
            messageForegroundBrush = Brushes.Black;
            messageBackgroundBrush = Brushes.White;
            message2 = null;
            message2Font = new Font("ＭＳ ゴシック", 16);
            message2ForegroundBrush = Brushes.Black;
            message2BackgroundBrush = Brushes.White;
            message2Height = 24;
            pageNumberFont = new Font("ＭＳ ゴシック", 16);
            pageNumberForegroundBrush = Brushes.Black;
            pageNumberBackgroundBrush = Brushes.White;
            maximizeMode = MaximizeMode.Normal;

            KeyDown += Form1_KeyDown;
            KeyPress += Form1_KeyPress;
            KeyUp += Form1_KeyUp;
            Load += Form1_Load;

            PictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox1_Paint);
            PictureBox1.MouseWheel += Form1_MouseWheel;
            PictureBox1.MouseMove += Form1_MouseMove;
            PictureBox1.MouseDown += Form1_MouseDown;

            MakeMenu();
        }

        /// <summary>
        /// Loadイベントハンドラ。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void Form1_Load(System.Object sender, System.EventArgs e)
        {
            Rectangle si = Config.ScreenInfo[InitialScreenNumber];
            nowScreenSize = si.Size;
            bmpCenter = si.Location;
            mouseValue = Array.FindIndex(Config.ResizeList, (double d) => d == 1.0);
            maximize = Config.Maximize;

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);

            if (maximize == true)
            {
                winMode = WindowMode.Maximize;
                MaximizeImage();
            }

            // 画像ファイル読み込み
            LoadFile(filename);

            if (WatchMode)
            {
                fileManager.SetLatestFile();
            }
        }

        /// <summary>
        /// Paintイベントハンドラ。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void PictureBox1_Paint(System.Object sender, System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = GraphicsUtil.GetInterpolationMode(GlobalInfo.InterpolationMode);
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            e.Graphics.FillRectangle(Brushes.Black, 0, 0, this.Width, this.Height);

            if (bmp != null)
            {
                if (winMode == WindowMode.Normal)
                {
                    DrawExpandImage(e.Graphics, filename, bmp, 0, 0, this.Width, this.Height);
                }
                else
                {
                    if (ProgramMode == ProgramModeType.MihirakiZengo)
                    {
                        // 見開き＋前後表示モード

                        int x1 = 0;
                        int y1 = 0;
                        int width1 = nowScreenSize.Width * 3 / 4;
                        int height1 = nowScreenSize.Height - message2Height;
                        DrawMihirakiImage(e.Graphics, bmp, filename, bmp2, beforeFilename, x1, y1, width1, height1, fileManager.showPages, ShowPageNumber);

                        Bitmap prevLeftBmp;
                        string prevLeftFilename;
                        Bitmap prevRightBmp;
                        string prevRightFilename;
                        int prevFileInfoIndex;
                        int prevShowPages;
                        bool prevChangeFlag;
                        GetPrevMihiraki(out prevLeftBmp, out prevLeftFilename, out prevRightBmp, out prevRightFilename, out prevFileInfoIndex, out prevShowPages, out prevChangeFlag);

                        int width2 = nowScreenSize.Width / 4;
                        int height2 = Math.Min(width2, height1 / 2);
                        int x2 = width1;
                        int y2 = 0;
                        DrawMihirakiImage(e.Graphics, prevLeftBmp, prevLeftFilename, prevRightBmp, prevRightFilename, x2, y2, width2, height2, prevShowPages, false);

                        Bitmap nextLeftBmp;
                        string nextLeftFilename;
                        Bitmap nextRightBmp;
                        string nextRightFilename;
                        int nextFileInfoIndex;
                        int nextShowPages;
                        bool nextChangeFlag;
                        GetNextMihiraki(out nextLeftBmp, out nextLeftFilename, out nextRightBmp, out nextRightFilename, out nextFileInfoIndex, out nextShowPages, out nextChangeFlag);

                        int width3 = nowScreenSize.Width / 4;
                        int height3 = Math.Min(width3, height1 / 2);
                        int x3 = width1;
                        int y3 = height1 - height3;
                        DrawMihirakiImage(e.Graphics, nextLeftBmp, nextLeftFilename, nextRightBmp, nextRightFilename, x3, y3, width3, height3, nextShowPages, false);
                    }
                    else if (ProgramMode == ProgramModeType.Zengo)
                    {
                        // 前後表示モード

                        // 現在の画像：幅は画面幅の3/4、高さは画面高さ-メッセージ分
                        int x1 = 0;
                        int y1 = 0;
                        int width1 = nowScreenSize.Width * 3 / 4;
                        int height1 = nowScreenSize.Height - message2Height;
                        DrawOneImage(e.Graphics, bmp, filename, x1, y1, width1, height1);

                        // 1つ前の画像：幅は画面幅の1/4、高さは画面幅の1/4と現在画像高さの1/2のうち小さい方
                        int width2 = nowScreenSize.Width / 4;
                        int height2 = Math.Min(width2, height1 / 2);
                        int x2 = width1;
                        int y2 = 0;
                        DrawOneImage(e.Graphics, bmp2, beforeFilename, x2, y2, width2, height2);

                        // 1つ次の画像：幅は画面幅の1/4、高さは画面幅の1/4と現在画像高さの1/2のうち小さい方
                        int width3 = nowScreenSize.Width / 4;
                        int height3 = Math.Min(width3, height1 / 2);
                        int x3 = width1;
                        int y3 = height1 - height3;
                        DrawOneImage(e.Graphics, bmp3, nextFilename, x3, y3, width3, height3);
                    }
                    else if (ProgramMode == ProgramModeType.Sanmen)
                    {
                        // 3面モード

                        // 左端画像サイズをもとに中央・右側画像サイズを作る
                        int w = 0;
                        int h = 0;
                        int b1w = bmp.Width;
                        int b2w = b1w;
                        int b3w = b1w;
                        int b12w = b1w * 2;
                        int b123w = b1w * 3;
                        double nd = Convert.ToDouble(nowScreenSize.Width) / nowScreenSize.Height;
                        double bd = Convert.ToDouble(b123w) / bmp.Height;
                        Rectangle d1 = default(Rectangle);
                        Rectangle d2 = default(Rectangle);
                        Rectangle d3 = default(Rectangle);

                        if (nd == bd)
                        {
                            // 縦横比が現在の画面と同じ
                            d1 = new Rectangle(0, 0, b1w, h);
                            d2 = new Rectangle(b1w, 0, b2w, h);
                            d3 = new Rectangle(b12w, 0, b3w, h);
                        }
                        else if (bd > nd)
                        {
                            // 縦横比が現在の画面より横長
                            double d = Convert.ToDouble(this.Width) / b123w;
                            w = (int)(d * b123w);
                            h = (int)(d * bmp.Height);
                            int y = (this.Height - h) / 2;

                            //e.Graphics.FillRectangle(Brushes.Black, 0, 0, this.Width, y);
                            //e.Graphics.FillRectangle(Brushes.Black, 0, y + h, this.Width, this.Height - (y + h));

                            d1 = new Rectangle(0, y, (int)(d * b1w), h);
                            d2 = new Rectangle((int)(d * b1w), y, (int)(d * b2w), h);
                            d3 = new Rectangle((int)(d * b12w), y, (int)(d * b3w), h);
                        }
                        else
                        {
                            // 縦横比が現在の画面より縦長
                            double d = Convert.ToDouble(this.Height) / bmp.Height;
                            w = (int)(d * b123w);
                            h = (int)(d * bmp.Height);
                            int x = (this.Width - w) / 2;
                            
                            //e.Graphics.FillRectangle(Brushes.Black, 0, 0, x, this.Height);
                            //e.Graphics.FillRectangle(Brushes.Black, x + w, 0, this.Width - (x + w), this.Height);

                            d1 = new Rectangle(x, 0, (int)(d * b1w), h);
                            d2 = new Rectangle(x + (int)(d * b1w), 0, (int)(d * b2w), h);
                            d3 = new Rectangle(x + (int)(d * b12w), 0, (int)(d * b3w), h);
                        }

                        if (fileManager.showPages == -2)
                        {
                            // 真ん中と右のみ
                            DrawExpandImage(e.Graphics, null, null, d1.X, d1.Y, d1.Width, d1.Height);
                            DrawExpandImage(e.Graphics, filename, bmp, d2.X, d2.Y, d2.Width, d2.Height);
                            DrawOneImage(e.Graphics, bmp2, beforeFilename, d3.X, d3.Y, d3.Width, d3.Height);
                        }
                        else if (fileManager.showPages == -1)
                        {
                            // 右のみ
                            DrawExpandImage(e.Graphics, null, null, d1.X, d1.Y, d1.Width, d1.Height);
                            DrawExpandImage(e.Graphics, null, null, d2.X, d2.Y, d2.Width, d2.Height);
                            DrawExpandImage(e.Graphics, filename, bmp, d3.X, d3.Y, d3.Width, d3.Height);
                        }
                        else
                        {
                            // 左・真ん中・右
                            DrawExpandImage(e.Graphics, filename, bmp, d1.X, d1.Y, d1.Width, d1.Height);
                            DrawOneImage(e.Graphics, bmp2, beforeFilename, d2.X, d2.Y, d2.Width, d2.Height);
                            DrawOneImage(e.Graphics, bmp3, beforeBeforeFilename, d3.X, d3.Y, d3.Width, d3.Height);
                        }
                    }
                    else if (ProgramMode == ProgramModeType.Mihiraki)
                    {
                        // 見開きモード
                        MihirakiResult mihirakiResult = DrawMihirakiImage(e.Graphics, bmp, filename, bmp2, beforeFilename, 0, 0, nowScreenSize.Width, nowScreenSize.Height, fileManager.showPages, ShowPageNumber);
                    }
                    else
                    {
                        // 1面モード
                        int x = 0;
                        int y = 0;
                        int w = 0;
                        int h = 0;
                        double nd = Convert.ToDouble(nowScreenSize.Width) / nowScreenSize.Height;
                        double bd = Convert.ToDouble(bmp.Width) / bmp.Height;

                        if (nd == bd)
                        {
                            // 縦横比が現在の画面と同じ
                            w = this.Width;
                            h = this.Height;
                            DrawExpandImage(e.Graphics, filename, bmp, 0, 0, w, h);
                        }
                        else if (bd > nd & maximizeMode == MaximizeMode.Normal)
                        {
                            // 縦横比が現在の画面より横長、内接
                            double d = Convert.ToDouble(this.Width) / bmp.Width;
                            w = (int)(d * bmp.Width);
                            h = (int)(d * bmp.Height);
                            y = (this.Height - h) / 2;

                            e.Graphics.FillRectangle(Brushes.Black, 0, 0, this.Width, y);
                            e.Graphics.FillRectangle(Brushes.Black, 0, y + h, this.Width, this.Height - (y + h));

                            DrawExpandImage(e.Graphics, filename, bmp, 0, y, w, h);
                        }
                        else if (bd < nd & maximizeMode == MaximizeMode.Normal)
                        {
                            // 縦横比が現在の画面より縦長、内接
                            double d = Convert.ToDouble(this.Height) / bmp.Height;
                            w = (int)(d * bmp.Width);
                            h = (int)(d * bmp.Height);
                            x = (this.Width - w) / 2;
                            e.Graphics.FillRectangle(Brushes.Black, 0, 0, x, this.Height);
                            e.Graphics.FillRectangle(Brushes.Black, x + w, 0, this.Width - (x + w), this.Height);
                            DrawExpandImage(e.Graphics, filename, bmp, x, 0, w, h);
                        }
                        else if (bd > nd & maximizeMode == MaximizeMode.Large)
                        {
                            // 縦横比が現在の画面より横長、外接
                            double d = Convert.ToDouble(this.Height) / bmp.Height;
                            w = (int)(d * bmp.Width);
                            h = (int)(d * bmp.Height);
                            x = (this.Width - w) / 2;
                            y = (this.Height - h) / 2;
                            DrawExpandImage(e.Graphics, filename, bmp, x, y, w, h);
                        }
                        else if (bd < nd & maximizeMode == MaximizeMode.Large)
                        {
                            // 縦横比が現在の画面より縦長、外接
                            double d = Convert.ToDouble(this.Width) / bmp.Width;
                            w = (int)(d * bmp.Width);
                            h = (int)(d * bmp.Height);
                            x = (this.Width - w) / 2;
                            y = (this.Height - h) / 2;
                            DrawExpandImage(e.Graphics, filename, bmp, x, y, w, h);
                        }

                        if (ShowPageNumber)
                        {
                            int pageNumber = FirstPageNumber + fileManager.fileInfoIndex;

                            string pageNumberStrLeft = pageNumber.ToString().PadLeft(3, (pageNumber >= 0) ? '0' : ' ');
                            SizeF szl = e.Graphics.MeasureString(pageNumberStrLeft, pageNumberFont);
                            int pnlx = (int)(x - szl.Width - 2);
                            int pnly = (int)(y + h - szl.Height);
                            e.Graphics.FillRectangle(pageNumberBackgroundBrush, pnlx, pnly, szl.Width, szl.Height);
                            e.Graphics.DrawString(pageNumberStrLeft, pageNumberFont, pageNumberForegroundBrush, pnlx, pnly);
                        }
                    }
                }
            }

            if (message != null)
            {
                SizeF sz = e.Graphics.MeasureString(message, messageFont);
                int mx = (int)((this.Width - sz.Width) / 2);
                int my = (int)((this.Height - sz.Height) / 2);
                e.Graphics.FillRectangle(messageBackgroundBrush, mx, my, sz.Width, sz.Height);
                e.Graphics.DrawString(message, messageFont, messageForegroundBrush, mx, my);
            }

            if (message2 != null)
            {
                SizeF sz = e.Graphics.MeasureString(message2, messageFont);
                int mx = (int)((this.Width - sz.Width) / 2);
                int my = (int)(this.Height - sz.Height);
                e.Graphics.FillRectangle(message2BackgroundBrush, mx, my, sz.Width, sz.Height);
                e.Graphics.DrawString(message2, message2Font, message2ForegroundBrush, mx, my);
            }

            // 先読み指定ありなら先読みスレッドを起動する(未起動の場合のみ)
            GlobalInfo.Loading = false;
            if (GlobalInfo.PreLoadNumber > 0)
            {
                ThreadTimer.Run();
            }

        }

        /// <summary>
        /// MouseDownイベントハンドラ。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void Form1_MouseDown(System.Object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (winMode == WindowMode.Normal)
            {
                if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    mousePoint = new Point(e.X, e.Y);
                }
            }
        }

        /// <summary>
        /// MouseMoveイベントハンドラ。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void Form1_MouseMove(System.Object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (winMode == WindowMode.Normal)
            {
                if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    this.Left += e.X - mousePoint.X;
                    this.Top += e.Y - mousePoint.Y;
                    normalRect = new Rectangle(this.Left, this.Top, normalRect.Width, normalRect.Height);
                    bmpCenter = new Point(this.Left + PictureBox1.Width / 2, this.Top + PictureBox1.Height / 2);
                }
            }
        }

        /// <summary>
        /// MouseWheelイベントハンドラ。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void Form1_MouseWheel(System.Object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (bmp != null)
            {
                if (winMode == WindowMode.Normal)
                {
                    if (e.Delta < 0)
                    {
                        ReduceImage();
                    }
                    else
                    {
                        ExpansionImage();
                    }
                }
            }
        }

        private MihirakiResult DrawMihirakiImage(Graphics graphics, Bitmap bmp, string filename, Bitmap bmp2, string beforeFilename, int x, int y, int width, int height, int showPages, bool showPageNumber = false)
        {
            MihirakiResult mihirakiResult = MihirakiResult.None;

            if (bmp != null && filename != null)
            {
                if (bmp.Width >= bmp.Height)
                {
                    DrawOneImage(graphics, bmp, filename, x, y, width, height, showPageNumber);
                }
                else
                {
                    // 左側画像サイズをもとに右側画像サイズを作る
                    int w = 0;
                    int h = 0;
                    int b1w = bmp.Width;
                    int b2w = b1w;
                    int b12w = b1w * 2;
                    double nd = Convert.ToDouble(width) / height;
                    double bd = Convert.ToDouble(b12w) / bmp.Height;
                    Rectangle d1 = default(Rectangle);
                    Rectangle d2 = default(Rectangle);

                    if (nd == bd)
                    {
                        // 縦横比が現在の画面と同じ
                        d1 = new Rectangle(x, y, b1w, h);
                        d2 = new Rectangle(x + b1w, y, b2w, h);
                    }
                    else if (bd > nd)
                    {
                        // 縦横比が現在の画面より横長
                        double d = Convert.ToDouble(width) / b12w;
                        w = (int)(d * b12w);
                        h = (int)(d * bmp.Height);
                        int y1 = (height - h) / 2;

                        //e.Graphics.FillRectangle(Brushes.Black, x, y, this.Width, y);
                        //e.Graphics.FillRectangle(Brushes.Black, x, y + y1 + h, this.Width, this.Height - (y + h));

                        d1 = new Rectangle(x, y + y1, (int)(d * b1w), h);
                        d2 = new Rectangle(x + (int)(d * b1w), y + y1, (int)(d * b2w), h);
                    }
                    else
                    {
                        // 縦横比が現在の画面より縦長
                        double d = Convert.ToDouble(height) / bmp.Height;
                        w = (int)(d * b12w);
                        h = (int)(d * bmp.Height);
                        int x1 = (width - w) / 2;

                        //e.Graphics.FillRectangle(Brushes.Black, x, y, x1, this.Height);
                        //e.Graphics.FillRectangle(Brushes.Black, x + x1 + w, y, this.Width - (x + w), this.Height);

                        d1 = new Rectangle(x + x1, y, (int)(d * b1w), h);
                        d2 = new Rectangle(x + x1 + (int)(d * b1w), y, (int)(d * b2w), h);
                    }

                    if (showPages == -1)
                    {
                        // 右のみ
                        DrawExpandImage(graphics, null, null, d1.X, d1.Y, d1.Width, d1.Height);
                        DrawExpandImage(graphics, filename, bmp, d2.X, d2.Y, d2.Width, d2.Height);
                        mihirakiResult = MihirakiResult.Right;
                    }
                    else
                    {
                        // 左右両方または左のみ

                        // 左
                        DrawExpandImage(graphics, filename, bmp, d1.X, d1.Y, d1.Width, d1.Height);
                        if (bmp2 != null && bmp2.Width < bmp2.Height)
                        {
                            // 右
                            DrawOneImage(graphics, bmp2, beforeFilename, d2.X, d2.Y, d2.Width, d2.Height);
                            mihirakiResult = MihirakiResult.Both;
                        }
                        else
                        {
                            mihirakiResult = MihirakiResult.Left;
                        }
                    }

                    if (showPageNumber)
                    {
                        int pageNumber = FirstPageNumber + fileManager.fileInfoIndex;
                        if (showPages == -1)
                        {
                            pageNumber += 1;
                        }

                        if (mihirakiResult == MihirakiResult.Left || mihirakiResult == MihirakiResult.Both)
                        {
                            string pageNumberStrLeft = pageNumber.ToString().PadLeft(3, (pageNumber >= 0) ? '0' : ' ');
                            SizeF szl = graphics.MeasureString(pageNumberStrLeft, pageNumberFont);
                            int pnlx = (int)(d1.X - szl.Width - 2);
                            int pnly = (int)(d1.Y + d1.Height - szl.Height);
                            graphics.FillRectangle(pageNumberBackgroundBrush, pnlx, pnly, szl.Width, szl.Height);
                            graphics.DrawString(pageNumberStrLeft, pageNumberFont, pageNumberForegroundBrush, pnlx, pnly);
                        }

                        if (mihirakiResult == MihirakiResult.Right || mihirakiResult == MihirakiResult.Both)
                        {
                            string pageNumberStrRight = (pageNumber - 1).ToString().PadLeft(3, ((pageNumber - 1) >= 0) ? '0' : ' ');
                            SizeF szr = graphics.MeasureString(pageNumberStrRight, pageNumberFont);
                            int pnrx = d2.X + d2.Width + 1;
                            int pnry = (int)(d2.Y + d2.Height - szr.Height);
                            graphics.FillRectangle(pageNumberBackgroundBrush, pnrx, pnry, szr.Width, szr.Height);
                            graphics.DrawString(pageNumberStrRight, pageNumberFont, pageNumberForegroundBrush, pnrx, pnry);
                        }
                    }
                }
            }

            return mihirakiResult;
        }

        private void DrawOneImage(Graphics graphics, Bitmap bmp, string filename, int x, int y, int width, int height, bool showPageNumber = false)
        {
            if (bmp == null || filename == null)
            {
                graphics.FillRectangle(Brushes.Black, x, y, width, height);
                return;
            }

            int x1 = 0;
            int y1 = 0;
            int w = 0;
            int h = 0;
            double nd = Convert.ToDouble(width) / height;
            double bd = Convert.ToDouble(bmp.Width) / bmp.Height;

            if (nd == bd)
            {
                // 縦横比が現在の画面と同じ
                w = width;
                h = height;
                DrawExpandImage(graphics, filename, bmp, x, y, w, h);
            }
            else if (bd > nd)
            {
                // 縦横比が現在の画面より横長、内接
                double d = Convert.ToDouble(width) / bmp.Width;
                w = (int)(d * bmp.Width);
                h = (int)(d * bmp.Height);
                y1 = (height - h) / 2;

                graphics.FillRectangle(Brushes.Black, x, y, width, y1);
                graphics.FillRectangle(Brushes.Black, x, y + y1 + h, width, height - (y1 + h));

                DrawExpandImage(graphics, filename, bmp, x, y + y1, w, h);
            }
            else if (bd < nd)
            {
                // 縦横比が現在の画面より縦長、内接
                double d = Convert.ToDouble(height) / bmp.Height;
                w = (int)(d * bmp.Width);
                h = (int)(d * bmp.Height);
                x1 = (width - w) / 2;
                graphics.FillRectangle(Brushes.Black, x, y, x + x1, height);
                graphics.FillRectangle(Brushes.Black, x + x1 + w, y, width - (x1 + w), height);
                DrawExpandImage(graphics, filename, bmp, x + x1, y, w, h);
            }

            if (showPageNumber)
            {
                int pageNumber = FirstPageNumber + fileManager.fileInfoIndex;

                string pageNumberStrLeft = pageNumber.ToString().PadLeft(3, (pageNumber >= 0) ? '0' : ' ');
                SizeF szl = graphics.MeasureString(pageNumberStrLeft, pageNumberFont);
                int pnlx = (int)(x + x1 - szl.Width - 2);
                int pnly = (int)(y + y1 + h - szl.Height);
                graphics.FillRectangle(pageNumberBackgroundBrush, pnlx, pnly, szl.Width, szl.Height);
                graphics.DrawString(pageNumberStrLeft, pageNumberFont, pageNumberForegroundBrush, pnlx, pnly);
            }
        }

        public void GetPrevMihiraki(out Bitmap prevLeftBmp, out string prevLeftFilename, out Bitmap prevRightBmp, out string prevRightFilename, out int prevFileInfoIndex, out int prevShowPages, out bool changeFlag)
        {
            prevLeftBmp = null;
            prevLeftFilename = null;
            prevRightBmp = null;
            prevRightFilename = null;
            prevFileInfoIndex = -1;
            prevShowPages = -1;
            changeFlag = false;

            string prevFilename = fileManager.GetBackwardFilename(1);
            if (prevFilename != null)
            {
                // 1面右側表示の場合
                if (fileManager.showPages == -1)
                {
                    // 現在の画像が縦長でない場合には1つだけ戻る
                    Bitmap bmp = ImageManager.GetImage(fileManager.filename);
                    if (bmp != null)
                    {
                        if (bmp.Width >= bmp.Height)
                        {
                            // 一つ前の画像が縦長でない場合は左側に1面、縦長は右側に1面表示する
                            Bitmap bmp2 = GetImage(prevFilename);
                            if (bmp2 != null && bmp2.Width >= bmp2.Height)
                            {
                                // 縦長でない場合：左側に1面表示
                                prevFileInfoIndex = fileManager.fileInfoIndex - 1;
                                prevShowPages = 1;
                                changeFlag = true;

                                prevLeftBmp = bmp2;
                                prevLeftFilename = prevFilename;
                            }
                            else
                            {
                                // 縦長：右側に1面表示
                                prevFileInfoIndex = fileManager.fileInfoIndex - 1;
                                prevShowPages = -1;
                                changeFlag = true;

                                prevLeftBmp = bmp2;
                                prevLeftFilename = prevFilename;
                            }
                        }
                        else
                        {
                            // 前の2面の左側画像(すなわち前の画像)を読み込み、縦長でない場合には1つだけ戻り、1面表示にする
                            Bitmap bmp2 = GetImage(prevFilename);
                            if (bmp2 != null && bmp2.Width >= bmp2.Height)
                            {
                                prevFileInfoIndex = fileManager.fileInfoIndex - 1;
                                prevShowPages = 1;
                                changeFlag = true;

                                prevLeftBmp = bmp2;
                                prevLeftFilename = prevFilename;
                            }
                            if (!changeFlag)
                            {
                                // 前の2面の右側画像(すなわち前の前の画像)を読み込み、縦長でない場合には1つ戻り、2面表示にする
                                string prevPrevFilename = fileManager.GetBackwardFilename(2);
                                if (prevPrevFilename != null)
                                {
                                    Bitmap bmp3 = GetImage(prevPrevFilename);
                                    if (bmp3 != null && bmp3.Width >= bmp3.Height)
                                    {
                                        prevFileInfoIndex = fileManager.fileInfoIndex - 1;
                                        prevShowPages = 1;
                                        changeFlag = true;

                                        prevLeftBmp = bmp2;
                                        prevLeftFilename = prevFilename;
                                    }
                                    else
                                    {
                                        prevFileInfoIndex = fileManager.fileInfoIndex - 1;
                                        prevShowPages = 2;
                                        changeFlag = true;

                                        prevRightBmp = bmp3;
                                        prevRightFilename = prevPrevFilename;
                                        prevLeftBmp = bmp2;
                                        prevLeftFilename = prevFilename;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (fileManager.fileInfoIndex >= 1)
                {
                    // 現在の画像が縦長でない場合には1つだけ戻る
                    Bitmap bmp = ImageManager.GetImage(fileManager.filename);
                    if (bmp != null)
                    {
                        if (bmp.Width >= bmp.Height)
                        {
                            // 一つ前の画像が縦長でない場合は左側に1面、縦長は右側に1面表示する
                            Bitmap bmp2 = GetImage(prevFilename);
                            if (bmp2 != null && bmp2.Width >= bmp2.Height)
                            {
                                // 縦長でない場合：左側に1面表示
                                prevFileInfoIndex = fileManager.fileInfoIndex - 1;
                                prevShowPages = 1;
                                changeFlag = true;

                                prevLeftBmp = bmp2;
                                prevLeftFilename = prevFilename;
                            }
                            else
                            {
                                // 縦長：右側に1面表示
                                prevFileInfoIndex = fileManager.fileInfoIndex - 1;
                                prevShowPages = -1;
                                changeFlag = true;

                                prevLeftBmp = bmp2;
                                prevLeftFilename = prevFilename;
                            }
                        }
                        else
                        {
                            // 今の2面の右側画像(すなわち前の画像)を読み込み、縦長でない場合には1つだけ戻る
                            Bitmap bmp2 = GetImage(prevFilename);
                            if (bmp2 != null && bmp2.Width >= bmp2.Height)
                            {
                                prevFileInfoIndex = fileManager.fileInfoIndex - 1;
                                prevShowPages = 1;
                                changeFlag = true;

                                prevLeftBmp = bmp2;
                                prevLeftFilename = prevFilename;
                            }
                            if (!changeFlag)
                            {
                                // 前の2面の左側画像(すなわち前の前の画像)を読み込み、縦長でない場合には2つ戻る
                                string prevPrevFilename = fileManager.GetBackwardFilename(2);
                                if (prevPrevFilename != null)
                                {
                                    Bitmap bmp3 = GetImage(prevPrevFilename);
                                    if (bmp3 != null && bmp3.Width >= bmp3.Height)
                                    {
                                        prevFileInfoIndex = fileManager.fileInfoIndex - 2;
                                        prevShowPages = 1;
                                        changeFlag = true;

                                        prevLeftBmp = bmp3;
                                        prevLeftFilename = prevPrevFilename;
                                    }
                                }
                            }
                            if (!changeFlag)
                            {
                                // 前の2面の右側画像(すなわち前の前の前の画像)を読み込み、縦長でない場合には2つ戻る
                                string prevPrevPrevFilename = fileManager.GetBackwardFilename(3);
                                if (prevPrevPrevFilename != null)
                                {
                                    Bitmap bmp4 = GetImage(prevPrevPrevFilename);
                                    if (bmp4 != null && bmp4.Width >= bmp4.Height)
                                    {
                                        prevFileInfoIndex = fileManager.fileInfoIndex - 2;
                                        prevShowPages = 1;
                                        changeFlag = true;

                                        string prevPrevFilename = fileManager.GetBackwardFilename(2);
                                        if (prevPrevFilename != null)
                                        {
                                            Bitmap bmp3 = GetImage(prevPrevFilename);
                                            prevLeftBmp = bmp3;
                                            prevLeftFilename = prevPrevFilename;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!changeFlag)
                {
                    if (fileManager.fileInfoIndex >= 2)
                    {
                        // 2つ戻る
                        prevFileInfoIndex = fileManager.fileInfoIndex - 2;
                        prevShowPages = 2;
                        changeFlag = true;

                        string prevPrevPrevFilename = fileManager.GetBackwardFilename(3);
                        if (prevPrevPrevFilename != null)
                        {
                            Bitmap bmp5 = GetImage(prevPrevPrevFilename);
                            prevRightBmp = bmp5;
                            prevRightFilename = prevPrevPrevFilename;
                        }
                        string prevPrevFilename = fileManager.GetBackwardFilename(2);
                        if (prevPrevFilename != null)
                        {
                            Bitmap bmp6 = GetImage(prevPrevFilename);
                            prevLeftBmp = bmp6;
                            prevLeftFilename = prevPrevFilename;
                        }
                    }
                    else if (fileManager.fileInfoIndex >= 1)
                    {
                        // 1つ戻る
                        prevFileInfoIndex = fileManager.fileInfoIndex - 1;
                        prevShowPages = 1;
                        changeFlag = true;

                        if (prevFilename != null)
                        {
                            Bitmap bmp7 = GetImage(prevFilename);
                            prevLeftBmp = bmp7;
                            prevLeftFilename = prevFilename;
                        }
                    }
                }
            }
        }

        public void GetNextMihiraki(out Bitmap nextLeftBmp, out string nextLeftFilename, out Bitmap nextRightBmp, out string nextRightFilename, out int nextFileInfoIndex, out int nextShowPages, out bool changeFlag)
        {
            nextLeftBmp = null;
            nextLeftFilename = null;
            nextRightBmp = null;
            nextRightFilename = null;
            nextFileInfoIndex = -1;
            nextShowPages = -1;
            changeFlag = false;

            if (fileManager.fileInfoIndex == fileManager.GetFileInfoListLength() - 2)
            {
                nextFileInfoIndex = fileManager.fileInfoIndex + 1;
                nextShowPages = -1;
                changeFlag = true;

                string nextFilename = fileManager.GetForwardFilename(1);
                if (nextFilename != null)
                {
                    Bitmap bmp4 = GetImage(nextFilename);
                    nextLeftBmp = bmp4;
                    nextLeftFilename = nextFilename;
                }
            }
            else if (fileManager.fileInfoIndex < fileManager.GetFileInfoListLength() - 2)
            {
                // 現在の画像が縦長でない場合には1つだけ進む
                Bitmap bmp = ImageManager.GetImage(fileManager.filename);
                if (bmp != null)
                {
                    if (bmp.Width >= bmp.Height)
                    {
                        // 次が左のみまたは1面
                        nextFileInfoIndex = fileManager.fileInfoIndex + 1;
                        nextShowPages = 1;
                        changeFlag = true;

                        string nextFilename = fileManager.GetForwardFilename(1);
                        if (nextFilename != null)
                        {
                            Bitmap bmp5 = GetImage(nextFilename);
                            nextLeftBmp = bmp5;
                            nextLeftFilename = nextFilename;
                        }

                    }
                    else
                    {
                        // 次の2面の右側画像(すなわち次の画像)を読み込み、縦長でない場合には1つだけ進む
                        string nextFilename = fileManager.GetForwardFilename(1);
                        if (nextFilename != null)
                        {
                            Bitmap bmp2 = GetImage(nextFilename);
                            if (bmp2 != null && bmp2.Width >= bmp2.Height)
                            {
                                // 次が1面
                                nextFileInfoIndex = fileManager.fileInfoIndex + 1;
                                nextShowPages = 1;
                                changeFlag = true;

                                nextLeftBmp = bmp2;
                                nextLeftFilename = nextFilename;
                            }
                            if (!changeFlag)
                            {
                                // 次の2面の左側画像(すなわち次の次の画像)を読み込み、縦長でない場合には1つだけ進む
                                string nextNextFilename = fileManager.GetForwardFilename(2);
                                if (nextNextFilename != null)
                                {
                                    Bitmap bmp3 = GetImage(nextNextFilename);
                                    if (bmp3 != null && bmp3.Width >= bmp3.Height)
                                    {
                                        // 次が右のみ
                                        nextFileInfoIndex = fileManager.fileInfoIndex + 1;
                                        nextShowPages = -1;
                                        changeFlag = true;

                                        // 右のみの場合はnextShowPagesを-1にして左(基準側)に入れる
                                        nextLeftBmp = bmp2;
                                        nextLeftFilename = nextFilename;
                                    }
                                }
                            }
                        }
                    }
                }
                if (!changeFlag)
                {
                    // 2つ進む
                    nextFileInfoIndex = fileManager.fileInfoIndex + 2;
                    nextShowPages = 2;
                    changeFlag = true;

                    string nextFilename = fileManager.GetForwardFilename(1);
                    if (nextFilename != null)
                    {
                        Bitmap bmp6 = GetImage(nextFilename);
                        nextRightBmp = bmp6;
                        nextRightFilename = nextFilename;
                    }
                    string nextNextFilename = fileManager.GetForwardFilename(2);
                    if (nextNextFilename != null)
                    {
                        Bitmap bmp7 = GetImage(nextNextFilename);
                        nextLeftBmp = bmp7;
                        nextLeftFilename = nextNextFilename;
                    }
                }
            }
        }

        /// <summary>
        /// 拡大。
        /// </summary>
        /// <remarks></remarks>
        public void ExpansionImage()
        {
            if ((mouseValue + 1) <= Config.ResizeList.Length - 1)
            {
                mouseValue += 1;
                int w = (int)(bmp.Width * Config.ResizeList[mouseValue]);
                int h = (int)(bmp.Height * Config.ResizeList[mouseValue]);
                ResizeImage(w, h);
            }
        }

        /// <summary>
        /// 縮小。
        /// </summary>
        /// <remarks></remarks>
        public void ReduceImage()
        {
            if ((mouseValue - 1) >= 0)
            {
                mouseValue -= 1;
                int w = (int)(bmp.Width * Config.ResizeList[mouseValue]);
                int h = (int)(bmp.Height * Config.ResizeList[mouseValue]);
                ResizeImage(w, h);
            }
        }

        /// <summary>
        /// 画像リサイズ。
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void ResizeImage(int width, int height)
        {
            PictureBox1.Width = width;
            PictureBox1.Height = height;
            Rectangle rec = new Rectangle(bmpCenter.X - width / 2, bmpCenter.Y - height / 2, width, height);
            SetWindowBounds(rec);
            normalRect = new Rectangle(this.Left, this.Top, this.Width, this.Height);
            PictureBox1.Invalidate();
        }

        /// <summary>
        /// 画像ロード。
        /// </summary>
        /// <param name="filename"></param>
        private void LoadFile(string filename)
        {
            this.Text = filename;

            if (ProgramMode == ProgramModeType.Zengo)
            {
                LoadFileZengo(filename);
            }
            else if (ProgramMode == ProgramModeType.Sanmen)
            {
                LoadFileSanmen(filename);
            }
            else if (ProgramMode == ProgramModeType.Mihiraki || ProgramMode == ProgramModeType.MihirakiZengo)
            {
                LoadFileMihiraki(filename);
            }
            else
            {
                LoadFileOne(filename);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        private void LoadFileOne(string filename)
        {
            // メッセージクリア
            initMessage();

            bmp = ImageManager.GetImage(filename);
            if (bmp == null)
            {
                bmp = NewBitmap(filename);
                if (bmp == null)
                {
                    return;
                }
                ImageManager.AddImage(filename, bmp.Width, bmp.Height, (Bitmap)(bmp.Clone()));
            }

            // 回転モードの場合には回転させる
            if (rotMode != RotateMode.Normal)
            {
                // ファイルをロックしたままRitateするとGDIエラーになるため、コピーしてからRotateする
                Bitmap bmpTmp = new Bitmap(bmp);
                switch (rotMode)
                {
                    case RotateMode.R90:
                        bmpTmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case RotateMode.R180:
                        bmpTmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case RotateMode.R270:
                        bmpTmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
                bmp = bmpTmp;
            }

            if (winMode == WindowMode.Normal)
            {
                // 通常
                int w = (int)(bmp.Width * Config.ResizeList[mouseValue]);
                int h = (int)(bmp.Height * Config.ResizeList[mouseValue]);
                bmpCenter = new Point(this.Left + w / 2, this.Top + h / 2);
                normalRect = new Rectangle(this.Left, this.Top, w, h);
                Rectangle rec = new Rectangle(normalRect.Left, normalRect.Top, normalRect.Width, normalRect.Height);
                SetWindowBounds(rec);
                PictureBox1.Width = w;
                PictureBox1.Height = h;
            }
            else
            {
                if (Control.IsKeyLocked(Keys.Scroll))
                {
                    // Scrollキーロックの場合には、コンフィグの横長、縦長スクリーンに表示
                    int screenNumber = 0;
                    if ((bmp.Width > bmp.Height))
                    {
                        // 横長
                        screenNumber = Config.horizontallyLongScreen;
                    }
                    else
                    {
                        // 縦長、縦横同じ
                        screenNumber = Config.verticallyLongScreen;
                    }
                    MaximizeImage(screenNumber);
                }
                else
                {
                    // 最大化済みの場合にはウィンドウサイズを変更しない
                    int w = (int)(bmp.Width * Config.ResizeList[mouseValue]);
                    int h = (int)(bmp.Height * Config.ResizeList[mouseValue]);
                    bmpCenter = new Point(this.Left + w / 2, this.Top + h / 2);
                    normalRect = new Rectangle(this.Left, this.Top, w, h);
                }
            }
        }

        /// <summary>
        /// 見開きで画像ロード。
        /// </summary>
        /// <param name="filename"></param>
        private void LoadFileMihiraki(string filename)
        {
            // メッセージクリア
            initMessage();

            beforeFilename = null;
            bmp2 = null;

            // 左側画像読み込み
            bmp = GetImage(filename);

            if (fileManager.showPages != -1)
            {
                // 右側画像読み込み
                beforeFilename = fileManager.GetBeforeFilename(filename);
                if (beforeFilename != null)
                {
                    bmp2 = GetImage(beforeFilename);
                }
            }
        }

        /// <summary>
        /// 3面で画像ロード。
        /// </summary>
        /// <param name="filename"></param>
        private void LoadFileSanmen(string filename)
        {
            // メッセージクリア
            initMessage();

            beforeFilename = null;
            beforeBeforeFilename = null;
            bmp2 = null;
            bmp3 = null;

            // 左側画像読み込み
            bmp = GetImage(filename);

            // 中央画像ファイル名取得
            beforeFilename = fileManager.GetBeforeFilename(filename);
            if (beforeFilename == null)
            {
                // 中央画像なし(左側のみ)
            }
            else
            {
                // 中央画像読み込み
                bmp2 = GetImage(beforeFilename);

                // 右側画像ファイル名取得
                beforeBeforeFilename = fileManager.GetBeforeFilename(beforeFilename);
                if (beforeBeforeFilename == null)
                {
                    // 右側画像なし
                }
                else
                {
                    // 右側画像読み込み
                    bmp3 = GetImage(beforeBeforeFilename);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        private void LoadFileZengo(string filename)
        {
            // メッセージクリア
            initMessage();

            // 現在の画像を読み込み
            bmp = GetImage(filename);

            // 1つ前の画像を読み込み
            beforeFilename = fileManager.GetBeforeFilename(filename);
            bmp2 = GetImage(beforeFilename);

            // 1つ次の画像を読み込み
            nextFilename = fileManager.GetNextFilename(filename);
            bmp3 = GetImage(nextFilename);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public void LoadFileOneCache(string filename)
        {
            int w = 0;
            int h = 0;

            ImageBufferStatus status = ImageManager.GetImageStatus(filename);
            Bitmap bmp = null;
            int bmpWidth = 0;
            int bmpHeight = 0;
            ImageBuffer imageBuffer = null;
            if (status == ImageBufferStatus.None)
            {
                if (bmp == null)
                {
                    bmp = NewBitmap(filename);
                    ImageManager.AddImage(filename, bmp.Width, bmp.Height, (Bitmap)(bmp.Clone()));
                }

                // 回転モードの場合には回転させる
                if (rotMode != RotateMode.Normal)
                {
                    // ファイルをロックしたままRitateするとGDIエラーになるため、コピーしてからRotateする
                    Bitmap bmpTmp = new Bitmap(bmp);
                    switch (rotMode)
                    {
                        case RotateMode.R90:
                            bmpTmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case RotateMode.R180:
                            bmpTmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case RotateMode.R270:
                            bmpTmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }
                    bmp = bmpTmp;
                }

                bmpWidth = bmp.Width;
                bmpHeight = bmp.Height;
            }
            else
            {
                imageBuffer = ImageManager.GetImageBuffer(filename);
                bmpWidth = imageBuffer.width;
                bmpHeight = imageBuffer.height;
            }

            double nd = Convert.ToDouble(nowScreenSize.Width) / nowScreenSize.Height;
            double bd = Convert.ToDouble(bmpWidth) / bmpHeight;

            if (nd == bd)
            {
                // 縦横比が現在の画面と同じ
                w = this.Width;
                h = this.Height;
            }
            else if (bd > nd)
            {
                // 縦横比が現在の画面より横長
                double d = Convert.ToDouble(this.Width) / bmpWidth;
                w = (int)(d * bmpWidth);
                h = (int)(d * bmpHeight);
            }
            else
            {
                // 縦横比が現在の画面より縦長
                double d = Convert.ToDouble(this.Height) / bmpHeight;
                w = (int)(d * bmpWidth);
                h = (int)(d * bmpHeight);
            }

            ExpandInfo ei = new ExpandInfo(filename, w, h, rotMode);
            status = ImageManager.GetExpandImageStatus(ei);
            if (status != ImageBufferStatus.None)
            {
                return;
            }
            int borderWidth = (ShowBorder ? GlobalInfo.BorderWidth : 0);
            Bitmap extendBitmap = new Bitmap(w - borderWidth * 2, h - borderWidth * 2);
            Graphics eg = Graphics.FromImage(extendBitmap);
            eg.InterpolationMode = GraphicsUtil.GetInterpolationMode(GlobalInfo.InterpolationMode);
            eg.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            eg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            if (bmp == null)
            {
                bmp = ImageManager.GetImage(filename);
            }
            DrawImage(eg, bmp, 0, 0, w, h);
            ImageManager.AddExpandImage(ei, extendBitmap);
            eg.Dispose();

        }

        /// <summary>
        /// 
        /// </summary>
        public void ReloadFile()
        {
            // 読み込み済みの画像を削除する
            ImageManager.Remove(filename);

            // 読み込み直す
            ChangeFile();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ChangeFile()
        {
            LoadFile(filename);
            this.Refresh();
        }

        /// <summary>
        /// 最大化。
        /// </summary>
        /// <remarks></remarks>
        public void MaximizeImage()
        {
            int sn = Config.GetScreenNumber(bmpCenter);
            MaximizeImage(sn);
        }

        /// <summary>
        /// 最大化。
        /// </summary>
        /// <remarks></remarks>
        public void MaximizeImage(int screenNumber)
        {
            Rectangle sc = Config.ScreenInfo[screenNumber];
            nowScreenSize = sc.Size;

            PictureBox1.Width = nowScreenSize.Width;
            PictureBox1.Height = nowScreenSize.Height;
            Rectangle rec = new Rectangle(sc.Left, sc.Top, nowScreenSize.Width, nowScreenSize.Height);
            SetWindowBounds(rec);
            this.Refresh();
        }

        public void MoveMaximizeWindow(int screenNumber)
        {
            if ((0 <= screenNumber && screenNumber < Config.ScreenInfo.Length))
            {
                Rectangle sc = Config.ScreenInfo[screenNumber];
                bmpCenter = new Point(sc.Left + sc.Width / 2, sc.Top + sc.Height / 2);
                MaximizeImage();
            }
        }

        public void CalcFromCenter()
        {
            int w = (int)(bmp.Width * Config.ResizeList[mouseValue]);
            int h = (int)(bmp.Height * Config.ResizeList[mouseValue]);
            normalRect = new Rectangle(bmpCenter.X - w / 2, bmpCenter.Y - h / 2, w, h);
        }

        /// <summary>
        /// KeyPressイベントハンドラ。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void Form1_KeyPress(System.Object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!keyDownExecute)
            {
                List<KeyManagerDelegate> addrs = KeyManager.GetKeyPress(e.KeyChar);
                if (addrs != null)
                {
                    try
                    {
                        foreach (KeyManagerDelegate addr in addrs)
                        {
                            addr(sender, e);
                        }
                    }
                    // キー入力で設定を更新した場合にはループ中にaddrsが変更されてここに来る
                    catch (InvalidOperationException ex)
                    {
                        // 処理を打ち切る
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// KeyDownイベントハンドラ。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void Form1_KeyDown(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            List<KeyManagerDelegate> addrs = KeyManager.GetKeyDown(e.KeyData);
            if (addrs != null)
            {
                // キー処理でダイアログを開いた場合に、KeyDown実行中にKeyPressが発生し、SupressKeyPressが効かないため、
                // KeyDown処理がある場合には自前のフラグでKeyPress処理を抑制している。
                keyDownExecute = true;

                foreach (KeyManagerDelegate addr in addrs)
                {
                    addr(sender, e);
                }

                // KeyPressイベントを発生させないようにする
                e.SuppressKeyPress = true;
            }
        }

        private void Form1_KeyUp(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            keyDownExecute = false;
        }

        //Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
        // Paintイベントで画面をクリアされるのを抑制するためからメソッドでオーバーライドする
        //End Sub

        /// <summary>
        /// ウィンドウ移動
        /// </summary>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <remarks></remarks>
        public void MoveWindow(int xOffset, int yOffset)
        {
            this.Left += xOffset;
            this.Top += yOffset;
            normalRect = new Rectangle(normalRect.Left + xOffset, normalRect.Top + yOffset, normalRect.Width, normalRect.Height);
            bmpCenter = new Point(bmpCenter.X + xOffset, bmpCenter.Y + yOffset);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RotateModeRight()
        {
            switch (rotMode)
            {
                case RotateMode.Normal:
                    rotMode = RotateMode.R90;
                    break;
                case RotateMode.R90:
                    rotMode = RotateMode.R180;
                    break;
                case RotateMode.R180:
                    rotMode = RotateMode.R270;
                    break;
                case RotateMode.R270:
                    rotMode = RotateMode.Normal;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RotateModeLeft()
        {
            switch (rotMode)
            {
                case RotateMode.Normal:
                    rotMode = RotateMode.R270;
                    break;
                case RotateMode.R270:
                    rotMode = RotateMode.R180;
                    break;
                case RotateMode.R180:
                    rotMode = RotateMode.R90;
                    break;
                case RotateMode.R90:
                    rotMode = RotateMode.Normal;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Bitmap NewBitmap(string filename)
        {
            Bitmap bmp = null;

            // GCしてから読み込む
            System.GC.Collect();

            try
            {
                //bmp = New Bitmap(filename)
                bmp = NewBitmap2(filename);
            }
            catch (ArgumentException e)
            {
                // ファイルなし等
            }
            catch (Exception e)
            {
                System.GC.Collect();
                //bmp = New Bitmap(filename)
                bmp = NewBitmap2(filename);
            }

            return bmp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private Bitmap NewBitmap2(string filename)
        {
            // FromStream()だとRotateFlip()がExternalException(GDI+エラー)となるため、FromFile()に戻した

            Bitmap bmp = null;

            if (FlagZipFile)
            {
                ZipArchiveEntry entry = fileManager.GetZipEntry(filename);
                using (Stream stream = entry.Open())
                {
                    bmp = (Bitmap)System.Drawing.Image.FromStream(stream);
                }
            }
            else
            {
                if (filename == null)
                {
                    message = "'" + fileManager.dirname + "'は空です。";
                    return null;
                }

                try
                {
                    using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bmp = (Bitmap)System.Drawing.Image.FromStream(fs);
                    }
                }
                catch (FileNotFoundException ex)
                {
                    // ファイルがない(なくなった)
                    message = "'" + filename + "'が存在しません。";
                }
                catch (Exception ex)
                {
                    // その他
                    message = "'" + filename + "'の読み込みに失敗しました。";
                }
            }

            return bmp;

            //Return CType(Image.FromFile(filename), Bitmap)
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="image"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void DrawImage(Graphics g, Image image, int x, int y, int width, int height)
        {
            // 念のため例外が発生したらGCして描き直す(しかしうまくいかない)
            try
            {
                g.DrawImage(image, x, y, width, height);
            }
            catch (Exception ex)
            {
                System.GC.Collect();
                g.DrawImage(image, x, y, width, height);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="filename"></param>
        /// <param name="bmp"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        private void DrawExpandImage(Graphics g, string filename, Bitmap bmp, int x, int y, int w, int h)
        {
            if (filename == null || bmp == null)
            {
                g.FillRectangle(Brushes.Black, x, y, w, h);
            }
            else
            {
                int borderWidth = (ShowBorder ? GlobalInfo.BorderWidth : 0);
                int bw = w - borderWidth * 2;
                int bh = h - borderWidth * 2;

                if (borderWidth > 0)
                {
                    Pen borderPen = new Pen(Color.Red, borderWidth);
                    g.DrawRectangle(borderPen, x, y, w, h);
                }

                ExpandInfo ei = new ExpandInfo(filename, bw, bh, rotMode);
                Bitmap extendBitmap = ImageManager.GetExpandImage(ei);
                if (extendBitmap == null)
                {
                    extendBitmap = new Bitmap(bw, bh);
                    Graphics eg = Graphics.FromImage(extendBitmap);
                    eg.InterpolationMode = GraphicsUtil.GetInterpolationMode(GlobalInfo.InterpolationMode);
                    eg.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                    eg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    DrawImage(eg, bmp, 0, 0, bw, bh);
                    ImageManager.AddExpandImage(ei, extendBitmap);
                }
                g.DrawImage(extendBitmap, new Point(x + borderWidth, y + borderWidth));
            }
        }

        [DllImport("user32.dll")]
        extern static bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        /// <summary>
        /// ウィンドウの位置とサイズを変更する
        /// </summary>
        /// <param name="rect">変更後のウィンドウの位置とサイズ</param>
        public void SetWindowBounds(Rectangle rect)
        {
            GlobalInfo.Loading = true;

            //MaximumSizeを大きくしておく
            if (this.MaximumSize.Width < rect.Width)
            {
                this.MaximumSize = new Size(rect.Width, this.MaximumSize.Height);
            }
            if (this.MaximumSize.Height < rect.Height)
            {
                this.MaximumSize = new Size(this.MaximumSize.Width, rect.Height);
            }

            MoveWindow(this.Handle, rect.X, rect.Y, rect.Width, rect.Height, true);
            this.UpdateBounds();
        }

        public void init()
        {
            fileManager = new FileManager(this);
            fileWatcher = new FileWatcher(this);
        }

        public void SetMessage(string message)
        {
            this.message = message;
            Refresh();
        }

        public void initMessage()
        {
            message = null;

            if (message2Count > 0)
            {
                message2Count--;
                if (message2Count <= 0)
                {
                    message2 = null;
                }
            }
        }

        public void MakeMenu()
        {
            contextMenuStrip1.Items.Clear();

            ToolStripMenuItem tsmi1 = new ToolStripMenuItem();
            tsmi1.Text = Constant.MSG_DO_OPEN_FILE;
            tsmi1.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoOpenFile);
            tsmi1.Click += Operation.DoOpenFile;
            contextMenuStrip1.Items.Add(tsmi1);

            ToolStripMenuItem tsmi2 = new ToolStripMenuItem();
            tsmi2.Text = Constant.MSG_DO_OPEN_DIRECTORY;
            tsmi2.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoOpenDirectory);
            tsmi2.Click += Operation.DoOpenDirectory;
            contextMenuStrip1.Items.Add(tsmi2);


            ToolStripMenuItem tsmi3 = new ToolStripMenuItem();
            tsmi3.Text = Constant.MSG_DO_MOVE;

            ToolStripMenuItem tsmi3_1 = new ToolStripMenuItem();
            tsmi3_1.Text = Constant.MSG_DO_PREV_FILE;
            tsmi3_1.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoPrevFile);
            if (Config.ChangeLeftRight != 0 && (ProgramMode == ProgramModeType.Mihiraki || ProgramMode == ProgramModeType.Sanmen || ProgramMode == ProgramModeType.MihirakiZengo))
            {
                tsmi3_1.ShortcutKeyDisplayString += "," + ReplaceShowMenuString(Properties.Settings.Default.DoNextFileChangable);
            }
            else
            {
                tsmi3_1.ShortcutKeyDisplayString += "," + ReplaceShowMenuString(Properties.Settings.Default.DoPrevFileChangable);
            }
            tsmi3_1.Click += Operation.DoPrevFile;
            tsmi3.DropDownItems.Add(tsmi3_1);

            ToolStripMenuItem tsmi3_2 = new ToolStripMenuItem();
            tsmi3_2.Text = Constant.MSG_DO_NEXT_FILE;
            tsmi3_2.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoNextFile);
            if (Config.ChangeLeftRight != 0 && (ProgramMode == ProgramModeType.Mihiraki || ProgramMode == ProgramModeType.Sanmen || ProgramMode == ProgramModeType.MihirakiZengo))
            {
                tsmi3_2.ShortcutKeyDisplayString += "," + ReplaceShowMenuString(Properties.Settings.Default.DoPrevFileChangable);
            }
            else
            {
                tsmi3_2.ShortcutKeyDisplayString += "," + ReplaceShowMenuString(Properties.Settings.Default.DoNextFileChangable);
            }
            tsmi3_2.Click += Operation.DoNextFile;
            tsmi3.DropDownItems.Add(tsmi3_2);

            ToolStripMenuItem tsmi3_3 = new ToolStripMenuItem();
            tsmi3_3.Text = Constant.MSG_DO_SKIP_PREV_FILE;
            tsmi3_3.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoSkipPrevFile);
            tsmi3_3.Click += Operation.DoSkipPrevFile;
            tsmi3.DropDownItems.Add(tsmi3_3);

            ToolStripMenuItem tsmi3_4 = new ToolStripMenuItem();
            tsmi3_4.Text = Constant.MSG_DO_SKIP_NEXT_FILE;
            tsmi3_4.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoSkipNextFile);
            tsmi3_4.Click += Operation.DoSkipNextFile;
            tsmi3.DropDownItems.Add(tsmi3_4);

            ToolStripMenuItem tsmi3_5 = new ToolStripMenuItem();
            tsmi3_5.Text = Constant.MSG_DO_FIRST_FILE;
            tsmi3_5.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoFirstFile);
            tsmi3_5.Click += Operation.DoFirstFile;
            tsmi3.DropDownItems.Add(tsmi3_5);

            ToolStripMenuItem tsmi3_6 = new ToolStripMenuItem();
            tsmi3_6.Text = Constant.MSG_DO_LAST_FILE;
            tsmi3_6.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoLastFile);
            tsmi3_6.Click += Operation.DoLastFile;
            tsmi3.DropDownItems.Add(tsmi3_6);

            contextMenuStrip1.Items.Add(tsmi3);


            ToolStripMenuItem tsmi4 = new ToolStripMenuItem();
            tsmi4.Text = Constant.MSG_DO_IMAGE;

            ToolStripMenuItem tsmi4_1 = new ToolStripMenuItem();
            tsmi4_1.Text = Constant.MSG_DO_RELOAD;
            tsmi4_1.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoReload);
            tsmi4_1.Click += Operation.DoReload;
            tsmi4.DropDownItems.Add(tsmi4_1);

            ToolStripMenuItem tsmi4_2 = new ToolStripMenuItem();
            tsmi4_2.Text = Constant.MSG_DO_MAXIMIZE;
            tsmi4_2.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoMaximize);
            tsmi4_2.Click += Operation.DoMaximize;
            tsmi4.DropDownItems.Add(tsmi4_2);

            ToolStripMenuItem tsmi4_3 = new ToolStripMenuItem();
            tsmi4_3.Text = Constant.MSG_DO_MAXIMIZE_MODE_CHANGE;
            tsmi4_3.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoMaximizeModeChange);
            tsmi4_3.Click += Operation.DoMaximizeModeChange;
            tsmi4.DropDownItems.Add(tsmi4_3);

            ToolStripMenuItem tsmi4_4 = new ToolStripMenuItem();
            tsmi4_4.Text = Constant.MSG_DO_ROTATE_LEFT;
            tsmi4_4.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoRotateLeft);
            tsmi4_4.Click += Operation.DoRotateLeft;
            tsmi4.DropDownItems.Add(tsmi4_4);

            ToolStripMenuItem tsmi4_5 = new ToolStripMenuItem();
            tsmi4_5.Text = Constant.MSG_DO_ROTATE_RIGHT;
            tsmi4_5.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoRotateRight);
            tsmi4_5.Click += Operation.DoRotateRight;
            tsmi4.DropDownItems.Add(tsmi4_5);

            ToolStripMenuItem tsmi4_6 = new ToolStripMenuItem();
            tsmi4_6.Text = Constant.MSG_DO_EXPANSION;
            tsmi4_6.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoExpansion);
            tsmi4_6.Click += Operation.DoExpansion;
            tsmi4.DropDownItems.Add(tsmi4_6);

            ToolStripMenuItem tsmi4_7 = new ToolStripMenuItem();
            tsmi4_7.Text = Constant.MSG_DO_REDUCE;
            tsmi4_7.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoReduce);
            tsmi4_7.Click += Operation.DoReduce;
            tsmi4.DropDownItems.Add(tsmi4_7);

            contextMenuStrip1.Items.Add(tsmi4);


            ToolStripMenuItem tsmi5 = new ToolStripMenuItem();
            tsmi5.Text = Constant.MSG_DO_SCREEN;

            ToolStripMenuItem tsmi5_1 = new ToolStripMenuItem();
            tsmi5_1.Text = Constant.MSG_DO_MOVE_UP_SCREEN;
            tsmi5_1.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoMoveUpScreen);
            tsmi5_1.Click += Operation.DoMoveUpScreen;
            tsmi5.DropDownItems.Add(tsmi5_1);

            ToolStripMenuItem tsmi5_2 = new ToolStripMenuItem();
            tsmi5_2.Text = Constant.MSG_DO_MOVE_DOWN_SCREEN;
            tsmi5_2.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoMoveDownScreen);
            tsmi5_2.Click += Operation.DoMoveDownScreen;
            tsmi5.DropDownItems.Add(tsmi5_2);

            ToolStripMenuItem tsmi5_3 = new ToolStripMenuItem();
            tsmi5_3.Text = Constant.MSG_DO_MOVE_LEFT_SCREEN;
            tsmi5_3.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoMoveLeftScreen);
            tsmi5_3.Click += Operation.DoMoveLeftScreen;
            tsmi5.DropDownItems.Add(tsmi5_3);

            ToolStripMenuItem tsmi5_4 = new ToolStripMenuItem();
            tsmi5_4.Text = Constant.MSG_DO_MOVE_RIGHT_SCREEN;
            tsmi5_4.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoMoveRightScreen);
            tsmi5_4.Click += Operation.DoMoveRightScreen;
            tsmi5.DropDownItems.Add(tsmi5_4);

            contextMenuStrip1.Items.Add(tsmi5);


            ToolStripMenuItem tsmi6 = new ToolStripMenuItem();
            tsmi6.Text = Constant.MSG_DO_COPY;

            ToolStripMenuItem tsmi6_1 = new ToolStripMenuItem();
            tsmi6_1.Text = Constant.MSG_DO_NO_COPY_FILE;
            tsmi6_1.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoNoCopyFile);
            tsmi6_1.Click += Operation.DoNoCopyFile;
            tsmi6.DropDownItems.Add(tsmi6_1);

            ToolStripMenuItem tsmi6_2 = new ToolStripMenuItem();
            tsmi6_2.Text = Constant.MSG_DO_NO_COPY_FILE_REVERSE;
            tsmi6_2.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoNoCopyFileReverse);
            tsmi6_2.Click += Operation.DoNoCopyFileReverse;
            tsmi6.DropDownItems.Add(tsmi6_2);

            ToolStripMenuItem tsmi6_3 = new ToolStripMenuItem();
            tsmi6_3.Text = Constant.MSG_DO_COPY_FILE_1;
            tsmi6_3.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoCopyFile1);
            tsmi6_3.Click += Operation.DoCopyFile1;
            tsmi6.DropDownItems.Add(tsmi6_3);

            ToolStripMenuItem tsmi6_4 = new ToolStripMenuItem();
            tsmi6_4.Text = Constant.MSG_DO_COPY_FILE_2;
            tsmi6_4.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoCopyFile2);
            tsmi6_4.Click += Operation.DoCopyFile2;
            tsmi6.DropDownItems.Add(tsmi6_4);

            ToolStripMenuItem tsmi6_5 = new ToolStripMenuItem();
            tsmi6_5.Text = Constant.MSG_DO_COPY_FILE_3;
            tsmi6_5.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoCopyFile3);
            tsmi6_5.Click += Operation.DoCopyFile3;
            tsmi6.DropDownItems.Add(tsmi6_5);

            ToolStripMenuItem tsmi6_6 = new ToolStripMenuItem();
            tsmi6_6.Text = Constant.MSG_DO_COPY_FILE_4;
            tsmi6_6.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoCopyFile4);
            tsmi6_6.Click += Operation.DoCopyFile4;
            tsmi6.DropDownItems.Add(tsmi6_6);

            ToolStripMenuItem tsmi6_7 = new ToolStripMenuItem();
            tsmi6_7.Text = Constant.MSG_DO_COPY_FILE_5;
            tsmi6_7.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoCopyFile5);
            tsmi6_7.Click += Operation.DoCopyFile5;
            tsmi6.DropDownItems.Add(tsmi6_7);

            ToolStripMenuItem tsmi6_8 = new ToolStripMenuItem();
            tsmi6_8.Text = Constant.MSG_DO_COPY_FILE_6;
            tsmi6_8.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoCopyFile6);
            tsmi6_8.Click += Operation.DoCopyFile6;
            tsmi6.DropDownItems.Add(tsmi6_8);

            ToolStripMenuItem tsmi6_9 = new ToolStripMenuItem();
            tsmi6_9.Text = Constant.MSG_DO_COPY_FILE_7;
            tsmi6_9.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoCopyFile7);
            tsmi6_9.Click += Operation.DoCopyFile7;
            tsmi6.DropDownItems.Add(tsmi6_9);

            ToolStripMenuItem tsmi6_10 = new ToolStripMenuItem();
            tsmi6_10.Text = Constant.MSG_DO_COPY_FILE_8;
            tsmi6_10.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoCopyFile8);
            tsmi6_10.Click += Operation.DoCopyFile8;
            tsmi6.DropDownItems.Add(tsmi6_10);

            ToolStripMenuItem tsmi6_11 = new ToolStripMenuItem();
            tsmi6_11.Text = Constant.MSG_DO_COPY_FILE_9;
            tsmi6_11.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoCopyFile9);
            tsmi6_11.Click += Operation.DoCopyFile9;
            tsmi6.DropDownItems.Add(tsmi6_11);

            contextMenuStrip1.Items.Add(tsmi6);


            ToolStripMenuItem tsmi7 = new ToolStripMenuItem();
            tsmi7.Text = Constant.MSG_DO_SORT;

            ToolStripMenuItem tsmi7_1 = new ToolStripMenuItem();
            tsmi7_1.Text = Constant.MSG_DO_SORT_MODE_FILENAME_ASC;
            tsmi7_1.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoSortModeFilenameAsc);
            tsmi7_1.Click += Operation.DoSortModeFilenameAsc;
            tsmi7.DropDownItems.Add(tsmi7_1);

            ToolStripMenuItem tsmi7_2 = new ToolStripMenuItem();
            tsmi7_2.Text = Constant.MSG_DO_SORT_MODE_FILENAME_DESC;
            tsmi7_2.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoSortModeFilenameDesc);
            tsmi7_2.Click += Operation.DoSortModeFilenameDesc;
            tsmi7.DropDownItems.Add(tsmi7_2);

            ToolStripMenuItem tsmi7_3 = new ToolStripMenuItem();
            tsmi7_3.Text = Constant.MSG_DO_SORT_MODE_TIME_ASC;
            tsmi7_3.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoSortModeTimeAsc);
            tsmi7_3.Click += Operation.DoSortModeTimeAsc;
            tsmi7.DropDownItems.Add(tsmi7_3);

            ToolStripMenuItem tsmi7_4 = new ToolStripMenuItem();
            tsmi7_4.Text = Constant.MSG_DO_SORT_MODE_TIME_DESC;
            tsmi7_4.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoSortModeTimeDesc);
            tsmi7_4.Click += Operation.DoSortModeTimeDesc;
            tsmi7.DropDownItems.Add(tsmi7_4);

            contextMenuStrip1.Items.Add(tsmi7);


            ToolStripMenuItem tsmi8 = new ToolStripMenuItem();
            tsmi8.Text = Constant.MSG_DO_EDIT;

            ToolStripMenuItem tsmi8_1 = new ToolStripMenuItem();
            tsmi8_1.Text = Constant.MSG_DO_COPY_FULL_NAME;
            tsmi8_1.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoCopyFullName);
            tsmi8_1.Click += Operation.DoCopyFullName;
            tsmi8.DropDownItems.Add(tsmi8_1);

            ToolStripMenuItem tsmi8_2 = new ToolStripMenuItem();
            tsmi8_2.Text = Constant.MSG_DO_COPY_FILE;
            tsmi8_2.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoCopyFile);
            tsmi8_2.Click += Operation.DoCopyFile;
            tsmi8.DropDownItems.Add(tsmi8_2);

            ToolStripMenuItem tsmi8_3 = new ToolStripMenuItem();
            tsmi8_3.Text = Constant.MSG_DO_INCREMENT_PAGE_NUMBER;
            tsmi8_3.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoIncrementPageNumber);
            tsmi8_3.Click += Operation.DoIncrementPageNumber;
            tsmi8.DropDownItems.Add(tsmi8_3);

            ToolStripMenuItem tsmi8_4 = new ToolStripMenuItem();
            tsmi8_4.Text = Constant.MSG_DO_DECREMENT_PAGE_NUMBER;
            tsmi8_4.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoDecrementPageNumber);
            tsmi8_4.Click += Operation.DoDecrementPageNumber;
            tsmi8.DropDownItems.Add(tsmi8_4);

            contextMenuStrip1.Items.Add(tsmi8);


            ToolStripMenuItem tsmi9 = new ToolStripMenuItem();
            tsmi9.Text = Constant.MSG_DO_NEXT_FILE;

            ToolStripMenuItem tsmi9_1 = new ToolStripMenuItem();
            tsmi9_1.Text = Constant.MSG_DO_SELECT_EXPLORER;
            tsmi9_1.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoSelectExplorer);
            tsmi9_1.Click += Operation.DoSelectExplorer;
            tsmi9.DropDownItems.Add(tsmi9_1);

            ToolStripMenuItem tsmi9_2 = new ToolStripMenuItem();
            tsmi9_2.Text = Constant.MSG_DO_EXEC_COMMAND_1;
            tsmi9_2.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoExecCommand1);
            tsmi9_2.Click += Operation.DoExecCommand1;
            tsmi9.DropDownItems.Add(tsmi9_2);

            ToolStripMenuItem tsmi9_3 = new ToolStripMenuItem();
            tsmi9_3.Text = Constant.MSG_DO_EXEC_COMMAND_2;
            tsmi9_3.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoExecCommand2);
            tsmi9_3.Click += Operation.DoExecCommand2;
            tsmi9.DropDownItems.Add(tsmi9_3);

            ToolStripMenuItem tsmi9_4 = new ToolStripMenuItem();
            tsmi9_4.Text = Constant.MSG_DO_EXEC_COMMAND_3;
            tsmi9_4.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoExecCommand3);
            tsmi9_4.Click += Operation.DoExecCommand3;
            tsmi9.DropDownItems.Add(tsmi9_4);

            ToolStripMenuItem tsmi9_5 = new ToolStripMenuItem();
            tsmi9_5.Text = Constant.MSG_DO_EXEC_COMMAND_4;
            tsmi9_5.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoExecCommand4);
            tsmi9_5.Click += Operation.DoExecCommand4;
            tsmi9.DropDownItems.Add(tsmi9_5);

            ToolStripMenuItem tsmi9_6 = new ToolStripMenuItem();
            tsmi9_6.Text = Constant.MSG_DO_EXEC_COMMAND_5;
            tsmi9_6.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoExecCommand5);
            tsmi9_6.Click += Operation.DoExecCommand5;
            tsmi9.DropDownItems.Add(tsmi9_6);

            ToolStripMenuItem tsmi9_7 = new ToolStripMenuItem();
            tsmi9_7.Text = Constant.MSG_DO_MOVE_FILES;
            tsmi9_7.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoMoveFiles);
            tsmi9_7.Click += Operation.DoMoveFiles;
            tsmi9.DropDownItems.Add(tsmi9_7);

            ToolStripMenuItem tsmi9_8 = new ToolStripMenuItem();
            tsmi9_8.Text = Constant.MSG_DO_RENAME_PAGE;
            tsmi9_8.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoRenamePage);
            tsmi9_8.Click += Operation.DoRenamePage;
            tsmi9.DropDownItems.Add(tsmi9_8);

            contextMenuStrip1.Items.Add(tsmi9);


            ToolStripMenuItem tsmi10 = new ToolStripMenuItem();
            tsmi10.Text = Constant.MSG_DO_MODE;

            ToolStripMenuItem tsmi10_1 = new ToolStripMenuItem();
            tsmi10_1.Text = Constant.MSG_DO_SINGLE_PAGE;
            tsmi10_1.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoSinglePage);
            tsmi10_1.Click += Operation.DoSinglePage;
            tsmi10.DropDownItems.Add(tsmi10_1);

            ToolStripMenuItem tsmi10_2 = new ToolStripMenuItem();
            tsmi10_2.Text = Constant.MSG_DO_TWO_PAGES;
            tsmi10_2.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoTwoPages);
            tsmi10_2.Click += Operation.DoTwoPages;
            tsmi10.DropDownItems.Add(tsmi10_2);

            ToolStripMenuItem tsmi10_3 = new ToolStripMenuItem();
            tsmi10_3.Text = Constant.MSG_DO_THREE_PAGES;
            tsmi10_3.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoThreePages);
            tsmi10_3.Click += Operation.DoThreePages;
            tsmi10.DropDownItems.Add(tsmi10_3);

            ToolStripMenuItem tsmi10_4 = new ToolStripMenuItem();
            tsmi10_4.Text = Constant.MSG_DO_FRONT_BACK;
            tsmi10_4.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoFrontBack);
            tsmi10_4.Click += Operation.DoFrontBack;
            tsmi10.DropDownItems.Add(tsmi10_4);

            ToolStripMenuItem tsmi10_5 = new ToolStripMenuItem();
            tsmi10_5.Text = Constant.MSG_DO_TWO_PAGES_FRONT_BACK;
            tsmi10_5.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoTwoPagesFrontBack);
            tsmi10_5.Click += Operation.DoTwoPagesFrontBack;
            tsmi10.DropDownItems.Add(tsmi10_5);

            ToolStripMenuItem tsmi10_6 = new ToolStripMenuItem();
            tsmi10_6.Text = Constant.MSG_DO_INTERLOCK;
            tsmi10_6.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoInterlock);
            tsmi10_6.Click += Operation.DoInterlock;
            tsmi10.DropDownItems.Add(tsmi10_6);

            ToolStripMenuItem tsmi10_7 = new ToolStripMenuItem();
            tsmi10_7.Text = Constant.MSG_DO_WATCH_MODE;
            tsmi10_7.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoWatchMode);
            tsmi10_7.Click += Operation.DoWatchMode;
            tsmi10.DropDownItems.Add(tsmi10_7);

            ToolStripMenuItem tsmi10_8 = new ToolStripMenuItem();
            tsmi10_8.Text = Constant.MSG_DO_CHANGE_FOCUS;
            tsmi10_8.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoChangeFocus);
            tsmi10_8.Click += Operation.DoChangeFocus;
            tsmi10.DropDownItems.Add(tsmi10_8);

            ToolStripMenuItem tsmi10_9 = new ToolStripMenuItem();
            tsmi10_9.Text = Constant.MSG_DO_CHANGE_LEFT_RIGHT;
            tsmi10_9.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoChangeLeftRight);
            tsmi10_9.Click += Operation.DoChangeLeftRight;
            tsmi10.DropDownItems.Add(tsmi10_9);

            contextMenuStrip1.Items.Add(tsmi10);

            ToolStripMenuItem tsmi11 = new ToolStripMenuItem();
            tsmi11.Text = Constant.MSG_DO_CONFIG;
            tsmi11.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoConfig);
            tsmi11.Click += Operation.DoConfig;
            contextMenuStrip1.Items.Add(tsmi11);

            ToolStripMenuItem tsmi12 = new ToolStripMenuItem();
            tsmi12.Text = Constant.MSG_DO_CLOSE;
            tsmi12.ShortcutKeyDisplayString = ReplaceShowMenuString(Properties.Settings.Default.DoClose);
            tsmi12.Click += Operation.DoClose;
            contextMenuStrip1.Items.Add(tsmi12);
        }

        private string ReplaceShowMenuString(string str)
        {
            string retStr = str;
            retStr = retStr.Replace("Up", "↑");
            retStr = retStr.Replace("Down", "↓");
            retStr = retStr.Replace("Left", "←");
            retStr = retStr.Replace("Right", "→");
            retStr = retStr.Replace("NumPad", "テンキー");
            retStr = retStr.Replace("Decimal", "テンキー.");
            return retStr;
        }

        public Bitmap GetImage(string filename)
        {
            Bitmap retBmp = null;

            if (filename != null)
            {
                ImageManager.GetImage(filename);
                if (retBmp == null)
                {
                    retBmp = NewBitmap(filename);
                    if (retBmp != null)
                    {
                        ImageManager.AddImage(filename, retBmp.Width, retBmp.Height, retBmp);
                    }
                }
            }

            return retBmp;
        }

    }
}
