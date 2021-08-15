using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.EventAggres;
using Microsoft.Extensions.Logging;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditor.GUI.Services
{
    public class StartupCheck : IStartupCheck
    {
        private int RevCompareNum = 0;
        private int TaskCount = 0;

        private int _langRevNumberLocal = 0;
        private int _langRevNumberServer = 0;

        private int _userRevNumberLocal = 0;
        private int _userRevNumberServer = 0;


        private AppConfigServer _langConfigServer;

        private IEventAggregator _ea;
        private ILangTextRepoClient _langTextRepo;
        private ILangTextAccess _langTextAccess;
        private IUserAccess _userAccess;
        private IMapper _mapper;
        private JsonSerializerOptions _jsonOption;
        private ILogger _logger;


        public StartupCheck(IEventAggregator ea, ILangTextRepoClient langTextRepo,
            ILangTextAccess langTextAccess, IUserAccess userAccess, IMapper Mapper, ILogger logger)
        {
            _ea = ea;
            _langTextRepo = langTextRepo;
            _langTextAccess = langTextAccess;
            _userAccess = userAccess;
            _mapper = Mapper;
            _logger = logger;

            _jsonOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };


        }

        public async Task StartupTaskList()
        {
            _ea.GetEvent<ConnectProgressString>().Publish("正在读取服务器版本数据……");
            _ea.GetEvent<ConnectStatusChangeEvent>().Publish(ClientConnectStatus.Connecting);

            _langConfigServer = await GetServerRespondAndConfig();


            if (_langConfigServer != null)
            {

                _logger.LogDebug("获取服务器配置成功");

                if (App.LangConfig.LangUpdaterVersion != _langConfigServer.LangUpdaterVersion 
                    || !GetFileExistAndSha256("ESO_LangEditorUpdater.exe", _langConfigServer.LangUpdaterSha256))
                {
                    await UpdateUpdater();
                }
                else
                {
                    _logger.LogDebug("更新器版本已最新");
                }

                if (App.LangConfig.LangEditorVersion != _langConfigServer.LangEditorVersion)
                {
                    UpdateEditor();
                }
                else
                {
                    _logger.LogDebug("编辑器版本已最新");
                }

            }
        }

        public async Task Login()
        {
            if (App.LangConfig.UserAuthToken != "" & App.LangConfig.UserRefreshToken != "")
            {
                var Logintoken = await _userAccess.GetTokenByTokenDto(App.LangConfig.UserGuid, new TokenDto
                {
                    AuthToken = App.LangConfig.UserAuthToken,
                    RefreshToken = App.LangConfig.UserRefreshToken,
                });

                if (Logintoken != null)
                {
                    _logger.LogDebug("登录并获取Token成功");

                    _userAccess.SaveToken(Logintoken);
                    await LoginTaskList();
                }
                else
                {
                    _ea.GetEvent<LoginRequiretEvent>().Publish();
                }
            }

            if (App.LangConfig.UserAuthToken == "" || App.LangConfig.UserRefreshToken == "")
            {
                _ea.GetEvent<LoginRequiretEvent>().Publish();
            }
        }

        public async Task LoginTaskList()
        {
            var revDto = await _langTextAccess.GetAllRevisedNumber();

            foreach (var rev in revDto)
            {
                if (rev.Id == 1)    //ID 1 为 Langtext rev
                {
                    _langRevNumberServer = rev.Rev;
                }

                if (rev.Id == 2)    //ID 2 为 User rev
                {
                    _userRevNumberServer = rev.Rev;
                }
            }

            _logger.LogDebug($"langRevNumberServer 为 {_langRevNumberServer}, userRevNumberServer 为 {_userRevNumberServer}");

            await GetConfigFromDb();

            if (_userRevNumberLocal != _userRevNumberServer)
            {
                await SyncUsers();
            }

            if (_langRevNumberServer != 0 && _langRevNumberLocal != _langRevNumberServer)
            {
                _logger.LogDebug("本地步进号：" + _langRevNumberLocal);
                _logger.LogDebug("服务器端步进号：" + _langRevNumberServer);
                await SyncRevDatabase();
            }
            else
            {
                _logger.LogDebug("步进号已最新");
                _ea.GetEvent<ConnectProgressString>().Publish("启动检查完成");
                _ea.GetEvent<ConnectStatusChangeEvent>().Publish(ClientConnectStatus.Login);
            }
            //_ea.GetEvent<LoginFromUcEvent>().Unsubscribe(LoginTaskCallFromUC);
        }

        public void UpdateEditor()
        {
            _logger.LogDebug("发现编辑器版本更新");

            string args = App.ServerPath + _langConfigServer.LangFullpackPath    //服务器地址 + 编辑器包名
                + " " + _langConfigServer.LangFullpackSHA256   //编辑器服务器端SHA256
                + " " + _langConfigServer.LangEditorVersion;   //编辑器服务器端版本号
            Debug.WriteLine(args);

            ProcessStartInfo startUpdaterInfo = new ProcessStartInfo
            {
                FileName = "ESO_LangEditorUpdater.exe",
                Arguments = args,
            };

            Process updater = new Process
            {
                StartInfo = startUpdaterInfo
            };
            updater.Start();
            Application.Current.Shutdown();

        }

        public async Task UpdateUpdater()
        {
            _logger.LogDebug("发现更新器版本更新");
            _ea.GetEvent<ConnectProgressString>().Publish("正在下载更新器……");

            using (WebClient client = new WebClient())
            {
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(DelegateHashAndUnzip);
                await client.DownloadFileTaskAsync(new Uri(App.ServerPath + _langConfigServer.LangUpdaterPackPath),
                "UpdaterPack.zip");
            }
        }

        private void DelegateHashAndUnzip(object sender, AsyncCompletedEventArgs e)
        {
            _ea.GetEvent<ConnectProgressString>().Publish("下载完成！");
            Debug.WriteLine("下载完成！");

            Task.Delay(1000);

            if (GetFileExistAndSha256("UpdaterPack.zip", _langConfigServer.LangUpdaterPackSha256))
            {
                _ea.GetEvent<ConnectProgressString>().Publish("SHA256校验通过，准备解压文件。");
                Debug.WriteLine("SHA256校验通过，准备解压文件。");

                ZipFile.ExtractToDirectory("UpdaterPack.zip", App.WorkingDirectory, true);

                //App.LangConfig.LangUpdaterSha256 = _langConfigServer.LangUpdaterSha256;
                App.LangConfig.LangUpdaterVersion = _langConfigServer.LangUpdaterVersion;

                AppConfigClient.Save(App.LangConfig);

                File.Delete("UpdaterPack.zip");
            }
            else
            {
                _ea.GetEvent<ConnectProgressString>().Publish("校验SHA256失败！");
                _logger.LogCritical("=====新版更新器更新失败======");
            }
        }

        public async Task SyncUsers()
        {
            Debug.WriteLine("SYNC USER");
            var userListFromServer = await _userAccess.GetUserList();

            if (userListFromServer != null)
            {
                var userList = _mapper.Map<List<UserInClient>>(userListFromServer);
                await _langTextRepo.UpdateUsers(userList);

                if (await _langTextRepo.UpdateRevNumber(2, _userRevNumberServer))
                {
                    _logger.LogDebug("用户同步完成");
                }
            }
        }

        public Task DownloadFullDatabase()
        {
            throw new NotImplementedException();
        }

        public async Task SyncRevDatabase()
        {
            _ea.GetEvent<ConnectProgressString>().Publish("发现更新数据库……");
            _ea.GetEvent<DatabaseUpdateEvent>().Publish(true);

            int RevCount = _langRevNumberServer - _langRevNumberLocal;
            int revised = _langRevNumberLocal + 1;

            Debug.WriteLine("Revised number: {0}", revised);
            _logger.LogDebug("步进计数：" + RevCount);
            _logger.LogDebug("当前新步进号：" + revised);

            Dictionary<Guid, ReviewReason> langtextDeletedDict = new Dictionary<Guid, ReviewReason>();
            Dictionary<Guid, ReviewReason> langtextAddedDict = new Dictionary<Guid, ReviewReason>();
            Dictionary<Guid, ReviewReason> langtextUpdateDict = new Dictionary<Guid, ReviewReason>();

            for (int i = 1; i <= RevCount; i++)
            {
                _logger.LogDebug("当前步进：" + i);
                _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("正在下载需要同步的文本列表(" + i + "/" + RevCount + ")");
                //获取当前步进号的步进列表
                var langRevisedDto = await _langTextAccess.GetLangTextRevisedDtos(revised);

                if (langRevisedDto != null)
                {
                    _logger.LogDebug("当前langRevisedDto数量：" + langRevisedDto.Count());
                    foreach (var rev in langRevisedDto)
                    {
                        switch (rev.ReasonFor)
                        {
                            case ReviewReason.Deleted:
                                langtextDeletedDict.Add(rev.LangtextID, rev.ReasonFor);
                                break;
                            case ReviewReason.NewAdded:
                                langtextAddedDict.Add(rev.LangtextID, rev.ReasonFor);
                                break;
                            case ReviewReason.EnChanged:
                                langtextUpdateDict.Add(rev.LangtextID, rev.ReasonFor);
                                break;
                            case ReviewReason.ZhChanged:
                                langtextUpdateDict.Add(rev.LangtextID, rev.ReasonFor);
                                break;
                        }
                    }

                    _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Publish("正在获取服务器文本");
                    //查询langtext的步进编号
                    var currentRevLangDto = await _langTextAccess.GetLangTexts(revised);
                    if (currentRevLangDto != null)
                    {
                        Debug.WriteLine("langtext from server count: {0}", currentRevLangDto.Count);
                        _logger.LogDebug("当前步进服务器端lang文本数量：" + currentRevLangDto.Count);

                        List<LangTextClient> newLangtexts = new List<LangTextClient>();
                        List<LangTextClient> langtextToUpdateList = new List<LangTextClient>();

                        foreach (var lang in currentRevLangDto)
                        {
                            if (langtextAddedDict.ContainsKey(lang.Id))
                            {
                                var langclient = _mapper.Map<LangTextClient>(lang);
                                newLangtexts.Add(langclient);
                            }

                            if (langtextUpdateDict.ContainsKey(lang.Id))
                            {
                                var langtext = await _langTextRepo.GetLangTextByGuidAsync(lang.Id);
                                _mapper.Map(lang, langtext, typeof(LangTextDto), typeof(LangTextClient));
                                langtextToUpdateList.Add(langtext);
                            }
                        }

                        _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("更新文本列表正在写入数据库(" + i + "/" + RevCount + ")");

                        if (await _langTextRepo.AddLangtexts(newLangtexts))//应用新增文本
                        {
                            _logger.LogDebug("当前步进新增：" + newLangtexts.Count);
                            _logger.LogDebug("当前步进新增文本完成");
                        }

                        if (await _langTextRepo.UpdateLangtexts(langtextToUpdateList))//应用修改文本
                        {
                            _logger.LogDebug("当前步进修改：" + langtextToUpdateList.Count);
                            _logger.LogDebug("当前步进修改文本完成");
                        }

                        if (langtextDeletedDict.Count >= 1)
                        {
                            if (await _langTextRepo.DeleteLangtexts(langtextDeletedDict.Keys.ToList()))//应用删除文本
                            {
                                _logger.LogDebug("当前步进删除：" + langtextDeletedDict.Count);
                                _logger.LogDebug("当前步进删除文本完成");
                            }
                        }

                        if (await _langTextRepo.UpdateRevNumber(1, revised))    //更新步进号
                        {
                            _logger.LogDebug("当前步进号新增完成");
                            revised++;
                        }

                        langtextDeletedDict.Clear();
                        langtextAddedDict.Clear();
                        langtextUpdateDict.Clear();
                    }
                }
                else
                {
                    _logger.LogCritical("====当前步进号文本列表下载失败====");
                    _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("文本列表下载失败！");
                    _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Publish("请尝试重启工具");
                }
                _logger.LogDebug("全部步进号更新完成");
                _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("更新完成！");
                _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Publish("如果本窗口没有自动关闭，请点击下边的关闭按钮");
                _ea.GetEvent<CloseMainWindowDrawerHostEvent>().Publish();
            }
        }

        private async Task<AppConfigServer> GetServerRespondAndConfig()
        {
            AppConfigServer result = null;
            HttpClient client = App.HttpClient;

            HttpResponseMessage response = await client.GetAsync("AppConfig.json");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<AppConfigServer>(responseContent, _jsonOption);
            }
            return result;
        }

        private async Task GetConfigFromDb()
        {
            if (App.LangConfig.UserGuid != new Guid())
            {
                var user = await _langTextRepo.GetUserInClient(App.LangConfig.UserGuid);
                var userDto = _mapper.Map<UserInClientDto>(user);

                App.User = userDto;

                //Debug.WriteLine("id: {0}, name: {1}", App.User.Id, App.User.UserNickName);
            }

            var revList = await _langTextRepo.GetLangtextRevNumber();

            foreach (var rev in revList)
            {
                if (rev.Id == 1)
                {
                    _langRevNumberLocal = rev.Rev;
                }

                if (rev.Id == 2)
                {
                    _userRevNumberLocal = rev.Rev;
                }
            }

            _logger.LogDebug($"langRevNumberLocal 为 {_langRevNumberLocal}, userRevNumberLocal 为 {_userRevNumberLocal}");
        }

        private static bool GetFileExistAndSha256(string filePath, string fileSHA265)
        {
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


    }
}
