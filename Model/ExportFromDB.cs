using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Convert;

namespace ESO_Lang_Editor.Model
{
    class ExportFromDB
    {
        public void ExportAsText()
        {

            var outputIDFile = new List<string>();
            var outputTextFile = new List<string>();

            var connDB = new SQLiteController();

            var dbData = connDB.FullSearchData();

            if (!Directory.Exists("_tmp"))
                Directory.CreateDirectory("_tmp");

            foreach (var item in dbData)
            {
                outputIDFile.Add(item.ID_Type + "-" + item.ID_Unknown + "-" + item.ID_Index);
                outputTextFile.Add(item.Text_SC);
            }

            using (StreamWriter sw = new StreamWriter("_tmp/ID.txt"))
            {
                foreach (string s in outputIDFile)
                {
                    sw.WriteLine(s);
                }
                outputIDFile = null;
                sw.Flush();
                sw.Close();
            }

            using (StreamWriter sw = new StreamWriter("_tmp/Text.txt"))
            {
                foreach (string s in outputTextFile)
                {
                    sw.WriteLine(s);
                }
                outputTextFile = null;
                sw.Flush();
                sw.Close();
            }

        }

        public void ExportIDArray(int[] ID_Array)
        {
            var outputFile = new List<string>();
            var connDB = new SQLiteController();
            var dbData = connDB.FullSearchData();

            if (!Directory.Exists("_tmp"))
                Directory.CreateDirectory("_tmp");

            foreach (var item in dbData)
            {
                if (ID_Array.Contains(ToInt32(item.ID_Type)))
                    outputFile.Add(item.ID_Type
                        + "-"
                        + item.ID_Unknown
                        + "-"
                        + item.ID_Index
                        + "&"
                        + item.Text_EN
                        + "&"
                        + item.Text_SC);
            }
            using (StreamWriter sw = new StreamWriter("_tmp/ExportIDArray.txt"))
            {
                foreach (string s in outputFile)
                {
                    sw.WriteLine(s);
                }

                sw.Flush();
                sw.Close();
            }

        }

    }

}
