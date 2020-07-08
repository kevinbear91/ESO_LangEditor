using ESO_LangEditorLib.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace ESO_LangEditorGUI.Controller
{
    public class ListSearchReplace
    {

        public void SetMatchRule(List<LangText> input, string keyword, string replaceWord)
        {
            string pattern = @"\b" + keyword + @"\b";  // \bwest skyrim\b 天际西部


            RegexOptions regexOptions = RegexOptions.IgnoreCase;
            Regex regex = new Regex(pattern, regexOptions);

        }
        public int FindMatch(List<LangText> input, string keyword)
        {
            string pattern = @"\b" + keyword + @"\b";
            int count = 0;

            foreach (var text in input)
            {
                if (Regex.IsMatch(text.Text_ZH, pattern, RegexOptions.IgnoreCase))
                {
                    count++;
                }
            }
            return count;
        }
        public List<LangText> SearchReplace(List<LangText> input, string keyword,string replaceWord)
        {
            string pattern = @"\b" + keyword + @"\b";
            //int count = 0;

            List<LangText> result = new List<LangText>();

            foreach (var text in input)
            {
                
                if (Regex.IsMatch(text.Text_ZH, pattern, RegexOptions.IgnoreCase))
                {
                    string replacedWord = Regex.Replace(text.Text_ZH, pattern, replaceWord);

                    result.Add(new LangText
                    {
                        UniqueID = text.UniqueID,
                        ID = text.ID,
                        Unknown = text.Unknown,
                        Lang_Index = text.Lang_Index,
                        Text_EN = text.Text_EN,
                        Text_ZH = replacedWord,
                        IsTranslated = text.IsTranslated,
                        RowStats = text.RowStats,
                        UpdateStats = text.UpdateStats,

                    });
                    Debug.WriteLine("{0}, {1}.",text.UniqueID, replacedWord);
                }
            }
            return result;
        }
    }
}
