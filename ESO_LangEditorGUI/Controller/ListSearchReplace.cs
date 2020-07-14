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

        public string SetMatchRule(string keyword, bool isOnlyMatchWord)
        {
            string pattern;

            //if (keyword.Contains("?"))
            //{
            //    keyword.Replace("?", @"\?");
            //}

            if (isOnlyMatchWord)
                pattern = @"\b" + keyword + @"\b";
            else
                pattern =  keyword;

            //RegexOptions regexOptions = RegexOptions.IgnoreCase;
            //Regex regex = new Regex(pattern, regexOptions);

            return pattern;
        }
        public int FindMatch(List<LangText> input, string keyword, bool isOnlyMatchWord, RegexOptions option)
        #region 查询匹配 不区分大小写 重载
        {
            string pattern = SetMatchRule(keyword, isOnlyMatchWord);
            int count = 0;

            foreach (var text in input)
            {
                if (Regex.IsMatch(text.Text_ZH, pattern, option))
                {
                    count++;
                }
            }
            return count;
        }
        #endregion

        public int FindMatch(List<LangText> input, string keyword, bool isOnlyMatchWord)
        #region 查询匹配 区分大小写 重载
        {
            string pattern = SetMatchRule(keyword, isOnlyMatchWord);
            int count = 0;

            foreach (var text in input)
            {
                if (Regex.IsMatch(text.Text_ZH, pattern))
                {
                    count++;
                }
            }
            return count;
        }
        #endregion

        public List<LangText> SearchReplace(List<LangText> input, string keyword, string replaceWord, bool isOnlyMatchWord, RegexOptions option)
        #region 批量替换 不区分大小写 重载
        {
            string pattern = SetMatchRule(keyword, isOnlyMatchWord);
            //int count = 0;

            List<LangText> result = new List<LangText>();

            foreach (var text in input)
            {
                
                if (Regex.IsMatch(text.Text_ZH, pattern, option))
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
        #endregion

        public List<LangText> SearchReplace(List<LangText> input, string keyword, string replaceWord, bool isOnlyMatchWord)
        #region 批量替换 区分大小写 重载
        {
            string pattern = SetMatchRule(keyword, isOnlyMatchWord);
            //int count = 0;

            List<LangText> result = new List<LangText>();

            foreach (var text in input)
            {

                if (Regex.IsMatch(text.Text_ZH, pattern))
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
                    Debug.WriteLine("{0}, {1}.", text.UniqueID, replacedWord);
                }
            }
            return result;
        }
        #endregion
    }
}
