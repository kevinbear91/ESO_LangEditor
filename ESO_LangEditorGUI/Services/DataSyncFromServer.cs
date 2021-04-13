using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.NetClient;
using ESO_LangEditorGUI.EventAggres;
using ESO_LangEditorGUI.Services.AccessServer;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditorGUI.Services
{
    public class DataSyncFromServer
    {
        private LangTextRepoClientService _langTextRepo = new LangTextRepoClientService();
        private LangtextNetService _langtextNetService = new LangtextNetService(App.ServerPath);
        private IMapper _mapper;

        IEventAggregator _ea;

        public DataSyncFromServer(IEventAggregator ea)
        {
            _ea = ea;
            _mapper = App.Mapper;
        }

        public async Task UpdateUsers()
        {
            AccountService accountService = new AccountService(_ea);
            Dictionary<Guid, UserInClient> usersFromClient = new Dictionary<Guid, UserInClient>();
            Dictionary<Guid, UserInClientDto> usersFromServerDto;
            var users = _langTextRepo.GetUsers();
            var userFromServer = await accountService.GetUserList();
            usersFromClient = users.ToDictionary(u => u.Id);
            usersFromServerDto = userFromServer.ToDictionary(u => u.Id);

            var usersFromServer = _mapper.Map<Dictionary<Guid, UserInClient>>(usersFromServerDto);

            foreach (var user in usersFromServer)
            {
                if (!usersFromClient.ContainsKey(user.Key))
                {
                    usersFromClient.Add(user.Key, user.Value);
                }
                else
                {
                    var clientNewValue = _mapper.Map<UserInClient>(user.Value);
                    usersFromClient[user.Key] = clientNewValue;
                }
            }

            var userUpdateList = usersFromClient.Values.ToList();

            if(!await _langTextRepo.UpdateUsers(userUpdateList))
            {
                _ea.GetEvent<ConnectProgressString>().Publish("用户列表同步失败！");
            }
        }

        public async Task UpdateLangText()
        {
            int RevNumberServer = await _langtextNetService.GetLangTextRevisedNumberAsync(App.LangConfig.UserAuthToken);
            int RevNumberClient = await _langTextRepo.GetLangtextRevNumber();
            int RevCount = RevNumberServer - RevNumberClient;
            bool isServerNewer = RevNumberServer > RevNumberClient;
            int revised = RevNumberClient++;

            if (isServerNewer)
            {
                Dictionary<Guid, ReviewReason> langtextDeletedDict = new Dictionary<Guid, ReviewReason>();
                Dictionary<Guid, ReviewReason> langtextAddedDict = new Dictionary<Guid, ReviewReason>();

                for (int i = 1; i <= RevCount; i++)
                {
                    _ea.GetEvent<ConnectProgressString>().Publish("正在下载需要同步的文本列表(" + i + "/" + RevCount + ")");
                    var langRevisedDto = await _langtextNetService.GetLangTextRevisedDtosAsync(revised, App.LangConfig.UserAuthToken);

                    foreach(var rev in langRevisedDto)
                    {
                        if (rev.ReasonFor == ReviewReason.Deleted)
                        {
                            langtextDeletedDict.Add(rev.LangtextID, rev.ReasonFor);
                        }

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

                    var currentRevLangDto = await _langtextNetService.GetLangtextByRevisedAsync(revised, App.LangConfig.UserAuthToken);

                    if (langtextAddedDict != null && langtextAddedDict.Count >= 1)
                    {
                        List<LangTextClient> newLangtexts = new List<LangTextClient>();
                        _ea.GetEvent<ConnectProgressString>().Publish("分析并新增文本列表(" + i + "/" + RevCount + ")");

                        foreach (var lang in currentRevLangDto)
                        {
                            if (langtextAddedDict.ContainsKey(lang.Id))
                            {
                                newLangtexts.Add(_mapper.Map<LangTextClient>(lang));
                                currentRevLangDto.Remove(lang);
                            }    
                        }
                        await _langTextRepo.AddLangtexts(newLangtexts);
                    }

                    if (currentRevLangDto != null && currentRevLangDto.Count >= 1)
                    {
                        var updatedlang = _mapper.Map<List<LangTextClient>>(currentRevLangDto);
                        _ea.GetEvent<ConnectProgressString>().Publish("分析并更新文本列表(" + i + "/" + RevCount + ")");
                        await _langTextRepo.UpdateLangtexts(updatedlang);
                    }
                    revised++;
                }
            }
        }

        public int GetLocalUserCount()
        {
            return _langTextRepo.GetUsers().Count;
        }



    }
}
