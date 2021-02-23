using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorGUI.ViewModels
{
    public class UserProfileSettingViewModel : BindableBase
    {
        private string _userName = "用户名：" + App.LangConfig.UserName;
        private string _userGuid = "GUID：" + App.LangConfig.UserGuid.ToString();
        private string _nickName;
        private string _submitProgress;
        private bool _waitingResult = false;

        public string UserName 
        {
            get { return _userName; }
        }

        public string UserGuid
        {
            get { return _userGuid; }
        }

        public string NickName
        {
            get { return _nickName; }
            set { SetProperty(ref _nickName, value); }
        }

        public string SubmitProgress
        {
            get { return _submitProgress; }
            set { SetProperty(ref _submitProgress, value); }
        }

        public bool WaitingResult
        {
            get { return _waitingResult; }
            set { SetProperty(ref _waitingResult, value); }
        }

    }
}
