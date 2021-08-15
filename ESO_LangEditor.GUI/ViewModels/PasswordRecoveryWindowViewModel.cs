using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.Command;
using ESO_LangEditor.GUI.Services;
using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;

namespace ESO_LangEditor.GUI.ViewModels
{
    public class PasswordRecoveryWindowViewModel : BindableBase
    {
        private string _userName;
        private PasswordBox _passwordBox;
        private PasswordBox _passwordBoxConfirm;
        private string _recoveryCode;

        private IUserAccess _userAccess;
        private ILogger _logger;


        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string RecoveryCode
        {
            get => _recoveryCode;
            set => SetProperty(ref _recoveryCode, value);
        }

        public ExcuteViewModelMethod UserPasswordResetCommand => new ExcuteViewModelMethod(UserPasswordReset);

        public PasswordRecoveryWindowViewModel(IUserAccess userAccess, ILogger logger)
        {
            _userAccess = userAccess;
            _logger = logger;
        }

        public void Load(PasswordBox passwordBox, PasswordBox passwordBoxConfrim)
        {
            _passwordBox = passwordBox;
            _passwordBoxConfirm = passwordBoxConfrim;
        }

        private async void UserPasswordReset(object obj)
        {
            var respond = await _userAccess.SetUserPasswordByRecoveryCode(new UserPasswordRecoveryDto
            {
                UserName = UserName,
                NewPassword = _passwordBox.Password,
                NewPasswordConfirm = _passwordBoxConfirm.Password,
                RecoveryCode = RecoveryCode,
            });

            if (respond.Code == (int)RespondCode.Success)
            {
                MessageBox.Show(respond.Message, respond.Code.ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(respond.Message, respond.Code.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


    }
}
