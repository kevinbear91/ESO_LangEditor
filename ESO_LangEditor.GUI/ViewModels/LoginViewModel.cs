using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.NetClient;
using ESO_LangEditor.GUI.Command;
using ESO_LangEditor.GUI.EventAggres;
using ESO_LangEditor.GUI.Services.AccessServer;
using MaterialDesignThemes.Wpf;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ESO_LangEditor.GUI.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private Guid _userGuid;
        private string _userName;
        private PasswordBox _passwordBox;
        private AccountService _accountService;
        private bool _loginSuccess;
        private bool _isFirstime;

        public ICommand SumbitCommand { get; }
        public ICommand CloseDialogHostCommand { get; }


        public Guid UserGuid
        {
            get { return _userGuid; }
            set { SetProperty(ref _userGuid, value); }
        }

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        public bool LoginSuccess
        {
            get { return _loginSuccess; }
            set { SetProperty(ref _loginSuccess, value); }
        }

        public bool IsFirstime
        {
            get { return _isFirstime; }
            set { SetProperty(ref _isFirstime, value); }
        }

        IEventAggregator _ea;

        public LoginViewModel(IEventAggregator ea)
        {
            _ea = ea;
            UserGuid = GetGuid();
            SumbitCommand = new SaveConfigCommand(SaveGuid);
            CloseDialogHostCommand = new SaveConfigCommand(CloseDialogHost);
        }

        public void Load(PasswordBox passwordBox, AccountService accountService)
        {
            _passwordBox = passwordBox;
            _accountService = accountService;
        }


        private Guid GetGuid()
        {
            return App.LangConfig.UserGuid;
        }

        private void SaveGuid(object o)
        {
            var config = App.LangConfig;
            config.UserGuid = UserGuid;
            AppConfigClient.Save(config);

            LoginAsync();

            //DialogHost.CloseDialogCommand.Execute(null, null);
            //_mainWindowViewModel.GuidVaildStartupCheck();
            
        }

        private void CloseDialogHost(object o)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private async Task LoginAsync()
        {
            try
            {
                if (IsFirstime)
                {
                    var loginSuccess = await _accountService.LoginFirstTime(new LoginUserDto
                    {
                        UserID = App.LangConfig.UserGuid,
                        RefreshToken = _passwordBox.Password,
                    });

                    if (loginSuccess)
                    {
                        //_ea.GetEvent<InitUserRequired>().Publish();
                        DialogHost.CloseDialogCommand.Execute(null, null);
                    }
                        
                }
                else
                {
                    var loginSuccess = await _accountService.Login(new LoginUserDto
                    {
                        UserID = App.LangConfig.UserGuid,
                        Password = _passwordBox.Password,
                    });

                    if (loginSuccess)
                    {
                        DialogHost.CloseDialogCommand.Execute(null, null);
                    }
                        
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

    }
}
