using System;
using System.Diagnostics;
using System.Net;

namespace ESO_LangEditorUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 4)
            {
                new LangDownloader(args).StartDownload();
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("哦豁，不要随便打开这个程序。");
            }
        }
    }
}
