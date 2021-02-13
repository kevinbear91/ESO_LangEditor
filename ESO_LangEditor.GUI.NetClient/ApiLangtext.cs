using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.NetClient
{
    public class ApiLangtext
    {
        private readonly HttpClient client;
        private JsonSerializerOptions _jsonOption;

        public ApiLangtext()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44343/");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _jsonOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<List<LangTextDto>> GetLangtextAsync(string langtextGuid, string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await client.GetAsync(
                "api/langtext");
            response.EnsureSuccessStatusCode();

            var responseContent = response.Content.ReadAsStringAsync().Result;
            var json = JsonSerializer.Deserialize<List<LangTextDto>>(responseContent, _jsonOption);

            Debug.WriteLine(json);

            return json;

        }


        private HttpContent SerializeDataToHttpContent(object data)
        {
            var myContent = JsonSerializer.SerializeToUtf8Bytes(data);

            var byteContent = new ByteArrayContent(myContent);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }


    }
}
