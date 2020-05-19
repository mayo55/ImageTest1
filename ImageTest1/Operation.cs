using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace ImageTest1
{
    public class Operation
    {

        /// <summary>
        /// 最大化(トグル)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public static void DoMaximize(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            if (form.winMode == WindowMode.Normal)
            {
                form.winMode = WindowMode.Maximize;
                form.MaximizeImage();
            }
            else
            {
                form.winMode = WindowMode.Normal;
                form.SetWindowBounds(form.normalRect);
                form.PictureBox1.Width = form.normalRect.Width;
                form.PictureBox1.Height = form.normalRect.Height;
                form.Refresh();
            }
        }

        public static void DoMaximizeModeChange(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            if ((form.winMode == WindowMode.Maximize))
            {
                if ((form.maximizeMode == MaximizeMode.Normal))
                {
                    form.maximizeMode = MaximizeMode.Large;
                }
                else
                {
                    form.maximizeMode = MaximizeMode.Normal;
                }

                form.Refresh();
            }
        }

        public static void DoRotateRight(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            form.RotateModeRight();
            if (form.winMode == WindowMode.Normal)
            {
                // ファイルをロックしたままRotateするとGDIエラーになるため、コピーしてからRotateする
                Bitmap bmpTmp = new Bitmap(form.bmp);
                bmpTmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                form.bmp = bmpTmp;
                int w = (int)(form.bmp.Width * Config.ResizeList[form.mouseValue]);
                int h = (int)(form.bmp.Height * Config.ResizeList[form.mouseValue]);
                form.ResizeImage(w, h);
            }
            else
            {
                Bitmap bmpTmp = new Bitmap(form.bmp);
                bmpTmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                form.bmp = bmpTmp;
                form.MaximizeImage();
                form.CalcFromCenter();
            }
        }

        public static void DoRotateLeft(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            form.RotateModeLeft();
            if (form.winMode == WindowMode.Normal)
            {
                // ファイルをロックしたままRotateするとGDIエラーになるため、コピーしてからRotateする
                Bitmap bmpTmp = new Bitmap(form.bmp);
                bmpTmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                form.bmp = bmpTmp;
                int w = (int)(form.bmp.Width * Config.ResizeList[form.mouseValue]);
                int h = (int)(form.bmp.Height * Config.ResizeList[form.mouseValue]);
                form.ResizeImage(w, h);
            }
            else
            {
                Bitmap bmpTmp = new Bitmap(form.bmp);
                bmpTmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                form.bmp = bmpTmp;
                form.MaximizeImage();
                form.CalcFromCenter();
            }
        }

        public static void DoExpansion(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            if (form.winMode == WindowMode.Normal)
            {
                form.ExpansionImage();
            }
        }

        public static void DoReduce(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            if (form.winMode == WindowMode.Normal)
            {
                form.ReduceImage();
            }
        }

        public static void DoPrevFile(object sender, EventArgs e)
        {
            DoPrevFile(sender);
        }

        public static void DoPrevFile(object sender)
        {
            Form1 form = GetForm(sender);

            if (GlobalInfo.FlagInterlocking)
            {
                if (form.fileManager.fileInfoIndex <= 0)
                {
                    return;
                }

                Form1 otherForm = FormManager.GetOtherForm(form);
                otherForm.fileManager.MakeFileInfoListIfNotExist();
                if (otherForm.fileManager.fileInfoIndex <= 0)
                {
                    return;
                }
                DoPrevFile2(otherForm);
            }
            DoPrevFile2(form);
        }

        private static void DoPrevFile2(Form1 form)
        {
            bool changeFlag = false;

            form.fileManager.MakeFileInfoListIfNotExist();

            if (form.ProgramMode == ProgramModeType.Sanmen)
            {
                if (form.fileManager.showPages == -2)
                {
                    form.fileManager.fileInfoIndex -= 2;
                    form.fileManager.showPages = 3;
                    changeFlag = true;
                }
                else if (form.fileManager.showPages == -1)
                {
                    form.fileManager.fileInfoIndex -= 1;
                    form.fileManager.showPages = 3;
                    changeFlag = true;
                }
                else if (form.fileManager.fileInfoIndex >= 2)
                {
                    form.fileManager.fileInfoIndex -= 2;
                    form.fileManager.showPages = 3;
                    changeFlag = true;
                }
                if (changeFlag)
                {
                    form.fileManager.filename = form.fileManager.GetFullName();
                    form.filename = form.fileManager.filename;
                    form.ChangeFile();
                }
            }
            else if (form.ProgramMode == ProgramModeType.Mihiraki || form.ProgramMode == ProgramModeType.MihirakiZengo)
            {
                string prevFilename = form.fileManager.GetBackwardFilename(1);
                if (prevFilename != null)
                {
                    // 1面右側表示の場合
                    if (form.fileManager.showPages == -1)
                    {
                        // 現在の画像が縦長でない場合には1つだけ戻る
                        Bitmap bmp = ImageManager.GetImage(form.fileManager.filename);
                        if (bmp != null)
                        {
                            if (bmp.Width >= bmp.Height)
                            {
                                // 一つ前の画像が縦長でない場合は左側に1面、縦長は右側に1面表示する
                                Bitmap bmp2 = form.GetImage(prevFilename);
                                if (bmp2 != null && bmp2.Width >= bmp2.Height)
                                {
                                    // 縦長でない場合：左側に1面表示
                                    form.fileManager.fileInfoIndex -= 1;
                                    form.fileManager.showPages = 1;
                                    changeFlag = true;
                                }
                                else
                                {
                                    // 縦長：右側に1面表示
                                    form.fileManager.fileInfoIndex -= 1;
                                    form.fileManager.showPages = -1;
                                    changeFlag = true;
                                }
                            }
                            else
                            {
                                // 前の2面の左側画像(すなわち前の画像)を読み込み、縦長でない場合には1つだけ戻り、1面表示にする
                                Bitmap bmp2 = form.GetImage(prevFilename);
                                if (bmp2 != null && bmp2.Width >= bmp2.Height)
                                {
                                    form.fileManager.fileInfoIndex -= 1;
                                    form.fileManager.showPages = 1;
                                    changeFlag = true;
                                }
                                if (!changeFlag)
                                {
                                    // 前の2面の右側画像(すなわち前の前の画像)を読み込み、縦長でない場合には1つ戻り、2面表示にする
                                    string prevPrevFilename = form.fileManager.GetBackwardFilename(2);
                                    if (prevPrevFilename != null)
                                    {
                                        Bitmap bmp3 = form.GetImage(prevPrevFilename);
                                        if (bmp3 != null && bmp3.Width >= bmp3.Height)
                                        {
                                            form.fileManager.fileInfoIndex -= 1;
                                            form.fileManager.showPages = 1;
                                            changeFlag = true;
                                        }
                                        else
                                        {
                                            form.fileManager.fileInfoIndex -= 1;
                                            form.fileManager.showPages = 2;
                                            changeFlag = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (form.fileManager.fileInfoIndex >= 1)
                    {
                        // 現在の画像が縦長でない場合には1つだけ戻る
                        Bitmap bmp = ImageManager.GetImage(form.fileManager.filename);
                        if (bmp != null)
                        {
                            if (bmp.Width >= bmp.Height)
                            {
                                // 一つ前の画像が縦長でない場合は左側に1面、縦長は右側に1面表示する
                                Bitmap bmp2 = form.GetImage(prevFilename);
                                if (bmp2 != null && bmp2.Width >= bmp2.Height)
                                {
                                    // 縦長でない場合：左側に1面表示
                                    form.fileManager.fileInfoIndex -= 1;
                                    form.fileManager.showPages = 1;
                                    changeFlag = true;
                                }
                                else
                                {
                                    // 縦長：右側に1面表示
                                    form.fileManager.fileInfoIndex -= 1;
                                    form.fileManager.showPages = -1;
                                    changeFlag = true;
                                }
                            }
                            else
                            {
                                // 今の2面の右側画像(すなわち前の画像)を読み込み、縦長でない場合には1つだけ戻る
                                Bitmap bmp2 = form.GetImage(prevFilename);
                                if (bmp2 != null && bmp2.Width >= bmp2.Height)
                                {
                                    form.fileManager.fileInfoIndex -= 1;
                                    form.fileManager.showPages = 1;
                                    changeFlag = true;
                                }
                                if (!changeFlag)
                                {
                                    // 前の2面の左側画像(すなわち前の前の画像)を読み込み、縦長でない場合には2つ戻る
                                    string prevPrevFilename = form.fileManager.GetBackwardFilename(2);
                                    if (prevPrevFilename != null)
                                    {
                                        Bitmap bmp3 = form.GetImage(prevPrevFilename);
                                        if (bmp3 != null && bmp3.Width >= bmp3.Height)
                                        {
                                            form.fileManager.fileInfoIndex -= 2;
                                            form.fileManager.showPages = 1;
                                            changeFlag = true;
                                        }
                                    }
                                }
                                if (!changeFlag)
                                {
                                    // 前の2面の右側画像(すなわち前の前の前の画像)を読み込み、縦長でない場合には2つ戻る
                                    string prevPrevPrevFilename = form.fileManager.GetBackwardFilename(3);
                                    if (prevPrevPrevFilename != null)
                                    {
                                        Bitmap bmp4 = form.GetImage(prevPrevPrevFilename);
                                        if (bmp4 != null && bmp4.Width >= bmp4.Height)
                                        {
                                            form.fileManager.fileInfoIndex -= 2;
                                            form.fileManager.showPages = 1;
                                            changeFlag = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!changeFlag)
                    {
                        if (form.fileManager.fileInfoIndex >= 2)
                        {
                            // 2つ戻る
                            form.fileManager.fileInfoIndex -= 2;
                            form.fileManager.showPages = 2;
                            changeFlag = true;
                        }
                        else if (form.fileManager.fileInfoIndex >= 1)
                        {
                            // 1つ戻る
                            form.fileManager.fileInfoIndex -= 1;
                            form.fileManager.showPages = 1;
                            changeFlag = true;
                        }
                    }
                }
                if (changeFlag)
                {
                    form.fileManager.filename = form.fileManager.GetFullName();
                    form.filename = form.fileManager.filename;
                    form.ChangeFile();
                }
            }
            else
            {
                if (form.fileManager.fileInfoIndex > 0)
                {
                    form.fileManager.fileInfoIndex -= 1;
                    form.fileManager.filename = form.fileManager.GetFullName();
                    form.filename = form.fileManager.filename;
                    form.ChangeFile();
                }
            }
        }

        public static void DoNextFile(object sender, EventArgs e)
        {
            DoNextFile(sender);
        }

        public static void DoNextFile(object sender)
        {
            Form1 form = GetForm(sender);

            if (GlobalInfo.FlagInterlocking)
            {
                if (form.fileManager.fileInfoIndex >= (form.fileManager.GetFileInfoListLength() - 1))
                {
                    return;
                }

                Form1 otherForm = FormManager.GetOtherForm(form);
                otherForm.fileManager.MakeFileInfoListIfNotExist();
                if (otherForm.fileManager.fileInfoIndex >= (otherForm.fileManager.GetFileInfoListLength() - 1))
                {
                    return;
                }
                DoNextFile2(otherForm);
            }
            DoNextFile2(form);
        }

        private static void DoNextFile2(Form1 form)
        {
            bool changeFlag = false;

            form.fileManager.MakeFileInfoListIfNotExist();

            if (form.ProgramMode == ProgramModeType.Sanmen)
            {
                if (form.fileManager.fileInfoIndex == form.fileManager.GetFileInfoListLength() - 2)
                {
                    form.fileManager.fileInfoIndex += 1;
                    form.fileManager.showPages = -1;
                    changeFlag = true;
                }
                else if (form.fileManager.fileInfoIndex == form.fileManager.GetFileInfoListLength() - 3)
                {
                    form.fileManager.fileInfoIndex += 2;
                    form.fileManager.showPages = -2;
                    changeFlag = true;
                }
                else if (form.fileManager.fileInfoIndex < form.fileManager.GetFileInfoListLength() - 3)
                {
                    form.fileManager.fileInfoIndex += 2;
                    form.fileManager.showPages = 2;
                    changeFlag = true;
                }
                if (changeFlag)
                {
                    form.fileManager.filename = form.fileManager.GetFullName();
                    form.filename = form.fileManager.filename;
                    form.ChangeFile();
                }
            }
            else if (form.ProgramMode == ProgramModeType.Mihiraki || form.ProgramMode == ProgramModeType.MihirakiZengo)
            {
                if (form.fileManager.fileInfoIndex == form.fileManager.GetFileInfoListLength() - 2)
                {
                    form.fileManager.fileInfoIndex += 1;
                    form.fileManager.showPages = -1;
                    changeFlag = true;
                }
                else if (form.fileManager.fileInfoIndex < form.fileManager.GetFileInfoListLength() - 2)
                {
                    // 現在の画像が縦長でない場合には1つだけ進む
                    Bitmap bmp = ImageManager.GetImage(form.fileManager.filename);
                    if (bmp != null)
                    {
                        if (bmp.Width >= bmp.Height)
                        {
                            form.fileManager.fileInfoIndex += 1;
                            form.fileManager.showPages = 1;
                            changeFlag = true;
                        }
                        else
                        {
                            // 次の2面の右側画像(すなわち次の画像)を読み込み、縦長でない場合には1つだけ進む
                            string nextFilename = form.fileManager.GetForwardFilename(1);
                            if (nextFilename != null)
                            {
                                Bitmap bmp2 = form.GetImage(nextFilename);
                                if (bmp2 != null && bmp2.Width >= bmp2.Height)
                                {
                                    form.fileManager.fileInfoIndex += 1;
                                    form.fileManager.showPages = 1;
                                    changeFlag = true;
                                }
                                if (!changeFlag)
                                {
                                    // 次の2面の左側画像(すなわち次の次の画像)を読み込み、縦長でない場合には1つだけ進む
                                    string nextNextFilename = form.fileManager.GetForwardFilename(2);
                                    if (nextNextFilename != null)
                                    {
                                        Bitmap bmp3 = form.GetImage(nextNextFilename);
                                        if (bmp3 != null && bmp3.Width >= bmp3.Height)
                                        {
                                            form.fileManager.fileInfoIndex += 1;
                                            form.fileManager.showPages = -1;
                                            changeFlag = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!changeFlag)
                    {
                        // 2つ進む
                        form.fileManager.fileInfoIndex += 2;
                        form.fileManager.showPages = 2;
                        changeFlag = true;
                    }
                }
                if (changeFlag)
                {
                    form.fileManager.filename = form.fileManager.GetFullName();
                    form.filename = form.fileManager.filename;
                    form.ChangeFile();
                }
            }
            else
            {
                if (form.fileManager.fileInfoIndex < (form.fileManager.GetFileInfoListLength() - 1))
                {
                    form.fileManager.fileInfoIndex += 1;
                    form.fileManager.filename = form.fileManager.GetFullName();
                    form.filename = form.fileManager.filename;
                    form.ChangeFile();
                }
            }
        }

        public static void DoPrevFileChangable(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            if (Config.ChangeLeftRight != 0 && (form.ProgramMode == ProgramModeType.Mihiraki || form.ProgramMode == ProgramModeType.Sanmen || form.ProgramMode == ProgramModeType.MihirakiZengo))
            {
                DoNextFile(sender);
            }
            else
            {
                DoPrevFile(sender);
            }
        }

        public static void DoNextFileChangable(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            if (Config.ChangeLeftRight != 0 && (form.ProgramMode == ProgramModeType.Mihiraki || form.ProgramMode == ProgramModeType.Sanmen || form.ProgramMode == ProgramModeType.MihirakiZengo))
            {
                DoPrevFile(sender);
            }
            else
            {
                DoNextFile(sender);
            }
        }

        public static void DoFirstFile(object sender, EventArgs e)
        {
            if (GlobalInfo.FlagInterlocking)
            {
                return;
            }

            Form1 form = GetForm(sender);
            bool changeFlag = false;

            form.fileManager.MakeFileInfoListIfNotExist();

            if (form.ProgramMode == ProgramModeType.Sanmen)
            {
                if (form.fileManager.showPages != 1)
                {
                    form.fileManager.fileInfoIndex = 0;
                    form.fileManager.showPages = 3;
                    changeFlag = true;
                }
                if (changeFlag)
                {
                    form.fileManager.filename = form.fileManager.GetFullName();
                    form.filename = form.fileManager.filename;
                    form.ChangeFile();
                }
            }
            else if (form.ProgramMode == ProgramModeType.Mihiraki)
            {
                if (form.fileManager.fileInfoIndex > 0)
                {
                    form.fileManager.fileInfoIndex = 0;
                    form.fileManager.showPages = 2;
                    changeFlag = true;
                }
                if (changeFlag)
                {
                    form.fileManager.filename = form.fileManager.GetFullName();
                    form.filename = form.fileManager.filename;
                    form.ChangeFile();
                }
            }
            else
            {
                if (form.fileManager.fileInfoIndex > 0)
                {
                    form.fileManager.fileInfoIndex = 0;
                    form.fileManager.filename = form.fileManager.GetFullName();
                    form.filename = form.fileManager.filename;
                    form.ChangeFile();
                }
            }
        }

        public static void DoLastFile(object sender, EventArgs e)
        {
            if (GlobalInfo.FlagInterlocking)
            {
                return;
            }

            Form1 form = GetForm(sender);
            bool changeFlag = false;

            form.fileManager.MakeFileInfoListIfNotExist();

            if (form.ProgramMode == ProgramModeType.Sanmen)
            {
                if (form.fileManager.fileInfoIndex < form.fileManager.GetFileInfoListLength() - 1)
                {
                    form.fileManager.fileInfoIndex = form.fileManager.GetFileInfoListLength() - 1;
                    form.fileManager.showPages = 3;
                    changeFlag = true;
                }
            }
            else if (form.ProgramMode == ProgramModeType.Mihiraki || form.ProgramMode == ProgramModeType.MihirakiZengo)
            {
                if (form.fileManager.fileInfoIndex < form.fileManager.GetFileInfoListLength() - 1)
                {
                    form.fileManager.fileInfoIndex = form.fileManager.GetFileInfoListLength() - 1;
                    form.fileManager.showPages = -1;
                    changeFlag = true;
                }
            }
            else
            {
                if (form.fileManager.fileInfoIndex < (form.fileManager.GetFileInfoListLength() - 1))
                {
                    form.fileManager.fileInfoIndex = form.fileManager.GetFileInfoListLength() - 1;
                    changeFlag = true;
                }
            }
            if (changeFlag)
            {
                if (form.fileManager.fileInfoIndex < form.fileManager.GetFileInfoListLength())
                {
                    form.fileManager.filename = form.fileManager.GetFullName();
                }
                else
                {
                    form.fileManager.filename = null;
                }
                form.filename = form.fileManager.filename;
                form.ChangeFile();
            }
        }

        public static void DoSkipPrevFile(object sender, EventArgs e)
        {
            for (int i = 1; i <= 10; i++)
            {
                DoPrevFile(sender);
            }
        }

        public static void DoSkipNextFile(object sender, EventArgs e)
        {
            for (int i = 1; i <= 10; i++)
            {
                DoNextFile(sender);
            }
        }

        public static void DoIncrementPageNumber(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);
            form.FirstPageNumber += 1;
            form.ChangeFile();
        }

        public static void DoDecrementPageNumber(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);
            form.FirstPageNumber -= 1;
            form.ChangeFile();
        }

        public static void DoClose(object sender, EventArgs e)
        {
            if (GlobalInfo.FlagInterlocking)
            {
                // 1画面目を取得
                Form1 form1 = FormManager.GetFirstForm();

                // 2画面目を取得
                Form1 form1_2 = FormManager.GetSecondForm();
                if (form1_2 != null)
                {
                    FormManager.CloseForm(form1_2);
                }

                if (GlobalInfo.FlagChangeScreen)
                {
                    // 画面を複製する
                    Rectangle[] newSi = new Rectangle[1];
                    newSi[0] = Config.ScreenInfo[0];
                    Config.ScreenInfo = newSi;

                    // 1画面目の横幅を倍にする
                    Config.ScreenInfo[0].Width *= 2;
                    form1.MoveMaximizeWindow(0);

                    GlobalInfo.FlagChangeScreen = false;
                }

                GlobalInfo.FlagInterlocking = false;
            }
            else
            {
                Form1 form = GetForm(sender);

                FormManager.CloseForm(form);
            }
        }

        public static void DoMoveLeftSmall(object sender, EventArgs e)
        {
            Form1 form = (Form1)sender;

            if (form.winMode == WindowMode.Normal)
            {
                form.MoveWindow(-Config.MoveValueSmall, 0);
            }
        }

        public static void DoMoveRightSmall(object sender, EventArgs e)
        {
            Form1 form = (Form1)sender;

            if (form.winMode == WindowMode.Normal)
            {
                form.MoveWindow(Config.MoveValueSmall, 0);
            }
        }

        public static void DoMoveUpSmall(object sender, EventArgs e)
        {
            Form1 form = (Form1)sender;

            if (form.winMode == WindowMode.Normal)
            {
                form.MoveWindow(0, -Config.MoveValueSmall);
            }
        }

        public static void DoMoveDownSmall(object sender, EventArgs e)
        {
            Form1 form = (Form1)sender;

            if (form.winMode == WindowMode.Normal)
            {
                form.MoveWindow(0, Config.MoveValueSmall);
            }
        }

        public static void DoMoveLeftLarge(object sender, EventArgs e)
        {
            Form1 form = (Form1)sender;

            if (form.winMode == WindowMode.Normal)
            {
                form.MoveWindow(-Config.MoveValueLarge, 0);
            }
        }

        public static void DoMoveRightLarge(object sender, EventArgs e)
        {
            Form1 form = (Form1)sender;

            if (form.winMode == WindowMode.Normal)
            {
                form.MoveWindow(Config.MoveValueLarge, 0);
            }
        }

        public static void DoMoveUpLarge(object sender, EventArgs e)
        {
            Form1 form = (Form1)sender;

            if (form.winMode == WindowMode.Normal)
            {
                form.MoveWindow(0, -Config.MoveValueLarge);
            }
        }

        public static void DoMoveDownLarge(object sender, EventArgs e)
        {
            Form1 form = (Form1)sender;

            if (form.winMode == WindowMode.Normal)
            {
                form.MoveWindow(0, Config.MoveValueLarge);
            }
        }

        public static void DoMoveLeftScreen(object sender, EventArgs e)
        {
            if (GlobalInfo.FlagInterlocking)
            {
                return;
            }

            Form1 form = GetForm(sender);

            if (form.winMode == WindowMode.Maximize)
            {
                int nextScreenNumber = Config.LeftList[Config.GetScreenNumber(form.Location)];
                form.MoveMaximizeWindow(nextScreenNumber);
            }
        }

        public static void DoMoveRightScreen(object sender, EventArgs e)
        {
            if (GlobalInfo.FlagInterlocking)
            {
                return;
            }

            Form1 form = GetForm(sender);

            if (form.winMode == WindowMode.Maximize)
            {
                int nextScreenNumber = Config.RightList[Config.GetScreenNumber(form.Location)];
                form.MoveMaximizeWindow(nextScreenNumber);
            }
        }

        public static void DoMoveUpScreen(object sender, EventArgs e)
        {
            if (GlobalInfo.FlagInterlocking)
            {
                return;
            }

            Form1 form = GetForm(sender);

            if (form.winMode == WindowMode.Maximize)
            {
                int nextScreenNumber = Config.UpList[Config.GetScreenNumber(form.Location)];
                form.MoveMaximizeWindow(nextScreenNumber);
            }
        }

        public static void DoMoveDownScreen(object sender, EventArgs e)
        {
            if (GlobalInfo.FlagInterlocking)
            {
                return;
            }

            Form1 form = GetForm(sender);

            if (form.winMode == WindowMode.Maximize)
            {
                int nextScreenNumber = Config.DownList[Config.GetScreenNumber(form.Location)];
                form.MoveMaximizeWindow(nextScreenNumber);
            }
        }

        public static void DoNoCopyFile(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            MoveAfterCopy(form);
        }

        public static void DoNoCopyFileReverse(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            MoveAfterCopyReverse(form);
        }

        public static void DoCopyFile1(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            CopyToDestination(form, Config.destDir1);
        }

        public static void DoCopyFile2(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            CopyToDestination(form, Config.destDir2);
        }

        public static void DoCopyFile3(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            CopyToDestination(form, Config.destDir3);
        }

        public static void DoCopyFile4(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            CopyToDestination(form, Config.destDir4);
        }

        public static void DoCopyFile5(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            CopyToDestination(form, Config.destDir5);
        }

        public static void DoCopyFile6(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            CopyToDestination(form, Config.destDir6);
        }

        public static void DoCopyFile7(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            CopyToDestination(form, Config.destDir7);
        }

        public static void DoCopyFile8(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            CopyToDestination(form, Config.destDir8);
        }

        public static void DoCopyFile9(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            CopyToDestination(form, Config.destDir9);
        }

        private static void CopyToDestination(Form1 form, string destDir)
        {
            form.fileManager.CopyToDestination(destDir);
            MoveAfterCopy(form);
        }

        private static void MoveAfterCopy(Form1 form)
        {
            if (Config.MoveAfterCopy < 0)
            {
                DoPrevFile(form);
            }
            else if (Config.MoveAfterCopy > 0)
            {
                DoNextFile(form);
            }
        }

        private static void MoveAfterCopyReverse(Form1 form)
        {
            if (Config.MoveAfterCopy < 0)
            {
                DoNextFile(form);
            }
            else if (Config.MoveAfterCopy > 0)
            {
                DoPrevFile(form);
            }
        }

        public static void DoSortModeFilenameAsc(object sender, EventArgs e)
        {
            if (GlobalInfo.FlagInterlocking)
            {
                return;
            }

            Form1 form = GetForm(sender);

            if (form.FlagZipFile)
            {
                form.SetMessage("zipファイルに対しては指定できません。");
                return;
            }

            form.SortMode = SortModeType.FilenameAsc;
            form.fileManager.MakeFileInfoList();
            form.ChangeFile();
        }

        public static void DoSortModeFilenameDesc(object sender, EventArgs e)
        {
            if (GlobalInfo.FlagInterlocking)
            {
                return;
            }

            Form1 form = GetForm(sender);

            if (form.FlagZipFile)
            {
                form.SetMessage("zipファイルに対しては指定できません。");
                return;
            }

            form.SortMode = SortModeType.FilenameDesc;
            form.fileManager.MakeFileInfoList();
            form.ChangeFile();
        }

        public static void DoSortModeTimeAsc(object sender, EventArgs e)
        {
            if (GlobalInfo.FlagInterlocking)
            {
                return;
            }

            Form1 form = GetForm(sender);

            if (form.FlagZipFile)
            {
                form.SetMessage("zipファイルに対しては指定できません。");
                return;
            }

            form.SortMode = SortModeType.TimeAsc;
            form.fileManager.MakeFileInfoList();
            form.ChangeFile();
        }

        public static void DoSortModeTimeDesc(object sender, EventArgs e)
        {
            if (GlobalInfo.FlagInterlocking)
            {
                return;
            }

            Form1 form = GetForm(sender);

            if (form.FlagZipFile)
            {
                form.SetMessage("zipファイルに対しては指定できません。");
                return;
            }

            form.SortMode = SortModeType.TimeDesc;
            form.fileManager.MakeFileInfoList();
            form.ChangeFile();
        }

        public static void DoCopyFullName(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            form.fileManager.CopyFullName();
        }

        public static void DoCopyFile(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            form.fileManager.CopyFile();
        }

        public static void DoSelectExplorer(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            form.fileManager.SelectExplorer();
        }

        public static void DoExecCommand1(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            form.fileManager.ExecCommand1();

            if (Config.ClearAfterExecCommand1 != 0)
            {
                ImageManager.Clear();

                if (GlobalInfo.FlagInterlocking)
                {
                    Form1 otherForm = FormManager.GetOtherForm(form);
                    otherForm.fileManager.MakeFileInfoListIfNotExist();
                    otherForm.ReloadFile();
                }
                form.ReloadFile();
            }
        }

        public static void DoExecCommand2(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            form.fileManager.ExecCommand2();

            if (Config.ClearAfterExecCommand2 != 0)
            {
                ImageManager.Clear();

                if (GlobalInfo.FlagInterlocking)
                {
                    Form1 otherForm = FormManager.GetOtherForm(form);
                    otherForm.fileManager.MakeFileInfoListIfNotExist();
                    otherForm.ReloadFile();
                }
                form.ReloadFile();
            }
        }

        public static void DoExecCommand3(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            form.fileManager.ExecCommand3();

            if (Config.ClearAfterExecCommand3 != 0)
            {
                ImageManager.Clear();

                if (GlobalInfo.FlagInterlocking)
                {
                    Form1 otherForm = FormManager.GetOtherForm(form);
                    otherForm.fileManager.MakeFileInfoListIfNotExist();
                    otherForm.ReloadFile();
                }
                form.ReloadFile();
            }
        }

        public static void DoExecCommand4(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            form.fileManager.ExecCommand4();

            if (Config.ClearAfterExecCommand4 != 0)
            {
                ImageManager.Clear();

                if (GlobalInfo.FlagInterlocking)
                {
                    Form1 otherForm = FormManager.GetOtherForm(form);
                    otherForm.fileManager.MakeFileInfoListIfNotExist();
                    otherForm.ReloadFile();
                }
                form.ReloadFile();
            }
        }

        public static void DoExecCommand5(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            form.fileManager.ExecCommand5();

            if (Config.ClearAfterExecCommand5 != 0)
            {
                ImageManager.Clear();

                if (GlobalInfo.FlagInterlocking)
                {
                    Form1 otherForm = FormManager.GetOtherForm(form);
                    otherForm.fileManager.MakeFileInfoListIfNotExist();
                    otherForm.ReloadFile();
                }
                form.ReloadFile();
            }
        }

        public static void DoReload(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            if (GlobalInfo.FlagInterlocking)
            {
                Form1 otherForm = FormManager.GetOtherForm(form);
                otherForm.fileManager.MakeFileInfoListIfNotExist();
                DoReload2(otherForm);
            }
            DoReload2(form);
        }

        private static void DoReload2(Form1 form)
        {
            form.fileManager.MakeFileInfoListIfNotExist();

            form.fileManager.filename = form.fileManager.GetFullName();
            form.ReloadFile();
        }

        public static void DoSinglePage(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            if (GlobalInfo.FlagInterlocking)
            {
                Form1 otherForm = FormManager.GetOtherForm(form);
                otherForm.fileManager.MakeFileInfoListIfNotExist();
                DoSinglePage2(otherForm);
            }
            DoSinglePage2(form);
        }

        private static void DoSinglePage2(Form1 form)
        {
            form.ProgramMode = ProgramModeType.OneWindow;

            form.MakeMenu();
            form.ChangeFile();
        }

        public static void DoTwoPages(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            if (GlobalInfo.FlagInterlocking)
            {
                Form1 otherForm = FormManager.GetOtherForm(form);
                otherForm.fileManager.MakeFileInfoListIfNotExist();
                DoTwoPages2(otherForm);
            }
            DoTwoPages2(form);
        }

        private static void DoTwoPages2(Form1 form)
        {
            if (form.ProgramMode == ProgramModeType.Mihiraki)
            {
                form.ProgramMode = ProgramModeType.OneWindow;
            }
            else
            {
                form.ProgramMode = ProgramModeType.Mihiraki;
            }

            form.MakeMenu();
            form.ChangeFile();
        }

        public static void DoInterlock(object sender, EventArgs e)
        {
            if (GlobalInfo.FlagInterlocking)
            {
                DoClose(sender, e);
                return;
            }

            Form1 form = GetForm(sender);

            GlobalInfo.FlagInterlocking = true;

            // 2つ目のForm1を作成する
            Form1 form1_2 = FormManager.GetNewForm();
            form1_2.init();

            if (form.winMode == WindowMode.Maximize)
            {
                // 2画面以上ある場合かつコンフィグが-2以外は、今の画面以外またはコンフィグで指定した画面に表示する
                if (Config.numberOfScreen >= 2 && Config.InterlockScreenNumber != -2)
                {
                    if (Config.InterlockScreenNumber < 0)
                    {
                        // コンフィグが-1(0未満)なら、今の画面以外に表示する(今の画面が0なら1、1なら0にする)
                        int newScreenNumber = 0;
                        if (Config.GetScreenNumber(form.Location) == 0)
                        {
                            newScreenNumber = 1;
                        }
                        form1_2.InitialScreenNumber = newScreenNumber;
                    }
                    else
                    {
                        // コンフィグで指定した画面に表示する
                        form1_2.InitialScreenNumber = Config.InterlockScreenNumber;
                    }
                }
                else
                {
                    // 1画面またはコンフィグが-2の場合は、画面分割する

                    // 画面を複製する
                    Rectangle[] newSi = new Rectangle[2];
                    newSi[0] = Config.ScreenInfo[0];
                    newSi[1] = Config.ScreenInfo[0];
                    Config.ScreenInfo = newSi;

                    // 1画面目の横幅を半分にする
                    Config.ScreenInfo[0].Width /= 2;
                    form.MoveMaximizeWindow(0);

                    // 2画面目を横幅半分で1画面目の右にする
                    Config.ScreenInfo[1].X = Config.ScreenInfo[0].Width;
                    Config.ScreenInfo[1].Width /= 2;

                    // 2つ目のForm1を2画面目にする
                    form1_2.InitialScreenNumber = 1;

                    GlobalInfo.FlagChangeScreen = true;
                }
            }

            // 元のモード(1画面、見開き、3画面)、ページ番号表示、ボーダー表示を引き継ぐ
            form1_2.ProgramMode = form.ProgramMode;
            form1_2.ShowPageNumber = form.ShowPageNumber;
            form1_2.ShowBorder = form.ShowBorder;

            // 連動フォルダを表示する
            string filename = null;
            form1_2.fileManager.filename = null;
            form1_2.fileManager.dirname = Config.InterlockFolder;
            form1_2.fileManager.MakeFileInfoList();
            filename = form1_2.fileManager.GetFileInfoListFullName(0);
            form1_2.fileManager.filename = filename;
            form1_2.filename = filename;
            if (filename != null)
            {
                FileInfo fi = new FileInfo(filename);
                form1_2.fileManager.dirname = fi.DirectoryName;
            }

            form1_2.Show();
        }

        public static void DoMoveFiles(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            if (form.FlagZipFile)
            {
                form.SetMessage("zipファイルに対しての移動はできません。");
                return;
            }

            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "現在のフォルダの画像ファイルの移動先フォルダを選択して下さい。";
                fbd.SelectedPath = form.fileManager.dirname;
                fbd.ShowNewFolderButton = true;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string destDir = fbd.SelectedPath;

                    if (String.Compare(destDir, form.fileManager.dirname, true) == 0)
                    {
                        form.SetMessage("指定されたフォルダは現在のフォルダのため、移動を行いません。");
                    }
                    else
                    {
                        // 監視モードの場合には監視を停止する(ファイル移動中に表示させないため)
                        if (form.WatchMode)
                        {
                            form.fileWatcher.Stop();
                        }

                        // 複数ファイル移動
                        bool result = form.fileManager.MoveFiles(destDir);

                        // 監視モードの場合には監視を再開する
                        if (form.WatchMode)
                        {
                            form.fileWatcher.Start();
                        }

                        if (result)
                        {
                            // 読み込み済みデータクリア
                            ImageManager.Clear();

                            // ディレクトリ表示として初期化する
                            form.fileManager.filename = null;
                            form.fileManager.MakeFileInfoList();
                            string filename = form.fileManager.GetFileInfoListFullName(0);
                            form.fileManager.filename = filename;
                            form.filename = filename;
                            if (filename != null)
                            {
                                FileInfo fi = new FileInfo(filename);
                                form.fileManager.dirname = fi.DirectoryName;
                            }
                            form.ChangeFile();

                            // 複数ファイル移動後コマンド実行
                            form.fileManager.ExecCommandAfterMoveFiles(destDir);
                        }
                    }
                }
            }
        }

        public static void DoRenamePage(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            if (form.FlagZipFile)
            {
                form.SetMessage("zipファイルに対しての一括リネームはできません。");
                return;
            }

            form.fileManager.MakeFileInfoListIfNotExist();

            string msg = String.Format(
                "'{0}'内の全ての画像ファイルを、ページ番号に合わせて拡張子以外の部分を{1:D3}～{2:D3}にリネームします。よろしいですか？",
                form.fileManager.dirname,
                form.FirstPageNumber,
                form.FirstPageNumber + form.fileManager.fileInfoList.Length - 1);

            DialogResult result = MessageBox.Show(
                msg,
                "確認",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                form.fileManager.RenamePage();
            }
        }

        public static void DoShowPage(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            if (GlobalInfo.FlagInterlocking)
            {
                Form1 otherForm = FormManager.GetOtherForm(form);
                DoShowPage2(otherForm);
            }
            DoShowPage2(form);
        }

        private static void DoShowPage2(Form1 form)
        {
            form.ShowPageNumber = !form.ShowPageNumber;
            form.ChangeFile();
        }

        public static void DoThreePages(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            if (GlobalInfo.FlagInterlocking)
            {
                Form1 otherForm = FormManager.GetOtherForm(form);
                otherForm.fileManager.MakeFileInfoListIfNotExist();
                DoThreePages2(otherForm);
            }
            DoThreePages2(form);
        }

        private static void DoThreePages2(Form1 form)
        {
            if (form.ProgramMode == ProgramModeType.Sanmen)
            {
                form.ProgramMode = ProgramModeType.OneWindow;
            }
            else
            {
                form.ProgramMode = ProgramModeType.Sanmen;
                form.fileManager.InitSanmen();
                form.filename = form.fileManager.filename;
            }

            form.MakeMenu();
            form.ChangeFile();
        }

        public static void DoWatchMode(object sender, EventArgs e)
        {
            Form1 form = GlobalInfo.FlagInterlocking ? FormManager.GetFirstForm() : GetForm(sender);

            if (form.FlagZipFile)
            {
                form.SetMessage("zipファイルに対して監視はできません。");
                return;
            }

            form.WatchMode = !form.WatchMode;
            if (form.WatchMode)
            {
                form.fileManager.SetLatestFile();
                form.fileWatcher.dir = form.fileManager.dirname;
                form.fileWatcher.Start();
                form.SetMessage("'" + form.fileManager.dirname + "'の監視を開始します。");
            }
            else
            {
                form.fileWatcher.Stop();
                form.SetMessage("'" + form.fileManager.dirname + "'の監視を終了します。");
            }
        }

        public static void DoOpenFile(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.FileName = form.fileManager.filename;
                ofd.InitialDirectory = form.fileManager.dirname;
                ofd.Filter = "画像ファイル・アーカイブファイル(*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.zip)|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.zip";
                ofd.FilterIndex = 1;
                ofd.Title = "表示するファイルを選択して下さい。監視中の場合には監視を終了します。";
                ofd.RestoreDirectory = true;
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string newFilename = ofd.FileName;

                    // 監視モードの場合には監視を停止する。新しいディレクトリは監視しない。
                    if (form.WatchMode)
                    {
                        form.fileWatcher.Stop();
                        form.WatchMode = false;
                    }

                    // 読み込み済みデータクリア
                    ImageManager.Clear();

                    string ext = Path.GetExtension(newFilename);
                    if (String.Compare(ext, ".zip", true) == 0)
                    {
                        // 引数がzipファイル
                        form.FlagZipFile = true;
                        form.Zip = ZipFile.Open(newFilename, ZipArchiveMode.Read);
                        ZipArchiveEntry entry = form.Zip.Entries[0];

                        form.fileManager.filename = entry.FullName;
                        form.fileManager.dirname = null;
                        form.filename = entry.FullName;
                    }
                    else
                    {
                        // 引数が画像ファイル
                        // ファイル表示として初期化する
                        form.fileManager.filename = newFilename;
                        form.filename = newFilename;
                        FileInfo fi = new FileInfo(newFilename);
                        form.fileManager.dirname = fi.DirectoryName;
                        form.fileManager.fileInfoList = null;
                    }
                    form.ChangeFile();
                }
            }

        }

        public static void DoOpenDirectory(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "表示するフォルダを選択して下さい。監視中の場合には監視を終了します。";
                fbd.SelectedPath = form.fileManager.dirname;
                fbd.ShowNewFolderButton = true;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string newDir = fbd.SelectedPath;

                    // 監視モードの場合には監視を停止する。新しいディレクトリは監視しない。
                    if (form.WatchMode)
                    {
                        form.fileWatcher.Stop();
                        form.WatchMode = false;
                    }

                    // 読み込み済みデータクリア
                    ImageManager.Clear();

                    // ディレクトリ表示として初期化する
                    form.fileManager.filename = null;
                    form.fileManager.dirname = newDir;
                    form.fileManager.MakeFileInfoList();
                    string filename = form.fileManager.GetFileInfoListFullName(0);
                    form.fileManager.filename = filename;
                    form.filename = filename;
                    if (filename != null)
                    {
                        FileInfo fi = new FileInfo(filename);
                        form.fileManager.dirname = fi.DirectoryName;
                    }
                    form.ChangeFile();
                }
            }
        }

        public static void DoChangeFocus(object sender, EventArgs e)
        {
            if (!GlobalInfo.FlagInterlocking)
            {
                return;
            }

            Form1 form = GetForm(sender);
            Form1 otherForm = FormManager.GetOtherForm(form);
            otherForm.Activate();
        }

        public static void DoFrontBack(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            if (GlobalInfo.FlagInterlocking)
            {
                Form1 otherForm = FormManager.GetOtherForm(form);
                otherForm.fileManager.MakeFileInfoListIfNotExist();
                DoFrontBack2(otherForm);
            }
            DoFrontBack2(form);
        }

        private static void DoFrontBack2(Form1 form)
        {
            if (form.ProgramMode == ProgramModeType.Zengo)
            {
                form.ProgramMode = ProgramModeType.OneWindow;
            }
            else
            {
                form.ProgramMode = ProgramModeType.Zengo;
            }

            form.MakeMenu();
            form.ChangeFile();
        }

        public static void DoShowBorder(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            if (GlobalInfo.FlagInterlocking)
            {
                Form1 otherForm = FormManager.GetOtherForm(form);
                DoShowBorder2(otherForm);
            }
            DoShowBorder2(form);
        }

        private static void DoShowBorder2(Form1 form)
        {
            form.ShowBorder = !form.ShowBorder;
            form.ChangeFile();
        }

        public static void DoChangeLeftRight(object sender, EventArgs e)
        {
            Config.ChangeLeftRight = Config.ChangeLeftRight != 0 ? 0 : 1;

            Form1 form = GetForm(sender);

            if (GlobalInfo.FlagInterlocking)
            {
                Form1 otherForm = FormManager.GetOtherForm(form);
                DoChangeLeftRight2(otherForm);
            }
            DoChangeLeftRight2(form);
        }

        private static void DoChangeLeftRight2(Form1 form)
        {
            form.MakeMenu();
        }

        public static void DoTwoPagesFrontBack(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            if (GlobalInfo.FlagInterlocking)
            {
                Form1 otherForm = FormManager.GetOtherForm(form);
                otherForm.fileManager.MakeFileInfoListIfNotExist();
                DoTwoPagesFrontBack2(otherForm);
            }
            DoTwoPagesFrontBack2(form);
        }

        private static void DoTwoPagesFrontBack2(Form1 form)
        {
            if (form.ProgramMode == ProgramModeType.MihirakiZengo)
            {
                form.ProgramMode = ProgramModeType.OneWindow;
            }
            else
            {
                form.ProgramMode = ProgramModeType.MihirakiZengo;
            }

            form.MakeMenu();
            form.ChangeFile();
        }

        public static void DoConfig(object sender, EventArgs e)
        {
            Form1 form = GetForm(sender);

            ConfigForm configForm = new ConfigForm();
            configForm.ShowDialog();
        }

        private static Form1 GetForm(Object sender)
        {
            Form1 form = null;
            if (sender is ToolStripMenuItem tsmi)
            {
                ContextMenuStrip cms = null;
                // サブメニュー一段だけ対応
                if (tsmi.Owner is ToolStripDropDownMenu tsddm)
                {
                    if (tsddm.OwnerItem == null)
                    {
                        cms = (ContextMenuStrip)tsmi.Owner;
                    }
                    else
                    {
                        cms = (ContextMenuStrip)tsddm.OwnerItem.Owner;
                    }
                }
                Control c = cms.SourceControl;
                form = (Form1)c.FindForm();
            }
            else
            {
                form = (Form1)sender;
            }

            return form;
        }

    }
}
