using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditorGUI.Services
{
    public class NetworkService
    {
        //public HttpClient ApiClient { get; set; }
        //private string _serverPath;
        private ConfigJson _config;
        //private HandshakeJson _configServer;
        private MainWindowViewModel _mainWindowViewModel;

        public NetworkService(ConfigJson config, MainWindowViewModel mainWindowViewModel)
        {
            _config = config;

            foreach (var server in _config.LangServerList)
            {
                if (server.ServerName == _config.DefaultServerName)
                    App.ServerPath = server.ServerURL;
            }

            _mainWindowViewModel = mainWindowViewModel;

            InitializeClient();
        }
        

        private void InitializeClient()
        {
            App.ApiClient = new HttpClient
            {
                BaseAddress = new Uri(App.ServerPath),
                Timeout = TimeSpan.FromMinutes(1),
                
            };
            App.ApiClient.DefaultRequestHeaders.Accept.Clear();
            App.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            TryToGetServerRespondAndConfig();
            

        }


        public void StartupCheck()
        {
            CompareServerConfig();
        }


        public async void TryToGetServerRespondAndConfig()
        {
            _mainWindowViewModel.ProgressInfo = "正在尝试连接服务器……";
            _mainWindowViewModel.ProgressbarVisibility = Visibility.Visible;

            try
            {
                using (HttpResponseMessage respond = await App.ApiClient.GetAsync(App.ServerPath + "/AppConfig.json"))
                {
                    string result = respond.Content.ReadAsStringAsync().Result;

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    if (respond.IsSuccessStatusCode)
                    {
                        //Debug.WriteLine(result);
                        _mainWindowViewModel.ProgressInfo = "正在读取服务器版本数据……";

                        App.LangConfigServer = JsonSerializer.Deserialize<HandshakeJson>(result, options);

                        StartupCheck();
                        //UpdateLangEditorVersion();

                    }
                    if (!respond.IsSuccessStatusCode)
                    {
                        _mainWindowViewModel.ProgressInfo = "连接服务器失败，错误码：" + respond.StatusCode;
                    }
                }
                //return respond.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                _mainWindowViewModel.ProgressInfo = "连接服务器失败";
                _mainWindowViewModel.ProgressbarVisibility = Visibility.Collapsed;
            }
        }

        public void CompareServerConfig()
        {
            if (_config.LangUpdaterVersion == App.LangConfigServer.LangUpdaterVersion)
            {
                if (_config.LangEditorVersion != App.LangConfigServer.LangEditorVersion)
                {
                    UpdateLangEditorVersion();
                }
                else if (CompareDatabaseRev())
                {
                    _mainWindowViewModel.ProgressInfo = "发现更新数据库……";
                    _mainWindowViewModel.ShowImportDbRevUC();
                }
                else
                {
                    _mainWindowViewModel.ProgressInfo = "启动检查通过。";
                    _mainWindowViewModel.ProgressbarVisibility = Visibility.Collapsed;
                }
            }
            else
            {
                DownloadUpdater();
            }
        }


        private bool GetFileExistAndSha256(string filePath, string fileSHA265)
        {
            //string updaterPath = "Updater.exe";
            string hashReslut;

            if (File.Exists(filePath))
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    Debug.WriteLine(filePath);
                    SHA256Managed sha = new SHA256Managed();
                    byte[] hash = sha.ComputeHash(stream);
                    hashReslut = BitConverter.ToString(hash).Replace("-", String.Empty);
                }
                return fileSHA265 == hashReslut;
            }
            else
            {
                return false;
            }
        }

        private void UpdateLangEditorVersion()
        {
            string args = App.ServerPath                      //服务器地址
                + App.LangConfigServer.LangFullpackPath           //编辑器包名
                + " " + "LangEditorFullUpdate.zip"         //下载本地文件名
                + " " + App.LangConfigServer.LangFullpackSHA256   //编辑器服务器端SHA256
                + " " + App.LangConfigServer.LangEditorVersion;   //编辑器服务器端版本号
            Debug.WriteLine(args);

            ProcessStartInfo startUpdaterInfo = new ProcessStartInfo
            {
                FileName = "ESO_LangEditorUpdater.exe",
                Arguments = args,
            };

            Process updater = new Process();
            updater.StartInfo = startUpdaterInfo;
            updater.Start();
            Application.Current.Shutdown();
        }

        private void DownloadUpdater()
        {
            _mainWindowViewModel.ProgressInfo = "正在下载更新器……";

            using var client = new WebClient();
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(DelegateUpdaterDownloadComplete);
            client.DownloadFileAsync(new Uri(App.ServerPath + App.LangConfigServer.LangUpdaterPackPath), "UpdaterPack.zip");
            
        }

        void DelegateUpdaterDownloadComplete(object s, AsyncCompletedEventArgs e)
        {
            HashAndUnzipUpdaterPack();
        }

        private void HashAndUnzipUpdaterPack()
        {
            _mainWindowViewModel.ProgressInfo = "下载完成！";
            Debug.WriteLine("下载完成！");
            Task.Delay(1000);
            if (GetFileExistAndSha256("UpdaterPack.zip", App.LangConfigServer.LangUpdaterPackSha256))
            {
                _mainWindowViewModel.ProgressInfo = "SHA256校验通过，准备解压文件。";
                Debug.WriteLine("SHA256校验通过，准备解压文件。");
                ZipFile.ExtractToDirectory("UpdaterPack.zip", App.WorkingDirectory, true);
                _config.LangUpdaterDllSha256 = App.LangConfigServer.LangUpdaterDllSha256;
                _config.LangUpdaterVersion = App.LangConfigServer.LangUpdaterVersion;
                ConfigJson.Save(_config);
                File.Delete("UpdaterPack.zip");
                CompareServerConfig();
            }
            else
                _mainWindowViewModel.ProgressInfo = "校验SHA256失败！";
        }

        private bool CompareDatabaseRev()
        {
            return _config.DatabaseRev != App.LangConfigServer.LangDatabaseRevised;
        }
    }
}
