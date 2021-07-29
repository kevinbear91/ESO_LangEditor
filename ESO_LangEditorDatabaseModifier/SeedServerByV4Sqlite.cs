using AutoMapper;
using ESO_LangEditor.Core.Models;
using ESO_LangEditorDatabaseModifier.Controller;
using ESO_LangEditorDatabaseModifier.Controller.Server;
using ESO_LangEditorDatabaseModifier.Model.v4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ESO_LangEditorDatabaseModifier
{
    public class SeedServerByV4Sqlite
    {
        private List<LangText> _langTextsv4;
        private List<ESO_LangEditor.Core.Entities.LangText> langTextsServer;
        private List<ESO_LangEditor.Core.Entities.User> userList;

        private LangTextRepository _langTextRepository = new LangTextRepository();


        IMapper mapper;

        public void SeedServerByV4SqliteDatabase()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<LangTextMappingProfile>();
            });

            mapper = new Mapper(config);

            userList = new List<ESO_LangEditor.Core.Entities.User>
            {
                new ESO_LangEditor.Core.Entities.User
                {
                    Id = new Guid(""),   
                    UserName = "",
                    UserNickName = "",
                    PasswordHash = "",
                    SecurityStamp = "",
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    TranslatedCount = 0,
                    InReviewCount = 0,
                    RemovedCount = 0,

                },
            };

            _langTextRepository.AddUsersToServer(userList);

            var userDict = _langTextRepository.GetUserAllFromServer();

            Debug.WriteLine("Dict Count: {0}", userDict.Count);

            foreach (var user in userDict)
            {
                Debug.WriteLine("Key: {0}, Id: {1}, Name: {2}", user.Key, user.Value.Id, user.Value.UserNickName);
            }

            //_langTextsv4 = _langTextRepository.GeAlltLangTexts_v4();

            List<ESO_LangEditor.Core.Entities.LangText> langTexts = new List<ESO_LangEditor.Core.Entities.LangText>();

            foreach (var lang in _langTextsv4)
            {
                if (userDict.TryGetValue(lang.UserId, out ESO_LangEditor.Core.Entities.User user1))
                {
                    langTexts.Add(new ESO_LangEditor.Core.Entities.LangText
                    {
                        Id = lang.Id,
                        TextId = lang.TextId,
                        IdType = lang.IdType,
                        TextEn = lang.TextEn,
                        TextZh = lang.TextZh,
                        LangTextType = (ESO_LangEditor.Core.EnumTypes.LangType)lang.LangTextType,
                        IsTranslated = (byte)3,
                        UpdateStats = lang.UpdateStats,
                        EnLastModifyTimestamp = lang.EnLastModifyTimestamp,
                        ZhLastModifyTimestamp = lang.ZhLastModifyTimestamp,
                        UserId = user1.Id,
                        ReviewerId = user1.Id,
                        ReviewTimestamp = DateTime.Now,
                        Revised = 0,
                    });
                }
            }

            //var ListToServer = mapper.Map<List<ESO_LangEditor.Core.Entities.LangText>>(_langTextsv4);

            Debug.WriteLine("lang list count: {0}", langTexts);

            _langTextRepository.AddLangTextsToServer(langTexts);

            Debug.WriteLine("save change done!");

        }
    }
}
