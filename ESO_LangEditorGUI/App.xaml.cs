using ESO_LangEditorGUI.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditorGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //public static HttpClient ApiClient { get; set; }
        public static bool OnlineMode { get; set; }

        public void App_Startup(object sender, StartupEventArgs e)
        {
            //string filePath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;


            //InitializeClient();
            //RegisterDependencies();

            var dbCheck = new StartupDBCheck(@"Data\LangData.db", @"Data\LangData.update");

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
