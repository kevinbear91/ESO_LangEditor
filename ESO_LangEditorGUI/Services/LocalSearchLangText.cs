using ESO_LangEditorLib;
using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorLib.Models.Client.Enum;
using ESO_LangEditorLib.Services.Client;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditorGUI.Services
{
    public class LocalSearchLangText
    {
        private readonly LangTextRepository _localDB = new LangTextRepository();

        public async Task<List<LangTextDto>> GetLangTexts(SearchPostion searchPostion, SearchTextType searchType, string keyWord)
        {
            string searchPosAndWord = searchPostion switch  //设定关键字出现的位置
            {
                SearchPostion.Full => "%" + keyWord + "%",     //任意位置
                SearchPostion.OnlyOnFront => keyWord + "%",           //仅在开头
                SearchPostion.OnlyOnEnd => "%" + keyWord,           //仅在末尾
                _ => "%" + keyWord + "%",     //默认 - 任意位置
            };

            List<LangTextDto> LangTexts = (List<LangTextDto>)_localDB.GetLangTexts(searchPosAndWord, searchType);

            return LangTexts;
        }

        public LangTextDto GetLangText(SearchPostion searchPostion, SearchTextType searchType, int keyWord)
        {
            throw new NotImplementedException();
        }
    }
}
