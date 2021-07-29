using ESO_LangEditorDatabaseModifier.Controller;
using ESO_LangEditorDatabaseModifier.Model.v4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESO_LangEditorDatabaseModifier
{
    public class CleanEnInZh
    {
        private Dictionary<string, LangText> _langTextsv4;

        private LangTextRepository _langTextRepository = new LangTextRepository();


        public CleanEnInZh()
        {
            _langTextsv4 = _langTextRepository.GeAlltLangTexts_v4();

            int enCount = 0;
            int zhCount = 0;

            foreach (var lang in _langTextsv4)
            {
                string langZH = lang.Value.TextZh;

                //Console.WriteLine($"ID: {lang.Key} 为 {(IsEn(langZH) ? "英文" : "非英文")}");

                //if (lang.Value.IdType != 100)
                //{
                    
                //}

                if (IsEn(langZH))
                {
                    enCount++;
                    lang.Value.TextZh = null;
                }
                else
                {
                    zhCount++;
                }

            }

            Dictionary<string, LangText> _langTextsv4Orderd = _langTextsv4.OrderBy(o => o.Value.IdType).ToDictionary(o => o.Key, p => p.Value);

            //foreach(var k in _langTextsv4Orderd)
            //{
            //    Console.WriteLine(k.Key);
            //}

            //Console.WriteLine("_langTextsv4Orderd count = {0}", _langTextsv4Orderd.Count);


            Console.WriteLine("EN count = {0}", enCount);
            Console.WriteLine("ZH count = {0}", zhCount);

            var users = _langTextRepository.GetUserAllToV4();

            _langTextRepository.AddUsersToV5(users.Values.ToList());

            _langTextRepository.AddNewLangsToV5(_langTextsv4Orderd.Values.ToList());



        }

        private static bool IsEn(string langText)
        {
            if (string.IsNullOrWhiteSpace(langText))
            {
                return true;
            }

            var bytes = Encoding.UTF8.GetBytes(langText);
            bool result = bytes.Length == langText.Length;
            return result;
        }


    }
}
