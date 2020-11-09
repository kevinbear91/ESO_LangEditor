using ESO_LangEditorGUI.Services;
using ESO_LangEditorModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditorGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static HttpClient ApiClient { get; set; }
        public static bool OnlineMode { get; set; }
        public static IDCatalog iDCatalog = new IDCatalog();
        public static GameVersion gameUpdateVersionName = new GameVersion();
        public static ConfigJson LangConfig;
        public static HandshakeJson LangConfigServer;
        public static NetworkService LangNetworkService;
        public static string ServerPath;
        public static readonly string WorkingName = Process.GetCurrentProcess().MainModule?.FileName;
        public static readonly string WorkingDirectory = Path.GetDirectoryName(WorkingName);

        public void App_Startup(object sender, StartupEventArgs e)
        {
            //string filePath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;


            //InitializeClient();
            //RegisterDependencies();

            LangConfig = ConfigJson.Load();
            ConfigJson.Save(LangConfig);

            //HandshakeJson handshake = new HandshakeJson();
            //HandshakeJson.Save(handshake);

            
            //LangNetworkService.CompareServerConfig();

            //var startupNetCheck = new StartupNetHandShake(LangConfig);



            var dbCheck = new StartupDBCheck(@"Data\LangData_v3.db", @"Data\LangData_v3.update");

            if (dbCheck.IsDBExist & dbCheck.CheckDbUpdateExist)
            {
                dbCheck.ProcessUpdateMerge();
                new MainWindow().Show();
            }
            else if (dbCheck.IsDBExist)
            {
                new MainWindow().Show();
            }
            else if (dbCheck.CheckDbUpdateExist)
            {
                dbCheck.RenameUpdateDB();
                new MainWindow().Show();
            }
            else
            {
                MessageBox.Show("无法找到数据库文件！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            OnlineMode = false;
        }

    }
    
}
