using ESO_LangEditorGUI.Command;
using ESO_LangEditorModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private Guid _userGuid;
        private MainWindowViewModel _mainWindowViewModel;

        public ICommand SumbitCommand { get; }


        public Guid UserGuid
        {
            get { return _userGuid; }
            set { _userGuid = value; NotifyPropertyChanged(); }
        }

        public LoginViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            SumbitCommand = new SaveConfigCommand(SaveGuid);
        }


        private void SaveGuid(object o)
        {
            var config = App.LangConfig;
            config.UserGuid = UserGuid;
            ConfigJson.Save(config);
            DialogHost.CloseDialogCommand.Execute(null, null);
            _mainWindowViewModel.GuidVaildStartupCheck();
            
        }

    }
}
