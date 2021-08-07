using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

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
            _userClient = App.HttpClient;

            //_userClient = new HttpClient
            //{
            //    BaseAddress = new Uri(App.ServerPath),
            //};

            //_userClient.DefaultRequestHeaders.Accept.Clear();
            //_userClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

            HttpResponseMessage respond = await _userClient.GetAsync("api/account/registrationcode");

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
                var code = JsonSerializer.Deserialize<ApiMessageWithCode>(respondContent, _jsonOption);
                MessageBox.Show(code.ApiMessageCodeString());
            }
            return tokenDto;
        }

        public async Task<TokenDto> GetTokenByLogin(LoginUserDto loginUserDto)
        {
            var content = SerializeDataToHttpContent(loginUserDto);
            TokenDto tokenDto = null;   // = new TokenDto();

            HttpResponseMessage respond = await _userClient.PostAsync("api/account/login", content);

            if (respond.IsSuccessStatusCode)
            {
                string respondContent = await respond.Content.ReadAsStringAsync();
                tokenDto = JsonSerializer.Deserialize<TokenDto>(respondContent, _jsonOption);
            }
            else
            {
                string respondContent = await respond.Content.ReadAsStringAsync();
                var code = JsonSerializer.Deserialize<ApiMessageWithCode>(respondContent, _jsonOption);
                MessageBox.Show(code.ApiMessageCodeString());
            }
            return tokenDto;
        }

        public Task<UserDto> GetUserInfoFromServer(Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserInClientDto>> GetUserList()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetUserPasswordRecoveryCode(Guid userGuid)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            string code = "";

            HttpResponseMessage respond = await _userClient.GetAsync("api/admin/passwordrecoverycode/" + userGuid);

            if (respond.IsSuccessStatusCode)
            {
                code = await respond.Content.ReadAsStringAsync();
            }
            else
            {
                string respondContent = await respond.Content.ReadAsStringAsync();
                var Apicode = JsonSerializer.Deserialize<ApiMessageWithCode>(respondContent, _jsonOption);
                MessageBox.Show(Apicode.ApiMessageCodeString());
            }
            return code;
        }

        public async Task<List<string>> GetUserRoles(Guid userGuid)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            List<string> roles = null;

            HttpResponseMessage response = await _userClient.GetAsync(
                "api/account/roles/" + userGuid.ToString());

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                roles = JsonSerializer.Deserialize<List<string>>(responseContent, _jsonOption);
            }
            else
            {
                var Apicode = JsonSerializer.Deserialize<ApiMessageWithCode>(responseContent, _jsonOption);
                MessageBox.Show(Apicode.ApiMessageCodeString());
            }

            return roles;
        }

        public async Task<ApiMessageWithCode> SetUserRoles(Guid userId, List<string> roles)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            var content = SerializeDataToHttpContent(roles);

            HttpResponseMessage response = await _userClient.PostAsync(
                "api/admin/roles/" + userId.ToString(), content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var Apicode = JsonSerializer.Deserialize<ApiMessageWithCode>(responseContent, _jsonOption);
            //MessageBox.Show(Apicode.ApiMessageCodeString());

            return Apicode;
        }

        public void SaveToken(TokenDto token)
        {
            var username = GetUserNameFromToken(token.AuthToken);
            var config = App.LangConfig;

            config.UserAuthToken = token.AuthToken;
            config.UserRefreshToken = token.RefreshToken;
            config.UserName = username;

            App.LangConfig.UserAuthToken = token.AuthToken;
            App.LangConfig.UserRefreshToken = token.RefreshToken;

            AppConfigClient.Save(config);
        }

        public List<string> GetUserRoleFromToken(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiMessageWithCode> SetUserPasswordChange(UserPasswordChangeDto userPasswordChangeDto)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            var content = SerializeDataToHttpContent(userPasswordChangeDto);

            HttpResponseMessage response = await _userClient.PostAsync(
                "api/account/passwordchange", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var Apicode = JsonSerializer.Deserialize<ApiMessageWithCode>(responseContent, _jsonOption);
            //MessageBox.Show(Apicode.ApiMessageCodeString());

            return Apicode;
        }

        public async Task<ApiMessageWithCode> SetUserInfoChange(UserInfoChangeDto userInfoChangeDto)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            var content = SerializeDataToHttpContent(userInfoChangeDto);

            HttpResponseMessage response = await _userClient.PostAsync(
                "api/account/infochange", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var Apicode = JsonSerializer.Deserialize<ApiMessageWithCode>(responseContent, _jsonOption);
            //MessageBox.Show(Apicode.ApiMessageCodeString());

            return Apicode;
        }

        public Task<ApiMessageWithCode> AddNewRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiMessageWithCode> SetUserInfo(SetUserInfoDto setUserInfoDto)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            var content = SerializeDataToHttpContent(setUserInfoDto);

            HttpResponseMessage response = await _userClient.PostAsync(
                "api/admin/modifyUser/" + setUserInfoDto.UserID.ToString(), content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var Apicode = JsonSerializer.Deserialize<ApiMessageWithCode>(responseContent, _jsonOption);
            //MessageBox.Show(Apicode.ApiMessageCodeString());

            return Apicode;
        }

        public async Task<ApiMessageWithCode> SetUserPasswordToRandom(Guid userGuid)
        {
            _userClient.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            HttpResponseMessage response = await _userClient.GetAsync(
                "api/admin/setPasswordRandom/" + userGuid.ToString());

            var responseContent = await response.Content.ReadAsStringAsync();

            var Apicode = JsonSerializer.Deserialize<ApiMessageWithCode>(responseContent, _jsonOption);
            //MessageBox.Show(Apicode.ApiMessageCodeString());

            return Apicode;
        }

        public async Task<ApiMessageWithCode> AddNewUser(RegistrationUserDto registrationUserDto)
        {
            var content = SerializeDataToHttpContent(registrationUserDto);

            HttpResponseMessage respond = await _userClient.PostAsync("api/account/register", content);

            string respondContent = await respond.Content.ReadAsStringAsync();
            var code = JsonSerializer.Deserialize<ApiMessageWithCode>(respondContent, _jsonOption);
            //MessageBox.Show(code.ApiMessageCodeString());

            return code;
        }

        public async Task<ApiMessageWithCode> SetUserPasswordByRecoveryCode(UserPasswordRecoveryDto userPasswordResetDto)
        {
            var content = SerializeDataToHttpContent(userPasswordResetDto);

            HttpResponseMessage response = await _userClient.PostAsync(
                "api/account/passwordrecovery", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var Apicode = JsonSerializer.Deserialize<ApiMessageWithCode>(responseContent, _jsonOption);
            //MessageBox.Show(Apicode.ApiMessageCodeString());

            return Apicode;
        }

        private HttpContent SerializeDataToHttpContent(object data)
        {
            var myContent = JsonSerializer.SerializeToUtf8Bytes(data);

            var byteContent = new ByteArrayContent(myContent);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }

        private string GetUserNameFromToken(string token)
        {
            string userNameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

            var jsontoken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var userName = jsontoken.Claims.First(claim => claim.Type == userNameClaim).Value;

            return userName;
        }


    }
}
