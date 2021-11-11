using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class AppConfigServer
    {
        public string LangEditorVersion { get; set; }
        public string LangFullpackPath { get; set; }
        public string LangFullpackSHA256 { get; set; }
        public string LangDatabasePath { get; set; }
        public string LangDatabasePackSha256 { get; set; }
        public string LangUpdaterSha256 { get; set; }
        public string LangUpdaterVersion { get; set; }
        public string LangUpdaterPackPath { get; set; }
        public string LangUpdaterPackSha256 { get; set; }
        public string LangResourcesPackPath { get; set; }
        public string LangResourcesPackSha256 { get; set; }
        public string LangJpPackPath { get; set; }
        public string LangJpPackSHA256 { get; set; }
    }
}
