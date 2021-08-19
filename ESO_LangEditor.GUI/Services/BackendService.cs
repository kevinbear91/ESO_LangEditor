using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.EventAggres;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

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

        //public async Task LangtextZhUpdateUpload(LangTextForUpdateZhDto langTextUpdateZhDto)
        //{
        //    var code = await _langTextAccess.UpdateLangTextZh(langTextUpdateZhDto);

        //    Debug.WriteLine("langID: {0}, langZh: {1}", langTextUpdateZhDto.Id, langTextUpdateZhDto.TextZh);

        //    if (code.Code == (int)RespondCode.Success)
        //    {
        //        langTextUpdateZhDto.IsTranslated = 3;
        //        await _langTextRepo.UpdateLangtextZh(langTextUpdateZhDto);
        //    }

        //    //MainWindowMessageQueue.Enqueue("状态码：" + code.ToString());
        //}

        //public async Task LangtextZhUpdateUpload(List<LangTextForUpdateZhDto> langTextUpdateZhDtos)
        //{
        //    var code = await _langTextAccess.UpdateLangTextZh(langTextUpdateZhDtos);

        //    if (code.Code == (int)RespondCode.Success)
        //    {
        //        foreach (var lang in langTextUpdateZhDtos)
        //        {
        //            lang.IsTranslated = 3;
        //        }
        //        await _langTextRepo.UpdateLangtextZh(langTextUpdateZhDtos);
        //    }

        //    //MainWindowMessageQueue.Enqueue("状态码：" + code.ToString());
        //}

        //public Task StartupConnectSequenceCheck()
        //{
        //    IStartupCheck startupService = new StartupCheck();



        //}


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

    }
}
