using Core.EnumTypes;
using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Services
{
    public interface ILangFile
    {
        Task<Dictionary<string, LangTextDto>> ParseLangFile(string filePath, bool isOfficialZh = false);
        Task<Dictionary<string, string>> ParseJpLangFile(string filePath);
        Task<Dictionary<string, LangTextDto>> ParseCsvFile(string filePath);
        Task<Dictionary<string, LangTextDto>> ParseLuaFile(List<string> filePath);
        Task<Dictionary<string, string>> ParseJpLuaFile(List<string> filePath);
        Task<Dictionary<string, string>> ParseTextFile(string filePath);
        Task<string> ExportLangTextsAsJson(List<LangTextDto> langtextsList, LangChangeType changeType);
        Task ExportLangTextsToText(Dictionary<string, LangTextDto> langList, string path);
        Task ExportLuaToStr(List<LangTextDto> langList);
        Task ExportAddonDictToLua(Dictionary<string, string> langDict, int luaType);
        Task ExportLangTextsToLang(Dictionary<string, LangTextDto> langList, string path);
        Task ExportLangTextsToLang(Dictionary<string, string> langList, string path);


        //JsonFileDto JsonDtoDeserialize(string path);
    }
}
