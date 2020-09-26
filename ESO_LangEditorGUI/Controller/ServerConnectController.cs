using ESO_LangEditorLib.Models.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditorGUI.Controller
{
    public class ServerConnectController
    {
        public HttpClient apiClient { get; set; }
        List<LangTextDto> langText;

        public ServerConnectController()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            apiClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44379/")
            };
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<LangTextDto>> GetLangTexts()
        {
            using (HttpResponseMessage respond = await apiClient.GetAsync("api/users/148ed451-bf19-43e9-a8d3-55f922cd349e/langtexts"))
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
    }
}
