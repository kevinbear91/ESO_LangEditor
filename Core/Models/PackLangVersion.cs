using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Core.Models
{
    public class PackLangVersion
    {
        public string AddonVersion { get; set; }
        public string AddonApiVersion { get; set; }
        public string AddonVersionInt { get; set; }
        public string UpdateLog { get; set; }

        private static readonly string ADDONVERSION_FILE = "AddonVersion.json";

        public PackLangVersion()
        {
            AddonVersion = "v0.16.2";
            AddonApiVersion = "100034";
            AddonVersionInt = "2";
            UpdateLog = "\n\n|ac|t256:128:EsoZH/Textures/EsozhLogo.dds|t\n\n\n|al汉化插件本次更新内容如下：\n\n已经结束的小丑节翻译了部分新增任务。\n已经结束的周年庆翻译了一部分物品。\n皇冠商城物品翻译了一部分。\n天际西部已有译者@luka开始翻译工作，目前翻译的文本量不是很多。\n技能与装备部分继续修正。\n插件本体更改了初始化方式，增加了若干个弹窗描述当前状态。\n插件本体增加了恰饭打赏二维码以支持汉化服务器费用。";
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
