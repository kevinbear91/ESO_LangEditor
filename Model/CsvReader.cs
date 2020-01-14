using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using FileHelpers.Events;

namespace ESO_Lang_Editor.Model
{
    class CsvReader
    {
        CsvParser fileParser = new CsvParser();

        public Dictionary<string, string> LoadCsv(string path)
        {
            var engine = new FileHelperEngine<FileModel_Csv>(Encoding.UTF8);
            var reader = engine.ReadFile("@" + path).ToList();
            var Dict = fileParser.CsvListToDict(reader);
            return Dict;
        }
        




    }
}
