using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ESO_LangEditor.Core.Models
{
    public class PackLangVersion
    {
        public string AddonVersion { get; set; }
        public string AddonApiVersion { get; set; }

        private static readonly string ADDONVERSION_FILE = "AddonVersion.json";

        public PackLangVersion()
        {
            AddonVersion = "v0.16.2";
            AddonApiVersion = "100034";
        }

        public static PackLangVersion Load()
        {
            PackLangVersion config;

            if (File.Exists(ADDONVERSION_FILE))
            {
                string configContent = File.ReadAllText(ADDONVERSION_FILE);
                config = JsonSerializer.Deserialize<PackLangVersion>(configContent);
                return config;
            }
            else
            {
                config = new PackLangVersion();
                return config;
            }
        }

        public static void Save(PackLangVersion config)
        {
            FileStream configFileStream = null;
            StreamWriter configStreamWriter = null;

            configFileStream = File.Open(ADDONVERSION_FILE, FileMode.Create);
            configStreamWriter = new StreamWriter(configFileStream);

            var json = JsonSerializer.Serialize(config);

            configStreamWriter.Write(json);
            configStreamWriter.Close();
        }


    }
}
