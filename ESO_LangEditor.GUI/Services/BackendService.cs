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
            _ea.GetEvent<UploadLangtextZhListUpdateEvent>().Subscribe(UploadlangtextsUpdateZh);
            _ea.GetEvent<RefreshTokenEvent>().Subscribe(SyncToken);
        }

        public async Task LangtextZhUpdateUpload(LangTextForUpdateZhDto langTextUpdateZhDto)
        {
            var code = await _langTextAccess.UpdateLangTextZh(langTextUpdateZhDto);

            Debug.WriteLine("langID: {0}, langZh: {1}", langTextUpdateZhDto.Id, langTextUpdateZhDto.TextZh);

            if (code.Code == (int)RespondCode.Success)
            {
                langTextUpdateZhDto.IsTranslated = 3;
                await _langTextRepo.UpdateLangtextZh(langTextUpdateZhDto);
            }

            //MainWindowMessageQueue.Enqueue("状态码：" + code.ToString());
        }

        public async Task LangtextZhUpdateUpload(List<LangTextForUpdateZhDto> langTextUpdateZhDtos)
        {
            var code = await _langTextAccess.UpdateLangTextZh(langTextUpdateZhDtos);

            if (code.Code == (int)RespondCode.Success)
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

        public void SyncToken()
        {
            var timer = new System.Threading.Timer(async e => await _userAccess.GetTokenByTokenDto(App.LangConfig.UserGuid, 
                new TokenDto
                {
                    AuthToken = App.LangConfig.UserAuthToken,
                    RefreshToken = App.LangConfig.UserRefreshToken,
                }),
            null, TimeSpan.Zero, TimeSpan.FromMinutes(10));

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
