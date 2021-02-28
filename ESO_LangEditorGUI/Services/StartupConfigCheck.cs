using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.NetClient;
using ESO_LangEditorGUI.EventAggres;
using ESO_LangEditorGUI.Services.AccessServer;
using ESO_LangEditorGUI.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditorGUI.Services
{
    public class StartupConfigCheck
    {
        IEventAggregator _ea;
        private StartupNetCheck _startupNetwork;

        public StartupConfigCheck(IEventAggregator ea)
        {
            _ea = ea;
            _startupNetwork = new StartupNetCheck();
        }

        public async Task TryToGetServerRespondAndConfig()
        {
            //StartupNetCheck startupNetwork = new StartupNetCheck();


            try
            {
                _ea.GetEvent<ConnectProgressString>().Publish("正在读取服务器版本数据……");
                _ea.GetEvent<ConnectStatusChangeEvent>().Publish(ClientConnectStatus.Connecting);
                string result = await _startupNetwork.GetServerRespondAndConfig(App.ServerPath + "/AppConfig.json");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                //_ea.GetEvent<ConnectProgressString>().Publish("正在读取服务器版本数据……");
                //_mainWindowViewModel.ProgressInfo = "正在读取服务器版本数据……";
                App.LangConfigServer = JsonSerializer.Deserialize<AppConfigServer>(result, options);
                await CompareServerConfig();

                
                //var userService = new AccountService();
                //userService.DecodeTokenToGetUserName("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYmV2aXMiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImNjOTM3MzhjLWE5ZjQtNDZlMy04ZGRjLWI3OWM3NDlkMzcxYiIsImV4cCI6MTYxMTc2MDE4OSwiaXNzIjoiZGVtb19pc3N1ZXIiLCJhdWQiOiJkZW1vX2F1ZGllbmNlIn0.TQ-B19oGPB6O4oYrYy8DTCCvRLGeqaZFSD_SsnubMDo");


                //if (App.LangConfig.UserRefreshToken != "" & App.LangConfig.UserRefreshToken != "")
                //{
                //    var userService = new AccountService();
                //    userService.LoginByToken();
                //}

                //if (App.LangConfig.UserRefreshToken == "" || App.LangConfig.UserRefreshToken == "")
                //{
                //    _ea.GetEvent<LoginRequiretEvent>().Publish();
                //}






                //using (HttpResponseMessage respond = await App.ApiClient.GetAsync(App.ServerPath + "/AppConfig.json"))
                //{
                //    string result = respond.Content.ReadAsStringAsync().Result;



                //    if (respond.IsSuccessStatusCode)
                //    {
                //        //Debug.WriteLine(result);
                //        _mainWindowViewModel.ProgressInfo = "正在读取服务器版本数据……";

                //        App.LangConfigServer = JsonSerializer.Deserialize<HandshakeJson>(result, options);

                //        StartupCheck();
                //        //UpdateLangEditorVersion();

                //    }
                //    if (!respond.IsSuccessStatusCode)
                //    {
                //        _mainWindowViewModel.ProgressInfo = "连接服务器失败，错误码：" + respond.StatusCode;
                //    }
                //}
                //return respond.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                _ea.GetEvent<ConnectProgressString>().Publish("连接服务器失败");
                _ea.GetEvent<ConnectStatusChangeEvent>().Publish(ClientConnectStatus.ConnectError);
            }
        }


        public async Task CompareServerConfig()
        {
            
            if (App.LangConfig.LangUpdaterVersion == App.LangConfigServer.LangUpdaterVersion
                && App.LangConfig.LangUpdaterDllSha256 == App.LangConfigServer.LangUpdaterDllSha256)
            {
                if (App.LangConfig.LangEditorVersion != App.LangConfigServer.LangEditorVersion)
                {
                    UpdateLangEditorVersion();
                }
                else if (CompareDatabaseVersion())
                {
                    //_mainWindowViewModel.ProgressInfo = "发现全量数据库……";
                    //_mainWindowViewModel.ShowImportDbRevUC(false);
                }
                else if (CompareDatabaseRev())
                {
                    //_mainWindowViewModel.ProgressInfo = "发现更新数据库……";
                    //_mainWindowViewModel.ShowImportDbRevUC(true);
                }
                else
                {
                    //_mainWindowViewModel.ProgressInfo = "启动检查通过。";
                    //_mainWindowViewModel.ProgressbarVisibility = Visibility.Collapsed;
                }
            }
            else
            {
                await DownloadUpdaterAsync();
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

        private async Task DownloadUpdaterAsync()
        {
            _ea.GetEvent<ConnectProgressString>().Publish("正在下载更新器……");

            await _startupNetwork.DownloadUpdater(App.ServerPath + App.LangConfigServer.LangUpdaterPackPath);
            HashAndUnzipUpdaterPack();

        }

        private void HashAndUnzipUpdaterPack()
        {
            _ea.GetEvent<ConnectProgressString>().Publish("下载完成！");
            Debug.WriteLine("下载完成！");
            Task.Delay(1000);
            if (GetFileExistAndSha256("UpdaterPack.zip", App.LangConfigServer.LangUpdaterPackSha256))
            {
                _ea.GetEvent<ConnectProgressString>().Publish("SHA256校验通过，准备解压文件。");
                Debug.WriteLine("SHA256校验通过，准备解压文件。");
                ZipFile.ExtractToDirectory("UpdaterPack.zip", App.WorkingDirectory, true);
                App.LangConfig.LangUpdaterDllSha256 = App.LangConfigServer.LangUpdaterDllSha256;
                App.LangConfig.LangUpdaterVersion = App.LangConfigServer.LangUpdaterVersion;
                AppConfigClient.Save(App.LangConfig);
                File.Delete("UpdaterPack.zip");
                CompareServerConfig();
            }
            else
                _ea.GetEvent<ConnectProgressString>().Publish("校验SHA256失败！");

        }

        private bool CompareDatabaseRev()
        {
            return App.LangConfig.DatabaseRev != App.LangConfigServer.LangDatabaseRevised;
        }

        private bool CompareDatabaseVersion()
        {
            return App.LangConfig.LangDatabaseVersion != App.LangConfigServer.LangDatabaseVersion;
        }
    }
}
