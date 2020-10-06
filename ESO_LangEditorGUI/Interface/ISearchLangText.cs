using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorLib.Models.Client.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditorGUI.Interface
{
    public interface ISearchLangText
    {
        Task<List<LangTextDto>> GetLangTexts(SearchPostion searchPostion, SearchTextType searchType, string keyWord);
        LangTextDto GetLangText(SearchPostion searchPostion, SearchTextType searchType, int keyWord);



        //LangTextDto GetLangText(Guid userGuid, Guid langGuid);
    }
}
