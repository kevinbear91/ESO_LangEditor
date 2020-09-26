using ESO_LangEditorGUI.Models.Enum;
using ESO_LangEditorLib.Models.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditorGUI.Interface
{
    public class SearchLangText : ISearchLangText
    {
        private List<LangTextDto> langText;

        public HttpClient ApiClient { get; private set; }

        public async Task<List<LangTextDto>> GetLangText(SearchPostion searchPostion, SearchTextType searchType, string keyWord)
        {
            GetKeywordWithPostion(searchPostion, keyWord);

            InitializeClient();

            using (HttpResponseMessage respond = await ApiClient.GetAsync(GetRequestPath(searchType, keyWord)))
            {
                string result = respond.Content.ReadAsStringAsync().Result;


                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                try
                {
                    if (respond.IsSuccessStatusCode)
                    {
                        Debug.WriteLine(result);

                        langText = JsonSerializer.Deserialize<List<LangTextDto>>(result, options);
                        Debug.WriteLine("{0}", langText.Count);
                        //foreach (var lang in langText)
                        //{
                        //    Debug.WriteLine("{0},{1}", lang.Id, lang.TextZh);
                        //}
                        //Debug.WriteLine("{0},{1}",langText.Id, langText.TextZh);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }

            return langText;
        }

        

        public LangTextDto GetLangText(SearchPostion searchPostion, SearchTextType searchType, int keyWord)
        {
            throw new NotImplementedException();
        }

        private static string GetKeywordWithPostion(SearchPostion searchPostion, string keyWord)
        {
            string searchPosAndWord = searchPostion switch
            {
                SearchPostion.Full => "%" + keyWord + "%",     //任意位置
                SearchPostion.OnlyOnFront => keyWord + "%",           //仅在开头
                SearchPostion.OnlyOnEnd => "%" + keyWord,           //仅在末尾
                _ => "%" + keyWord + "%",     //默认 - 任意位置
            };

            return searchPosAndWord;
        }

        private static string GetRequestPath(SearchTextType searchType, string keyWord)
        {
            string searchRequestPath = searchType switch
            {
                SearchTextType.ByUser => "api/users/"+ keyWord + "/langtexts",  //用户ID
                //SearchTextType.Guid => "api/users/" + keyWord + "/langtexts",  //用户ID

                _ => "api/users/" + keyWord + "/langtexts",  //默认 - 用户ID

            };

            return searchRequestPath;
        }

        private void InitializeClient()
        {
            ApiClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44379/")
            };
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
