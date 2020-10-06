using ESO_LangEditorLib.Models.Client.Enum;
using ESO_LangEditorLib.Models.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditorLib.Services.Client
{
    public interface ILangTextRepository
    {
       IEnumerable<LangTextDto> GetLangTexts(string keyWord, SearchTextType searchType);
    }
}
