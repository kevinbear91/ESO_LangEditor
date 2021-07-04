using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.Services
{
    public interface ILangFile
    {
        Task<Dictionary<string, LangTextDto>> ParseLangFile(string filePath);
        Task<Dictionary<string, LangTextDto>> ParseCsvFile(string filePath);
        Task<Dictionary<string, LangTextDto>> ParseLuaFile(List<string> filePath);
        Task<string> ExportLangTextsAsJson(List<LangTextDto> langtextsList, LangChangeType changeType);
        Task ExportToText(List<LangTextDto> langList);
        Task ExportLuaToStr(List<LangTextDto> langList);
        Task ExportToLang(List<LangTextDto> langList);



        //JsonFileDto JsonDtoDeserialize(string path);
    }
}
