using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ESO_LangEditorLib.Models.Client
{
    public class ConfigJson
    {
        public string LangEditorVersion { get; set; }
        public string LangUpdaterSha256 { get; set; }
        public List<ServerNameDto> LangServerList { get; set; }
        public string DefaultServerName { get; set; }
        public int DatabaseRev { get; set; }
        public string UserToken { get; set; }
        public Guid UserGuid { get; set; }

        public ConfigJson()
        {
            LangServerList = new List<ServerNameDto>();

            LangEditorVersion = "3.0.0";
            LangUpdaterSha256 = "6153176c0e166c98940dd4d56412503ee37b7b663e622ca684e9e88b031c12dc";
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
            UserToken = "";
            UserGuid = Guid.Empty;
        }
        private static readonly string CONFIG_FILE = "LangConfig.json";

        public static ConfigJson Load()
        {
            ConfigJson config;

            if (File.Exists(CONFIG_FILE))
            {
                string configContent = File.ReadAllText(CONFIG_FILE);
                config = JsonSerializer.Deserialize<ConfigJson>(configContent);
                return config;
            }
            else
            {
                config = new ConfigJson();
                return config;
            }
        }


        public static void Save(ConfigJson config)
        {
            FileStream configFileStream = null;
            StreamWriter configStreamWriter = null;

            configFileStream = File.Open(CONFIG_FILE, FileMode.Create);
            configStreamWriter = new StreamWriter(configFileStream);

            var json = JsonSerializer.Serialize(config);

            configStreamWriter.Write(json);
            configStreamWriter.Flush();


        }

    }


    public class ServerNameDto
    {
        public string ServerName { get; set; }
        public string ServerURL { get; set; }
        public string ServerType { get; set; }
    }


}
