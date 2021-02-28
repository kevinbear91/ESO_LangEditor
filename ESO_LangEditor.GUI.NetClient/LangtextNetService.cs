using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.NetClient
{
    public class LangtextNetService
    {
        private readonly HttpClient client;
        private JsonSerializerOptions _jsonOption;

        public LangtextNetService()
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
            List<LangTextDto> respondedList = null;

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await client.GetAsync(
                "api/langtext" + langtextGuid);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                respondedList = JsonSerializer.Deserialize<List<LangTextDto>>(responseContent, _jsonOption);
            }

            Debug.WriteLine(respondedList);

            return respondedList;

        }

        public async Task<List<LangTextDto>> GetLangtextFromArchiveAsync(string langtextGuid, string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            List<LangTextDto> respondedList = null;

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await client.GetAsync(
                "api/langtext/archive/" + langtextGuid);

            if(response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                respondedList = JsonSerializer.Deserialize<List<LangTextDto>>(responseContent, _jsonOption);
            }

            Debug.WriteLine(respondedList);

            return respondedList;
        }

        public async Task<HttpStatusCode> UpdateLangtextZh(LangTextForUpdateZhDto langTextForUpdateZhDto, string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var content = SerializeDataToHttpContent(langTextForUpdateZhDto);

            HttpResponseMessage response = await client.PutAsync(
                "api/langtext/" + langTextForUpdateZhDto.Id, content);

            return response.StatusCode;

            //var responseContent = await response.Content.ReadAsStringAsync();
            //var json = JsonSerializer.Deserialize<List<LangTextDto>>(responseContent, _jsonOption);

            //Debug.WriteLine(json);

            //return json;

        }

        public async Task<List<LangTextForReviewDto>> GetLangtextInReviewByIdAsync(string langtextGuid, string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            List<LangTextForReviewDto> respondedList = null;

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await client.GetAsync(
                "api/langtext/review" + langtextGuid);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                respondedList = JsonSerializer.Deserialize<List<LangTextForReviewDto>>(responseContent, _jsonOption);
            }

            Debug.WriteLine(respondedList);

            return respondedList;

        }

        public async Task<List<LangTextForReviewDto>> GetLangtextInReviewAllAsync(string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            List<LangTextForReviewDto> respondedList = null;

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await client.GetAsync(
                "api/langtext/review");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                respondedList = JsonSerializer.Deserialize<List<LangTextForReviewDto>>(responseContent, _jsonOption);
            }

            Debug.WriteLine(respondedList);

            return respondedList;

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
