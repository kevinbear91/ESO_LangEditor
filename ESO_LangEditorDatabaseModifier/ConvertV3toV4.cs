using ESO_LangEditorDatabaseModifier.Controller;
using ESO_LangEditorDatabaseModifier.Model.v3;
using ESO_LangEditorDatabaseModifier.Model.v4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ESO_LangEditorDatabaseModifier
{
    public class ConvertV3toV4
    {

        private List<LangTextDto_v3> _langTextsv3;
        private List<LangText> _langTextsv4;

        private LangTextRepository _langTextRepository = new LangTextRepository();


        public void Convertv3Tov4()
        {

            _langTextsv4 = new List<LangText>();

            _langTextsv3 = _langTextRepository.GeAlltLangTexts_v3();

            

            //_langTextRepository.AddUsersToV4(users);

           

            var userDict = _langTextRepository.GetUserAllToV4();

            Debug.WriteLine("Dict Count: {0}", userDict.Count);

            foreach(var user in userDict)
            {
                Debug.WriteLine("Key: {0}, Id: {1}, Name: {2}", user.Key, user.Value.Id, user.Value.UserNickName);
            }

            foreach (var langv3 in _langTextsv3)
            {
               

                if(userDict.TryGetValue(langv3.UserId, out UserInClient user1))
                {
                    Debug.WriteLineIf(user1 == null, "lang Id: " + langv3.Id
                    + "text Id: " + langv3.TextId
                    + "zh: " + langv3.TextZh
                    + "User Id: " + langv3.UserId.ToString());

                    _langTextsv4.Add(new LangText
                    {
                        Id = langv3.Id,
                        TextId = langv3.TextId,
                        IdType = langv3.IdType,
                        TextEn = langv3.TextEn,
                        TextZh = langv3.TextZh,
                        LangTextType = (Model.v4.LangType)langv3.LangLuaType,
                        IsTranslated = (byte)langv3.IsTranslated,
                        UpdateStats = langv3.UpdateStats,
                        EnLastModifyTimestamp = langv3.EnLastModifyTimestamp,
                        ZhLastModifyTimestamp = langv3.ZhLastModifyTimestamp,
                        UserId = user1.Id,
                        ReviewerId = user1.Id,
                        ReviewTimestamp = DateTime.Now,
                    });

                }
               
            }

            Debug.WriteLine(_langTextsv3.Count);


            Debug.WriteLine("Convert Done!");

            _langTextRepository.AddNewLangsToV4(_langTextsv4);
        }


    }
}
