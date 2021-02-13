using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class AppConfigServer
    {
        public string LangEditorVersion { get; set; }
        public string LangFullpackPath { get; set; }
        public string LangFullpackSHA256 { get; set; }
        public int LangDatabaseRevised { get; set; }
        public string LangDatabasePath { get; set; }
        public int LangDatabaseVersion { get; set; }
        public string LangDatabasePackSha256 { get; set; }
        public string LangUpdaterExeSha256 { get; set; }
        public string LangUpdaterDllSha256 { get; set; }
        public string LangUpdaterVersion { get; set; }
        public string LangUpdaterPackPath { get; set; }
        public string LangUpdaterPackSha256 { get; set; }
        public string LangResourcesPackPath { get; set; }
    }
}
