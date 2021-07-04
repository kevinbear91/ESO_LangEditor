using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.NetClient.Old
{
    public class UserAccess : IUserAccess
    {
        private readonly HttpClient client;
        private JsonSerializerOptions _jsonOption;

        public UserAccess(string serverAddress)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(serverAddress)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _jsonOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        

        public async Task<TokenDto> GetTokenFromServer(string url, object Dto)
        {
            var content = SerializeDataToHttpContent(Dto);
            TokenDto revicedToken = null;

            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                revicedToken = JsonSerializer.Deserialize<TokenDto>(responseContent, _jsonOption);
            }

            Debug.WriteLine("AuthToken: {0}. RefreshToken: {1} .", revicedToken.AuthToken, revicedToken.RefreshToken);
            return revicedToken;
        }

        public Task<string> GetRegistrationCode()
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetUserInfoFromServer(Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserInClientDto>> GetUserList(string token)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserPwResetCode(Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetUserRoles(Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public Task<string> SetUserRoles(Guid userId, List<string> roles)
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

        public Task Get(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task GetToken(Guid userId, object tokenDto)
        {
            throw new NotImplementedException();
        }
    }
}
