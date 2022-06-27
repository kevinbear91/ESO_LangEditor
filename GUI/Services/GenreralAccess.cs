using Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GUI.Services
{
    public class GenreralAccess : IGeneralAccess
    {
        private readonly HttpClient _langHttpClient;
        private JsonSerializerOptions _jsonOption;

        public GenreralAccess()
        {
            //_langHttpClient = App.HttpClient;

            _jsonOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<List<LangTextRevNumberDto>> GetAllRevisedNumber()
        {
            //_langHttpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            List<LangTextRevNumberDto> responded = null;

            HttpResponseMessage response = await _langHttpClient.GetAsync("api/revise");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                responded = JsonSerializer.Deserialize<List<LangTextRevNumberDto>>(responseContent, _jsonOption);
            }

            Debug.WriteLine(responded);

            return responded;
        }

        public async Task<List<GameVersionDto>> GetGameVersionDtos()
        {
            //_langHttpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            List<GameVersionDto> responded = null;

            HttpResponseMessage response = await _langHttpClient.GetAsync("api/gameVersion");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                responded = JsonSerializer.Deserialize<List<GameVersionDto>>(responseContent, _jsonOption);
            }

            Debug.WriteLine(responded);

            return responded;
        }

        public async Task<List<LangTypeCatalogDto>> GetIdtypeDtos()
        {
            //_langHttpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            List<LangTypeCatalogDto> responded = null;

            HttpResponseMessage response = await _langHttpClient.GetAsync("api/langIdType");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                responded = JsonSerializer.Deserialize<List<LangTypeCatalogDto>>(responseContent, _jsonOption);
            }

            Debug.WriteLine(responded);

            return responded;
        }

        public async Task<List<LangTypeCatalogDto>> GetIdTypesFromReview()
        {
            //_langHttpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            List<LangTypeCatalogDto> responded = null;

            HttpResponseMessage response = await _langHttpClient.GetAsync("api/langIdType/review");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                responded = JsonSerializer.Deserialize<List<LangTypeCatalogDto>>(responseContent, _jsonOption);
            }

            Debug.WriteLine(responded);

            return responded;
        }

        public async Task<MessageWithCode> UploadIdTypeDto(LangTypeCatalogDto langTypeCatalogDto)
        {
            //_langHttpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            var content = SerializeDataToHttpContent(langTypeCatalogDto);

            HttpResponseMessage response = await _langHttpClient.PostAsync(
                "api/langIdType", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);

            return code;
        }
        public async Task<MessageWithCode> UploadIdTypeDto(List<LangTypeCatalogDto> langTypeCatalogDto)
        {
            //_langHttpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            var content = SerializeDataToHttpContent(langTypeCatalogDto);

            HttpResponseMessage response = await _langHttpClient.PostAsync(
                "api/langIdType/list", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);

            return code;
        }

        public async Task<MessageWithCode> UploadNewGameVersion(GameVersionDto gameVersionDto)
        {
            //_langHttpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            var content = SerializeDataToHttpContent(gameVersionDto);

            HttpResponseMessage response = await _langHttpClient.PostAsync(
                "api/gameVersion", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);

            return code;
        }

        public async Task<MessageWithCode> ApproveIdTypeFromReview(List<int> ids)
        {
            //_langHttpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            var content = SerializeDataToHttpContent(ids);

            HttpResponseMessage response = await _langHttpClient.PostAsync(
                "api/langIdType/review", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);

            return code;
        }

        public async Task<MessageWithCode> DeleteIdTypeFromReview(List<int> ids)
        {
            //_langHttpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            var content = SerializeDataToHttpContent(ids);

            HttpResponseMessage response = await _langHttpClient.PostAsync(
                "api/langIdType/review/del", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);

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
