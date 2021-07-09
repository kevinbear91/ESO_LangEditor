using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.NetClient.Old;
using ESO_LangEditor.GUI.EventAggres;
using ESO_LangEditor.GUI.Services.AccessServer;
using ESO_LangEditor.GUI.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditor.GUI.Services
{
    public class StartupConfigCheck_Depart
    {
        IEventAggregator _ea;
        private readonly StartupNetCheck _startupNetwork;

        //private LangTextRepoClientService _langTextRepo = new LangTextRepoClientService();
        private LangtextNetService _langtextNet = new LangtextNetService(App.ServerPath);
       //private IMapper _mapper = App.Mapper;

        private List<string> DatabaseRevDownloadList = new List<string>();

        private static readonly string DatabaseRevPath = @"database/data_rev";
        private static readonly string DownloadFolder = @"_Tmp/";

        private int RevCompareNum = 0;
        private int TaskCount = 0;

        private int _RevNumberLocal = 0;
        private int _RevNumberServer = 0;
        //private UserInClientDto _userInfo = App.LangConfig.UserInfo;

        public StartupConfigCheck_Depart(IEventAggregator ea)
        {
            _ea = ea;
            _startupNetwork = new StartupNetCheck();
            _ea.GetEvent<ConnectProgressString>().Subscribe(StartupCompare);
        }

        public async Task TryToGetServerRespondAndConfig()
        {
            await GetConfigFromDb();
            //App.ServerPath = "http://langlocal.test";

            try
            {
                _ea.GetEvent<ConnectProgressString>().Publish("正在读取服务器版本数据……");
                _ea.GetEvent<ConnectStatusChangeEvent>().Publish(ClientConnectStatus.Connecting);
                var result = await _startupNetwork.GetServerRespondAndConfig(App.ServerPath + "AppConfig.json");
                
                if (result != null)
                {
                    //App.LangConfigServer = result;

                    await CompareUpdaterVersion();

                    CompareEditorVersion();

                    if (App.LangConfig.UserAuthToken != "" & App.LangConfig.UserRefreshToken != "")
                    {
                        var userService = new AccountService(_ea);
                        userService.LoginByToken();
                    }

                    if (App.LangConfig.UserAuthToken == "" || App.LangConfig.UserRefreshToken == "")
                    {
                        _ea.GetEvent<LoginRequiretEvent>().Publish();
                    }

                }

                //var userService = new AccountService();
                //userService.DecodeTokenToGetUserName("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYmV2aXMiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImNjOTM3MzhjLWE5ZjQtNDZlMy04ZGRjLWI3OWM3NDlkMzcxYiIsImV4cCI6MTYxMTc2MDE4OSwiaXNzIjoiZGVtb19pc3N1ZXIiLCJhdWQiOiJkZW1vX2F1ZGllbmNlIn0.TQ-B19oGPB6O4oYrYy8DTCCvRLGeqaZFSD_SsnubMDo");

                
            }
            catch (HttpRequestException)
            {
                _ea.GetEvent<ConnectProgressString>().Publish("连接服务器失败");
                _ea.GetEvent<ConnectStatusChangeEvent>().Publish(ClientConnectStatus.ConnectError);
            }
        }

        public async void StartupCompare(string str)
        {
            if (str == "登录成功")
            {
                _RevNumberServer = await _langtextNet.GetLangTextRevisedNumberAsync(App.LangConfig.UserAuthToken);
                Debug.WriteLine("Server RevNumber: {0}", _RevNumberServer);
                if (_RevNumberServer != 0)
                {

                    await CompareDatabaseVersion();

                    await CompareDatabaseRevAsync();

                    await SyncUserInfo();

                    _ea.GetEvent<ConnectProgressString>().Unsubscribe(StartupCompare);
                }
            }
            
        }

        public async Task GetConfigFromDb()
        {
            //if (App.LangConfig.UserGuid != new Guid())
            //{
            //    var user = await _langTextRepo.GetUserInClient(App.LangConfig.UserGuid);
            //    var userDto = _mapper.Map<UserInClientDto>(user);

            //    App.User = userDto;

            //    //Debug.WriteLine("id: {0}, name: {1}", App.User.Id, App.User.UserNickName);
            //}

            //_RevNumberLocal = await _langTextRepo.GetLangtextRevNumber();
        }

        public async Task CompareServerConfig()
        {
            //var compareArray = new List<Task<bool>>()
            //{
            //    CompareUpdaterVersion(),
            //    CompareDatabaseVersion(),
            //    CompareDatabaseRevAsync()

            //};

            //Task<bool> compares = await Task.WhenAny(compareArray);


            



            //if (App.LangConfig.LangUpdaterVersion == App.LangConfigServer.LangUpdaterVersion
            //    && App.LangConfig.LangUpdaterDllSha256 == App.LangConfigServer.LangUpdaterDllSha256)
            //{
            //    if (App.LangConfig.LangEditorVersion != App.LangConfigServer.LangEditorVersion)
            //    {
            //        UpdateLangEditorVersion();
            //    }
            //    else if (CompareDatabaseVersion())
            //    {
            //        //_mainWindowViewModel.ProgressInfo = "发现全量数据库……";
            //        //_mainWindowViewModel.ShowImportDbRevUC(false);
            //    }
            //    else if (CompareDatabaseRevAsync())
            //    {
            //        //_mainWindowViewModel.ProgressInfo = "发现更新数据库……";
            //        //_mainWindowViewModel.ShowImportDbRevUC(true);
            //    }
            //    else
            //    {
            //        //_mainWindowViewModel.ProgressInfo = "启动检查通过。";
            //        //_mainWindowViewModel.ProgressbarVisibility = Visibility.Collapsed;
            //    }
            //}
            //else
            //{
            //    await DownloadUpdaterAsync();
            //}
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

        //private void UpdateLangEditorVersion()
        //{
        //    string args = App.ServerPath                      //服务器地址
        //        + App.LangConfigServer.LangFullpackPath           //编辑器包名
        //        + " " + "LangEditorFullUpdate.zip"         //下载本地文件名
        //        + " " + App.LangConfigServer.LangFullpackSHA256   //编辑器服务器端SHA256
        //        + " " + App.LangConfigServer.LangEditorVersion;   //编辑器服务器端版本号
        //    Debug.WriteLine(args);

        //    ProcessStartInfo startUpdaterInfo = new ProcessStartInfo
        //    {
        //        FileName = "ESO_LangEditorUpdater.exe",
        //        Arguments = args,
        //    };

        //    Process updater = new Process();
        //    updater.StartInfo = startUpdaterInfo;
        //    updater.Start();
        //    Application.Current.Shutdown();
        //}

        private async Task CompareUpdaterVersion()
        {
            //if (App.LangConfig.LangUpdaterVersion != App.LangConfigServer.LangUpdaterVersion
            //    || App.LangConfig.LangUpdaterDllSha256 != App.LangConfigServer.LangUpdaterDllSha256)
            //{
            //    _ea.GetEvent<ConnectProgressString>().Publish("正在下载更新器……");

            //    await _startupNetwork.DownloadUpdater(App.ServerPath + App.LangConfigServer.LangUpdaterPackPath);
            //    HashAndUnzipUpdaterPack();
            //}
        }

        private void HashAndUnzipUpdaterPack()
        {
            _ea.GetEvent<ConnectProgressString>().Publish("下载完成！");
            Debug.WriteLine("下载完成！");
            Task.Delay(1000);
            //if (GetFileExistAndSha256("UpdaterPack.zip", App.LangConfigServer.LangUpdaterPackSha256))
            //{
            //    _ea.GetEvent<ConnectProgressString>().Publish("SHA256校验通过，准备解压文件。");
            //    Debug.WriteLine("SHA256校验通过，准备解压文件。");
            //    ZipFile.ExtractToDirectory("UpdaterPack.zip", App.WorkingDirectory, true);

            //    App.LangConfig.LangUpdaterDllSha256 = App.LangConfigServer.LangUpdaterDllSha256;
            //    App.LangConfig.LangUpdaterVersion = App.LangConfigServer.LangUpdaterVersion;

            //    AppConfigClient.Save(App.LangConfig);

            //    File.Delete("UpdaterPack.zip");

            //    //CompareServerConfig();
            //}
            //else
            //    _ea.GetEvent<ConnectProgressString>().Publish("校验SHA256失败！");

        }

        private void CompareEditorVersion()
        {
            //if (App.LangConfig.LangEditorVersion != App.LangConfigServer.LangEditorVersion)
            //{
            //    string args = App.ServerPath                      //服务器地址
            //    + App.LangConfigServer.LangFullpackPath           //编辑器包名
            //    + " " + "LangEditorFullUpdate.zip"         //下载本地文件名
            //    + " " + App.LangConfigServer.LangFullpackSHA256   //编辑器服务器端SHA256
            //    + " " + App.LangConfigServer.LangEditorVersion;   //编辑器服务器端版本号
            //    Debug.WriteLine(args);

            //    ProcessStartInfo startUpdaterInfo = new ProcessStartInfo
            //    {
            //        FileName = "ESO_LangEditorUpdater.exe",
            //        Arguments = args,
            //    };
                

            //    Process updater = new Process
            //    {
            //        StartInfo = startUpdaterInfo
            //    };
            //    updater.Start();
            //    Application.Current.Shutdown();
            //}
        }


        //private bool CompareUpdaterVersion()
        //{
        //    return App.LangConfig.LangUpdaterVersion == App.LangConfigServer.LangUpdaterVersion
        //        && App.LangConfig.LangUpdaterDllSha256 == App.LangConfigServer.LangUpdaterDllSha256;
        //}

        private async Task OLD_CompareDatabaseRevAsync()
        {
            //if (App.LangConfig.DatabaseRev != App.LangConfigServer.LangDatabaseRevised)
            //{
            //    _ea.GetEvent<ConnectProgressString>().Publish("发现更新数据库……");
            //    _ea.GetEvent<DatabaseUpdateEvent>().Publish(true);

            //    int localRev = App.LangConfig.DatabaseRev;
            //    int serverRev = App.LangConfigServer.LangDatabaseRevised;

            //    RevCompareNum = serverRev - localRev;
            //    int count = 0;

            //    if (!Directory.Exists(DownloadFolder))
            //        Directory.CreateDirectory(DownloadFolder);

            //    var db = new LangTextRepository();

            //    for (int i = 1; i <= RevCompareNum; i++)
            //    {
            //        DatabaseRevDownloadList.Add(App.ServerPath + DatabaseRevPath + (localRev + i).ToString() + ".zip");
            //    }

            //    _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("共有 " + RevCompareNum.ToString() + " 个数据文件下载。");

            //    //CurrentExcuteText = "共有 " + RevCompareNum.ToString() + " 个数据文件下载。";

            //    foreach (var path in DatabaseRevDownloadList)
            //    {
            //        Debug.WriteLine(path);
            //        count++;

            //        _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("当前正在下载和导入第 " + count.ToString() + " 项，共有 " + RevCompareNum.ToString() + " 个数据文件下载。" +
            //            "\n如果窗口突然假死请勿强行结束程序，目前导入时会导致此问题。\n如果完成后没有自动退出此窗口，可以点击关闭按钮。");

            //        //CurrentExcuteText = "当前正在下载和导入第 " + count.ToString() + " 项，共有 " + RevCompareNum.ToString() + " 个数据文件下载。" +
            //        //    "\n如果窗口突然假死请勿强行结束程序，目前导入时会导致此问题。\n如果完成后没有自动退出此窗口，可以点击关闭按钮。";

            //        Uri uri = new Uri(path);
            //        string filename = Path.GetFileName(uri.LocalPath);

            //        using (WebClient client = new WebClient())
            //        {
            //            try
            //            {
            //                client.DownloadProgressChanged += Editor_DownloadProgressChanged;
            //                await client.DownloadFileTaskAsync(uri, DownloadFolder + filename);
            //                //client.DownloadFileCompleted += DatabaseZipExtractAndImport(DownloadFolder + filename);
            //            }

            //            catch (WebException ex)
            //            {
            //                Debug.WriteLine(ex.ToString());
            //            }

            //        }

            //        await DatabaseZipExtractAndImport(DownloadFolder + filename);


            //    }
            //}


        }

        private async Task CompareDatabaseRevAsync()
        {
            //if (_RevNumberLocal != _RevNumberServer)
            //{
            //    _ea.GetEvent<ConnectProgressString>().Publish("发现更新数据库……");
            //    _ea.GetEvent<DatabaseUpdateEvent>().Publish(true);

            //    int RevCount = _RevNumberServer - _RevNumberLocal;
            //    bool isServerNewer = _RevNumberServer > _RevNumberLocal;
            //    int revised = _RevNumberLocal + 1;
            //    Debug.WriteLine("Revised number: {0}", revised);

            //    if (isServerNewer)
            //    {
            //        Dictionary<Guid, ReviewReason> langtextDeletedDict = new Dictionary<Guid, ReviewReason>();
            //        Dictionary<Guid, ReviewReason> langtextAddedDict = new Dictionary<Guid, ReviewReason>();

            //        for (int i = 1; i <= RevCount; i++)
            //        {
            //            _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("正在下载需要同步的文本列表(" + i + "/" + RevCount + ")");
            //            //获取当前步进号的步进列表
            //            var langRevisedDto = await _langtextNet.GetLangTextRevisedDtosAsync(revised, App.LangConfig.UserAuthToken);

            //            if (langRevisedDto != null)
            //            {
            //                foreach (var rev in langRevisedDto)
            //                {
            //                    //Debug.WriteLine("Rev Number: ", rev.LangTextRevNumber);
            //                    //if (rev.ReasonFor == ReviewReason.Deleted)
            //                    //{
            //                    //    langtextDeletedDict.Add(rev.LangtextID, rev.ReasonFor);
            //                    //}

            //                    switch (rev.ReasonFor)
            //                    {
            //                        case ReviewReason.Deleted:
            //                            langtextDeletedDict.Add(rev.LangtextID, rev.ReasonFor);
            //                            break;
            //                        case ReviewReason.NewAdded:
            //                            langtextAddedDict.Add(rev.LangtextID, rev.ReasonFor);
            //                            break;
            //                    }
            //                }

            //                _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Publish("正在获取服务器文本");
            //                //查询langtext的步进编号
            //                var currentRevLangDto = await _langtextNet.GetLangtextByRevisedAsync(revised, App.LangConfig.UserAuthToken);
            //                if (currentRevLangDto != null)
            //                {
            //                    Debug.WriteLine("langtext from server count: {0}", currentRevLangDto.Count);

            //                    if (langtextAddedDict.Count >= 1)
            //                    {
            //                        List<LangTextClient> newLangtexts = new List<LangTextClient>();
            //                        List<LangTextDto> tempList = new List<LangTextDto>();
            //                        _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("分析并新增文本列表(" + i + "/" + RevCount + ")");
            //                        _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Publish("正在应用到数据库……");
            //                        foreach (var lang in currentRevLangDto)
            //                        {
            //                            if (langtextAddedDict.ContainsKey(lang.Id))
            //                            {
            //                                var langclient = _mapper.Map<LangTextClient>(lang);
            //                                newLangtexts.Add(langclient);
            //                                tempList.Add(lang);
            //                            }
            //                        }

            //                        foreach (var lang in tempList)
            //                        {
            //                            currentRevLangDto.Remove(lang);
            //                        }
            //                        await _langTextRepo.AddLangtexts(newLangtexts); //应用新增文本
            //                    }

            //                    if (currentRevLangDto != null && currentRevLangDto.Count >= 1)
            //                    {
            //                        _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("分析并更新文本列表(" + i + "/" + RevCount + ")");
            //                        var langtextToUpdateList = new List<LangTextClient>();
            //                        foreach (var lang in currentRevLangDto)
            //                        {
            //                            var langtext = await _langTextRepo.GetLangTextByGuidAsync(lang.Id);
            //                            _mapper.Map(lang, langtext, typeof(LangTextDto), typeof(LangTextClient));
            //                            langtextToUpdateList.Add(langtext);
            //                        }

            //                        //var updatedlang = _mapper.Map<List<LangTextClient>>(currentRevLangDto);
            //                        await _langTextRepo.UpdateLangtexts(langtextToUpdateList);   //应用修改文本
            //                    }
            //                }

            //                if (langtextDeletedDict.Count >= 1)
            //                {
            //                    _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("分析并应用删除文本列表(" + i + "/" + RevCount + ")");
            //                    //var deletedlangList = new List<LangTextClient>();

            //                    await _langTextRepo.DeleteLangtexts(langtextDeletedDict.Keys.ToList()); //应用删除文本

            //                }
            //                await _langTextRepo.UpdateRevNumber(revised);   //更新步进号
            //                revised++;
            //            }
            //            else
            //            {
            //                _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("文本列表下载失败！");
            //                _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Publish("请尝试重启工具");
            //            }
            //        }

            //        _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("更新完成！");
            //        _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Publish("如果本窗口没有自动关闭，请点击下边的关闭按钮");
            //        _ea.GetEvent<CloseMainWindowDrawerHostEvent>().Publish();
            //    }
            //}
        }

        private async Task SyncUserInfo()
        {
            //var userApi = new ApiAccess(App.ServerPath);
            //var userListFromServer = await userApi.GetUserList(App.LangConfig.UserAuthToken);

            //if (userListFromServer != null)
            //{
            //    var userList = _mapper.Map<List<UserInClient>>(userListFromServer);
            //    await _langTextRepo.UpdateUsers(userList);
            //}

        }


        private async Task DatabaseZipExtractAndImport(string filename)
        {
            List<string> fileList = new List<string>();
            LangFileParser parseLangFile = new LangFileParser();

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
                await ImportDataToDbAsync(parseLangFile.JsonToDtoReader(DownloadFolder + path));
            }
            TaskCount++;
            CompletedImportCheck();

        }

        private void CompletedImportCheck()
        {
            if (TaskCount == RevCompareNum)
            {
                var langconfig = App.LangConfig;
                //langconfig.DatabaseRev = App.LangConfigServer.LangDatabaseRevised;
                AppConfigClient.Save(langconfig);

                _ea.GetEvent<CloseMainWindowDrawerHostEvent>().Publish();

                //App.LangNetworkService.CompareServerConfig();
            }

        }

        private async Task ImportDataToDbAsync(JsonFileDto json)
        {
            //var db = new LangTextRepoClientService();
            //IMapper mapper = App.Mapper;

            //switch (json.ChangeType)
            //{
            //    case LangChangeType.Added:
            //        var listForAdd = mapper.Map<List<LangTextClient>>(json.LangTexts);
            //        await db.AddLangtexts(listForAdd);
            //        break;
            //    case LangChangeType.ChangedEN:
            //        var listForEnChanged = mapper.Map<List<LangTextForUpdateEnDto>>(json.LangTexts);
            //        await db.UpdateLangtextEn(listForEnChanged);
            //        break;
            //    case LangChangeType.ChangedZH:
            //        var listForZhChanged = mapper.Map<List<LangTextForUpdateZhDto>>(json.LangTexts);
            //        await db.UpdateLangtextZh(listForZhChanged);
            //        break;
            //    case LangChangeType.Removed:
            //        //var listForDelete = mapper.Map<List<LangTextClient>>(json.LangTexts);
            //        //await db.DeleteLangtexts(json.LangTexts.);
            //        break;
            //}
        }

        void Editor_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Publish("进度：" + e.ProgressPercentage
                + "%，已下载大小：" + SizeSuffix(e.BytesReceived)
                + "，总大小：" + SizeSuffix(e.TotalBytesToReceive));

            //DownloadSpeed = "进度："
            //    + e.ProgressPercentage
            //    + "%，已下载大小："
            //    + SizeSuffix(e.BytesReceived)
            //    + "，总大小："
            //    + SizeSuffix(e.TotalBytesToReceive);
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


        private async Task CompareDatabaseVersion()
        {
            //if (App.LangConfig.LangDatabaseVersion != App.LangConfigServer.LangDatabaseVersion)
            //{
            //    _ea.GetEvent<ConnectProgressString>().Publish("发现全量数据库……");
            //    _ea.GetEvent<DatabaseUpdateEvent>().Publish(false);

            //    string downloadPath = App.ServerPath + App.LangConfigServer.LangDatabasePath;

            //    Uri uri = new Uri(downloadPath);
            //    string filename = Path.GetFileName(uri.LocalPath);

            //    string localPath = App.WorkingDirectory + @"\" + filename;

            //    var fileExist = GetFileExistAndSha256(localPath, App.LangConfigServer.LangDatabasePackSha256);

            //    if (fileExist)
            //    {

            //        try
            //        {
            //            _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("正在处理数据库相关任务……");
            //            //CurrentExcuteText = "正在处理数据库相关任务……";

            //            _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Publish("正在解压……");

            //            //DownloadSpeed = "正在解压……";
            //            ZipFile.ExtractToDirectory(filename, App.WorkingDirectory, true);

            //            var dbCheck = new StartupDBCheck(@"Data\LangData_v3.db", @"Data\LangData_v3.update");

            //            _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Publish("对比数据库……");
            //            //DownloadSpeed = "对比数据库……";
            //            await dbCheck.ProcessUpdateMerge();

            //            _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Publish("完成");
            //            //DownloadSpeed = "完成";


            //            var langconfig = App.LangConfig;
            //            langconfig.LangDatabaseVersion = App.LangConfigServer.LangDatabaseVersion;
            //            AppConfigClient.Save(langconfig);
            //            App.LangNetworkService.CompareServerConfig();
            //            File.Delete(localPath);

            //            //DialogHost.CloseDialogCommand.Execute(null, null);
            //            //CloseButtonEnable = true;
            //            //CloseButtonVisibility = Visibility.Visible;

            //        }
            //        catch (Exception e)
            //        {
            //            MessageBox.Show("解压出错！" + Environment.NewLine + e.ToString());
            //        }
            //    }
            //    else
            //    {
            //        using (WebClient client = new WebClient())
            //        {
            //            try
            //            {
            //                _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("正在下载全量数据库，下载后会自动导出已翻译的内容。");
            //                //CurrentExcuteText = "正在下载全量数据库，下载后会自动导出已翻译的内容。";

            //                client.DownloadProgressChanged += Editor_DownloadProgressChanged;
            //                await client.DownloadFileTaskAsync(uri, localPath);
            //                //client.DownloadFileCompleted += DatabaseZipExtractAndImport(DownloadFolder + filename);
            //            }

            //            catch (WebException ex)
            //            {
            //                Debug.WriteLine(ex.ToString());
            //            }

            //        }

            //        //DatabaseVersionCal();


            //    }

            //}

        }
    }
}
