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
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditor.GUI.Services
{
    public class BackendService : IBackendService
    {

        private readonly ILangTextRepoClient _langTextRepo;
        private readonly ILangTextAccess _langTextAccess;
        private readonly IUserAccess _userAccess;
        private readonly IMapper _mapper;
        private readonly IEventAggregator _ea;
        private readonly ILogger _logger;

        //public AppConfigServer _appConfigServer;
        private string _localFileName;
        private string _fileSha256;

        public event EventHandler<string> SetSha256;
        public event EventHandler<string> SetAppConfigClientJpLangSha256;

        public BackendService(IEventAggregator ea, ILangTextRepoClient langTextRepoClient,
            ILangTextAccess langTextAccess, IUserAccess userAccess, IMapper Mapper,
            ILogger logger)
        {
            _langTextRepo = langTextRepoClient;
            _langTextAccess = langTextAccess;
            _userAccess = userAccess;
            _mapper = Mapper;
            _ea = ea;
            _logger = logger;

            _ea.GetEvent<UploadLangtextZhUpdateEvent>().Subscribe(UploadlangtextUpdateZh);
            _ea.GetEvent<UploadLangtextZhListUpdateEvent>().Subscribe(UploadlangtextsUpdateZh);
            _ea.GetEvent<RefreshTokenEvent>().Subscribe(SyncToken);
        }

        private async void SyncToken()
        {
            _logger.LogDebug("开始定期刷新Token");

            while (App.ConnectStatus == ClientConnectStatus.Login)
            {
                await Task.Delay(TimeSpan.FromMinutes(10)).ContinueWith(GetAuthToken);

                //await GetAuthToken();

            }

            //var startTimeSpan = TimeSpan.Zero;
            //var periodTimeSpan = TimeSpan.FromMinutes(2);

            //new System.Threading.Timer(async (e) => 
            //{
            //    await GetAuthToken();
            //},null, startTimeSpan, periodTimeSpan);

        }

        private async Task GetAuthToken(object o)
        {
            var token = await _userAccess.GetTokenByTokenDto(App.LangConfig.UserGuid,
                new TokenDto
                {
                    AuthToken = App.LangConfig.UserAuthToken,
                    RefreshToken = App.LangConfig.UserRefreshToken,
                });

            if (token != null)
            {
                _userAccess.SaveToken(token);
                //var timer = _userAccess.GetTokenExpireTimer();
                //_logger.LogDebug(timer.ToString());
                _logger.LogDebug("已刷新Token");
            }
            
        }


        private async void UploadlangtextUpdateZh(LangTextForUpdateZhDto langTextUpdateZhDto)
        {

            var respond = await _langTextAccess.UpdateLangTextZh(langTextUpdateZhDto);

            //var code = await apiLangtext.UpdateLangtextZh(langTextUpdateZhDto, App.LangConfig.UserAuthToken);

            Debug.WriteLine("langID: {0}, langZh: {1}", langTextUpdateZhDto.Id, langTextUpdateZhDto.TextZh);

            if (respond.Code == (int)RespondCode.Success)
            {
                langTextUpdateZhDto.IsTranslated = 3;
                await _langTextRepo.UpdateLangtextZh(langTextUpdateZhDto);
            }
            else
            {
                MessageBox.Show(respond.Message);
            }
        }

        private async void UploadlangtextsUpdateZh(List<LangTextForUpdateZhDto> langTextForUpdateZhDtoList)
        {

            var respond = await _langTextAccess.UpdateLangTextZh(langTextForUpdateZhDtoList);

            if (respond.Code == (int)RespondCode.Success)
            {
                foreach (var lang in langTextForUpdateZhDtoList)
                {
                    lang.IsTranslated = 3;
                }
                await _langTextRepo.UpdateLangtextZh(langTextForUpdateZhDtoList);
                //langTextUpdateZhDto.IsTranslated = 3;
                //await _langTextRepo.UpdateLangtextZh(langTextUpdateZhDto);
            }
            else
            {
                MessageBox.Show(respond.Message);
            }
        }

        public async Task DownloadFileFromServer(string downloadPath, string localFileName, string fileSha256)
        {
            _fileSha256 = fileSha256;
            _localFileName = localFileName;

            _ea.GetEvent<ConnectProgressString>().Publish($"正在下载 {localFileName}");

            using (WebClient client = new WebClient())
            {
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(DelegateHashAndUnzip);
                client.DownloadProgressChanged += Client_DownloadProgressChanged;
                await client.DownloadFileTaskAsync(new Uri(downloadPath), localFileName);
            }
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            _ea.GetEvent<ConnectProgressString>().Publish($"({e.ProgressPercentage}%) 正在下载 {_localFileName}");
        }

        private void DelegateHashAndUnzip(object sender, AsyncCompletedEventArgs e)
        {
            _ea.GetEvent<ConnectProgressString>().Publish($"{_localFileName} 下载完成！");
            Debug.WriteLine("下载完成！");

            Task.Delay(1000);

            if (GetFileExistAndSha256(_localFileName, _fileSha256))
            {
                _ea.GetEvent<ConnectProgressString>().Publish($"{_localFileName} SHA256校验通过，准备解压文件。");
                Debug.WriteLine("SHA256校验通过，准备解压文件。");

                ZipFile.ExtractToDirectory(_localFileName, App.WorkingDirectory, true);

                SetSha256(this, _fileSha256);

                File.Delete(_localFileName);
            }
            else
            {
                _ea.GetEvent<ConnectProgressString>().Publish($"校验 {_localFileName} SHA256失败！");
                _logger.LogCritical($"====={_localFileName} 更新失败======");
            }
        }

        public bool GetFileExistAndSha256(string filePath, string fileSHA265)
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

        public async Task<AppConfigServer> GetServerRespondAndConfig()
        {
            var _jsonOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

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

        //public async Task TestEvent()
        //{
        //    SetAppConfigClientJpLangSha256(this, "sha265here");
        //}
    }
}
