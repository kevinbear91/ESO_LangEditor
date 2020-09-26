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
        public static HttpClient ApiClient { get; set; }

        public void App_Startup(object sender, StartupEventArgs e)
        {
            //string filePath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;


            //InitializeClient();
            //RegisterDependencies();

            
            new MainWindow().Show();
        }

        
    }
    
}
