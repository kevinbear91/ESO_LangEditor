using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.NetClient;
using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.Services.AccessServer;
using MaterialDesignThemes.Wpf;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private Guid _userGuid;
        private string _userName;
        private PasswordBox _passwordBox;
        private AccountService _accountService;

        public ICommand SumbitCommand { get; }


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

        public LoginViewModel()
        {
            UserGuid = GetGuid();
            SumbitCommand = new SaveConfigCommand(SaveGuid);
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

        private async Task LoginAsync()
        {
            try
            {
                var loginSuccess = await _accountService.Login(new LoginUserDto
                {
                    UserID = App.LangConfig.UserGuid,
                    Password = _passwordBox.Password,
                });

                if (loginSuccess)
                    DialogHost.CloseDialogCommand.Execute(null, null);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

    }
}
