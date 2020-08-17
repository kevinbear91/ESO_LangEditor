using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;

namespace ESO_LangEditorGUI.Controller
{
    public class WindowController
    {


        public ObservableCollection<string> GetSearchPostion()
        {
            ObservableCollection<string> searchTextInPosition = new ObservableCollection<string>
            {
                "包含全文",
                "仅包含开头",
                "仅包含结尾"
            };

            return searchTextInPosition;
        }

        public ObservableCollection<string> GetSearchTextType()
        {
            ObservableCollection<string> searchTextType = new ObservableCollection<string>
            {
                "搜类型",   //0
                "搜英文",   //1
                "搜译文",   //2
                "搜版本号",
                "搜唯一ID",
                "搜已翻译条目"
            };

            return searchTextType;
        }

        


        public bool InputCheck(int searchType, string SearchWord)
        {
            if (searchType == 0 || searchType == 5)   // 0 == 搜类型, 5 == 搜已翻译条目
                return MatchInput(SearchWord);
            else
                return true;   //如果支持 string 类型则返回 true。
        }


        private bool MatchInput(string Inputword)
        {
            bool result = false;
            string pattern = @"^[0-9-]*$";
            RegexOptions regexOptions = RegexOptions.None;
            Regex regex = new Regex(pattern, regexOptions);

            foreach (Match match in regex.Matches(Inputword))
            {
                if (match.Success)
                    result = true;
            }
            return result;
        }

    }
}
