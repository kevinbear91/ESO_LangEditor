using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.Command;
using ESO_LangEditor.GUI.EventAggres;
using ESO_LangEditor.GUI.Services;
using ESO_LangEditor.GUI.Views;
using MaterialDesignThemes.Wpf;
using NLog;
using Prism.Events;
using Prism.Mvvm;
using System;
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
        private bool _loginSuccess;
        private bool _isFirstime;

        private IUserAccess _userAccess;
        private ILogger _logger;

        public ICommand SumbitCommand => new ExcuteViewModelMethod(SubmitLoginInfo);
        public ICommand PasswordRecoveryCommand => new ExcuteViewModelMethod(OpenPasswordRecoveryWindow);
        public ICommand RegisterCommand => new ExcuteViewModelMethod(OpenRegisterWindow);
        public ICommand CloseDialogHostCommand { get; }


        public Guid UserGuid
        {
            get => _userGuid;
            set => SetProperty(ref _userGuid, value);
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public bool LoginSuccess
        {
            get => _loginSuccess;
            set => SetProperty(ref _loginSuccess, value);
        }

        public bool IsFirstime
        {
            get => _isFirstime;
            set => SetProperty(ref _isFirstime, value);
        }

        IEventAggregator _ea;

        public LoginViewModel(IEventAggregator ea, IUserAccess userAccess, ILogger logger)
        {
            _ea = ea;
            _userAccess = userAccess;
            _logger = logger;

            CloseDialogHostCommand = new SaveConfigCommand(CloseDialogHost);
        }

        public void Load(PasswordBox passwordBox)
        {
            _passwordBox = passwordBox;
        }

        private async void SubmitLoginInfo(object obj)
        {
            var Logintoken = await _userAccess.GetTokenByLogin(new LoginUserDto
            {
                UserName = UserName,
                Password = _passwordBox.Password,
            });

            if (Logintoken != null)
            {
                _logger.Debug("账号密码登录并获取Token成功");

                _userAccess.SaveToken(Logintoken);
                _ea.GetEvent<LoginFromUcEvent>().Publish();

                DialogHost.CloseDialogCommand.Execute(null, null);
            }
        }

        private void OpenPasswordRecoveryWindow(object obj)
        {
            var window = new PasswordRecoveryWindow
            {
                Owner = Application.Current.MainWindow
            };
            window.Show();
        }

        private void OpenRegisterWindow(object obj)
        {
            var window = new RegisterWindow
            {
                Owner = Application.Current.MainWindow
            };
            window.Show();
        }

        private void CloseDialogHost(object o)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private async Task LoginAsync()
        {





            //try
            //{
            //    if (IsFirstime)
            //    {
            //        var loginSuccess = await _accountService.LoginFirstTime(new LoginUserDto
            //        {
            //            UserID = App.LangConfig.UserGuid,
            //            RefreshToken = _passwordBox.Password,
            //        });

            //        if (loginSuccess)
            //        {
            //            //_ea.GetEvent<InitUserRequired>().Publish();
            //            DialogHost.CloseDialogCommand.Execute(null, null);
            //        }

            //    }
            //    else
            //    {
            //        var loginSuccess = await _accountService.Login(new LoginUserDto
            //        {
            //            UserID = App.LangConfig.UserGuid,
            //            Password = _passwordBox.Password,
            //        });

            //        if (loginSuccess)
            //        {
            //            DialogHost.CloseDialogCommand.Execute(null, null);
            //        }

            //    }

            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}

        }

    }
}
