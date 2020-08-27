using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ESO_LangEditorGUI.Controller
{
    public class ThirdPartController
    {
        public void OpenCCtoCHT()
        {
            ProcessStartInfo startOpenCCInfo = new ProcessStartInfo
            {
                FileName = @"opencc\opencc.exe",
                Arguments = @" -i _tmp\Text.txt -o _tmp\Text_cht.txt -c opencc\s2twp.json"
            };

            Process opencc = new Process();
            opencc.StartInfo = startOpenCCInfo;
            opencc.Start();
            opencc.WaitForExit();
        }
        public void LuaStrToCHT()
        {
            ProcessStartInfo startOpenCCInfo = new ProcessStartInfo
            {
                FileName = @"opencc\opencc.exe",
                Arguments = @" -i Export\zh_client.str -o Export\zht_client.str -c opencc\s2twp.json"
            };

            ProcessStartInfo startOpenCCInfo2 = new ProcessStartInfo
            {
                FileName = @"opencc\opencc.exe",
                Arguments = @" -i Export\zh_pregame.str -o Export\zht_pregame.str -c opencc\s2twp.json"
            };

            Process opencc = new Process
            {
                StartInfo = startOpenCCInfo
            };
            opencc.Start();
            opencc.WaitForExit();

            Process opencc2 = new Process
            {
                StartInfo = startOpenCCInfo2
            };
            opencc2.Start();
            opencc2.WaitForExit();

        }

        public void ConvertTxTtoLang(bool isCHT)
        {
            string textFileName;
            string langName;

            if(isCHT)
            {
                textFileName = "Text_cht.txt";
                langName = @"Export\zht.lang";
            }
            else
            {
                textFileName = "Text.txt";
                langName = @"Export\zh.lang";
            }

            ProcessStartInfo startEEDInfo = new ProcessStartInfo
            {
                FileName = @"EsoExtractData\EsoExtractData.exe",
                Arguments = @" -x _tmp\" + textFileName + @" -i _tmp\ID.txt -t -o " + langName,
            };

            Process eed = new Process
            {
                StartInfo = startEEDInfo
            };
            eed.Start();
            eed.WaitForExit();
        }

    }
}
