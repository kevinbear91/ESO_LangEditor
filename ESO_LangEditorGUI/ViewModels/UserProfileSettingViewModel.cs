using ESO_LangEditor.Core.Models;
using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.EventAggres;
using ESO_LangEditorGUI.Services.AccessServer;
using ESO_LangEditorGUI.Views;
using Microsoft.Win32;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ESO_LangEditorGUI.ViewModels
{
    public class UserProfileSettingViewModel : BindableBase
    {
        private string _userName = App.LangConfig.UserName;
        private string _userGuid = "GUID：" + App.LangConfig.UserGuid.ToString();
        private string _nickName;
        private string _esoId;
        private string _submitProgress;
        private bool _waitingResult = false;
        private bool _FirstTimeInit = false;
        private string _userAvatarPath = App.UserAvatarPath;

        private PasswordBox _passwordBox;
        private PasswordBox _passwordBoxConfirm;
        private AccountService _accountService;
        

        public ExcuteViewModelMethod UploadInfoCommand => new ExcuteViewModelMethod(UploadUserInfo);
        public ExcuteViewModelMethod UploadAvatarCommand => new ExcuteViewModelMethod(UploadUserAvatar);
        public UserProfileSetting UserProfileSettingWindow;


        public string UserName 
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
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

        public string EsoId
        {
            get { return _esoId; }
            set { SetProperty(ref _esoId, value); }
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

        public string UserAvatarPath
        {
            get { return _userAvatarPath; }
            set { SetProperty(ref _userAvatarPath, value); }
        }

        IEventAggregator _ea;


        public UserProfileSettingViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _ea.GetEvent<ConnectProgressString>().Subscribe(ConnectStatus);
        }

        private void ConnectStatus(string str)
        {
            SubmitProgress = str;
        }

        public void Load(PasswordBox passwordBox, PasswordBox passwordBoxConfrim)
        {
            _passwordBox = passwordBox;
            _passwordBoxConfirm = passwordBoxConfrim;
            //_accountService = accountService;
            _accountService = new AccountService(_ea);
        }

        public async void UploadUserInfo(object o)
        {
            var regex = new Regex(@"(?=.*[0-9])(?=.*[a-zA-Z])(?=([\x21-\x7e]+)[^a-zA-Z0-9]).{8,30}", RegexOptions.IgnorePatternWhitespace);
            List<string> roleList = _accountService.GetUserRoleFromToken(App.LangConfig.UserAuthToken);

            if (!string.IsNullOrEmpty(_passwordBox.Password) || roleList.Contains("InitUser"))
            {
                if (_passwordBox.Password != _passwordBoxConfirm.Password)
                {
                    SubmitProgress = "两次输入的密码不一致！";
                }
                else
                {
                    if (regex.IsMatch(_passwordBox.Password))
                    {
                        bool isSuccessed;
                        if (roleList.Contains("InitUser"))
                        {
                            WaitingResult = true;
                            isSuccessed = await _accountService.UserInfoInit(new UserInfoChangeDto
                            {
                                NewPassword = _passwordBox.Password,
                                UserID = App.LangConfig.UserGuid,
                                UserName = UserName,
                                UserNickName = NickName,
                                UserEsoId = EsoId,
                            });
                        }
                        else
                        {
                            WaitingResult = true;
                            isSuccessed = await _accountService.UserInfoChange(new UserInfoChangeDto
                            {
                                NewPassword = _passwordBox.Password,
                                UserID = App.LangConfig.UserGuid,
                                UserName = UserName,
                                UserNickName = NickName,
                                UserEsoId = EsoId,
                            });
                        }

                        _passwordBox.Clear();
                        _passwordBoxConfirm.Clear();

                        if (isSuccessed)
                        {
                            WaitingResult = false;
                            SubmitProgress = "修改成功，请退出工具用密码重新登录！";
                        }
                    }
                    else
                    {
                        _passwordBox.Clear();
                        _passwordBoxConfirm.Clear();
                        SubmitProgress = "密码必须大于8位小于30位，包含数字、英文字母与特殊符号！";
                    }
                }
            }
            else
            {
                WaitingResult = true;
                var isSuccessed = await _accountService.UserInfoChange(new UserInfoChangeDto
                {
                    UserID = App.LangConfig.UserGuid,
                    UserName = UserName,
                    UserNickName = NickName
                });

                if (isSuccessed)
                {
                    WaitingResult = false;
                    SubmitProgress = "修改成功，请重新启动工具来更新缓存。";
                }
            }
        }

        private async void UploadUserAvatar(object obj)
        {
            OpenFileDialog dialog = new OpenFileDialog { Multiselect = false };
            

            if (dialog.ShowDialog(UserProfileSettingWindow) == true)
            {
                if (dialog.FileName.EndsWith(".jpg") && new FileInfo(dialog.FileName).Length < 512 * 1024)  // 512 * 1024 = 512 KB
                {
                    WaitingResult = true;
                    if (await _accountService.UserAvatarUpload(dialog.FileName))
                    {
                        WaitingResult = false;
                    }
                    
                }
                else
                {
                    MessageBox.Show("仅支持上传 .jpg 文件并且体积小于 512 KB！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
        }



    }
}
