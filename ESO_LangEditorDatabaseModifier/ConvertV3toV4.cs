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


            foreach(var langv3 in _langTextsv3)
            {
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
                    UserId = langv3.UserId,
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                }) ;
            }

            Debug.WriteLine(_langTextsv3.Count);


            Debug.WriteLine("Convert Done!");

            _langTextRepository.AddNewLangsToV4(_langTextsv4);
        }


    }
}
