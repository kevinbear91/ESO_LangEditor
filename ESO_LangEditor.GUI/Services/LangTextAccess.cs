using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditor.GUI.Services
{
    public class LangTextAccess : ILangTextAccess
    {
        private readonly HttpClient _langHttpClient;
        private JsonSerializerOptions _jsonOption;

        public LangTextAccess()
        {
            _langHttpClient = App.HttpClient;
            //_langHttpClient = new HttpClient
            //{
            //    BaseAddress = new Uri(App.ServerPath),
            //};

            //_langHttpClient.DefaultRequestHeaders.Accept.Clear();
            //_langHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_langHttpClient.DefaultRequestHeaders.Connection.Add("keep-alive");

            _jsonOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }



        public Task<ApiMessageWithCode> AddLangTexts(List<LangTextForCreationDto> langTextForCreationDtos)
        {
            throw new NotImplementedException();
        }

        public Task<ApiMessageWithCode> ApproveLangTextsInReview(List<Guid> langTextGuids)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LangTextRevisedDto>> GetLangTextRevisedDtos(int revNumber)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetLangTextRevisedNumber()
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            int responded = 0;

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await _langHttpClient.GetAsync("api/revise");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                responded = JsonSerializer.Deserialize<int>(responseContent, _jsonOption);
            }

            Debug.WriteLine(responded);

            return responded;
        }

        public async Task<IEnumerable<LangTextDto>> GetLangTexts(Guid langtextId)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            List<LangTextDto> respondedList = null;

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await _langHttpClient.GetAsync(
                "api/langtext/" + langtextId);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                respondedList = JsonSerializer.Deserialize<List<LangTextDto>>(responseContent, _jsonOption);
            }

            Debug.WriteLine(respondedList);

            return respondedList;
        }

        public async Task<IEnumerable<LangTextDto>> GetLangTexts(List<Guid> langtextIds)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            List<LangTextDto> respondedList = null;

            var content = SerializeDataToHttpContent(langtextIds);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_langHttpClient.BaseAddress + "api/langtext/list"),
                Content = content,
            };

            var response = await _langHttpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                respondedList = JsonSerializer.Deserialize<List<LangTextDto>>(responseContent, _jsonOption);
            }

            Debug.WriteLine(respondedList);

            return respondedList;
        }

        public async Task<List<LangTextDto>> GetLangTexts(int langtextRevised)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            List<LangTextDto> respondedList = null;

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await _langHttpClient.GetAsync(
                "api/revise/" + langtextRevised);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                respondedList = JsonSerializer.Deserialize<List<LangTextDto>>(responseContent, _jsonOption);
            }

            Debug.WriteLine(respondedList);

            return respondedList;
        }

        public Task<IEnumerable<LangTextDto>> GetLangTextsFromArchive(Guid langtextId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LangTextForReviewDto>> GetLangTextsFromReview(Guid Guid, bool isUser)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Guid>> GetUsersInReview()
        {
            throw new NotImplementedException();
        }

        public Task<ApiMessageWithCode> RemoveLangTexts(List<Guid> langTextGuids)
        {
            throw new NotImplementedException();
        }

        public Task<ApiMessageWithCode> RemoveLangTextsInReview(Guid langTextGuid)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiMessageWithCode> UpdateLangTextEn(List<LangTextForUpdateEnDto> langTextForUpdateEnDtos)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            var content = SerializeDataToHttpContent(langTextForUpdateEnDtos);

            HttpResponseMessage response = await _langHttpClient.PostAsync(
                "api/langtext/en/list", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var code = JsonSerializer.Deserialize<ApiMessageWithCode>(responseContent, _jsonOption);

            return code;
        }

        public async Task<ApiMessageWithCode> UpdateLangTextZh(LangTextForUpdateZhDto langTextForUpdateZhDto)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            var content = SerializeDataToHttpContent(langTextForUpdateZhDto);
            string langTextId = langTextForUpdateZhDto.Id.ToString();

            HttpResponseMessage response = await _langHttpClient.PostAsync(
                "api/langtext/" + langTextId, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var code = JsonSerializer.Deserialize<ApiMessageWithCode>(responseContent, _jsonOption);

            return code;
        }

        public async Task<ApiMessageWithCode> UpdateLangTextZh(List<LangTextForUpdateZhDto> langTextForUpdateZhDtos)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            var content = SerializeDataToHttpContent(langTextForUpdateZhDtos);

            HttpResponseMessage response = await _langHttpClient.PostAsync(
                "api/langtext/zh/list", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var code = JsonSerializer.Deserialize<ApiMessageWithCode>(responseContent, _jsonOption);

            return code;
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
