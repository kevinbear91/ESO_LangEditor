using ESO_LangEditorDatabaseModifier.Controller;
using ESO_LangEditorDatabaseModifier.Model.v2;
using ESO_LangEditorDatabaseModifier.Model.v3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ESO_LangEditorDatabaseModifier
{
    public class ConvertV2toV3
    {
        private List<LangTextDto_v2> _langTextsv2;
        private List<LuaUIData_v2> _luaTextsv2;

        private List<LangTextDto_v3> _langTextsv3;

        private LangTextRepository _langTextRepository = new LangTextRepository();


        public void Convertv2Tov3()
        {
            _langTextsv2 = _langTextRepository.GeAlltLangTexts_v2();
            _luaTextsv2 = _langTextRepository.GeAlltLangLuas_v2();

            _langTextsv3 = new List<LangTextDto_v3>();

            //_langTextsv3 = _langTextRepository.


            foreach(var langv2 in _langTextsv2)
            {
                _langTextsv3.Add(new LangTextDto_v3
                {
                    Id = new Guid(),
                    TextId = langv2.UniqueID,
                    IdType = langv2.ID,
                    TextEn = langv2.Text_EN,
                    TextZh = langv2.Text_ZH,
                    LangType = LangType.LangText,
                    IsTranslated = langv2.IsTranslated,
                    UpdateStats = langv2.UpdateStats,
                    EnLastModifyTimestamp = DateTime.Now,
                    ZhLastModifyTimestamp = DateTime.Now,
                    UserId = new Guid("148ED451-BF19-43E9-A8D3-55F922CD349E"),
                }) ;
            }

            Debug.WriteLine(_langTextsv3.Count);

            foreach(var luav2 in _luaTextsv2)
            {
                _langTextsv3.Add(new LangTextDto_v3
                {
                    Id = new Guid(),
                    TextId = luav2.UniqueID,
                    IdType = 100,
                    TextEn = luav2.Text_EN,
                    TextZh = luav2.Text_ZH,
                    LangType = (LangType)luav2.DataEnum,
                    IsTranslated = luav2.IsTranslated,
                    UpdateStats = luav2.UpdateStats,
                    EnLastModifyTimestamp = DateTime.Now,
                    ZhLastModifyTimestamp = DateTime.Now,
                    UserId = new Guid("148ED451-BF19-43E9-A8D3-55F922CD349E"),
                });
            }

            Debug.WriteLine("Convert Done!");

            _langTextRepository.AddNewLangs(_langTextsv3);
        }


    }
}
