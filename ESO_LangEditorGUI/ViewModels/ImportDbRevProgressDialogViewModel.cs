using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorLib.Models.Client.Enum;
using ESO_LangEditorLib.Services.Client;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class ImportDbRevProgressDialogViewModel : BaseViewModel
    {
        private string _currentExcuteText;
        private Visibility _beginButtonVisibility;
        private bool _closeButtonEnable;
        private bool _progressbarDisplay;
        private string _downloadSpeed;

        //public ICommand RunDialogCommand => new ExcuteViewModelMethod(ExportLangAsync);

        public string CurrentExcuteText
        {
            get { return _currentExcuteText; }
            set { _currentExcuteText = value; NotifyPropertyChanged(); }
        }

        public Visibility BeginButtonVisibility
        {
            get { return _beginButtonVisibility; }
            set { _beginButtonVisibility = value; NotifyPropertyChanged(); }
        }

        public bool ProgressbarDisplay
        {
            get { return _progressbarDisplay; }
            set { _progressbarDisplay = value; NotifyPropertyChanged(); }
        }

        public string DownloadSpeed
        {
            get { return _downloadSpeed; }
            set { _downloadSpeed = value; NotifyPropertyChanged(); }
        }


        private int _DatabaseRevServer;
        private List<string> DatabaseRevDownloadList = new List<string>();

        private static string DatabaseRevPath = @"database/data_rev";
        private static string DownloadFolder = @"_Tmp/";

        private int RevCompareNum = 0;
        private int TaskCount = 0;

        private HttpClient apiClient { get; set; }


        public ImportDbRevProgressDialogViewModel()
        {
            //CurrentExcuteText = "导出为.lang，等待点击开始按钮执行";
            ProgressbarDisplay = false;
            //BeginButtonEnable = true;
            //CloseButtonEnable = true;
            DatabaseRevCal();
        }



        private async void DatabaseRevCal()
        {
            int localRev = App.LangConfig.DatabaseRev;
            int serverRev = App.LangConfigServer.LangDatabaseRevised;

            RevCompareNum = serverRev - localRev;
            int count = 0;

            if (!Directory.Exists(DownloadFolder))
                Directory.CreateDirectory(DownloadFolder);

            var db = new LangTextRepository();

            for (int i = 1; i <= RevCompareNum; i++)
            {
                DatabaseRevDownloadList.Add(App.ServerPath + DatabaseRevPath + (localRev + i).ToString() + ".zip");
            }

            CurrentExcuteText = "共有 " + RevCompareNum.ToString() + " 个数据文件下载。";

            foreach (var path in DatabaseRevDownloadList)
            {
                Debug.WriteLine(path);
                count++;

                CurrentExcuteText = "当前正在下载和导入第 " + count.ToString() + " 项，共有 " + RevCompareNum.ToString() + " 个数据文件下载。" +
                    "\n如果窗口突然假死请勿强行结束程序，目前导入时会导致此问题，完成后会自动退出。";

                DownloadDatabaseRevFile(path);
            }

        }

        private void CompletedImportCheck()
        {
            if (TaskCount == RevCompareNum)
            {
                var langconfig = App.LangConfig;
                langconfig.DatabaseRev = App.LangConfigServer.LangDatabaseRevised;
                ConfigJson.Save(langconfig);
                DialogHost.CloseDialogCommand.Execute(null, null);
                App.LangNetworkService.CompareServerConfig();
                
            }

        }

        private void DownloadDatabaseRevFile(string path)
        {
            //JsonDto langText;
            Uri uri = new Uri(path);
            string filename = Path.GetFileName(uri.LocalPath);

            using (WebClient client = new WebClient())
            {
                client.DownloadProgressChanged += Editor_DownloadProgressChanged;
                client.DownloadFileCompleted += DatabaseZipExtractAndImport(DownloadFolder + filename);
                client.DownloadFileAsync(uri, DownloadFolder + filename);
            }

        }

        private void DelegateUnzip(object s, AsyncCompletedEventArgs e)
        {
            DatabaseZipExtractAndImport((string)s);
        }

        private AsyncCompletedEventHandler DatabaseZipExtractAndImport(string filename)
        {
            Action<object, AsyncCompletedEventArgs> action = (sender, e) =>
            {
                List<string> fileList = new List<string>();
                ParseLangFile parseLangFile = new ParseLangFile();

                using (ZipArchive archive = ZipFile.OpenRead(filename))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        fileList.Add(entry.FullName);
                        Debug.WriteLine(entry.FullName);
                    }
                }
                ZipFile.ExtractToDirectory(filename, DownloadFolder, true);

                foreach (var path in fileList)
                {
                    ImportDataToDb(parseLangFile.JsonToDtoReader(DownloadFolder + path));
                }
                TaskCount++;
                CompletedImportCheck();
            };
            return new AsyncCompletedEventHandler(action);
        }

        private void ImportDataToDb(JsonDto json)
        {
            var db = new LangTextRepository();

            switch (json.ChangeType)
            {
                case LangChangeType.Added:
                    db.AddNewLangs(json.LangTexts);
                    break;
                case LangChangeType.ChangedEN:
                    db.UpdateLangsEN(json.LangTexts);
                    break;
                case LangChangeType.ChangedZH:
                    db.UpdateLangsZH(json.LangTexts);
                    break;
                case LangChangeType.Removed:
                    db.DeleteLangs(json.LangTexts);
                    break;
            }
        }

        void Editor_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadSpeed = "进度：" 
                + e.ProgressPercentage 
                + "%，已下载大小：" 
                + SizeSuffix(e.BytesReceived) 
                + "，总大小："
                + SizeSuffix(e.TotalBytesToReceive);
        }

        static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }
    }
}
