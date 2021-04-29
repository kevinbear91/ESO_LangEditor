using AutoMapper;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore;
using ESO_LangEditor.EFCore.RepositoryWrapper;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorGUI.Views;
using ESO_LangEditorGUI.Views.UserControls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
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
    public partial class App : PrismApplication
    {
        public static bool OnlineMode { get; set; }
        public static IDCatalog iDCatalog = new IDCatalog();
        public static GameVersion gameUpdateVersionName = new GameVersion();
        public static AppConfigClient LangConfig;
        public static AppConfigServer LangConfigServer;
        public static StartupConfigCheck LangNetworkService;
        public static UserInClientDto User;
        public static string ServerPath;
        public static readonly string WorkingName = Process.GetCurrentProcess().MainModule?.FileName;
        public static readonly string WorkingDirectory = Path.GetDirectoryName(WorkingName);
        public static string UserAvatarPath { get; set; }
        public static readonly IMapper Mapper;
        public static readonly DbContextOptions<LangtextClientDbContext> DbOptionsBuilder;
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddDebug(); });

        static App()
        {
            DbOptionsBuilder = new DbContextOptionsBuilder<LangtextClientDbContext>()
                .UseSqlite(@"Data Source=Data/LangData_v4.db")
                .UseLoggerFactory(MyLoggerFactory)
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<LangTextMappingProfile>();
            });

            Mapper = new Mapper(config);

        }

        public void App_Startup(object sender, StartupEventArgs e)
        {
            //string filePath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

            LangConfig = AppConfigClient.Load();
            AppConfigClient.Save(LangConfig);
            
            #if DEBUG
            Console.WriteLine("Debug version");
            //MessageBox.Show("Debug version");
            #endif

            foreach (var server in LangConfig.LangServerList)
            {
                if (server.ServerName == LangConfig.DefaultServerName)
                    ServerPath = server.ServerURL;
            }

            var avatarPath = WorkingDirectory + "/_tmp/" + LangConfig.UserAvatarPath;
            if (File.Exists(avatarPath))
            {
                UserAvatarPath = avatarPath;
            }
            else
            {
                UserAvatarPath = WorkingDirectory + "/Data/TempAvatar.png";
            }

            //App.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;


            //var dbCheck = new StartupDBCheck(@"Data\LangData_v3.db", @"Data\LangData_v3.update");

            //if (dbCheck.IsDBExist & dbCheck.CheckDbUpdateExist)
            //{
            //    dbCheck.ProcessUpdateMerge();
            //    new MainWindow().Show();
            //}
            //else if (dbCheck.IsDBExist)
            //{
            //    new MainWindow().Show();
            //}
            //else if (dbCheck.CheckDbUpdateExist)
            //{
            //    dbCheck.RenameUpdateDB();
            //    new MainWindow().Show();
            //}
            //else
            //{
            //    MessageBox.Show("无法找到数据库文件！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //new MainWindow().Show();
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //throw new NotImplementedException();
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
