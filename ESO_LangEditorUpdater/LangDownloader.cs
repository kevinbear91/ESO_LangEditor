using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;

namespace ESO_LangEditorUpdater
{
    public class LangDownloader
    {
        private string _downloadPath;
        private string _fileName;
        private string _fileSHA256;
        private string _langEditorServerVersion;
        public static readonly string WorkingName = Process.GetCurrentProcess().MainModule?.FileName;
        public static readonly string WorkingDirectory = Path.GetDirectoryName(WorkingName);

        public LangDownloader(string[] args)
        {
            _downloadPath = args[0];
            _fileSHA256 = args[1];
            _langEditorServerVersion = args[2];

            _fileName = "ESO_LangEditor_v" + _langEditorServerVersion + ".zip";
        }
        public void StartDownload()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 5;

            if (File.Exists(_fileName))
            {
                HashAndUnzip();
            }
            else
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += Editor_DownloadProgressChanged;
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DelegateHashAndUnzip);
                    client.DownloadFileAsync(new Uri(_downloadPath), _fileName);
                }
            }
        }

        private void DelegateHashAndUnzip(object s, AsyncCompletedEventArgs e)
        {
            HashAndUnzip();
        }

        private void HashAndUnzip()
        {
            Console.WriteLine("下载完成！");
            if (HashDownloadFile())
            {
                Console.WriteLine("SHA256校验通过，准备解压文件。");
                LangUnzip();
            }
            else
            {
                Console.WriteLine("SHA256校验失败，请重新下载！");

                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += Editor_DownloadProgressChanged;
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DelegateHashAndUnzip);
                    client.DownloadFileAsync(new Uri(_downloadPath), _fileName);
                }
            }
                
        }



        private bool HashDownloadFile()
        {
            string hashReslut;

            Console.WriteLine("正在校验压缩包的SHA256值。");
            Console.WriteLine("服务器端文件SHA256：{0}", _fileSHA256);

            using (FileStream stream = File.OpenRead(_fileName))
            {
                SHA256Managed sha = new SHA256Managed();
                byte[] hash = sha.ComputeHash(stream);
                hashReslut = BitConverter.ToString(hash).Replace("-", String.Empty);
                Console.WriteLine("下载文件SHA256：{0}", hashReslut);
            }
            return _fileSHA256 == hashReslut;
        }
        private void LangUnzip()
        {
            Process[] EditorGUI = Process.GetProcessesByName("ESO_LangEditorGUI");

            foreach(var editor in EditorGUI)
            {
                editor.Kill();
                editor.WaitForExit();
                editor.Dispose();
            }
            Console.WriteLine("正在解压已下载的压缩包。");
            
            try
            {
                ZipFile.ExtractToDirectory(_fileName, WorkingDirectory, true);
                //UpdateJsonVersion();

                File.Delete(_fileName);

                //string args = _langEditorServerVersion, //服务器端版本号
                //Debug.WriteLine(args);


                ProcessStartInfo startUpdaterInfo = new ProcessStartInfo
                {
                    FileName = "ESO_LangEditorGUI.exe",
                    Arguments = _langEditorServerVersion, //服务器端版本号
                };

                Process updater = new Process
                {
                    StartInfo = startUpdaterInfo,
                };
                updater.Start();
                Environment.Exit(0);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        //private void UpdateJsonVersion()
        //{
        //    AppConfigClient LangConfig = AppConfigClient.Load();
        //    LangConfig.LangEditorVersion = _langEditorServerVersion;
        //    AppConfigClient.Save(LangConfig);
        //}

        


        void Editor_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.Write("\r正在下载压缩包 {0}，进度：{1}%，已下载大小：{2}, 总大小：{3}", _fileName, e.ProgressPercentage, SizeSuffix(e.BytesReceived), SizeSuffix(e.TotalBytesToReceive));
        }

        static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }

    }
}
