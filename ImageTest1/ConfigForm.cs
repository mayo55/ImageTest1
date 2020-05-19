using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageTest1
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            MinimizeBox = false;
            KeyPreview = true;

            dataGridView1.ColumnCount = 3;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns[0].Name = "key";
            dataGridView1.Columns[0].HeaderText = "項目";
            dataGridView1.Columns[0].ReadOnly = true;

            dataGridView1.Columns[1].Name = "value";
            dataGridView1.Columns[1].HeaderText = "値";

            dataGridView1.Columns[2].Name = "id";
            dataGridView1.Columns[2].Visible = false;

            dataGridView1.Rows.Add(Constant.MSG_KEY_CONFIG, null, null);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGreen;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;

            dataGridView1.Rows.Add(Constant.MSG_DO_ALL, null, null);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightCyan;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;

            dataGridView1.Rows.Add(Constant.MSG_DO_OPEN_FILE, Properties.Settings.Default.DoOpenFile, Constant.ID_DO_OPEN_FILE);
            dataGridView1.Rows.Add(Constant.MSG_DO_OPEN_DIRECTORY, Properties.Settings.Default.DoOpenDirectory, Constant.ID_DO_OPEN_DIRECTORY);
            dataGridView1.Rows.Add(Constant.MSG_DO_CONFIG, Properties.Settings.Default.DoConfig, Constant.ID_DO_CONFIG);
            dataGridView1.Rows.Add(Constant.MSG_DO_CLOSE, Properties.Settings.Default.DoClose, Constant.ID_DO_CLOSE);

            dataGridView1.Rows.Add(Constant.MSG_DO_MOVE, null, null);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightCyan;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;

            dataGridView1.Rows.Add(Constant.MSG_DO_PREV_FILE, Properties.Settings.Default.DoPrevFile, Constant.ID_DO_PREV_FILE);
            dataGridView1.Rows.Add(Constant.MSG_DO_NEXT_FILE, Properties.Settings.Default.DoNextFile, Constant.ID_DO_NEXT_FILE);
            dataGridView1.Rows.Add(Constant.MSG_DO_PREV_FILE_CHANGABLE, Properties.Settings.Default.DoPrevFileChangable, Constant.ID_DO_PREV_FILE_CHANGABLE);
            dataGridView1.Rows.Add(Constant.MSG_DO_NEXT_FILE_CHANGABLE, Properties.Settings.Default.DoNextFileChangable, Constant.ID_DO_NEXT_FILE_CHANGABLE);
            dataGridView1.Rows.Add(Constant.MSG_DO_SKIP_PREV_FILE, Properties.Settings.Default.DoSkipPrevFile, Constant.ID_DO_SKIP_PREV_FILE);
            dataGridView1.Rows.Add(Constant.MSG_DO_SKIP_NEXT_FILE, Properties.Settings.Default.DoSkipNextFile, Constant.ID_DO_SKIP_NEXT_FILE);
            dataGridView1.Rows.Add(Constant.MSG_DO_FIRST_FILE, Properties.Settings.Default.DoFirstFile, Constant.ID_DO_FIRST_FILE);
            dataGridView1.Rows.Add(Constant.MSG_DO_LAST_FILE, Properties.Settings.Default.DoLastFile, Constant.ID_DO_LAST_FILE);

            dataGridView1.Rows.Add(Constant.MSG_DO_IMAGE, null, null);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightCyan;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;

            dataGridView1.Rows.Add(Constant.MSG_DO_RELOAD, Properties.Settings.Default.DoReload, Constant.ID_DO_RELOAD);
            dataGridView1.Rows.Add(Constant.MSG_DO_MAXIMIZE, Properties.Settings.Default.DoMaximize, Constant.ID_DO_MAXIMIZE);
            dataGridView1.Rows.Add(Constant.MSG_DO_MAXIMIZE_MODE_CHANGE, Properties.Settings.Default.DoMaximizeModeChange, Constant.ID_DO_MAXIMIZE_MODE_CHANGE);
            dataGridView1.Rows.Add(Constant.MSG_DO_ROTATE_LEFT, Properties.Settings.Default.DoRotateLeft, Constant.ID_DO_ROTATE_LEFT);
            dataGridView1.Rows.Add(Constant.MSG_DO_ROTATE_RIGHT, Properties.Settings.Default.DoRotateRight, Constant.ID_DO_ROTATE_RIGHT);
            dataGridView1.Rows.Add(Constant.MSG_DO_EXPANSION, Properties.Settings.Default.DoExpansion, Constant.ID_DO_EXPANSION);
            dataGridView1.Rows.Add(Constant.MSG_DO_REDUCE, Properties.Settings.Default.DoReduce, Constant.ID_DO_REDUCE);

            dataGridView1.Rows.Add(Constant.MSG_DO_SCREEN, null, null);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightCyan;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;

            dataGridView1.Rows.Add(Constant.MSG_DO_MOVE_UP_SCREEN, Properties.Settings.Default.DoMoveUpScreen, Constant.ID_DO_MOVE_UP_SCREEN);
            dataGridView1.Rows.Add(Constant.MSG_DO_MOVE_DOWN_SCREEN, Properties.Settings.Default.DoMoveDownScreen, Constant.ID_DO_MOVE_DOWN_SCREEN);
            dataGridView1.Rows.Add(Constant.MSG_DO_MOVE_LEFT_SCREEN, Properties.Settings.Default.DoMoveLeftScreen, Constant.ID_DO_MOVE_LEFT_SCREEN);
            dataGridView1.Rows.Add(Constant.MSG_DO_MOVE_RIGHT_SCREEN, Properties.Settings.Default.DoMoveRightScreen, Constant.ID_DO_MOVE_RIGHT_SCREEN);

            dataGridView1.Rows.Add(Constant.MSG_DO_COPY, null, null);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightCyan;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;

            dataGridView1.Rows.Add(Constant.MSG_DO_NO_COPY_FILE, Properties.Settings.Default.DoNoCopyFile, Constant.ID_DO_NO_COPY_FILE);
            dataGridView1.Rows.Add(Constant.MSG_DO_NO_COPY_FILE_REVERSE, Properties.Settings.Default.DoNoCopyFileReverse, Constant.ID_DO_NO_COPY_FILE_REVERSE);
            dataGridView1.Rows.Add(Constant.MSG_DO_COPY_FILE_1, Properties.Settings.Default.DoCopyFile1, Constant.ID_DO_COPY_FILE_1);
            dataGridView1.Rows.Add(Constant.MSG_DO_COPY_FILE_2, Properties.Settings.Default.DoCopyFile2, Constant.ID_DO_COPY_FILE_2);
            dataGridView1.Rows.Add(Constant.MSG_DO_COPY_FILE_3, Properties.Settings.Default.DoCopyFile3, Constant.ID_DO_COPY_FILE_3);
            dataGridView1.Rows.Add(Constant.MSG_DO_COPY_FILE_4, Properties.Settings.Default.DoCopyFile4, Constant.ID_DO_COPY_FILE_4);
            dataGridView1.Rows.Add(Constant.MSG_DO_COPY_FILE_5, Properties.Settings.Default.DoCopyFile5, Constant.ID_DO_COPY_FILE_5);
            dataGridView1.Rows.Add(Constant.MSG_DO_COPY_FILE_6, Properties.Settings.Default.DoCopyFile6, Constant.ID_DO_COPY_FILE_6);
            dataGridView1.Rows.Add(Constant.MSG_DO_COPY_FILE_7, Properties.Settings.Default.DoCopyFile7, Constant.ID_DO_COPY_FILE_7);
            dataGridView1.Rows.Add(Constant.MSG_DO_COPY_FILE_8, Properties.Settings.Default.DoCopyFile8, Constant.ID_DO_COPY_FILE_8);
            dataGridView1.Rows.Add(Constant.MSG_DO_COPY_FILE_9, Properties.Settings.Default.DoCopyFile9, Constant.ID_DO_COPY_FILE_9);

            dataGridView1.Rows.Add(Constant.MSG_DO_SORT, null, null);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightCyan;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;

            dataGridView1.Rows.Add(Constant.MSG_DO_SORT_MODE_FILENAME_ASC, Properties.Settings.Default.DoSortModeFilenameAsc, Constant.ID_DO_SORT_MODE_FILENAME_ASC);
            dataGridView1.Rows.Add(Constant.MSG_DO_SORT_MODE_FILENAME_DESC, Properties.Settings.Default.DoSortModeFilenameDesc, Constant.ID_DO_SORT_MODE_FILENAME_DESC);
            dataGridView1.Rows.Add(Constant.MSG_DO_SORT_MODE_TIME_ASC, Properties.Settings.Default.DoSortModeTimeAsc, Constant.ID_DO_SORT_MODE_TIME_ASC);
            dataGridView1.Rows.Add(Constant.MSG_DO_SORT_MODE_TIME_DESC, Properties.Settings.Default.DoSortModeTimeDesc, Constant.ID_DO_SORT_MODE_TIME_DESC);

            dataGridView1.Rows.Add(Constant.MSG_DO_EDIT, null, null);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightCyan;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;

            dataGridView1.Rows.Add(Constant.MSG_DO_COPY_FULL_NAME, Properties.Settings.Default.DoCopyFullName, Constant.ID_DO_COPY_FULL_NAME);
            dataGridView1.Rows.Add(Constant.MSG_DO_COPY_FILE, Properties.Settings.Default.DoCopyFile, Constant.ID_DO_COPY_FILE);
            dataGridView1.Rows.Add(Constant.MSG_DO_INCREMENT_PAGE_NUMBER, Properties.Settings.Default.DoIncrementPageNumber, Constant.ID_DO_INCREMENT_PAGE_NUMBER);
            dataGridView1.Rows.Add(Constant.MSG_DO_DECREMENT_PAGE_NUMBER, Properties.Settings.Default.DoDecrementPageNumber, Constant.ID_DO_DECREMENT_PAGE_NUMBER);

            dataGridView1.Rows.Add(Constant.MSG_DO_COMMAND, null, null);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightCyan;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;

            dataGridView1.Rows.Add(Constant.MSG_DO_SELECT_EXPLORER, Properties.Settings.Default.DoSelectExplorer, Constant.ID_DO_SELECT_EXPLORER);
            dataGridView1.Rows.Add(Constant.MSG_DO_EXEC_COMMAND_1, Properties.Settings.Default.DoExecCommand1, Constant.ID_DO_EXEC_COMMAND_1);
            dataGridView1.Rows.Add(Constant.MSG_DO_EXEC_COMMAND_2, Properties.Settings.Default.DoExecCommand2, Constant.ID_DO_EXEC_COMMAND_2);
            dataGridView1.Rows.Add(Constant.MSG_DO_EXEC_COMMAND_3, Properties.Settings.Default.DoExecCommand3, Constant.ID_DO_EXEC_COMMAND_3);
            dataGridView1.Rows.Add(Constant.MSG_DO_EXEC_COMMAND_4, Properties.Settings.Default.DoExecCommand4, Constant.ID_DO_EXEC_COMMAND_4);
            dataGridView1.Rows.Add(Constant.MSG_DO_EXEC_COMMAND_5, Properties.Settings.Default.DoExecCommand5, Constant.ID_DO_EXEC_COMMAND_5);
            dataGridView1.Rows.Add(Constant.MSG_DO_MOVE_FILES, Properties.Settings.Default.DoMoveFiles, Constant.ID_DO_MOVE_FILES);
            dataGridView1.Rows.Add(Constant.MSG_DO_RENAME_PAGE, Properties.Settings.Default.DoRenamePage, Constant.ID_DO_RENAME_PAGE);

            dataGridView1.Rows.Add(Constant.MSG_DO_MODE, null, null);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightCyan;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;

            dataGridView1.Rows.Add(Constant.MSG_DO_SINGLE_PAGE, Properties.Settings.Default.DoSinglePage, Constant.ID_DO_SINGLE_PAGE);
            dataGridView1.Rows.Add(Constant.MSG_DO_TWO_PAGES, Properties.Settings.Default.DoTwoPages, Constant.ID_DO_TWO_PAGES);
            dataGridView1.Rows.Add(Constant.MSG_DO_THREE_PAGES, Properties.Settings.Default.DoThreePages, Constant.ID_DO_THREE_PAGES);
            dataGridView1.Rows.Add(Constant.MSG_DO_FRONT_BACK, Properties.Settings.Default.DoFrontBack, Constant.ID_DO_FRONT_BACK);
            dataGridView1.Rows.Add(Constant.MSG_DO_TWO_PAGES_FRONT_BACK, Properties.Settings.Default.DoTwoPagesFrontBack, Constant.ID_DO_TWO_PAGES_FRONT_BACK);
            dataGridView1.Rows.Add(Constant.MSG_DO_INTERLOCK, Properties.Settings.Default.DoInterlock, Constant.ID_DO_INTERLOCK);
            dataGridView1.Rows.Add(Constant.MSG_DO_WATCH_MODE, Properties.Settings.Default.DoWatchMode, Constant.ID_DO_WATCH_MODE);
            dataGridView1.Rows.Add(Constant.MSG_DO_CHANGE_FOCUS, Properties.Settings.Default.DoChangeFocus, Constant.ID_DO_CHANGE_FOCUS);
            dataGridView1.Rows.Add(Constant.MSG_DO_CHANGE_LEFT_RIGHT, Properties.Settings.Default.DoChangeLeftRight, Constant.ID_DO_CHANGE_LEFT_RIGHT);


            dataGridView1.Rows.Add(Constant.MSG_CONFIG, null, null);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGreen;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;

            dataGridView1.Rows.Add(Constant.MSG_SCREEN_INFO_AUTO, Properties.Settings.Default.ScreenInfoAuto, Constant.ID_SCREEN_INFO_AUTO);
            dataGridView1.Rows.Add(Constant.MSG_NUMBER_OF_SCREEN, Properties.Settings.Default.NumberOfScreen, Constant.ID_NUMBER_OF_SCREEN);
            dataGridView1.Rows.Add(Constant.MSG_SCREEN_INFOMATIONS, Properties.Settings.Default.ScreenInfomations, Constant.ID_SCREEN_INFOMATIONS);
            dataGridView1.Rows.Add(Constant.MSG_LEFT_DESTINATIONS, Properties.Settings.Default.LeftDestinations, Constant.ID_LEFT_DESTINATIONS);
            dataGridView1.Rows.Add(Constant.MSG_RIGHT_DESTINATIONS, Properties.Settings.Default.RightDestinations, Constant.ID_RIGHT_DESTINATIONS);
            dataGridView1.Rows.Add(Constant.MSG_UP_DESTINATIONS, Properties.Settings.Default.UpDestinations, Constant.ID_UP_DESTINATIONS);
            dataGridView1.Rows.Add(Constant.MSG_DOWN_DESTINATIONS, Properties.Settings.Default.DownDestinations, Constant.ID_DOWN_DESTINATIONS);
            dataGridView1.Rows.Add(Constant.MSG_RESIZE_LIST, Properties.Settings.Default.ResizeList, Constant.ID_RESIZE_LIST);
            dataGridView1.Rows.Add(Constant.MSG_HORIZONTALLY_LONG_SCREEN, Properties.Settings.Default.HorizontallyLongScreen, Constant.ID_HORIZONTALLY_LONG_SCREEN);
            dataGridView1.Rows.Add(Constant.MSG_VERTICALLY_LONG_SCREEN, Properties.Settings.Default.VerticallyLongScreen, Constant.ID_VERTICALLY_LONG_SCREEN);
            dataGridView1.Rows.Add(Constant.MSG_MAXIMIZE, Properties.Settings.Default.Maximize, Constant.ID_MAXIMIZE);
            dataGridView1.Rows.Add(Constant.MSG_MOVE_VALUE_SMALL, Properties.Settings.Default.MoveValueSmall, Constant.ID_MOVE_VALUE_SMALL);
            dataGridView1.Rows.Add(Constant.MSG_MOVE_VALUE_LARGE, Properties.Settings.Default.MoveValueLarge, Constant.ID_MOVE_VALUE_LARGE);
            dataGridView1.Rows.Add(Constant.MSG_COMMAND_LINE, Properties.Settings.Default.CommandLine, Constant.ID_COMMAND_LINE);
            dataGridView1.Rows.Add(Constant.MSG_BORDER_WIDTH, Properties.Settings.Default.BorderWidth, Constant.ID_BORDER_WIDTH);
            dataGridView1.Rows.Add(Constant.MSG_PRE_LOAD_NUMBER, Properties.Settings.Default.PreLoadNumber, Constant.ID_PRE_LOAD_NUMBER);
            dataGridView1.Rows.Add(Constant.MSG_MAX_THREADS, Properties.Settings.Default.MaxThreads, Constant.ID_MAX_THREADS);
            dataGridView1.Rows.Add(Constant.MSG_INTERPOLATION_MODE, Properties.Settings.Default.InterpolationMode, Constant.ID_INTERPOLATION_MODE);
            dataGridView1.Rows.Add(Constant.MSG_BUFFER_NUMBER, Properties.Settings.Default.BufferNumber, Constant.ID_BUFFER_NUMBER);
            dataGridView1.Rows.Add(Constant.MSG_INTERLOCK_SCREEN_NUMBER, Properties.Settings.Default.InterlockScreenNumber, Constant.ID_INTERLOCK_SCREEN_NUMBER);
            dataGridView1.Rows.Add(Constant.MSG_MOVE_AFTER_COPY, Properties.Settings.Default.MoveAfterCopy, Constant.ID_MOVE_AFTER_COPY);
            dataGridView1.Rows.Add(Constant.MSG_CHANGE_LEFT_RIGHT, Properties.Settings.Default.ChangeLeftRight, Constant.ID_CHANGE_LEFT_RIGHT);
            dataGridView1.Rows.Add(Constant.MSG_DESTINATION_DIRECTORY_1, Properties.Settings.Default.DestinationDirectory1, Constant.ID_DESTINATION_DIRECTORY_1);
            dataGridView1.Rows.Add(Constant.MSG_DESTINATION_DIRECTORY_2, Properties.Settings.Default.DestinationDirectory2, Constant.ID_DESTINATION_DIRECTORY_2);
            dataGridView1.Rows.Add(Constant.MSG_DESTINATION_DIRECTORY_3, Properties.Settings.Default.DestinationDirectory3, Constant.ID_DESTINATION_DIRECTORY_3);
            dataGridView1.Rows.Add(Constant.MSG_DESTINATION_DIRECTORY_4, Properties.Settings.Default.DestinationDirectory4, Constant.ID_DESTINATION_DIRECTORY_4);
            dataGridView1.Rows.Add(Constant.MSG_DESTINATION_DIRECTORY_5, Properties.Settings.Default.DestinationDirectory5, Constant.ID_DESTINATION_DIRECTORY_5);
            dataGridView1.Rows.Add(Constant.MSG_DESTINATION_DIRECTORY_6, Properties.Settings.Default.DestinationDirectory6, Constant.ID_DESTINATION_DIRECTORY_6);
            dataGridView1.Rows.Add(Constant.MSG_DESTINATION_DIRECTORY_7, Properties.Settings.Default.DestinationDirectory7, Constant.ID_DESTINATION_DIRECTORY_7);
            dataGridView1.Rows.Add(Constant.MSG_DESTINATION_DIRECTORY_8, Properties.Settings.Default.DestinationDirectory8, Constant.ID_DESTINATION_DIRECTORY_8);
            dataGridView1.Rows.Add(Constant.MSG_DESTINATION_DIRECTORY_9, Properties.Settings.Default.DestinationDirectory9, Constant.ID_DESTINATION_DIRECTORY_9);
            dataGridView1.Rows.Add(Constant.MSG_EXEC_COMMAND_1, Properties.Settings.Default.ExecCommand1, Constant.ID_EXEC_COMMAND_1);
            dataGridView1.Rows.Add(Constant.MSG_CLEAR_AFTER_EXEC_COMMAND_1, Properties.Settings.Default.ClearAfterExecCommand1, Constant.ID_CLEAR_AFTER_EXEC_COMMAND_1);
            dataGridView1.Rows.Add(Constant.MSG_EXEC_COMMAND_2, Properties.Settings.Default.ExecCommand2, Constant.ID_EXEC_COMMAND_2);
            dataGridView1.Rows.Add(Constant.MSG_CLEAR_AFTER_EXEC_COMMAND_2, Properties.Settings.Default.ClearAfterExecCommand2, Constant.ID_CLEAR_AFTER_EXEC_COMMAND_2);
            dataGridView1.Rows.Add(Constant.MSG_EXEC_COMMAND_3, Properties.Settings.Default.ExecCommand3, Constant.ID_EXEC_COMMAND_3);
            dataGridView1.Rows.Add(Constant.MSG_CLEAR_AFTER_EXEC_COMMAND_3, Properties.Settings.Default.ClearAfterExecCommand3, Constant.ID_CLEAR_AFTER_EXEC_COMMAND_3);
            dataGridView1.Rows.Add(Constant.MSG_EXEC_COMMAND_4, Properties.Settings.Default.ExecCommand4, Constant.ID_EXEC_COMMAND_4);
            dataGridView1.Rows.Add(Constant.MSG_CLEAR_AFTER_EXEC_COMMAND_4, Properties.Settings.Default.ClearAfterExecCommand4, Constant.ID_CLEAR_AFTER_EXEC_COMMAND_4);
            dataGridView1.Rows.Add(Constant.MSG_EXEC_COMMAND_5, Properties.Settings.Default.ExecCommand5, Constant.ID_EXEC_COMMAND_5);
            dataGridView1.Rows.Add(Constant.MSG_CLEAR_AFTER_EXEC_COMMAND_5, Properties.Settings.Default.ClearAfterExecCommand5, Constant.ID_CLEAR_AFTER_EXEC_COMMAND_5);
            dataGridView1.Rows.Add(Constant.MSG_INTERLOCK_FOLDER, Properties.Settings.Default.InterlockFolder, Constant.ID_INTERLOCK_FOLDER);
            dataGridView1.Rows.Add(Constant.MSG_INITIAL_FOLDER, Properties.Settings.Default.InitialFolder, Constant.ID_INITIAL_FOLDER);
            dataGridView1.Rows.Add(Constant.MSG_EXEC_COMMAND_AFTER_MOVE_FILES, Properties.Settings.Default.ExecCommandAfterMoveFiles, Constant.ID_EXEC_COMMAND_AFTER_MOVE_FILES);

            dataGridView1.CurrentCell = dataGridView1["value", 2];

            lblMessage.Text = null;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // 入力チェック
            string numberOfScreenStr = GetGridValue(Constant.ID_NUMBER_OF_SCREEN);
            int numberOfScreen = int.Parse(numberOfScreenStr);

            string screenInformationsStr = GetGridValue(Constant.ID_SCREEN_INFOMATIONS);
            string[] screenInfoStrs = screenInformationsStr.Split('/');
            if (screenInfoStrs.Length != numberOfScreen)
            {
                MessageBox.Show("[" + Constant.MSG_SCREEN_INFOMATIONS + "]の数が[" + Constant.MSG_NUMBER_OF_SCREEN + "]と一致しません。");
                return;
            }

            string leftListStr = GetGridValue(Constant.ID_LEFT_DESTINATIONS);
            string[] leftValueStr = leftListStr.Split(',');
            if (leftValueStr.Length != numberOfScreen)
            {
                MessageBox.Show("[" + Constant.MSG_LEFT_DESTINATIONS + "]の数が[" + Constant.MSG_NUMBER_OF_SCREEN + "]と一致しません。");
                return;
            }

            string rightListStr = GetGridValue(Constant.ID_RIGHT_DESTINATIONS);
            string[] rightValueStr = rightListStr.Split(',');
            if (rightValueStr.Length != numberOfScreen)
            {
                MessageBox.Show("[" + Constant.MSG_RIGHT_DESTINATIONS + "]の数が[" + Constant.MSG_NUMBER_OF_SCREEN + "]と一致しません。");
                return;
            }

            string upListStr = GetGridValue(Constant.ID_UP_DESTINATIONS);
            string[] upValueStr = upListStr.Split(',');
            if (upValueStr.Length != numberOfScreen)
            {
                MessageBox.Show("[" + Constant.MSG_UP_DESTINATIONS + "]の数が[" + Constant.MSG_NUMBER_OF_SCREEN + "]と一致しません。");
                return;
            }

            string downListStr = GetGridValue(Constant.ID_DOWN_DESTINATIONS);
            string[] downValueStr = downListStr.Split(',');
            if (downValueStr.Length != numberOfScreen)
            {
                MessageBox.Show("[" + Constant.MSG_DOWN_DESTINATIONS + "]の数が[" + Constant.MSG_NUMBER_OF_SCREEN + "]と一致しません。");
                return;
            }

            // 設定値を保存
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string id = (string)dataGridView1["id", i].Value;
                string value = (string)dataGridView1["value", i].Value;

                switch (id)
                {
                    case Constant.ID_DO_OPEN_FILE:
                        Properties.Settings.Default.DoOpenFile = value;
                        break;

                    case Constant.ID_DO_OPEN_DIRECTORY:
                        Properties.Settings.Default.DoOpenDirectory = value;
                        break;

                    case Constant.ID_DO_CONFIG:
                        Properties.Settings.Default.DoConfig = value;
                        break;

                    case Constant.ID_DO_CLOSE:
                        Properties.Settings.Default.DoClose = value;
                        break;

                    case Constant.ID_DO_PREV_FILE:
                        Properties.Settings.Default.DoPrevFile = value;
                        break;

                    case Constant.ID_DO_NEXT_FILE:
                        Properties.Settings.Default.DoNextFile = value;
                        break;

                    case Constant.ID_DO_PREV_FILE_CHANGABLE:
                        Properties.Settings.Default.DoPrevFileChangable = value;
                        break;

                    case Constant.ID_DO_NEXT_FILE_CHANGABLE:
                        Properties.Settings.Default.DoNextFileChangable = value;
                        break;

                    case Constant.ID_DO_SKIP_PREV_FILE:
                        Properties.Settings.Default.DoSkipPrevFile = value;
                        break;

                    case Constant.ID_DO_SKIP_NEXT_FILE:
                        Properties.Settings.Default.DoSkipNextFile = value;
                        break;

                    case Constant.ID_DO_FIRST_FILE:
                        Properties.Settings.Default.DoFirstFile = value;
                        break;

                    case Constant.ID_DO_LAST_FILE:
                        Properties.Settings.Default.DoLastFile = value;
                        break;

                    case Constant.ID_DO_RELOAD:
                        Properties.Settings.Default.DoReload = value;
                        break;

                    case Constant.ID_DO_MAXIMIZE:
                        Properties.Settings.Default.DoMaximize = value;
                        break;

                    case Constant.ID_DO_MAXIMIZE_MODE_CHANGE:
                        Properties.Settings.Default.DoMaximizeModeChange = value;
                        break;

                    case Constant.ID_DO_ROTATE_LEFT:
                        Properties.Settings.Default.DoRotateLeft = value;
                        break;

                    case Constant.ID_DO_ROTATE_RIGHT:
                        Properties.Settings.Default.DoRotateRight = value;
                        break;

                    case Constant.ID_DO_EXPANSION:
                        Properties.Settings.Default.DoExpansion = value;
                        break;

                    case Constant.ID_DO_REDUCE:
                        Properties.Settings.Default.DoReduce = value;
                        break;

                    case Constant.ID_DO_MOVE_UP_SCREEN:
                        Properties.Settings.Default.DoMoveUpScreen = value;
                        break;

                    case Constant.ID_DO_MOVE_DOWN_SCREEN:
                        Properties.Settings.Default.DoMoveDownScreen = value;
                        break;

                    case Constant.ID_DO_MOVE_LEFT_SCREEN:
                        Properties.Settings.Default.DoMoveLeftScreen = value;
                        break;

                    case Constant.ID_DO_MOVE_RIGHT_SCREEN:
                        Properties.Settings.Default.DoMoveRightScreen = value;
                        break;

                    case Constant.ID_DO_NO_COPY_FILE:
                        Properties.Settings.Default.DoNoCopyFile = value;
                        break;

                    case Constant.ID_DO_NO_COPY_FILE_REVERSE:
                        Properties.Settings.Default.DoNoCopyFileReverse = value;
                        break;

                    case Constant.ID_DO_COPY_FILE_1:
                        Properties.Settings.Default.DoCopyFile1 = value;
                        break;

                    case Constant.ID_DO_COPY_FILE_2:
                        Properties.Settings.Default.DoCopyFile2 = value;
                        break;

                    case Constant.ID_DO_COPY_FILE_3:
                        Properties.Settings.Default.DoCopyFile3 = value;
                        break;

                    case Constant.ID_DO_COPY_FILE_4:
                        Properties.Settings.Default.DoCopyFile4 = value;
                        break;

                    case Constant.ID_DO_COPY_FILE_5:
                        Properties.Settings.Default.DoCopyFile5 = value;
                        break;

                    case Constant.ID_DO_COPY_FILE_6:
                        Properties.Settings.Default.DoCopyFile6 = value;
                        break;

                    case Constant.ID_DO_COPY_FILE_7:
                        Properties.Settings.Default.DoCopyFile7 = value;
                        break;

                    case Constant.ID_DO_COPY_FILE_8:
                        Properties.Settings.Default.DoCopyFile8 = value;
                        break;

                    case Constant.ID_DO_COPY_FILE_9:
                        Properties.Settings.Default.DoCopyFile9 = value;
                        break;

                    case Constant.ID_DO_SORT_MODE_FILENAME_ASC:
                        Properties.Settings.Default.DoSortModeFilenameAsc = value;
                        break;

                    case Constant.ID_DO_SORT_MODE_FILENAME_DESC:
                        Properties.Settings.Default.DoSortModeFilenameDesc = value;
                        break;

                    case Constant.ID_DO_SORT_MODE_TIME_ASC:
                        Properties.Settings.Default.DoSortModeTimeAsc = value;
                        break;

                    case Constant.ID_DO_SORT_MODE_TIME_DESC:
                        Properties.Settings.Default.DoSortModeTimeDesc = value;
                        break;

                    case Constant.ID_DO_COPY_FULL_NAME:
                        Properties.Settings.Default.DoCopyFullName = value;
                        break;

                    case Constant.ID_DO_COPY_FILE:
                        Properties.Settings.Default.DoCopyFile = value;
                        break;

                    case Constant.ID_DO_INCREMENT_PAGE_NUMBER:
                        Properties.Settings.Default.DoIncrementPageNumber = value;
                        break;

                    case Constant.ID_DO_DECREMENT_PAGE_NUMBER:
                        Properties.Settings.Default.DoDecrementPageNumber = value;
                        break;

                    case Constant.ID_DO_SELECT_EXPLORER:
                        Properties.Settings.Default.DoSelectExplorer = value;
                        break;

                    case Constant.ID_DO_EXEC_COMMAND_1:
                        Properties.Settings.Default.DoExecCommand1 = value;
                        break;

                    case Constant.ID_DO_EXEC_COMMAND_2:
                        Properties.Settings.Default.DoExecCommand2 = value;
                        break;

                    case Constant.ID_DO_EXEC_COMMAND_3:
                        Properties.Settings.Default.DoExecCommand3 = value;
                        break;

                    case Constant.ID_DO_EXEC_COMMAND_4:
                        Properties.Settings.Default.DoExecCommand4 = value;
                        break;

                    case Constant.ID_DO_EXEC_COMMAND_5:
                        Properties.Settings.Default.DoExecCommand5 = value;
                        break;

                    case Constant.ID_DO_SINGLE_PAGE:
                        Properties.Settings.Default.DoSinglePage = value;
                        break;

                    case Constant.ID_DO_TWO_PAGES:
                        Properties.Settings.Default.DoTwoPages = value;
                        break;

                    case Constant.ID_DO_THREE_PAGES:
                        Properties.Settings.Default.DoThreePages = value;
                        break;

                    case Constant.ID_DO_FRONT_BACK:
                        Properties.Settings.Default.DoFrontBack = value;
                        break;

                    case Constant.ID_DO_INTERLOCK:
                        Properties.Settings.Default.DoInterlock = value;
                        break;

                    case Constant.ID_DO_WATCH_MODE:
                        Properties.Settings.Default.DoWatchMode = value;
                        break;

                    case Constant.ID_DO_CHANGE_FOCUS:
                        Properties.Settings.Default.DoChangeFocus = value;
                        break;

                    case Constant.ID_DO_CHANGE_LEFT_RIGHT:
                        Properties.Settings.Default.DoChangeLeftRight = value;
                        break;

                    case Constant.ID_SCREEN_INFO_AUTO:
                        Properties.Settings.Default.ScreenInfoAuto = value;
                        break;

                    case Constant.ID_NUMBER_OF_SCREEN:
                        Properties.Settings.Default.NumberOfScreen = value;
                        break;

                    case Constant.ID_SCREEN_INFOMATIONS:
                        Properties.Settings.Default.ScreenInfomations = value;
                        break;

                    case Constant.ID_LEFT_DESTINATIONS:
                        Properties.Settings.Default.LeftDestinations = value;
                        break;

                    case Constant.ID_RIGHT_DESTINATIONS:
                        Properties.Settings.Default.RightDestinations = value;
                        break;

                    case Constant.ID_UP_DESTINATIONS:
                        Properties.Settings.Default.UpDestinations = value;
                        break;

                    case Constant.ID_DOWN_DESTINATIONS:
                        Properties.Settings.Default.DownDestinations = value;
                        break;

                    case Constant.ID_RESIZE_LIST:
                        Properties.Settings.Default.ResizeList = value;
                        break;

                    case Constant.ID_HORIZONTALLY_LONG_SCREEN:
                        Properties.Settings.Default.HorizontallyLongScreen = value;
                        break;

                    case Constant.ID_VERTICALLY_LONG_SCREEN:
                        Properties.Settings.Default.VerticallyLongScreen = value;
                        break;

                    case Constant.ID_MAXIMIZE:
                        Properties.Settings.Default.Maximize = value;
                        break;

                    case Constant.ID_MOVE_VALUE_SMALL:
                        Properties.Settings.Default.MoveValueSmall = value;
                        break;

                    case Constant.ID_MOVE_VALUE_LARGE:
                        Properties.Settings.Default.MoveValueLarge = value;
                        break;

                    case Constant.ID_COMMAND_LINE:
                        Properties.Settings.Default.CommandLine = value;
                        break;

                    case Constant.ID_BORDER_WIDTH:
                        Properties.Settings.Default.BorderWidth = value;
                        break;

                    case Constant.ID_PRE_LOAD_NUMBER:
                        Properties.Settings.Default.PreLoadNumber = value;
                        break;

                    case Constant.ID_MAX_THREADS:
                        Properties.Settings.Default.MaxThreads = value;
                        break;

                    case Constant.ID_INTERPOLATION_MODE:
                        Properties.Settings.Default.InterpolationMode = value;
                        break;

                    case Constant.ID_BUFFER_NUMBER:
                        Properties.Settings.Default.BufferNumber = value;
                        break;

                    case Constant.ID_INTERLOCK_SCREEN_NUMBER:
                        Properties.Settings.Default.InterlockScreenNumber = value;
                        break;

                    case Constant.ID_MOVE_AFTER_COPY:
                        Properties.Settings.Default.MoveAfterCopy = value;
                        break;

                    case Constant.ID_CHANGE_LEFT_RIGHT:
                        Properties.Settings.Default.ChangeLeftRight = value;
                        break;

                    case Constant.ID_DESTINATION_DIRECTORY_1:
                        Properties.Settings.Default.DestinationDirectory1 = value;
                        break;

                    case Constant.ID_DESTINATION_DIRECTORY_2:
                        Properties.Settings.Default.DestinationDirectory2 = value;
                        break;

                    case Constant.ID_DESTINATION_DIRECTORY_3:
                        Properties.Settings.Default.DestinationDirectory3 = value;
                        break;

                    case Constant.ID_DESTINATION_DIRECTORY_4:
                        Properties.Settings.Default.DestinationDirectory4 = value;
                        break;

                    case Constant.ID_DESTINATION_DIRECTORY_5:
                        Properties.Settings.Default.DestinationDirectory5 = value;
                        break;

                    case Constant.ID_DESTINATION_DIRECTORY_6:
                        Properties.Settings.Default.DestinationDirectory6 = value;
                        break;

                    case Constant.ID_DESTINATION_DIRECTORY_7:
                        Properties.Settings.Default.DestinationDirectory7 = value;
                        break;

                    case Constant.ID_DESTINATION_DIRECTORY_8:
                        Properties.Settings.Default.DestinationDirectory8 = value;
                        break;

                    case Constant.ID_DESTINATION_DIRECTORY_9:
                        Properties.Settings.Default.DestinationDirectory9 = value;
                        break;

                    case Constant.ID_EXEC_COMMAND_1:
                        Properties.Settings.Default.ExecCommand1 = value;
                        break;

                    case Constant.ID_CLEAR_AFTER_EXEC_COMMAND_1:
                        Properties.Settings.Default.ClearAfterExecCommand1 = value;
                        break;

                    case Constant.ID_EXEC_COMMAND_2:
                        Properties.Settings.Default.ExecCommand2 = value;
                        break;

                    case Constant.ID_CLEAR_AFTER_EXEC_COMMAND_2:
                        Properties.Settings.Default.ClearAfterExecCommand2 = value;
                        break;

                    case Constant.ID_EXEC_COMMAND_3:
                        Properties.Settings.Default.ExecCommand3 = value;
                        break;

                    case Constant.ID_CLEAR_AFTER_EXEC_COMMAND_3:
                        Properties.Settings.Default.ClearAfterExecCommand3 = value;
                        break;

                    case Constant.ID_EXEC_COMMAND_4:
                        Properties.Settings.Default.ExecCommand4 = value;
                        break;

                    case Constant.ID_CLEAR_AFTER_EXEC_COMMAND_4:
                        Properties.Settings.Default.ClearAfterExecCommand4 = value;
                        break;

                    case Constant.ID_EXEC_COMMAND_5:
                        Properties.Settings.Default.ExecCommand5 = value;
                        break;

                    case Constant.ID_CLEAR_AFTER_EXEC_COMMAND_5:
                        Properties.Settings.Default.ClearAfterExecCommand5 = value;
                        break;

                    case Constant.ID_INTERLOCK_FOLDER:
                        Properties.Settings.Default.InterlockFolder = value;
                        break;

                    case Constant.ID_INITIAL_FOLDER:
                        Properties.Settings.Default.InitialFolder = value;
                        break;

                    case Constant.ID_EXEC_COMMAND_AFTER_MOVE_FILES:
                        Properties.Settings.Default.ExecCommandAfterMoveFiles = value;
                        break;

                }
            }

            Properties.Settings.Default.Save();
            Config.LoadConfig();

            DialogResult = DialogResult.OK;
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            if (e.ColumnIndex != 1)
            {
                return;
            }

            string id = (string)dataGridView["id", e.RowIndex].Value;
            string formattedValue = e.FormattedValue.ToString();
            bool error = false;
            int intValue;
            float floatValue;
            bool boolValue;

            switch (id)
            {
                case Constant.ID_DO_OPEN_FILE:
                case Constant.ID_DO_OPEN_DIRECTORY:
                case Constant.ID_DO_CONFIG:
                case Constant.ID_DO_CLOSE:
                case Constant.ID_DO_PREV_FILE:
                case Constant.ID_DO_NEXT_FILE:
                case Constant.ID_DO_PREV_FILE_CHANGABLE:
                case Constant.ID_DO_NEXT_FILE_CHANGABLE:
                case Constant.ID_DO_SKIP_PREV_FILE:
                case Constant.ID_DO_SKIP_NEXT_FILE:
                case Constant.ID_DO_FIRST_FILE:
                case Constant.ID_DO_LAST_FILE:
                case Constant.ID_DO_RELOAD:
                case Constant.ID_DO_MAXIMIZE:
                case Constant.ID_DO_MAXIMIZE_MODE_CHANGE:
                case Constant.ID_DO_ROTATE_LEFT:
                case Constant.ID_DO_ROTATE_RIGHT:
                case Constant.ID_DO_EXPANSION:
                case Constant.ID_DO_REDUCE:
                case Constant.ID_DO_MOVE_UP_SCREEN:
                case Constant.ID_DO_MOVE_DOWN_SCREEN:
                case Constant.ID_DO_MOVE_LEFT_SCREEN:
                case Constant.ID_DO_MOVE_RIGHT_SCREEN:
                case Constant.ID_DO_NO_COPY_FILE:
                case Constant.ID_DO_NO_COPY_FILE_REVERSE:
                case Constant.ID_DO_COPY_FILE_1:
                case Constant.ID_DO_COPY_FILE_2:
                case Constant.ID_DO_COPY_FILE_3:
                case Constant.ID_DO_COPY_FILE_4:
                case Constant.ID_DO_COPY_FILE_5:
                case Constant.ID_DO_COPY_FILE_6:
                case Constant.ID_DO_COPY_FILE_7:
                case Constant.ID_DO_COPY_FILE_8:
                case Constant.ID_DO_COPY_FILE_9:
                case Constant.ID_DO_SORT_MODE_FILENAME_ASC:
                case Constant.ID_DO_SORT_MODE_FILENAME_DESC:
                case Constant.ID_DO_SORT_MODE_TIME_ASC:
                case Constant.ID_DO_SORT_MODE_TIME_DESC:
                case Constant.ID_DO_COPY_FULL_NAME:
                case Constant.ID_DO_COPY_FILE:
                case Constant.ID_DO_INCREMENT_PAGE_NUMBER:
                case Constant.ID_DO_DECREMENT_PAGE_NUMBER:
                case Constant.ID_DO_SELECT_EXPLORER:
                case Constant.ID_DO_EXEC_COMMAND_1:
                case Constant.ID_DO_EXEC_COMMAND_2:
                case Constant.ID_DO_EXEC_COMMAND_3:
                case Constant.ID_DO_EXEC_COMMAND_4:
                case Constant.ID_DO_EXEC_COMMAND_5:
                case Constant.ID_DO_MOVE_FILES:
                case Constant.ID_DO_RENAME_PAGE:
                case Constant.ID_DO_SINGLE_PAGE:
                case Constant.ID_DO_TWO_PAGES:
                case Constant.ID_DO_THREE_PAGES:
                case Constant.ID_DO_FRONT_BACK:
                case Constant.ID_DO_TWO_PAGES_FRONT_BACK:
                case Constant.ID_DO_INTERLOCK:
                case Constant.ID_DO_WATCH_MODE:
                case Constant.ID_DO_CHANGE_FOCUS:
                case Constant.ID_DO_CHANGE_LEFT_RIGHT:
                    if (!CheckKeyConfig(formattedValue))
                    {
                        lblMessage.Text = "キーの指定が正しくありません。";
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_SCREEN_INFO_AUTO:
                    if (formattedValue != "0" && formattedValue != "1")
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_NUMBER_OF_SCREEN:
                    if (!int.TryParse(formattedValue, out intValue) || intValue < 1)
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_SCREEN_INFOMATIONS:
                    string[] screenInfoStrs = formattedValue.Split('/');
                    foreach (string screenInfo in screenInfoStrs)
                    {
                        if (!Regex.IsMatch(screenInfo, @"^\s*\d+\s*,\s*\d+\s*,\s*\d+\s*,\s*\d+\s*$"))
                        {
                            error = true;
                            break;
                        }
                    }
                    if (error)
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_LEFT_DESTINATIONS:
                case Constant.ID_RIGHT_DESTINATIONS:
                case Constant.ID_UP_DESTINATIONS:
                case Constant.ID_DOWN_DESTINATIONS:
                    string[] screenNoStrs = formattedValue.Split(',');
                    foreach (string screenNo in screenNoStrs)
                    {
                        if (!int.TryParse(screenNo, out intValue) || intValue < -1)
                        {
                            error = true;
                            break;
                        }
                    }
                    if (error)
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_RESIZE_LIST:
                    string[] resizeStrs = formattedValue.Split(',');
                    foreach (string resize in resizeStrs)
                    {
                        if (!float.TryParse(resize, out floatValue) || floatValue < -1)
                        {
                            error = true;
                            break;
                        }
                    }
                    if (error)
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_HORIZONTALLY_LONG_SCREEN:
                    if (!int.TryParse(formattedValue, out intValue) || intValue < 0)
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_VERTICALLY_LONG_SCREEN:
                    if (!int.TryParse(formattedValue, out intValue) || intValue < 0)
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_MAXIMIZE:
                    if (!bool.TryParse(formattedValue, out boolValue))
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_MOVE_VALUE_SMALL:
                    if (!int.TryParse(formattedValue, out intValue) || intValue < 1)
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_MOVE_VALUE_LARGE:
                    if (!int.TryParse(formattedValue, out intValue) || intValue < 1)
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_BORDER_WIDTH:
                    if (!int.TryParse(formattedValue, out intValue) || intValue < 1)
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_PRE_LOAD_NUMBER:
                    if (!int.TryParse(formattedValue, out intValue) || intValue < 0)
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_MAX_THREADS:
                    if (!int.TryParse(formattedValue, out intValue) || intValue < 1)
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_BUFFER_NUMBER:
                    if (!int.TryParse(formattedValue, out intValue) || intValue < -1)
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_INTERLOCK_SCREEN_NUMBER:
                    if (!int.TryParse(formattedValue, out intValue) || intValue < -2)
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_MOVE_AFTER_COPY:
                    if (!int.TryParse(formattedValue, out intValue) || intValue < -1 || intValue > 1)
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_CHANGE_LEFT_RIGHT:
                    if (formattedValue != "0" && formattedValue != "1")
                    {
                        e.Cancel = true;
                    }
                    break;

                case Constant.ID_CLEAR_AFTER_EXEC_COMMAND_1:
                case Constant.ID_CLEAR_AFTER_EXEC_COMMAND_2:
                case Constant.ID_CLEAR_AFTER_EXEC_COMMAND_3:
                case Constant.ID_CLEAR_AFTER_EXEC_COMMAND_4:
                case Constant.ID_CLEAR_AFTER_EXEC_COMMAND_5:
                    if (formattedValue != "0" && formattedValue != "1")
                    {
                        e.Cancel = true;
                    }
                    break;

            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            string id = (string)dataGridView["id", e.RowIndex].Value;

            switch (id)
            {
                case Constant.ID_SCREEN_INFO_AUTO:
                    lblMessage.Text = "0:自動取得しない 1:自動取得する";
                    break;

                case Constant.ID_NUMBER_OF_SCREEN:
                    lblMessage.Text = "[画面自動取得0のみ] 1以上";
                    break;

                case Constant.ID_SCREEN_INFOMATIONS:
                    lblMessage.Text = "[画面自動取得0のみ] 画面数だけ X, Y, Width, Height / ... の形式で指定";
                    break;

                case Constant.ID_LEFT_DESTINATIONS:
                    lblMessage.Text = "[画面自動取得0のみ] 移動先画面を画面数だけ指定 例：1, 2, 0";
                    break;

                case Constant.ID_RIGHT_DESTINATIONS:
                    lblMessage.Text = "[画面自動取得0のみ] 移動先画面を画面数だけ指定 例：2, 1, 0";
                    break;

                case Constant.ID_UP_DESTINATIONS:
                    lblMessage.Text = "[画面自動取得0のみ] 移動先画面を画面数だけ指定 例：-1, -1, -1";
                    break;

                case Constant.ID_DOWN_DESTINATIONS:
                    lblMessage.Text = "[画面自動取得0のみ] 移動先画面を画面数だけ指定 例：-1, -1, -1";
                    break;

                case Constant.ID_RESIZE_LIST:
                    lblMessage.Text = "[原寸表示] 倍率をカンマ区切りで指定 例：0.25, 0.5, 1, 2, 4";
                    break;

                case Constant.ID_HORIZONTALLY_LONG_SCREEN:
                    lblMessage.Text = "[2画面以上] 横長画面番号 0以上";
                    break;

                case Constant.ID_VERTICALLY_LONG_SCREEN:
                    lblMessage.Text = "[2画面以上] 縦長画面番号 0以上";
                    break;

                case Constant.ID_MAXIMIZE:
                    lblMessage.Text = "初期表示 False:原寸表示 True:フルスクリーン";
                    break;

                case Constant.ID_MOVE_VALUE_SMALL:
                    lblMessage.Text = "[原寸表示] 1以上";
                    break;

                case Constant.ID_MOVE_VALUE_LARGE:
                    lblMessage.Text = "[原寸表示] 1以上";
                    break;

                case Constant.ID_COMMAND_LINE:
                    lblMessage.Text = "コマンドラインパラメータ";
                    break;

                case Constant.ID_BORDER_WIDTH:
                    lblMessage.Text = "1以上";
                    break;

                case Constant.ID_PRE_LOAD_NUMBER:
                    lblMessage.Text = "0以上";
                    break;

                case Constant.ID_MAX_THREADS:
                    lblMessage.Text = "[プリロード] 1以上";
                    break;

                case Constant.ID_BUFFER_NUMBER:
                    lblMessage.Text = "0以上:記憶する画像数 -1:無限(デフォルト)";
                    break;

                case Constant.ID_INTERLOCK_SCREEN_NUMBER:
                    lblMessage.Text = "[2画面以上] 0以上:画面番号 -1:画面0か1(デフォルト) -2:画面分割";
                    break;

                case Constant.ID_MOVE_AFTER_COPY:
                    lblMessage.Text = "1:次の画像(デフォルト) 0:移動しない -1:前の画像";
                    break;

                case Constant.ID_CHANGE_LEFT_RIGHT:
                    lblMessage.Text = "1：左右を入れ替える(デフォルト) 0：左右を入れ替えない";
                    break;

                case Constant.ID_DESTINATION_DIRECTORY_1:
                case Constant.ID_DESTINATION_DIRECTORY_2:
                case Constant.ID_DESTINATION_DIRECTORY_3:
                case Constant.ID_DESTINATION_DIRECTORY_4:
                case Constant.ID_DESTINATION_DIRECTORY_5:
                case Constant.ID_DESTINATION_DIRECTORY_6:
                case Constant.ID_DESTINATION_DIRECTORY_7:
                case Constant.ID_DESTINATION_DIRECTORY_8:
                case Constant.ID_DESTINATION_DIRECTORY_9:
                    lblMessage.Text = "フルパスフォルダ指定 空で指定なし";
                    break;

                case Constant.ID_EXEC_COMMAND_1:
                case Constant.ID_EXEC_COMMAND_2:
                case Constant.ID_EXEC_COMMAND_3:
                case Constant.ID_EXEC_COMMAND_4:
                case Constant.ID_EXEC_COMMAND_5:
                    lblMessage.Text = "コマンド指定 空で指定なし";
                    break;

                case Constant.ID_CLEAR_AFTER_EXEC_COMMAND_1:
                case Constant.ID_CLEAR_AFTER_EXEC_COMMAND_2:
                case Constant.ID_CLEAR_AFTER_EXEC_COMMAND_3:
                case Constant.ID_CLEAR_AFTER_EXEC_COMMAND_4:
                case Constant.ID_CLEAR_AFTER_EXEC_COMMAND_5:
                    lblMessage.Text = "0:コマンド実行後にキャッシュクリアしない(デフォルト) 1:クリアする";
                    break;

                case Constant.ID_INTERLOCK_FOLDER:
                    lblMessage.Text = "フルパスフォルダ指定";
                    break;

                case Constant.ID_INITIAL_FOLDER:
                    lblMessage.Text = "フルパスフォルダ指定 空で引数必須";
                    break;

                case Constant.ID_EXEC_COMMAND_AFTER_MOVE_FILES:
                    lblMessage.Text = "コマンド指定 空で指定なし";
                    break;

            }
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            lblMessage.Text = null;
        }

        private bool CheckKeyConfig(string keyStr)
        {
            dynamic keyStrArray = keyStr.Split(',');
            foreach (string keyStr2 in keyStrArray)
            {
                if (keyStr2.Length == 1)
                {
                    continue;
                }
                else
                {
                    Keys keyData = 0;

                    string[] keyArray = keyStr2.Split('+');
                    foreach (string key in keyArray)
                    {
                        switch (key)
                        {
                            case "Control":
                            case "Ctrl":
                                keyData |= Keys.Control;
                                break;
                            case "Alt":
                                keyData |= Keys.Alt;
                                break;
                            case "Shift":
                                keyData |= Keys.Shift;
                                break;
                            case "Escape":
                            case "Esc":
                                keyData |= Keys.Escape;
                                break;
                            case "Left":
                                keyData |= Keys.Left;
                                break;
                            case "Right":
                                keyData |= Keys.Right;
                                break;
                            case "Up":
                                keyData |= Keys.Up;
                                break;
                            case "Down":
                                keyData |= Keys.Down;
                                break;
                            case "NumPad0":
                                keyData |= Keys.NumPad0;
                                break;
                            case "NumPad1":
                                keyData |= Keys.NumPad1;
                                break;
                            case "NumPad2":
                                keyData |= Keys.NumPad2;
                                break;
                            case "NumPad3":
                                keyData |= Keys.NumPad3;
                                break;
                            case "NumPad4":
                                keyData |= Keys.NumPad4;
                                break;
                            case "NumPad5":
                                keyData |= Keys.NumPad5;
                                break;
                            case "NumPad6":
                                keyData |= Keys.NumPad6;
                                break;
                            case "NumPad7":
                                keyData |= Keys.NumPad7;
                                break;
                            case "NumPad8":
                                keyData |= Keys.NumPad8;
                                break;
                            case "NumPad9":
                                keyData |= Keys.NumPad9;
                                break;
                            case "Back":
                                keyData |= Keys.Back;
                                break;
                            case "CapsLock":
                                keyData |= Keys.CapsLock;
                                break;
                            case "Decimal":
                                keyData |= Keys.Decimal;
                                break;
                            case "Divide":
                                keyData |= Keys.Divide;
                                break;
                            case "End":
                                keyData |= Keys.End;
                                break;
                            case "Enter":
                                keyData |= Keys.Enter;
                                break;
                            case "F1":
                                keyData |= Keys.F1;
                                break;
                            case "F2":
                                keyData |= Keys.F2;
                                break;
                            case "F3":
                                keyData |= Keys.F3;
                                break;
                            case "F4":
                                keyData |= Keys.F4;
                                break;
                            case "F5":
                                keyData |= Keys.F5;
                                break;
                            case "F6":
                                keyData |= Keys.F6;
                                break;
                            case "F7":
                                keyData |= Keys.F7;
                                break;
                            case "F8":
                                keyData |= Keys.F8;
                                break;
                            case "F9":
                                keyData |= Keys.F9;
                                break;
                            case "F10":
                                keyData |= Keys.F10;
                                break;
                            case "F11":
                                keyData |= Keys.F11;
                                break;
                            case "F12":
                                keyData |= Keys.F12;
                                break;
                            case "FinalMode":
                                keyData |= Keys.FinalMode;
                                break;
                            case "Home":
                                keyData |= Keys.Home;
                                break;
                            case "IMEConvert":
                                keyData |= Keys.IMEConvert;
                                break;
                            case "IMEModeChange":
                                keyData |= Keys.IMEModeChange;
                                break;
                            case "IMENonconvert":
                                keyData |= Keys.IMENonconvert;
                                break;
                            case "Insert":
                                keyData |= Keys.Insert;
                                break;
                            case "KanaMode":
                                keyData |= Keys.KanaMode;
                                break;
                            case "KanjiMode":
                                keyData |= Keys.KanjiMode;
                                break;
                            case "LControlKey":
                                keyData |= Keys.LControlKey;
                                break;
                            case "LMenu":
                                keyData |= Keys.LMenu;
                                break;
                            case "LShiftKey":
                                keyData |= Keys.LShiftKey;
                                break;
                            case "LWin":
                                keyData |= Keys.LWin;
                                break;
                            case "MButton":
                                keyData |= Keys.MButton;
                                break;
                            case "Menu":
                                keyData |= Keys.Menu;
                                break;
                            case "Multiply":
                                keyData |= Keys.Multiply;
                                break;
                            case "Next":
                                keyData |= Keys.Next;
                                break;
                            case "NumLock":
                                keyData |= Keys.NumLock;
                                break;
                            case "PageDown":
                                keyData |= Keys.PageDown;
                                break;
                            case "PageUp":
                                keyData |= Keys.PageUp;
                                break;
                            case "Pause":
                                keyData |= Keys.Pause;
                                break;
                            case "PrintScreen":
                                keyData |= Keys.PrintScreen;
                                break;
                            case "RButton":
                                keyData |= Keys.RButton;
                                break;
                            case "RMenu":
                                keyData |= Keys.RMenu;
                                break;
                            case "RShiftKey":
                                keyData |= Keys.RShiftKey;
                                break;
                            case "RWin":
                                keyData |= Keys.RWin;
                                break;
                            case "Scroll":
                                keyData |= Keys.Scroll;
                                break;
                            case "ShiftKey":
                                keyData |= Keys.ShiftKey;
                                break;
                            case "Space":
                                keyData |= Keys.Space;
                                break;
                            case "Subtract":
                                keyData |= Keys.Subtract;
                                break;
                            case "Tab":
                                keyData |= Keys.Tab;
                                break;
                            case "XButton1":
                                keyData |= Keys.XButton1;
                                break;
                            case "XButton2":
                                keyData |= Keys.XButton2;
                                break;
                            case "A":
                            case "a":
                                keyData |= Keys.A;
                                break;
                            case "B":
                            case "b":
                                keyData |= Keys.B;
                                break;
                            case "C":
                            case "c":
                                keyData |= Keys.C;
                                break;
                            case "D":
                            case "d":
                                keyData |= Keys.D;
                                break;
                            case "E":
                            case "e":
                                keyData |= Keys.E;
                                break;
                            case "F":
                            case "f":
                                keyData |= Keys.F;
                                break;
                            case "G":
                            case "g":
                                keyData |= Keys.G;
                                break;
                            case "H":
                            case "h":
                                keyData |= Keys.H;
                                break;
                            case "I":
                            case "i":
                                keyData |= Keys.I;
                                break;
                            case "J":
                            case "j":
                                keyData |= Keys.J;
                                break;
                            case "K":
                            case "k":
                                keyData |= Keys.K;
                                break;
                            case "L":
                            case "l":
                                keyData |= Keys.L;
                                break;
                            case "M":
                            case "m":
                                keyData |= Keys.M;
                                break;
                            case "N":
                            case "n":
                                keyData |= Keys.N;
                                break;
                            case "O":
                            case "o":
                                keyData |= Keys.O;
                                break;
                            case "P":
                            case "p":
                                keyData |= Keys.P;
                                break;
                            case "Q":
                            case "q":
                                keyData |= Keys.Q;
                                break;
                            case "R":
                            case "r":
                                keyData |= Keys.R;
                                break;
                            case "S":
                            case "s":
                                keyData |= Keys.S;
                                break;
                            case "T":
                            case "t":
                                keyData |= Keys.T;
                                break;
                            case "U":
                            case "u":
                                keyData |= Keys.U;
                                break;
                            case "V":
                            case "v":
                                keyData |= Keys.V;
                                break;
                            case "W":
                            case "w":
                                keyData |= Keys.W;
                                break;
                            case "X":
                            case "x":
                                keyData |= Keys.X;
                                break;
                            case "Y":
                            case "y":
                                keyData |= Keys.Y;
                                break;
                            case "Z":
                            case "z":
                                keyData |= Keys.Z;
                                break;
                            case "0":
                                keyData |= Keys.D0;
                                break;
                            case "1":
                                keyData |= Keys.D1;
                                break;
                            case "2":
                                keyData |= Keys.D2;
                                break;
                            case "3":
                                keyData |= Keys.D3;
                                break;
                            case "4":
                                keyData |= Keys.D4;
                                break;
                            case "5":
                                keyData |= Keys.D5;
                                break;
                            case "6":
                                keyData |= Keys.D6;
                                break;
                            case "7":
                                keyData |= Keys.D7;
                                break;
                            case "8":
                                keyData |= Keys.D8;
                                break;
                            case "9":
                                keyData |= Keys.D9;
                                break;
                            default:
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        private string GetGridValue(string id)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string gridId = (string)dataGridView1["id", i].Value;
                if (gridId == id)
                {
                    return (string)dataGridView1["value", i].Value;
                }
            }

            return null;
        }

        private void ConfigForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataGridView1.IsCurrentCellInEditMode)
            {
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
