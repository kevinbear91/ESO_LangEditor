using AutoMapper;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore;
using ESO_LangEditor.GUI.Services;
using ESO_LangEditor.GUI.ViewModels;
using ESO_LangEditor.GUI.Views;
using ESO_LangEditor.GUI.Views.UserControls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;

namespace ESO_LangEditor.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public static bool OnlineMode { get; set; }
        public static IDCatalog iDCatalog = new IDCatalog();
        public static GameVersion gameUpdateVersionName = new GameVersion();
        public static AppConfigClient LangConfig;
        public static UserInClientDto User;
        public static string ServerPath;
        public static readonly string WorkingName = Process.GetCurrentProcess().MainModule?.FileName;
        public static readonly string WorkingDirectory = Path.GetDirectoryName(WorkingName);
        public static HttpClient HttpClient;

        public static readonly DbContextOptions<LangtextClientDbContext> DbOptionsBuilder;
        //public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddDebug(); });

        static App()
        {
            DbOptionsBuilder = new DbContextOptionsBuilder<LangtextClientDbContext>()
                .UseSqlite(@"Data Source=Data/LangData_v4.db")
                //.UseLoggerFactory(MyLoggerFactory)
                .Options;
        }

        public void App_Startup(object sender, StartupEventArgs e)
        {
            LangConfig = AppConfigClient.Load();
            AppConfigClient.Save(LangConfig);

#if DEBUG
            Console.WriteLine("Debug version");
#endif

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
            HttpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
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
