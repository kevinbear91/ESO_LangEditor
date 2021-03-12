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

namespace ESO_LangEditor.GUI.NetClient
{
    public class ApiAccess
    {
        private readonly HttpClient client;
        private JsonSerializerOptions _jsonOption;
        //private AppConfigClient _configClient;

        public ApiAccess()
        {
            //_configClient = configClient;

            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44343/");

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

            var responseContent = response.Content.ReadAsStringAsync().Result;
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

            var responseContent = response.Content.ReadAsStringAsync().Result;
            var json = JsonSerializer.Deserialize<TokenDto>(responseContent, _jsonOption);

            Debug.WriteLine("AuthToken: {0}. RefreshToken: {1} .", json.AuthToken, json.RefreshToken);

            return json;

        }

        public async Task<TokenDto> GetRefreshedTokenByCurrentToken(TokenDto token)
        {
            var content = SerializeDataToHttpContent(token);

            HttpResponseMessage response = await client.PostAsync(
                "auth/refresh", content);

            response.EnsureSuccessStatusCode();

            var responseContent = response.Content.ReadAsStringAsync().Result;
            var json = JsonSerializer.Deserialize<TokenDto>(responseContent, _jsonOption);

            Debug.WriteLine("AuthToken: {0}. RefreshToken: {1} .", json.AuthToken, json.RefreshToken);

            return json;

        }

        public async Task<bool> UserProfileInit(UserInfoChangeDto userInfoChangeDto,string token)
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

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            //var content = SerializeDataToHttpContent(userInfoChangeDto);

            var byteContent = new ByteArrayContent(File.ReadAllBytes(filePath));
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            HttpResponseMessage response = await client.PostAsync(
                "api/account/" + userId + "/avatar", byteContent);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;

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
