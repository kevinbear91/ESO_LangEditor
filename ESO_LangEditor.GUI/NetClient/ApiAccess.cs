using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.NetClient
{
    public class ApiAccess : IApiAccess
    {
        private HttpClient _userClient;

        public ApiAccess(string serverAddress)
        {
            _userClient = new HttpClient
            {
                BaseAddress = new Uri(serverAddress + "api/account/"),
            };

            _userClient.DefaultRequestHeaders.Accept.Clear();
            _userClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        }

        public Task<HttpResponseMessage> Get(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> Get(string url, string token)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            //var content = SerializeDataToHttpContent(Dto);

            HttpResponseMessage response = await _userClient.GetAsync(url);

            return response;

        }

        public async Task<HttpResponseMessage> Get(string url, string token, byte[] dto)
        {
            _userClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var content = SerializeDataToHttpContent(dto);

            HttpResponseMessage response = await _userClient.GetAsync(url);

            return response;
        }

        public async Task<HttpResponseMessage> GetToken(byte[] loginUserDto)
        {
            var content = SerializeDataToHttpContent(loginUserDto);

            HttpResponseMessage response = await _userClient.PostAsync("login", content);

            //if (response.IsSuccessStatusCode)
            //{
            //    var responseContent = await response.Content.ReadAsStringAsync();
            //}

            //Debug.WriteLine("AuthToken: {0}. RefreshToken: {1} .", revicedToken.AuthToken, revicedToken.RefreshToken);
            return response;
        }

        public async Task<HttpResponseMessage> GetToken(Guid userId, byte[] TokenDto)
        {
            var content = SerializeDataToHttpContent(TokenDto);

            HttpResponseMessage response = await _userClient.PostAsync(userId.ToString() + "/refresh", content);

            return response;
        }

        public Task<HttpResponseMessage> Post(string url, byte[] data)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> Post(string url, string token, byte[] data)
        {
            throw new NotImplementedException();
        }

        private HttpContent SerializeDataToHttpContent(byte[] data)
        {
            //var myContent = JsonSerializer.SerializeToUtf8Bytes(data);

            var byteContent = new ByteArrayContent(data);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }
    }
}
