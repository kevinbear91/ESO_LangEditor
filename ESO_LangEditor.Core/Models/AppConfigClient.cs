using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ESO_LangEditor.Core.Models
{
    public class AppConfigClient
    {
        public string LangEditorVersion { get; set; }
        public string LangUpdaterSha256 { get; set; }
        public string LangUpdaterDllSha256 { get; set; }
        public string LangUpdaterVersion { get; set; }
        public List<ServerNameDto> LangServerList { get; set; }
        public string DefaultServerName { get; set; }
        public int DatabaseRev { get; set; }
        public int LangDatabaseVersion { get; set; }
        public string UserAuthToken { get; set; }
        public string UserRefreshToken { get; set; }
        public Guid UserGuid { get; set; }
        public string UserName { get; set; }
        public string UserAvatarFileName { get; set; }

        public AppConfigClient()
        {
            LangServerList = new List<ServerNameDto>();

            LangEditorVersion = "3.0.0";
            LangUpdaterSha256 = "6153176c0e166c98940dd4d56412503ee37b7b663e622ca684e9e88b031c12dc";
            LangUpdaterSha256 = "963A3ECE7219EBFAA24180FD1EE1E191AA1A3BA5ED777757B5718423AC8B900C";
            LangUpdaterVersion = "v1.0";
            LangServerList.Add(new ServerNameDto
            {
                ServerName = "LangEditor-IKDC2-v4",
                ServerURL = "http://localhost",
                ServerType = "Ipv4"
            });
            LangServerList.Add(new ServerNameDto
            {
                ServerName = "LangEditor-IKDC2-v6",
                ServerURL = "http://localhost",
                ServerType = "Ipv6"
            });
            DefaultServerName = "LangEditor-IKDC2-v4";
            DatabaseRev = 25;
            LangDatabaseVersion = 1;
            UserAuthToken = "";
            UserRefreshToken = "";
            UserGuid = Guid.Empty;
            UserName = "";
            UserAvatarFileName = "";
        }
        private static readonly string CONFIG_FILE = "LangConfig.json";

        public static AppConfigClient Load()
        {
            AppConfigClient config;

            if (File.Exists(CONFIG_FILE))
            {
                string configContent = File.ReadAllText(CONFIG_FILE);
                config = JsonSerializer.Deserialize<AppConfigClient>(configContent);
                return config;
            }
            else
            {
                config = new AppConfigClient();
                return config;
            }
        }


        public static void Save(AppConfigClient config)
        {
            FileStream configFileStream = null;
            StreamWriter configStreamWriter = null;

            configFileStream = File.Open(CONFIG_FILE, FileMode.Create);
            configStreamWriter = new StreamWriter(configFileStream);

            var json = JsonSerializer.Serialize(config);

            configStreamWriter.Write(json);
            configStreamWriter.Close();


        }
    }

    public class ServerNameDto
    {
        public string ServerName { get; set; }
        public string ServerURL { get; set; }
        public string ServerType { get; set; }
    }
}
