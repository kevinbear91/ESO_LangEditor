using ESO_LangEditorLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditorGUI.Command
{
    class ParserFileAndLoadFromDbCommand : CommandBase
    {
        private List<string>  _filePath;

        public ParserFileAndLoadFromDbCommand()
        {

        }

        public override Task ExecuteAsync(object parameter)
        {
            throw new NotImplementedException();

            //foreach (var f in _filePath)
            //{
            //    if (f.EndsWith(".lua"))
            //    {
            //        ParserLuaStr luaParser = new ParserLuaStr();

            //        luaDict = luaParser.LuaStrParser(filepath);
            //        dbLuaStr = await Task.Run(() => db.GetAllLuaLangsDictionaryAsync());

            //        //DiffDictionary(dbLuaStr, luaDict);
            //        Debug.WriteLine(dbLuaStr.Count());
            //    }
            //    else
            //    {
            //        ParserCsv csvparser = new ParserCsv();

            //        CsvDict = await csvparser.CsvReaderToDictionaryAsync(filePath);
            //        dbLangDict = await Task.Run(() => db.GetAllLangsDictionaryAsync());

            //        //DiffDictionary(dbLangDict, CsvDict);
            //        Debug.WriteLine(dbLangDict.Count());
            //    }
            //}
        }
    }
}
