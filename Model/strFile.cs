using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ESO_Lang_Editor.Model
{
    class strFile
    {
        public string addString { get; set; }

        public string stringId { get; set; }

        public string stringEN { get; set; }

        public string stringZH { get; set; }

        public string stringVersion { get; set; }
        
        
        public void ParserLua()
        {
            //char[] delimiterChars = { '(',')' };
            //String pattern = @"(SI_\w+)^\,\w+$\,";
            String pattern = @"SafeAddString\((SI_\w+\,)";

            String path = @"D:\eso_zh\ESO_LangEditor\str\en_pregame.lua";

            var lines = System.IO.File.ReadAllLines(path, Encoding.UTF8);

            //string[] words;

            List<strFile> strlist = new List<strFile>();
            

            foreach (var line in lines)
            {
                if (line.StartsWith("SafeAddString"))
                {
                    //string[] words = line.Split(delimiterChars);

                    //int idx = words.Count() - 1;

                    //foreach (var m in Regex.Split(line, pattern))
                   // {

                        //Console.WriteLine(m);
                    if (Regex.Split(line, pattern).Count() != 0)
                    {
                        string stringText = Regex.Split(line, pattern)[2].Substring(2, Regex.Split(line, pattern)[2].LastIndexOf(',') - 3);

                        strlist.Add(new strFile
                        {
                            //addString = line.Split('(')[1].Split(',')[0].Trim(),
                            stringId = Regex.Split(line, pattern)[1].Trim(','),
                            stringEN = stringText,
                            stringVersion = Regex.Split(line, pattern)[2].Substring(Regex.Split(line, pattern)[2].LastIndexOf(',') +2).Trim(')'),
                        });
                    }
                            
                        
                    //}

                    //Console.WriteLine(words);

                    //if (idx != -1)
                    //{
                    /*
                    strlist.Add(new strFile
                    {
                        //addString = line.Split('(')[1].Split(',')[0].Trim(),
                        stringId = line.Split('(')[1].Split(',')[0].Trim(),
                        stringEN = line.Split(',')[1].Split(',')[0].Trim(),
                        stringVersion = line.Split(',')[2].Split(')')[0].Trim(),
                    });
                    */

                    //foreach (Match match in Regex.Matches(line, pattern, RegexOptions.IgnoreCase))
                    //   Console.WriteLine("{0}, {1}",match.Groups[1], match.Groups[2]);


                    //string id = line.Split('(')[1].Split(',')[0].Trim();
                    //string text = line.Split(',')[1].Split(',')[0].Trim();

                    //int textBeginIndex = line.Split(',')[1].Trim().Length + 2;
                    //string en = line.Substring(13 + id.Length + 2, line.Length - 4);
                    //string version = line.Split(',')[1];

                    //Console.WriteLine(words[1].Trim('('));

                    //int len = line.IndexOf('"');
                    //Console.WriteLine(line.IndexOf('"'));
                    //Console.WriteLine(line.Split('(')[1].Split(',')[0].Trim());
                    //Console.WriteLine(line.Split());
                    //Console.WriteLine(words[1].Split(',')[1].Substring(0, words[0].Length - 4));
                    //Console.WriteLine(line.Split(',')[1].Substring(line.Split(',')[1].Length,line.Length - 4));
                    //foreach (var word in words)
                    //{
                    //    Console.WriteLine(word);
                    //}


                    //

                    //Console.WriteLine("ID: {0}, EN: {1}, version: {2}.", id, en, version);
                    //if(words[3] != "")
                    //    Console.WriteLine("{0}, {1}", words[3], words[3].Substring(0, words[3].Length - 4));
                }

            }

            foreach (var word in strlist)
            {
                System.Console.WriteLine("ID: {0}, EN: {1}, version: {2}.", word.stringId, word.stringEN, word.stringVersion);
            }

        }


    }

    

}
