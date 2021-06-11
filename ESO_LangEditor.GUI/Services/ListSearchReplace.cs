using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace ESO_LangEditor.GUI.Services
{
    public class ListSearchReplace
    {

        private List<LangTextDto> _inputList;

        public ListSearchReplace(List<LangTextDto> langTexts)
        {
            _inputList = langTexts;
        }


        private string SetMatchRule(string keyword, bool isOnlyMatchWord)
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
        //public int FindMatch(List<LangTextDto> input, string keyword, bool isOnlyMatchWord, RegexOptions option)
        //#region 查询匹配 不区分大小写 重载
        //{
        //    string pattern = SetMatchRule(keyword, isOnlyMatchWord);
        //    int count = 0;

        //    foreach (var text in input)
        //    {
        //        if (Regex.IsMatch(text.TextZh, pattern, option))
        //        {
        //            count++;
        //        }
        //    }
        //    return count;
        //}
        //#endregion

        //public int FindMatch(List<LangTextDto> input, string keyword, bool isOnlyMatchWord)
        //#region 查询匹配 区分大小写 重载
        //{
        //    string pattern = SetMatchRule(keyword, isOnlyMatchWord);
        //    int count = 0;

        //    foreach (var text in input)
        //    {
        //        if (Regex.IsMatch(text.TextZh, pattern))
        //        {
        //            count++;
        //        }
        //    }
        //    return count;
        //}
        //#endregion

        public List<LangTextDto> SearchReplace(string keyword, string replaceWord, bool isOnlyMatchWord, RegexOptions option)
        #region 批量替换
        {
            string pattern = SetMatchRule(keyword, isOnlyMatchWord);
            var resultList = new List<LangTextDto>();

            Debug.WriteLine("isOnlyMatchWord = {0}", isOnlyMatchWord);

            foreach (var text in _inputList)
            {
                if (Regex.IsMatch(text.TextZh, pattern, option))
                {
                    string replacedWord = Regex.Replace(text.TextZh, pattern, replaceWord, option);

                    text.IsTranslated = 1;
                    text.ZhLastModifyTimestamp = DateTime.Now;
                    text.UserId = App.LangConfig.UserGuid;
                    text.TextZh = replacedWord;

                    resultList.Add(text);

                    Debug.WriteLine("Text GUID: {0}, TextID: {1}, TextEN: {2}, TextZH: {3}, Translated: {4}, LastModifyTimeZH: {5}, UserGUID: {6}",
                        text.Id, text.TextId, text.TextEn, text.TextZh, text.IsTranslated, text.ZhLastModifyTimestamp, text.UserId);
                }
            }


            //foreach (var text in input)
            //{
                
            //    if (Regex.IsMatch(text.TextZh, pattern, option))
            //    {
            //        string replacedWord = Regex.Replace(text.TextZh, pattern, replaceWord);

            //        result.Add(new LangTextDto
            //        {
            //            TextId = text.TextId,
            //            Id = text.Id,
            //            //Unknown = text.Unknown,
            //            //Lang_Index = text.Lang_Index,
            //            TextEn = text.TextEn,
            //            TextZh = replacedWord,
            //            IsTranslated = 1,
            //            //RowStats = text.RowStats,
            //            UpdateStats = text.UpdateStats,

            //        });
            //        Debug.WriteLine("{0}, {1}.",text.TextId, replacedWord);
            //    }
            //}
            return resultList;
        }
        #endregion



        #region 搜索匹配
        public List<LangTextDto> SearchResult(string keyword, bool isOnlyMatchWord, RegexOptions option)
        {
            var resultList = new List<LangTextDto>();

            string pattern = SetMatchRule(keyword, isOnlyMatchWord);
            //int count = 0;

            foreach (var text in _inputList)
            {
                if (Regex.IsMatch(text.TextZh, pattern, option))
                {
                    resultList.Add(text);
                }
            }

            return resultList;
        }
        #endregion

    }
}
