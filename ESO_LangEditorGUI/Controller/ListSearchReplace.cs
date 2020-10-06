using ESO_LangEditorLib.Models.Client;
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
        public int FindMatch(List<LangTextDto> input, string keyword, bool isOnlyMatchWord, RegexOptions option)
        #region 查询匹配 不区分大小写 重载
        {
            string pattern = SetMatchRule(keyword, isOnlyMatchWord);
            int count = 0;

            foreach (var text in input)
            {
                if (Regex.IsMatch(text.TextZh, pattern, option))
                {
                    count++;
                }
            }
            return count;
        }
        #endregion

        public int FindMatch(List<LangTextDto> input, string keyword, bool isOnlyMatchWord)
        #region 查询匹配 区分大小写 重载
        {
            string pattern = SetMatchRule(keyword, isOnlyMatchWord);
            int count = 0;

            foreach (var text in input)
            {
                if (Regex.IsMatch(text.TextZh, pattern))
                {
                    count++;
                }
            }
            return count;
        }
        #endregion

        public List<LangTextDto> SearchReplace(List<LangTextDto> input, string keyword, string replaceWord, bool isOnlyMatchWord, RegexOptions option)
        #region 批量替换 不区分大小写 重载
        {
            string pattern = SetMatchRule(keyword, isOnlyMatchWord);
            //int count = 0;

            List<LangTextDto> result = new List<LangTextDto>();

            foreach (var text in input)
            {
                
                if (Regex.IsMatch(text.TextZh, pattern, option))
                {
                    string replacedWord = Regex.Replace(text.TextZh, pattern, replaceWord);

                    result.Add(new LangTextDto
                    {
                        TextId = text.TextId,
                        Id = text.Id,
                        //Unknown = text.Unknown,
                        //Lang_Index = text.Lang_Index,
                        TextEn = text.TextEn,
                        TextZh = replacedWord,
                        IsTranslated = 1,
                        //RowStats = text.RowStats,
                        UpdateStats = text.UpdateStats,

                    });
                    Debug.WriteLine("{0}, {1}.",text.TextId, replacedWord);
                }
            }
            return result;
        }
        #endregion

        public List<LangTextDto> SearchReplace(List<LangTextDto> input, string keyword, string replaceWord, bool isOnlyMatchWord)
        #region 批量替换 区分大小写 重载
        {
            string pattern = SetMatchRule(keyword, isOnlyMatchWord);
            //int count = 0;

            List<LangTextDto> result = new List<LangTextDto>();

            foreach (var text in input)
            {

                if (Regex.IsMatch(text.TextZh, pattern))
                {
                    string replacedWord = Regex.Replace(text.TextZh, pattern, replaceWord);

                    result.Add(new LangTextDto
                    {
                        TextId = text.TextId,
                        Id = text.Id,
                        //Unknown = text.Unknown,
                        //Lang_Index = text.Lang_Index,
                        TextEn = text.TextEn,
                        TextZh = replacedWord,
                        IsTranslated = 1,
                        //RowStats = text.RowStats,
                        UpdateStats = text.UpdateStats,

                    });
                    Debug.WriteLine("{0}, {1}.", text.TextId, replacedWord);
                }
            }
            return result;
        }
        #endregion
    }
}
