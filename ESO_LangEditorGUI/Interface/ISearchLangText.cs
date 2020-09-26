using ESO_LangEditorGUI.Models.Enum;
using ESO_LangEditorLib.Models.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditorGUI.Interface
{
    public interface ISearchLangText
    {
        Task<List<LangTextDto>> GetLangText(SearchPostion searchPostion, SearchTextType searchType, string keyWord);
        LangTextDto GetLangText(SearchPostion searchPostion, SearchTextType searchType, int keyWord);



        //LangTextDto GetLangText(Guid userGuid, Guid langGuid);
    }
}
