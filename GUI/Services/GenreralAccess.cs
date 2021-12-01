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
            _langHttpClient = App.HttpClient;

            _jsonOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public Task<MessageWithCode> ApproveIdTypeFromReview()
        {
            throw new NotImplementedException();
        }

        public Task<MessageWithCode> DeleteIdTypeFromReview()
        {
            throw new NotImplementedException();
        }

        public async Task<List<LangTextRevNumberDto>> GetAllRevisedNumber()
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
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

        public Task<List<GameVersionDto>> GetGameVersionDtos()
        {
            throw new NotImplementedException();
        }

        public Task<List<LangTypeCatalogDto>> GetIdtypeDtos()
        {
            throw new NotImplementedException();
        }

        public Task<MessageWithCode> GetIdTypeFromReview()
        {
            throw new NotImplementedException();
        }

        public Task<MessageWithCode> UploadIdTypeDto()
        {
            throw new NotImplementedException();
        }

        public Task<MessageWithCode> UploadNewGameVersion()
        {
            throw new NotImplementedException();
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
