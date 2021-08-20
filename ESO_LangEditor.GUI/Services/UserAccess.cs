using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.EventAggres;
using Microsoft.Extensions.Logging;
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
        private ILogger _logger;
        //private readonly ApiMessageWithCodeExtensions _codeToString;

        public UserAccess(IEventAggregator ea, ILogger logger)
        {
            _userClient = App.HttpClient;
            _ea = ea;
            _logger = logger;

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

        public async Task<TokenDto> GetTokenByTokenDto(Guid userGuid, TokenDto Dto)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            var content = SerializeDataToHttpContent(Dto);
            TokenDto tokenDto = null;   // = new TokenDto();

            HttpResponseMessage respond = await _userClient.PostAsync("api/account/token/" + userGuid, content);
           
            if (respond.IsSuccessStatusCode)
            {
                string respondContent = await respond.Content.ReadAsStringAsync();
                tokenDto = JsonSerializer.Deserialize<TokenDto>(respondContent, _jsonOption);
                GetUserRoleFromToken(tokenDto.AuthToken);
                //_logger.LogDebug("获取TokenDto成功。");
            }
            else
            {
                string respondContent = await respond.Content.ReadAsStringAsync();
                var code = JsonSerializer.Deserialize<MessageWithCode>(respondContent, _jsonOption);
                MessageBox.Show(code.Message, code.Code.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
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
                GetUserRoleFromToken(tokenDto.AuthToken);
            }
            else
            {
                string respondContent = await respond.Content.ReadAsStringAsync();
                var code = JsonSerializer.Deserialize<MessageWithCode>(respondContent, _jsonOption);
                MessageBox.Show(code.Message, code.Code.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return tokenDto;
        }

        public async Task<UserDto> GetUserInfoFromServer(Guid userGuid)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            UserDto user = null;

            HttpResponseMessage respond = await _userClient.GetAsync("api/admin/user/" + userGuid);

            if (respond.IsSuccessStatusCode)
            {
                var responseContent = await respond.Content.ReadAsStringAsync();
                user = JsonSerializer.Deserialize<UserDto>(responseContent, _jsonOption);
            }
            else
            {
                string respondContent = await respond.Content.ReadAsStringAsync();
                var code = JsonSerializer.Deserialize<MessageWithCode>(respondContent, _jsonOption);
                MessageBox.Show(code.Message, code.Code.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return user;
        }

        public async Task<List<UserInClientDto>> GetUserList()
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            List<UserInClientDto> userList = null;

            HttpResponseMessage respond = await _userClient.GetAsync("api/account/users");

            if (respond.IsSuccessStatusCode)
            {
                var responseContent = await respond.Content.ReadAsStringAsync();
                userList = JsonSerializer.Deserialize<List<UserInClientDto>>(responseContent, _jsonOption);
            }
            else
            {
                string respondContent = await respond.Content.ReadAsStringAsync();
                var code = JsonSerializer.Deserialize<MessageWithCode>(respondContent, _jsonOption);
                MessageBox.Show(code.Message, code.Code.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return userList;
        }

        public async Task<string> GetUserPasswordRecoveryCode(Guid userGuid)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            string code = null;

            HttpResponseMessage respond = await _userClient.GetAsync("api/admin/passwordrecoverycode/" + userGuid);

            if (respond.IsSuccessStatusCode)
            {
                code = await respond.Content.ReadAsStringAsync();
            }
            else
            {
                string respondContent = await respond.Content.ReadAsStringAsync();
                var Apicode = JsonSerializer.Deserialize<MessageWithCode>(respondContent, _jsonOption);
                MessageBox.Show(Apicode.Message, Apicode.Code.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
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
                var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);
                MessageBox.Show(code.Message, code.Code.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return roles;
        }

        public async Task<MessageWithCode> SetUserRoles(Guid userId, List<string> roles)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            var content = SerializeDataToHttpContent(roles);

            HttpResponseMessage response = await _userClient.PostAsync(
                "api/admin/roles/" + userId, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var Apicode = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);
            //MessageBox.Show(Apicode.ApiMessageCodeString());

            return Apicode;
        }

        public void SaveToken(TokenDto token)
        {
            var username = GetUserNameFromToken(token.AuthToken);
            var userGuid = GetUserGuidFromToken(token.AuthToken);
            var config = App.LangConfig;

            config.UserAuthToken = token.AuthToken;
            config.UserRefreshToken = token.RefreshToken;
            config.UserName = username;
            config.UserGuid = userGuid;

            App.LangConfig.UserAuthToken = token.AuthToken;
            App.LangConfig.UserRefreshToken = token.RefreshToken;

            AppConfigClient.Save(config);
        }

        public List<string> GetUserRoleFromToken(string token)
        {
            string userRoleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

            var jsontoken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var roleList = jsontoken.Claims.Where(claim => claim.Type == userRoleClaim)
                .Select(c => c.Value).ToList();

            foreach (var role in roleList)
            {
                Debug.WriteLine(role);
            }

            _ea.GetEvent<RoleListUpdateEvent>().Publish(roleList);

            return roleList;
        }

        public async Task<MessageWithCode> SetUserPasswordChange(UserPasswordChangeDto userPasswordChangeDto)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            var content = SerializeDataToHttpContent(userPasswordChangeDto);

            HttpResponseMessage response = await _userClient.PostAsync(
                "api/account/passwordchange", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var Apicode = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);
            //MessageBox.Show(Apicode.ApiMessageCodeString());

            return Apicode;
        }

        public async Task<MessageWithCode> SetUserInfoChange(UserInfoChangeDto userInfoChangeDto)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            var content = SerializeDataToHttpContent(userInfoChangeDto);

            HttpResponseMessage response = await _userClient.PostAsync(
                "api/account/infochange", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var Apicode = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);
            //MessageBox.Show(Apicode.ApiMessageCodeString());

            return Apicode;
        }

        public Task<MessageWithCode> AddNewRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<MessageWithCode> SetUserInfo(SetUserInfoDto setUserInfoDto)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            var content = SerializeDataToHttpContent(setUserInfoDto);

            HttpResponseMessage response = await _userClient.PostAsync(
                "api/admin/modifyUser/" + setUserInfoDto.UserID, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var Apicode = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);
            //MessageBox.Show(Apicode.ApiMessageCodeString());

            return Apicode;
        }

        public async Task<MessageWithCode> SetUserPasswordToRandom(Guid userGuid)
        {
            _userClient.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            HttpResponseMessage response = await _userClient.GetAsync(
                "api/admin/setPasswordRandom/" + userGuid.ToString());

            var responseContent = await response.Content.ReadAsStringAsync();

            var Apicode = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);
            //MessageBox.Show(Apicode.ApiMessageCodeString());

            return Apicode;
        }

        public async Task<MessageWithCode> AddNewUser(RegistrationUserDto registrationUserDto)
        {
            var content = SerializeDataToHttpContent(registrationUserDto);

            HttpResponseMessage respond = await _userClient.PostAsync("api/account/register", content);

            string respondContent = await respond.Content.ReadAsStringAsync();
            var code = JsonSerializer.Deserialize<MessageWithCode>(respondContent, _jsonOption);
            //MessageBox.Show(code.ApiMessageCodeString());

            return code;
        }

        public async Task<MessageWithCode> SetUserPasswordByRecoveryCode(UserPasswordRecoveryDto userPasswordResetDto)
        {
            var content = SerializeDataToHttpContent(userPasswordResetDto);

            HttpResponseMessage response = await _userClient.PostAsync(
                "api/account/passwordrecovery", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var Apicode = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);
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

        private Guid GetUserGuidFromToken(string token)
        {
            string userGuidClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

            var jsontoken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var userGuidString = jsontoken.Claims.First(claim => claim.Type == userGuidClaim).Value;
            var userGuid = new Guid(userGuidString);

            return userGuid;
        }

        public DateTimeOffset GetTokenExpireTimer()
        {
            string TokenExpireClaim = "exp";

            var jsontoken = new JwtSecurityTokenHandler().ReadJwtToken(App.LangConfig.UserAuthToken);
            var userExpireTimerString = jsontoken.Claims.First(claim => claim.Type == TokenExpireClaim).Value;
            var expireTimer = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(userExpireTimerString));

            return expireTimer;
        }
    }
}
