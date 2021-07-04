using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.EventAggres;
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
    public class StartupCheck : IStartupCheck
    {
        private int RevCompareNum = 0;
        private int TaskCount = 0;

        private int _RevNumberLocal = 0;
        private int _RevNumberServer = 0;

        private IEventAggregator _ea;
        private ILangTextRepoClient _langTextRepo;
        private ILangTextAccess _langTextAccess;
        private IUserAccess _userAccess;
        private IMapper _mapper;
        private JsonSerializerOptions _jsonOption;

        public StartupCheck(IEventAggregator ea, ILangTextRepoClient langTextRepo, 
            ILangTextAccess langTextAccess, IUserAccess userAccess, IMapper Mapper)
        {
            _ea = ea;
            _langTextRepo = langTextRepo;
            _langTextAccess = langTextAccess;
            _userAccess = userAccess;
            _mapper = Mapper;

            _jsonOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task TaskList()
        {
            List<Task> tasks = new List<Task>();


            _ea.GetEvent<ConnectProgressString>().Publish("正在读取服务器版本数据……");
            _ea.GetEvent<ConnectStatusChangeEvent>().Publish(ClientConnectStatus.Connecting);

            var serverConfig = await GetServerRespondAndConfig();

            if (serverConfig != null)
            {
                if (App.LangConfig.LangEditorVersion != App.LangConfigServer.LangEditorVersion)
                {
                    UpdateEditor();
                }

                //if ()
            }

            



        }


        public bool CompareDatabaseRevNumber()
        {
            return _RevNumberLocal < _RevNumberServer;
        }

        public void UpdateEditor()
        {
            //if (App.LangConfig.LangEditorVersion != App.LangConfigServer.LangEditorVersion)
            //{
                
            //}

            string args = App.ServerPath + App.LangConfigServer.LangFullpackPath    //服务器地址 + 编辑器包名
                + " " + App.LangConfigServer.LangFullpackSHA256   //编辑器服务器端SHA256
                + " " + App.LangConfigServer.LangEditorVersion;   //编辑器服务器端版本号
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
            if (App.LangConfig.LangUpdaterVersion != App.LangConfigServer.LangUpdaterVersion
                || App.LangConfig.LangUpdaterDllSha256 != App.LangConfigServer.LangUpdaterDllSha256)
            {
                
            }

            _ea.GetEvent<ConnectProgressString>().Publish("正在下载更新器……");

            await DownloadUpdater();
            //HashAndUnzipUpdaterPack();
        }

        public Task DownloadFullDatabase()
        {
            throw new NotImplementedException();
        }

        public async Task SyncRevDatabase()
        {
            if (_RevNumberLocal != _RevNumberServer)
            {
                
            }

            _ea.GetEvent<ConnectProgressString>().Publish("发现更新数据库……");
            _ea.GetEvent<DatabaseUpdateEvent>().Publish(true);

            int RevCount = _RevNumberServer - _RevNumberLocal;
            //bool isServerNewer = _RevNumberServer > _RevNumberLocal;
            int revised = _RevNumberLocal + 1;
            Debug.WriteLine("Revised number: {0}", revised);

            //if (isServerNewer)
            //{
                
            //}

            Dictionary<Guid, ReviewReason> langtextDeletedDict = new Dictionary<Guid, ReviewReason>();
            Dictionary<Guid, ReviewReason> langtextAddedDict = new Dictionary<Guid, ReviewReason>();

            for (int i = 1; i <= RevCount; i++)
            {
                _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("正在下载需要同步的文本列表(" + i + "/" + RevCount + ")");
                //获取当前步进号的步进列表
                var langRevisedDto = await _langTextAccess.GetLangTextRevisedDtos(revised);

                if (langRevisedDto != null)
                {
                    foreach (var rev in langRevisedDto)
                    {
                        //Debug.WriteLine("Rev Number: ", rev.LangTextRevNumber);
                        //if (rev.ReasonFor == ReviewReason.Deleted)
                        //{
                        //    langtextDeletedDict.Add(rev.LangtextID, rev.ReasonFor);
                        //}

                        switch (rev.ReasonFor)
                        {
                            case ReviewReason.Deleted:
                                langtextDeletedDict.Add(rev.LangtextID, rev.ReasonFor);
                                break;
                            case ReviewReason.NewAdded:
                                langtextAddedDict.Add(rev.LangtextID, rev.ReasonFor);
                                break;
                        }
                    }

                    _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Publish("正在获取服务器文本");
                    //查询langtext的步进编号
                    var currentRevLangDto = await _langTextAccess.GetLangTexts(revised);
                    if (currentRevLangDto != null)
                    {
                        Debug.WriteLine("langtext from server count: {0}", currentRevLangDto.Count);

                        if (langtextAddedDict.Count >= 1)
                        {
                            List<LangTextClient> newLangtexts = new List<LangTextClient>();
                            List<LangTextDto> tempList = new List<LangTextDto>();
                            _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("分析并新增文本列表(" + i + "/" + RevCount + ")");
                            _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Publish("正在应用到数据库……");
                            foreach (var lang in currentRevLangDto)
                            {
                                if (langtextAddedDict.ContainsKey(lang.Id))
                                {
                                    var langclient = _mapper.Map<LangTextClient>(lang);
                                    newLangtexts.Add(langclient);
                                    tempList.Add(lang);
                                }
                            }

                            foreach (var lang in tempList)
                            {
                                currentRevLangDto.Remove(lang);
                            }
                            await _langTextRepo.AddLangtexts(newLangtexts); //应用新增文本
                        }

                        if (currentRevLangDto != null && currentRevLangDto.Count >= 1)
                        {
                            _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("分析并更新文本列表(" + i + "/" + RevCount + ")");
                            var langtextToUpdateList = new List<LangTextClient>();
                            foreach (var lang in currentRevLangDto)
                            {
                                var langtext = await _langTextRepo.GetLangTextByGuidAsync(lang.Id);
                                _mapper.Map(lang, langtext, typeof(LangTextDto), typeof(LangTextClient));
                                langtextToUpdateList.Add(langtext);
                            }

                            //var updatedlang = _mapper.Map<List<LangTextClient>>(currentRevLangDto);
                            await _langTextRepo.UpdateLangtexts(langtextToUpdateList);   //应用修改文本
                        }
                    }

                    if (langtextDeletedDict.Count >= 1)
                    {
                        _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("分析并应用删除文本列表(" + i + "/" + RevCount + ")");
                        //var deletedlangList = new List<LangTextClient>();

                        await _langTextRepo.DeleteLangtexts(langtextDeletedDict.Keys.ToList()); //应用删除文本

                    }
                    await _langTextRepo.UpdateRevNumber(revised);   //更新步进号
                    revised++;
                }
                else
                {
                    _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("文本列表下载失败！");
                    _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Publish("请尝试重启工具");
                }
                _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Publish("更新完成！");
                _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Publish("如果本窗口没有自动关闭，请点击下边的关闭按钮");
                _ea.GetEvent<CloseMainWindowDrawerHostEvent>().Publish();
            }
        }

        public async Task SyncUserInfo()
        {
            //var userApi = new ApiAccess(App.ServerPath);
            //var userListFromServer = await _userAccess.GetUserList(App.LangConfig.UserAuthToken);

            //if (userListFromServer != null)
            //{
            //    var userList = _mapper.Map<List<UserInClient>>(userListFromServer);
            //    await _langTextRepo.UpdateUsers(userList);
            //}
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

            _RevNumberLocal = await _langTextRepo.GetLangtextRevNumber();
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

                //CompareServerConfig();
            }
            else
                _ea.GetEvent<ConnectProgressString>().Publish("校验SHA256失败！");

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

        private async Task DownloadUpdater()
        {
            _ea.GetEvent<ConnectProgressString>().Publish("正在下载更新器……");

            using var client = new WebClient();
            //client.DownloadFileAsync(new Uri(uri), "UpdaterPack.zip");
            await client.DownloadFileTaskAsync(new Uri(App.ServerPath + App.LangConfigServer.LangUpdaterPackPath), 
                "UpdaterPack.zip");

            HashAndUnzipUpdaterPack();
        }

    }
}
