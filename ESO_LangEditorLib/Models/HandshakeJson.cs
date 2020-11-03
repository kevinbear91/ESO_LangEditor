using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ESO_LangEditorLib.Models
{
    public class HandshakeJson
    {
        public string LangEditorVersion { get; set; }
        public string LangFullpackPath { get; set; }
        public string LangFullpackSHA256 { get; set; }
        public int LangDatabaseRevised { get; set; }
        public string LangDatabasePath { get; set; }
        public string LangUpdaterExeSha256 { get; set; }
        public string LangUpdaterPackPath { get; set; }
        public string LangUpdaterPackSha256 { get; set; }
        public string LangResourcesPackPath { get; set; }
        //public string 

        public HandshakeJson()
        {
            LangEditorVersion = "3.0.0";
            LangFullpackPath = "LangEditorFullUpdate.zip";
            LangFullpackSHA256 = "";
            LangDatabaseRevised = 28;
            LangDatabasePath = "LangDatabasev3.db";
            LangUpdaterExeSha256 = "148A5B8E618226C749D06F33308DB435186ABA7CCC1FB34A75841D391594E886";
            LangUpdaterPackPath = "UpdaterPack.zip";
            LangUpdaterPackSha256 = "4D9E0E019680598B9599C622F60F2192F2A423F0119E0B030DE4F9BE1A9ACEAD";
            LangResourcesPackPath = "Resources.zip";
        }

        public static void Save(HandshakeJson serverConfig)
        {
            

            FileStream configFileStream = null;
            StreamWriter configStreamWriter = null;

            string CONFIG_FILE = "AppConfig.json";

            configFileStream = File.Open(CONFIG_FILE, FileMode.Create);
            configStreamWriter = new StreamWriter(configFileStream);

            var json = JsonSerializer.Serialize(serverConfig);

            configStreamWriter.Write(json);
            configStreamWriter.Flush();


        }
    }
}
