using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.NetClient
{
    public class StartupNetCheck
    {
        private readonly HttpClient client;

        public StartupNetCheck()
        {
            client = new HttpClient();
        }

        public async Task<string> GetServerRespondAndConfig(string path)
        {
            //AppConfigServer appConfigServer = null;
            string result = null;

            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
               result = response.Content.ReadAsStringAsync().Result;
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
