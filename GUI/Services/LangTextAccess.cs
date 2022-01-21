using Core.Models;
using Core.RequestParameters;
using Core.EnumTypes;
using GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace GUI.Services
{
    public class LangTextAccess : ILangTextAccess
    {
        private readonly HttpClient _langHttpClient;
        private JsonSerializerOptions _jsonOption;

        public LangTextAccess()
        {
            _langHttpClient = App.HttpClient;

            _jsonOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }


        public async Task<MessageWithCode> AddLangTexts(List<LangTextForCreationDto> langTextForCreationDtos)
        {
            //EncodingProvider provider = CodePagesEncodingProvider.Instance;
            //Encoding.RegisterProvider(provider);

            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            var content = SerializeDataToHttpContent(langTextForCreationDtos);

            HttpResponseMessage response = await _langHttpClient.PostAsync(
                "api/langtext/new/list", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            //byte[] buf = await response.Content.ReadAsByteArrayAsync();
            //string responseContent2 = Encoding.UTF8.GetString(buf);

            //MessageBox.Show(responseContent);
            //MessageBox.Show(responseContent2);

            var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);
            //var code = new MessageWithCode{ Code = 111, Message = "111" };

            return code;
        }

        public async Task<MessageWithCode> ApproveLangTextsInReview(List<Guid> langTextGuids)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            var content = SerializeDataToHttpContent(langTextGuids);

            HttpResponseMessage response = await _langHttpClient.PostAsync(
                "api/langtext/review", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);

            return code;
        }



        public async Task<IEnumerable<LangTextRevisedDto>> GetLangTextRevisedDtos(int revNumber)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            List<LangTextRevisedDto> respondedList = null;

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await _langHttpClient.GetAsync("api/revise/LangTextRev/" + revNumber);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                respondedList = JsonSerializer.Deserialize<List<LangTextRevisedDto>>(responseContent, _jsonOption);
            }

            //Debug.WriteLine(responded);

            return respondedList;
        }

        //public async Task<int> GetLangTextRevisedNumber()
        //{
        //    _langHttpClient.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
        //    int responded = 0;

        //    //var content = SerializeDataToHttpContent(langtextGuid);

        //    HttpResponseMessage response = await _langHttpClient.GetAsync("api/revise/LangTextRev/");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var responseContent = response.Content.ReadAsStringAsync().Result;
        //        responded = JsonSerializer.Deserialize<int>(responseContent, _jsonOption);
        //    }

        //    Debug.WriteLine(responded);

        //    return responded;
        //}

        public async Task<List<LangTextRevNumberDto>> GetAllRevisedNumber()
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            List<LangTextRevNumberDto> responded = null;

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await _langHttpClient.GetAsync("api/revise");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                responded = JsonSerializer.Deserialize<List<LangTextRevNumberDto>>(responseContent, _jsonOption);
            }

            Debug.WriteLine(responded);

            return responded;
        }

        public async Task<LangTextDto> GetLangText(Guid langtextGuid)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            LangTextDto respondedList = null;

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await _langHttpClient.GetAsync(
                "api/langtext/" + langtextGuid);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                respondedList = JsonSerializer.Deserialize<LangTextDto>(responseContent, _jsonOption);
            }
            else
            {
                var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);
                MessageBox.Show(code.Message);
            }

            //Debug.WriteLine(respondedList);
            return respondedList;
        }

        public async Task<LangTextDto> GetLangText(string langtextUniqueId)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            LangTextDto respondedList = null;

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await _langHttpClient.GetAsync(
                "api/langtext/" + langtextUniqueId);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                respondedList = JsonSerializer.Deserialize<LangTextDto>(responseContent, _jsonOption);
            }
            else
            {
                var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);
                MessageBox.Show(code.Message);
            }

            //Debug.WriteLine(respondedList);
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
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                respondedList = JsonSerializer.Deserialize<List<LangTextDto>>(responseContent, _jsonOption);
            }
            else
            {
                var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);
                MessageBox.Show(code.Message);
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
                "api/langtext/rev/" + langtextRevised);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                respondedList = JsonSerializer.Deserialize<List<LangTextDto>>(responseContent, _jsonOption);
            }
            else
            {
                var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);
                MessageBox.Show(code.Message);
            }

            Debug.WriteLine(respondedList);

            return respondedList;
        }

        public async Task<List<LangTextForArchiveDto>> GetLangTextsFromArchive(string langtextId)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            List<LangTextForArchiveDto> respondedList = null;

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await _langHttpClient.GetAsync(
                "api/langtext/archive/" + langtextId);
            var responseContent = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                respondedList = JsonSerializer.Deserialize<List<LangTextForArchiveDto>>(responseContent, _jsonOption);
                Debug.WriteLine($"GetLangTextsFromArchive count: {respondedList.Count}");
            }

            return respondedList;
        }

        public async Task<List<LangTextForReviewDto>> GetLangTextsFromReviewer(Guid userGuid)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            List<LangTextForReviewDto> respondedLangtext = null;

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await _langHttpClient.GetAsync(
                "api/langtext/review/user/" + userGuid);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                respondedLangtext = JsonSerializer.Deserialize<List<LangTextForReviewDto>>(responseContent, _jsonOption);
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);
                MessageBox.Show(code.Message);
            }

            return respondedLangtext;
        }

        public async Task<LangTextForReviewDto> GetLangTextFromReview(Guid langtextGuid)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            LangTextForReviewDto respondedLangtext = null;

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await _langHttpClient.GetAsync(
                "api/langtext/review/" + langtextGuid);
            var responseContent = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                respondedLangtext = JsonSerializer.Deserialize<LangTextForReviewDto>(responseContent, _jsonOption);
            }
            //else
            //{
            //    var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);

            //}

            return respondedLangtext;
        }

        public async Task<List<Guid>> GetUsersInReview()
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            List<Guid> respondedUserList = null;

            //var content = SerializeDataToHttpContent(langtextGuid);

            HttpResponseMessage response = await _langHttpClient.GetAsync(
                "api/langtext/review/users");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                respondedUserList = JsonSerializer.Deserialize<List<Guid>>(responseContent, _jsonOption);
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);
                MessageBox.Show(code.Message);
            }

            //Debug.WriteLine(respondedUserList);

            return respondedUserList;
        }

        public async Task<MessageWithCode> RemoveLangTexts(List<Guid> langTextGuids)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);

            var content = SerializeDataToHttpContent(langTextGuids);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(_langHttpClient.BaseAddress + "api/langtext"),
                Content = content,
            };

            var response = await _langHttpClient.SendAsync(request);
            var responseContent = response.Content.ReadAsStringAsync().Result;

            var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);

            return code;
        }

        public async Task<MessageWithCode> RemoveLangTextsInReview(Guid langTextGuid)
        {
            throw new NotImplementedException();
        }

        public async Task<MessageWithCode> RemoveLangTextsInReview(List<Guid> langTextGuids)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            var content = SerializeDataToHttpContent(langTextGuids);
            //string langTextId = langTextForUpdateZhDto.Id.ToString();

            HttpResponseMessage response = await _langHttpClient.PostAsync(
                "api/langtext/review/del/list", content);

            string responseContent = await response.Content.ReadAsStringAsync();
            MessageWithCode code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);

            return code;
        }

        public async Task<MessageWithCode> UpdateLangTextEn(List<LangTextForUpdateEnDto> langTextForUpdateEnDtos)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            var content = SerializeDataToHttpContent(langTextForUpdateEnDtos);

            HttpResponseMessage response = await _langHttpClient.PostAsync(
                "api/langtext/en/list", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);

            return code;
        }

        public async Task<MessageWithCode> UpdateLangTextZh(LangTextForUpdateZhDto langTextForUpdateZhDto)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            var content = SerializeDataToHttpContent(langTextForUpdateZhDto);
            string langTextId = langTextForUpdateZhDto.Id.ToString();

            HttpResponseMessage response = await _langHttpClient.PostAsync(
                "api/langtext/zh/" + langTextId, content);

            string responseContent = await response.Content.ReadAsStringAsync();
            MessageWithCode code = JsonSerializer.Deserialize<MessageWithCode>(responseContent, _jsonOption);

            return code;
        }

        public async Task<MessageWithCode> UpdateLangTextZh(List<LangTextForUpdateZhDto> langTextForUpdateZhDtos)
        {
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            var content = SerializeDataToHttpContent(langTextForUpdateZhDtos);

            HttpResponseMessage response = await _langHttpClient.PostAsync(
                "api/langtext/zh/list", content);

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

        public async Task<PagedList<LangTextDto>> GetLangTexts(string category, LangTextParameters langTextParameters)
        {
            //_langHttpClient.DefaultRequestHeaders.Clear();
            _langHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", App.LangConfig.UserAuthToken);
            PagedList<LangTextDto> respondedLangtext = null;
            PageData respondedPageData;
            string paraJson = null;

            paraJson = JsonSerializer.Serialize(langTextParameters);

            if (_langHttpClient.DefaultRequestHeaders.Contains("langTextParameters"))
            {
                _langHttpClient.DefaultRequestHeaders.Remove("langTextParameters");
            }

            _langHttpClient.DefaultRequestHeaders.Add("langTextParameters", paraJson);
            //Response.Headers.Add("langTextParameters", JsonSerializer.Serialize(langTextParameters));
            Debug.WriteLine(paraJson);

            HttpResponseMessage response = await _langHttpClient.GetAsync(
                "api/langtext/" + category);
            var responseContent = await response.Content.ReadAsStringAsync();
            string responseHeader = response.Headers.GetValues("X-Pagination").FirstOrDefault();

            Debug.WriteLine(responseHeader);

            if (response.IsSuccessStatusCode)
            {
                var langtext = JsonSerializer.Deserialize<List<LangTextDto>>(responseContent, _jsonOption);
                respondedPageData = JsonSerializer.Deserialize<PageData>(responseHeader, _jsonOption);
                //Debug.WriteLine($"respondedLangtext: {respondedLangtext.Count}");
                respondedLangtext = new PagedList<LangTextDto>(langtext, langtext.Count, respondedPageData.PageCount, respondedPageData.PageSize)
                {
                    PageData = respondedPageData
                };
            }

            return respondedLangtext;
        }

        
    }
}
