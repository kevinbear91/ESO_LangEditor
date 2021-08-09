using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.EventAggres;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public BackendService(IEventAggregator ea, ILangTextRepoClient langTextRepoClient,
            ILangTextAccess langTextAccess, IUserAccess userAccess, IMapper Mapper)
        {
            _langTextRepo = langTextRepoClient;
            _langTextAccess = langTextAccess;
            _userAccess = userAccess;
            _mapper = Mapper;
            _ea = ea;


            _ea.GetEvent<UploadLangtextZhUpdateEvent>().Subscribe(UploadlangtextUpdateZh);
        }

        

        public async Task ExportLangFile()
        {
            var langs = await _langTextRepo.GetAlltLangTextsDictionaryAsync(2);

            ILangFile langFile = new LangFile();

            await langFile.ExportToLang(langs);

        }

        public async Task LangtextZhUpdateUpload(LangTextForUpdateZhDto langTextUpdateZhDto)
        {
            var code = await _langTextAccess.UpdateLangTextZh(langTextUpdateZhDto);

            Debug.WriteLine("langID: {0}, langZh: {1}", langTextUpdateZhDto.Id, langTextUpdateZhDto.TextZh);

            if (code == ApiMessageWithCode.Success)
            {
                langTextUpdateZhDto.IsTranslated = 3;
                await _langTextRepo.UpdateLangtextZh(langTextUpdateZhDto);
            }

            //MainWindowMessageQueue.Enqueue("状态码：" + code.ToString());
        }

        public async Task LangtextZhUpdateUpload(List<LangTextForUpdateZhDto> langTextUpdateZhDtos)
        {
            var code = await _langTextAccess.UpdateLangTextZh(langTextUpdateZhDtos);

            if (code == ApiMessageWithCode.Success)
            {
                foreach (var lang in langTextUpdateZhDtos)
                {
                    lang.IsTranslated = 3;
                }
                await _langTextRepo.UpdateLangtextZh(langTextUpdateZhDtos);
            }

            //MainWindowMessageQueue.Enqueue("状态码：" + code.ToString());
        }

        //public Task StartupConnectSequenceCheck()
        //{
        //    IStartupCheck startupService = new StartupCheck();



        //}

        public async Task SyncToken()
        {
            var timer = new System.Threading.Timer(async e => await _userAccess.GetTokenByTokenDto(App.LangConfig.UserGuid, 
                new TokenDto
                {
                    AuthToken = App.LangConfig.UserAuthToken,
                    RefreshToken = App.LangConfig.UserRefreshToken,
                }),
            null, TimeSpan.Zero, TimeSpan.FromMinutes(10));


            //while (ConnectStatus == ClientConnectStatus.Login)
            //{
            //    await Task.Delay(TimeSpan.FromMinutes(10));

            //    await Task.Run(() =>
            //    {
            //        _accountService.LoginByToken();
            //    });

            //}
        }

        public async Task SyncUser()
        {
            Debug.WriteLine("SYNC USER");
            var userListFromServer = await _userAccess.GetUserList();

            if (userListFromServer != null)
            {
                var userList = _mapper.Map<List<UserInClient>>(userListFromServer);
                await _langTextRepo.UpdateUsers(userList);
            }
        }

        private async void UploadlangtextUpdateZh(LangTextForUpdateZhDto langTextUpdateZhDto)
        {

            var respond = await _langTextAccess.UpdateLangTextZh(langTextUpdateZhDto);

            //var code = await apiLangtext.UpdateLangtextZh(langTextUpdateZhDto, App.LangConfig.UserAuthToken);

            Debug.WriteLine("langID: {0}, langZh: {1}", langTextUpdateZhDto.Id, langTextUpdateZhDto.TextZh);

            if (respond == ApiMessageWithCode.Success)
            {
                langTextUpdateZhDto.IsTranslated = 3;
                await _langTextRepo.UpdateLangtextZh(langTextUpdateZhDto);
            }
            else
            {
                MessageBox.Show(respond.ApiMessageCodeString());
            }
        }

        //private async void UploadLangtextZhUpdate(LangTextForUpdateZhDto langTextUpdateZhDto)
        //{
        //    LangtextNetService apiLangtext = new LangtextNetService(App.ServerPath);
        //    LangTextRepoClientService langTextRepoClient = new LangTextRepoClientService();
        //    var code = await apiLangtext.UpdateLangtextZh(langTextUpdateZhDto, App.LangConfig.UserAuthToken);

        //    Debug.WriteLine("langID: {0}, langZh: {1}", langTextUpdateZhDto.Id, langTextUpdateZhDto.TextZh);

        //    if (code == System.Net.HttpStatusCode.OK || 
        //        code == System.Net.HttpStatusCode.Accepted ||
        //        code == System.Net.HttpStatusCode.Created)
        //    {
        //        langTextUpdateZhDto.IsTranslated = 3;
        //        await langTextRepoClient.UpdateLangtextZh(langTextUpdateZhDto);
        //    }

        //    MainWindowMessageQueue.Enqueue("状态码：" + code.ToString());

        //}

        //private async void UploadLangtextZhUpdate(List<LangTextForUpdateZhDto> langTextForUpdateZhDtoList)
        //{
        //    LangtextNetService apiLangtext = new LangtextNetService(App.ServerPath);
        //    LangTextRepoClientService langTextRepoClient = new LangTextRepoClientService();
        //    var code = await apiLangtext.UpdateLangtextZh(langTextForUpdateZhDtoList, App.LangConfig.UserAuthToken);

        //    if (code == System.Net.HttpStatusCode.OK ||
        //        code == System.Net.HttpStatusCode.Accepted ||
        //        code == System.Net.HttpStatusCode.Created)
        //    {
        //        foreach(var lang in langTextForUpdateZhDtoList)
        //        {
        //            lang.IsTranslated = 3;
        //        }
        //        await langTextRepoClient.UpdateLangtextZh(langTextForUpdateZhDtoList);
        //    }

        //    MainWindowMessageQueue.Enqueue("状态码：" + code.ToString());

        //}
    }
}
