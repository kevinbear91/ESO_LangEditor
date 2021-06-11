using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.GUI.Services
{
    interface IParseLangFile
    {
        Dictionary<string, LangTextDto> ParseLangFile(string filePath);
        Dictionary<string, LangTextDto> ParseCsvFile(string filePath);
        Dictionary<string, LangTextDto> ParseLuaFile(List<string> filePath);
        JsonFileDto JsonDtoDeserialize(string path);
    }
}
