using AutoMapper;
using Core.EnumTypes;
using Core.Models;
using GUI.Views;
using GUI.Views.UserControls;
using GUI.Services;
using GUI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public static IDCatalog iDCatalog = new IDCatalog();
        public static GameVersion gameUpdateVersionName = new GameVersion();
        public static AppConfigClient LangConfig;
        public static UserInClientDto User;
        public static string ServerPath;
        public static readonly string WorkingName = Process.GetCurrentProcess().MainModule?.FileName;
        public static readonly string WorkingDirectory = Path.GetDirectoryName(WorkingName);
        public static HttpClient HttpClient;
        public static ClientConnectStatus ConnectStatus;

        public static readonly DbContextOptions<LangtextClientDbContext> DbOptionsBuilder;

        static App()
        {
            DbOptionsBuilder = new DbContextOptionsBuilder<LangtextClientDbContext>()
                .UseSqlite(@"Data Source=Data/LangData_v5.db")
                //.UseLoggerFactory(MyLoggerFactory)
                .Options;
        }

        public void App_Startup(object sender, StartupEventArgs e)
        {
            Dictionary<string, string> argsDict = new Dictionary<string, string>();

            LangConfig = AppConfigClient.Load();
            

            string[] args = Environment.GetCommandLineArgs();

            for (int index = 1; index < args.Length; index += 2)
            {
                argsDict.Add(args[index], args[index + 1]);
            }

            foreach(var arg in argsDict)
            {
                //MessageBox.Show($"命令: {arg.Key}, 参数: {arg.Value}");

                if (arg.Key == "/NewVersion")
                {
                    LangConfig.LangEditorVersion = arg.Value;
                }

                //Debug.WriteLine($"arg: {arg.Key}, value: {arg.Value}");
            }

            foreach (var server in LangConfig.LangServerList)
            {
                if (server.ServerName == LangConfig.DefaultServerName)
                    ServerPath = server.ServerURL;
            }

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(ServerPath),
            };

            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("LangEditorWPF/" + LangConfig.LangEditorVersion);
            HttpClient.DefaultRequestHeaders.Connection.Add("keep-alive");

            AppConfigClient.Save(LangConfig);
            
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<LangTextMappingProfile>();
            });

            var factory = new NLogLoggerFactory();
            ILogger logger = factory.CreateLogger("LangEditor");

            //Services Register

            containerRegistry.Register<IStartupCheck, StartupCheck>();
            containerRegistry.RegisterInstance<IMapper>(new Mapper(config));
            containerRegistry.RegisterInstance(logger);
            containerRegistry.RegisterSingleton<ILangTextRepoClient, LangTextRepoClient>();
            containerRegistry.RegisterSingleton<ILangTextAccess, LangTextAccess>();
            containerRegistry.RegisterSingleton<IUserAccess, UserAccess>();
            containerRegistry.RegisterSingleton<ILangFile, LangFile>();
            containerRegistry.RegisterSingleton<IBackendService, BackendService>();
            containerRegistry.RegisterSingleton<IGeneralAccess, GenreralAccess>();

            //ViewModel Register;
            ViewModelLocationProvider.Register<MainWindowSearchbar, MainWindowSearchbarViewModel>();
            ViewModelLocationProvider.Register<MainMenu, MainMenuListViewModel>();
            ViewModelLocationProvider.Register<UC_Login, LoginViewModel>();
            ViewModelLocationProvider.Register<ImportDbRevProgressDialog, ImportDbRevProgressDialogViewModel>();
        }

        protected override Window CreateShell()
        {
            var w = Container.Resolve<MainWindow>();
            return w;
        }

    }

}
