using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ESO_LangEditorUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 3)
            {
                var downloader = new LangDownloader(args);
                //await downloader.UpdateSequence();
                Task.Run(() => downloader.UpdateSequence());
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("哦豁，不要随便打开这个程序。");
            }
        }
    }
}
