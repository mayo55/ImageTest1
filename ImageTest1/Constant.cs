namespace ImageTest1
{
    public enum WindowMode
    {
        // 通常
        Normal,

        // 最大化
        Maximize
    }

    public enum RotateMode
    {
        // 回転なし
        Normal,

        // 右90度回転
        R90,
        
        // 180度回転
        R180,
        
        // 左90度回転
        R270
    }

    public enum ProgramModeType
    {
        // 1ウィンドウ
        OneWindow,

        // 見開き
        Mihiraki,
        
        // 3面
        Sanmen,

        // 前後表示
        Zengo,

        // 予約
        Reserved,

        // 前後表示
        MihirakiZengo
    }

    public enum SortModeType
    {
        TimeAsc,
        TimeDesc,
        FilenameAsc,
        FilenameDesc
    }

    public enum ImageBufferStatus
    {
        None,
        Preparing,
        Stored
    }

    public enum MaximizeMode
    {
        Normal,
        // 通常
        Large
        // 短辺に合わせる
    }

    public enum MihirakiResult
    {
        // 両方なし
        None,
        // 左
        Left,
        // 右
        Right,
        // 両方
        Both
    }

    public class Constant
    {
        public const string ID_DO_OPEN_FILE = "DoOpenFile";
        public const string ID_DO_OPEN_DIRECTORY = "DoOpenDirectory";
        public const string ID_DO_CONFIG = "DoConfig";
        public const string ID_DO_CLOSE = "DoClose";

        public const string ID_DO_PREV_FILE = "DoPrevFile";
        public const string ID_DO_NEXT_FILE = "DoNextFile";
        public const string ID_DO_PREV_FILE_CHANGABLE = "DoPrevFileChangable";
        public const string ID_DO_NEXT_FILE_CHANGABLE = "DoNextFileChangable";
        public const string ID_DO_SKIP_PREV_FILE = "DoSkipPrevFile";
        public const string ID_DO_SKIP_NEXT_FILE = "DoSkipNextFile";
        public const string ID_DO_FIRST_FILE = "DoFirstFile";
        public const string ID_DO_LAST_FILE = "DoLastFile";

        public const string ID_DO_RELOAD = "DoReload";
        public const string ID_DO_MAXIMIZE = "DoMaximize";
        public const string ID_DO_MAXIMIZE_MODE_CHANGE = "DoMaximizeModeChange";
        public const string ID_DO_ROTATE_LEFT = "DoRotateLeft";
        public const string ID_DO_ROTATE_RIGHT = "DoRotateRight";
        public const string ID_DO_EXPANSION = "DoExpansion";
        public const string ID_DO_REDUCE = "DoReduce";

        public const string ID_DO_MOVE_UP_SCREEN = "DoMoveUpScreen";
        public const string ID_DO_MOVE_DOWN_SCREEN = "DoMoveDownScreen";
        public const string ID_DO_MOVE_LEFT_SCREEN = "DoMoveLeftScreen";
        public const string ID_DO_MOVE_RIGHT_SCREEN = "DoMoveRightScreen";

        public const string ID_DO_NO_COPY_FILE = "DoNoCopyFile";
        public const string ID_DO_NO_COPY_FILE_REVERSE = "DoNoCopyFileReverse";
        public const string ID_DO_COPY_FILE_1 = "DoCopyFile1";
        public const string ID_DO_COPY_FILE_2 = "DoCopyFile2";
        public const string ID_DO_COPY_FILE_3 = "DoCopyFile3";
        public const string ID_DO_COPY_FILE_4 = "DoCopyFile4";
        public const string ID_DO_COPY_FILE_5 = "DoCopyFile5";
        public const string ID_DO_COPY_FILE_6 = "DoCopyFile6";
        public const string ID_DO_COPY_FILE_7 = "DoCopyFile7";
        public const string ID_DO_COPY_FILE_8 = "DoCopyFile8";
        public const string ID_DO_COPY_FILE_9 = "DoCopyFile9";

        public const string ID_DO_SORT_MODE_FILENAME_ASC = "DoSortModeFilenameAsc";
        public const string ID_DO_SORT_MODE_FILENAME_DESC = "DoSortModeFilenameDesc";
        public const string ID_DO_SORT_MODE_TIME_ASC = "DoSortModeTimeAsc";
        public const string ID_DO_SORT_MODE_TIME_DESC = "DoSortModeTimeDesc";

        public const string ID_DO_COPY_FULL_NAME = "DoCopyFullName";
        public const string ID_DO_COPY_FILE = "DoCopyFile";
        public const string ID_DO_INCREMENT_PAGE_NUMBER = "DoIncrementPageNumber";
        public const string ID_DO_DECREMENT_PAGE_NUMBER = "DoDecrementPageNumber";

        public const string ID_DO_SELECT_EXPLORER = "DoSelectExplorer";
        public const string ID_DO_EXEC_COMMAND_1 = "DoExecCommand1";
        public const string ID_DO_EXEC_COMMAND_2 = "DoExecCommand2";
        public const string ID_DO_EXEC_COMMAND_3 = "DoExecCommand3";
        public const string ID_DO_EXEC_COMMAND_4 = "DoExecCommand4";
        public const string ID_DO_EXEC_COMMAND_5 = "DoExecCommand5";
        public const string ID_DO_MOVE_FILES = "DoMoveFiles";
        public const string ID_DO_RENAME_PAGE = "DoRenamePage";

        public const string ID_DO_SINGLE_PAGE = "DoSinglePage";
        public const string ID_DO_TWO_PAGES = "DoTwoPages";
        public const string ID_DO_THREE_PAGES = "DoThreePages";
        public const string ID_DO_FRONT_BACK = "DoFrontBack";
        public const string ID_DO_TWO_PAGES_FRONT_BACK = "DoTwoPagesFrontBack";
        public const string ID_DO_INTERLOCK = "DoInterlock";
        public const string ID_DO_WATCH_MODE = "DoWatchMode";
        public const string ID_DO_CHANGE_FOCUS = "DoChangeFocus";
        public const string ID_DO_CHANGE_LEFT_RIGHT = "DoChangeLeftRight";


        public const string ID_SCREEN_INFO_AUTO = "ScreenInfoAuto";
        public const string ID_NUMBER_OF_SCREEN = "NumberOfScreen";
        public const string ID_SCREEN_INFOMATIONS = "ScreenInfomations";
        public const string ID_LEFT_DESTINATIONS = "LeftDestinations";
        public const string ID_RIGHT_DESTINATIONS = "RightDestinations";
        public const string ID_UP_DESTINATIONS = "UpDestinations";
        public const string ID_DOWN_DESTINATIONS = "DownDestinations";
        public const string ID_RESIZE_LIST = "ResizeList";
        public const string ID_HORIZONTALLY_LONG_SCREEN = "HorizontallyLongScreen";
        public const string ID_VERTICALLY_LONG_SCREEN = "VerticallyLongScreen";
        public const string ID_MAXIMIZE = "Maximize";
        public const string ID_MOVE_VALUE_SMALL = "MoveValueSmall";
        public const string ID_MOVE_VALUE_LARGE = "MoveValueLarge";
        public const string ID_COMMAND_LINE = "CommandLine";
        public const string ID_BORDER_WIDTH = "BorderWidth";
        public const string ID_PRE_LOAD_NUMBER = "PreLoadNumber";
        public const string ID_MAX_THREADS = "MaxThreads";
        public const string ID_INTERPOLATION_MODE = "InterpolationMode";
        public const string ID_BUFFER_NUMBER = "BufferNumber";
        public const string ID_INTERLOCK_SCREEN_NUMBER = "InterlockScreenNumber";
        public const string ID_MOVE_AFTER_COPY = "MoveAfterCopy";
        public const string ID_CHANGE_LEFT_RIGHT = "ChangeLeftRight";
        public const string ID_DESTINATION_DIRECTORY_1 = "DestinationDirectory1";
        public const string ID_DESTINATION_DIRECTORY_2 = "DestinationDirectory2";
        public const string ID_DESTINATION_DIRECTORY_3 = "DestinationDirectory3";
        public const string ID_DESTINATION_DIRECTORY_4 = "DestinationDirectory4";
        public const string ID_DESTINATION_DIRECTORY_5 = "DestinationDirectory5";
        public const string ID_DESTINATION_DIRECTORY_6 = "DestinationDirectory6";
        public const string ID_DESTINATION_DIRECTORY_7 = "DestinationDirectory7";
        public const string ID_DESTINATION_DIRECTORY_8 = "DestinationDirectory8";
        public const string ID_DESTINATION_DIRECTORY_9 = "DestinationDirectory9";
        public const string ID_EXEC_COMMAND_1 = "ExecCommand1";
        public const string ID_CLEAR_AFTER_EXEC_COMMAND_1 = "ClearAfterExecCommand1";
        public const string ID_EXEC_COMMAND_2 = "ExecCommand2";
        public const string ID_CLEAR_AFTER_EXEC_COMMAND_2 = "ClearAfterExecCommand2";
        public const string ID_EXEC_COMMAND_3 = "ExecCommand3";
        public const string ID_CLEAR_AFTER_EXEC_COMMAND_3 = "ClearAfterExecCommand3";
        public const string ID_EXEC_COMMAND_4 = "ExecCommand4";
        public const string ID_CLEAR_AFTER_EXEC_COMMAND_4 = "ClearAfterExecCommand4";
        public const string ID_EXEC_COMMAND_5 = "ExecCommand5";
        public const string ID_CLEAR_AFTER_EXEC_COMMAND_5 = "ClearAfterExecCommand5";
        public const string ID_INTERLOCK_FOLDER = "InterlockFolder";
        public const string ID_INITIAL_FOLDER = "InitialFolder";
        public const string ID_EXEC_COMMAND_AFTER_MOVE_FILES = "ExecCommandAfterMoveFiles";


        public const string MSG_KEY_CONFIG = "キー設定";

        public const string MSG_DO_ALL = "全体";

        public const string MSG_DO_OPEN_FILE = "ファイルを開く";
        public const string MSG_DO_OPEN_DIRECTORY = "フォルダを開く";
        public const string MSG_DO_CONFIG = "設定";
        public const string MSG_DO_CLOSE = "終了";

        public const string MSG_DO_MOVE = "移動";

        public const string MSG_DO_PREV_FILE = "前の画像";
        public const string MSG_DO_NEXT_FILE = "次の画像";
        public const string MSG_DO_PREV_FILE_CHANGABLE = "前の画像(左右入れ替え可)";
        public const string MSG_DO_NEXT_FILE_CHANGABLE = "次の画像(左右入れ替え可)";
        public const string MSG_DO_SKIP_PREV_FILE = "10個前の画像";
        public const string MSG_DO_SKIP_NEXT_FILE = "10個後の画像";
        public const string MSG_DO_FIRST_FILE = "先頭の画像";
        public const string MSG_DO_LAST_FILE = "最後の画像";

        public const string MSG_DO_IMAGE = "画像";

        public const string MSG_DO_RELOAD = "読み込み直す";
        public const string MSG_DO_MAXIMIZE = "原寸表示/フルスクリーン表示切り替え";
        public const string MSG_DO_MAXIMIZE_MODE_CHANGE = "短辺/長辺を画面サイズに合わせる";
        public const string MSG_DO_ROTATE_LEFT = "左90度回転";
        public const string MSG_DO_ROTATE_RIGHT = "右90度回転";
        public const string MSG_DO_EXPANSION = "拡大(原寸表示モードのみ)";
        public const string MSG_DO_REDUCE = "縮小(原寸表示モードのみ)";

        public const string MSG_DO_SCREEN = "画面";

        public const string MSG_DO_MOVE_UP_SCREEN = "上の画面に移動";
        public const string MSG_DO_MOVE_DOWN_SCREEN = "下の画面に移動";
        public const string MSG_DO_MOVE_LEFT_SCREEN = "左の画面に移動";
        public const string MSG_DO_MOVE_RIGHT_SCREEN = "右の画面に移動";

        public const string MSG_DO_COPY = "コピー";

        public const string MSG_DO_NO_COPY_FILE = "コピーなしで進む";
        public const string MSG_DO_NO_COPY_FILE_REVERSE = "コピーなしで戻る";
        public const string MSG_DO_COPY_FILE_1 = "フォルダ1にコピー";
        public const string MSG_DO_COPY_FILE_2 = "フォルダ2にコピー";
        public const string MSG_DO_COPY_FILE_3 = "フォルダ3にコピー";
        public const string MSG_DO_COPY_FILE_4 = "フォルダ4にコピー";
        public const string MSG_DO_COPY_FILE_5 = "フォルダ5にコピー";
        public const string MSG_DO_COPY_FILE_6 = "フォルダ6にコピー";
        public const string MSG_DO_COPY_FILE_7 = "フォルダ7にコピー";
        public const string MSG_DO_COPY_FILE_8 = "フォルダ8にコピー";
        public const string MSG_DO_COPY_FILE_9 = "フォルダ9にコピー";

        public const string MSG_DO_SORT = "ソート";

        public const string MSG_DO_SORT_MODE_FILENAME_ASC = "ファイル名昇順";
        public const string MSG_DO_SORT_MODE_FILENAME_DESC = "ファイル名降順";
        public const string MSG_DO_SORT_MODE_TIME_ASC = "更新日時昇順";
        public const string MSG_DO_SORT_MODE_TIME_DESC = "更新日時降順";

        public const string MSG_DO_EDIT = "編集";

        public const string MSG_DO_COPY_FULL_NAME = "フルパスファイル名コピー";
        public const string MSG_DO_COPY_FILE = "ファイルコピー";
        public const string MSG_DO_INCREMENT_PAGE_NUMBER = "ページ番号増加";
        public const string MSG_DO_DECREMENT_PAGE_NUMBER = "ページ番号減少";

        public const string MSG_DO_COMMAND = "コマンド";

        public const string MSG_DO_SELECT_EXPLORER = "エクスプローラ起動";
        public const string MSG_DO_EXEC_COMMAND_1 = "コマンド1実行";
        public const string MSG_DO_EXEC_COMMAND_2 = "コマンド2実行";
        public const string MSG_DO_EXEC_COMMAND_3 = "コマンド3実行";
        public const string MSG_DO_EXEC_COMMAND_4 = "コマンド4実行";
        public const string MSG_DO_EXEC_COMMAND_5 = "コマンド5実行";
        public const string MSG_DO_MOVE_FILES = "複数ファイルコピー";
        public const string MSG_DO_RENAME_PAGE = "ページ番号に合わせて一括リネーム";

        public const string MSG_DO_MODE = "モード";

        public const string MSG_DO_SINGLE_PAGE = "1面モード";
        public const string MSG_DO_TWO_PAGES = "見開きモード/1面モード切り替え";
        public const string MSG_DO_THREE_PAGES = "3面モード/1面モード切り替え";
        public const string MSG_DO_FRONT_BACK = "前後表示モード/1面モード切り替え";
        public const string MSG_DO_TWO_PAGES_FRONT_BACK = "見開き前後表示モード/1面モード切り替え";
        public const string MSG_DO_INTERLOCK = "連動モード開始/終了";
        public const string MSG_DO_WATCH_MODE = "監視モード開始/停止";
        public const string MSG_DO_CHANGE_FOCUS = "フォーカス切り替え(連動モード)";
        public const string MSG_DO_CHANGE_LEFT_RIGHT = "左右切り替え";


        public const string MSG_CONFIG = "設定";

        public const string MSG_SCREEN_INFO_AUTO = "画面情報自動取得";
        public const string MSG_NUMBER_OF_SCREEN = "画面数";
        public const string MSG_SCREEN_INFOMATIONS = "解像度情報";
        public const string MSG_LEFT_DESTINATIONS = "左画面移動先";
        public const string MSG_RIGHT_DESTINATIONS = "右画面移動先";
        public const string MSG_UP_DESTINATIONS = "上画面移動先";
        public const string MSG_DOWN_DESTINATIONS = "下画面移動先";
        public const string MSG_RESIZE_LIST = "リサイズリスト";
        public const string MSG_HORIZONTALLY_LONG_SCREEN = "横長画面No";
        public const string MSG_VERTICALLY_LONG_SCREEN = "縦長画面No";
        public const string MSG_MAXIMIZE = "初期表示フルスクリーン";
        public const string MSG_MOVE_VALUE_SMALL = "移動量小";
        public const string MSG_MOVE_VALUE_LARGE = "移動量大";
        public const string MSG_COMMAND_LINE = "コマンドライン";
        public const string MSG_BORDER_WIDTH = "ボーダー幅";
        public const string MSG_PRE_LOAD_NUMBER = "プリロード数";
        public const string MSG_MAX_THREADS = "プリロードの最大スレッド数";
        public const string MSG_INTERPOLATION_MODE = "Interpolationモード";
        public const string MSG_BUFFER_NUMBER = "バッファ数";
        public const string MSG_INTERLOCK_SCREEN_NUMBER = "連動モード画面No";
        public const string MSG_MOVE_AFTER_COPY = "コピー後の移動方向";
        public const string MSG_CHANGE_LEFT_RIGHT = "左右入れ替え";
        public const string MSG_DESTINATION_DIRECTORY_1 = "コピー先ディレクトリ1";
        public const string MSG_DESTINATION_DIRECTORY_2 = "コピー先ディレクトリ2";
        public const string MSG_DESTINATION_DIRECTORY_3 = "コピー先ディレクトリ3";
        public const string MSG_DESTINATION_DIRECTORY_4 = "コピー先ディレクトリ4";
        public const string MSG_DESTINATION_DIRECTORY_5 = "コピー先ディレクトリ5";
        public const string MSG_DESTINATION_DIRECTORY_6 = "コピー先ディレクトリ6";
        public const string MSG_DESTINATION_DIRECTORY_7 = "コピー先ディレクトリ7";
        public const string MSG_DESTINATION_DIRECTORY_8 = "コピー先ディレクトリ8";
        public const string MSG_DESTINATION_DIRECTORY_9 = "コピー先ディレクトリ9";
        public const string MSG_EXEC_COMMAND_1 = "実行コマンド1";
        public const string MSG_CLEAR_AFTER_EXEC_COMMAND_1 = "コマンド実行後のキャッシュクリア有無1";
        public const string MSG_EXEC_COMMAND_2 = "実行コマンド2";
        public const string MSG_CLEAR_AFTER_EXEC_COMMAND_2 = "コマンド実行後のキャッシュクリア有無2";
        public const string MSG_EXEC_COMMAND_3 = "実行コマンド3";
        public const string MSG_CLEAR_AFTER_EXEC_COMMAND_3 = "コマンド実行後のキャッシュクリア有無3";
        public const string MSG_EXEC_COMMAND_4 = "実行コマンド4";
        public const string MSG_CLEAR_AFTER_EXEC_COMMAND_4 = "コマンド実行後のキャッシュクリア有無4";
        public const string MSG_EXEC_COMMAND_5 = "実行コマンド5";
        public const string MSG_CLEAR_AFTER_EXEC_COMMAND_5 = "コマンド実行後のキャッシュクリア有無5";
        public const string MSG_INTERLOCK_FOLDER = "連動フォルダ";
        public const string MSG_INITIAL_FOLDER = "初期表示フォルダ";
        public const string MSG_EXEC_COMMAND_AFTER_MOVE_FILES = "複数ファイル移動後実行コマンド";
    }

}
