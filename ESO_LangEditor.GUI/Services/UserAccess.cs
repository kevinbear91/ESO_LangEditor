using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.EventAggres;
using ESO_LangEditor.GUI.NetClient;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using static System.Convert;

namespace ESO_LangEditor.GUI.Services
{
    public class UserAccess : IUserAccess
    {
        private readonly HttpClient _userClient;

        private readonly string apiUri = "api/account/";
        private IEventAggregator _ea;
        private JsonSerializerOptions _jsonOption;
        //private readonly ApiMessageWithCodeExtensions _codeToString;

        public UserAccess(IEventAggregator ea)
        {
            _userClient = new HttpClient
            {
                BaseAddress = new Uri(App.ServerPath),
            };

            _userClient.DefaultRequestHeaders.Accept.Clear();
            _userClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _ea = ea;

            _jsonOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<string> GetRegistrationCode()
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            string responded = "";

            HttpResponseMessage respond = await _userClient.GetAsync("api/account/regcode");
            
            if (respond.IsSuccessStatusCode)
            {
                var responseContent = respond.Content.ReadAsStringAsync().Result;
                responded = JsonSerializer.Deserialize<string>(responseContent, _jsonOption);
            }

            Debug.WriteLine(responded);

            return responded;

        }

        public async Task<TokenDto> GetTokenByDto<T>(T Dto)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            var content = SerializeDataToHttpContent(Dto);
            TokenDto tokenDto = null;   // = new TokenDto();

            HttpResponseMessage respond = await _userClient.PostAsync("api/account/token", content);
            
            if (respond.IsSuccessStatusCode)
            {
                string respondContent = await respond.Content.ReadAsStringAsync();
                tokenDto = JsonSerializer.Deserialize<TokenDto>(respondContent, _jsonOption);
            }
            else
            {
                string respondContent = await respond.Content.ReadAsStringAsync();
                var code = JsonSerializer.Deserialize<MessageWithCode>(respondContent, _jsonOption);
                MessageBox.Show(code.Message);
            }    
            return tokenDto;
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

        public Task SetUserRoles(Guid userId, List<string> roles)
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
