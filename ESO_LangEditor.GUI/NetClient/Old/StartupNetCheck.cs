using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.NetClient.Old
{
    public class StartupNetCheck
    {
        private readonly HttpClient client;
        private JsonSerializerOptions _jsonOption;

        public StartupNetCheck()
        {
            client = new HttpClient();

            _jsonOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<AppConfigServer> GetServerRespondAndConfig(string path)
        {
            //AppConfigServer appConfigServer = null;
            AppConfigServer result = null;

            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                result = JsonSerializer.Deserialize<AppConfigServer>(responseContent, _jsonOption);
            }
            
            return result;
        }

        public async Task DownloadUpdater(string uri)
        {
            using var client = new WebClient();
            //client.DownloadFileAsync(new Uri(uri), "UpdaterPack.zip");
            await client.DownloadFileTaskAsync(new Uri(uri), "UpdaterPack.zip");

            return;
        }


    }
}
