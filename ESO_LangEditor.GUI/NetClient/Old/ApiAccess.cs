using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.NetClient.Old
{
    public class ApiAccess
    {
        private readonly HttpClient client;
        private JsonSerializerOptions _jsonOption;
        //private AppConfigClient _configClient;

        public ApiAccess(string serverAddress)
        {
            //_configClient = configClient;

            client = new HttpClient();
            client.BaseAddress = new Uri(serverAddress);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Authorization = 
            //    new AuthenticationHeaderValue("Bearer", _configClient.UserAuthToken);

            _jsonOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }


        public async Task<TokenDto> GetLoginToken(LoginUserDto loginUser)
        {
            //HttpContent content;

            var content = SerializeDataToHttpContent(loginUser);

            HttpResponseMessage response = await client.PostAsync(
                "api/account/login", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<TokenDto>(responseContent, _jsonOption);

            Debug.WriteLine("AuthToken: {0}. RefreshToken: {1} .", json.AuthToken, json.RefreshToken);

            return json;
            
        }

        public async Task<TokenDto> FirstTimeLogin(LoginUserDto loginUser)
        {
            //HttpContent content;

            var content = SerializeDataToHttpContent(loginUser);

            HttpResponseMessage response = await client.PostAsync(
                "api/account/FirstTimeLogin", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<TokenDto>(responseContent, _jsonOption);

            Debug.WriteLine("AuthToken: {0}. RefreshToken: {1} .", json.AuthToken, json.RefreshToken);

            return json;

        }

        public async Task<TokenDto> GetRefreshedTokenByCurrentToken(TokenDto token)
        {
            var content = SerializeDataToHttpContent(token);

            HttpResponseMessage response = await client.PostAsync(
                "api/account/refresh", content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<TokenDto>(responseContent, _jsonOption);

            Debug.WriteLine("AuthToken: {0}. RefreshToken: {1} .", json.AuthToken, json.RefreshToken);

            return json;

        }

        public async Task<List<UserInClientDto>> GetUserList(string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.GetAsync(
                "api/account/users");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<List<UserInClientDto>>(responseContent, _jsonOption);

            //Debug.WriteLine("AuthToken: {0}. RefreshToken: {1} .", json.AuthToken, json.RefreshToken);

            return json;

        }

        public async Task<List<string>> GetUserRoleList(Guid userId, string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.GetAsync(
                "api/account/" + userId.ToString() + "/roles");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<List<string>>(responseContent, _jsonOption);

            //Debug.WriteLine("AuthToken: {0}. RefreshToken: {1} .", json.AuthToken, json.RefreshToken);

            return json;

        }

        public async Task<bool> ModifyUserRoles(Guid userId, List<string> roles, string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var content = SerializeDataToHttpContent(roles);

            HttpResponseMessage response = await client.PostAsync(
                "api/admin/" + userId.ToString() + "/roles", content);

            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;

        }

        public async Task<bool> AddUserRole(string role, string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var content = SerializeDataToHttpContent(role);

            HttpResponseMessage response = await client.PostAsync(
                "api/admin/role", content);

            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;

        }

        public async Task<UserDto> AddUser(UserDto user, string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            //var user = new UserDto { UserName = username };

            var content = SerializeDataToHttpContent(user);

            HttpResponseMessage response = await client.PostAsync(
                "api/admin/register", content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<UserDto>(responseContent, _jsonOption);

            return json;

        }

        public async Task<UserDto> InitUser(Guid userGuid, string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.GetAsync(
                "api/admin/init/" + userGuid);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<UserDto>(responseContent, _jsonOption);

            return json;

        }

        public async Task<bool> UserProfileInit(UserInfoChangeDto userInfoChangeDto, string token)
        {
            //HttpContent content;

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var content = SerializeDataToHttpContent(userInfoChangeDto);

            HttpResponseMessage response = await client.PostAsync(
                "api/account/infoinit", content);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;

        }

        public async Task<bool> UserProfileChange(UserInfoChangeDto userInfoChangeDto, string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var content = SerializeDataToHttpContent(userInfoChangeDto);

            HttpResponseMessage response = await client.PostAsync(
                "api/account/infochange", content);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;

        }

        public async Task<bool> UploadUserAvatar(string filePath, string userId, string token)
        {
            //HttpContent content;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/jpeg"));

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);


            var filename = System.IO.Path.GetFileName(filePath);
            var byteContent = new ByteArrayContent(File.ReadAllBytes(filePath));
            //byteContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            var content = new MultipartFormDataContent
            {
                { byteContent, "avatar", filename }
            };

            HttpResponseMessage response = await client.PostAsync(
                "api/account/" + userId + "/avatar", content);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;

            //var responseContent = response.Content.ReadAsStringAsync().Result;
            //var json = JsonSerializer.Deserialize<TokenDto>(responseContent, _jsonOption);

            //Debug.WriteLine("AuthToken: {0}. RefreshToken: {1} .", json.AuthToken, json.RefreshToken);

            //return json;

        }

        public async Task<Stream> DownloadUserAvatar(string filename, string token)
        {
            //HttpContent content;
            //BitmapImage bitmapImage = new BitmapImage();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.GetAsync("images/" + filename);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync();

            //return response.IsSuccessStatusCode;

            //var responseContent = response.Content.ReadAsStringAsync().Result;
            //var json = JsonSerializer.Deserialize<TokenDto>(responseContent, _jsonOption);

            //Debug.WriteLine("AuthToken: {0}. RefreshToken: {1} .", json.AuthToken, json.RefreshToken);

            //return json;

        }


        private HttpContent SerializeDataToHttpContent(object data)
        {
            var myContent = JsonSerializer.SerializeToUtf8Bytes(data);

            var byteContent = new ByteArrayContent(myContent);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }


        //private void DeserializeDataFromHttpContent(HttpContent content)
        //{

        //    var json = JsonSerializer.Deserialize(content.ReadAsByteArrayAsync());
        //}
    }
}
