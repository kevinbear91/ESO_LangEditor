using ESO_LangEditor.GUI.EventAggres;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Net.Http;
using System.Windows;

namespace ESO_LangEditor.GUI.ViewModels
{
    public class ImportDbRevProgressDialogViewModel : BindableBase
    {
        private string _currentExcuteText;
        private Visibility _closeButtonVisibility = Visibility.Collapsed;
        private bool _closeButtonEnable = false;
        private bool _progressbarDisplay;
        private string _downloadSpeed;

        public string CurrentExcuteText
        {
            get { return _currentExcuteText; }
            set { SetProperty(ref _currentExcuteText, value); }
        }

        public Visibility CloseButtonVisibility
        {
            get { return _closeButtonVisibility; }
            set { SetProperty(ref _closeButtonVisibility, value); }
        }

        public bool CloseButtonEnable
        {
            get { return _closeButtonEnable; }
            set { SetProperty(ref _closeButtonEnable, value); }
        }

        public bool ProgressbarDisplay
        {
            get { return _progressbarDisplay; }
            set { SetProperty(ref _progressbarDisplay, value); }
        }

        public string DownloadSpeed
        {
            get { return _downloadSpeed; }
            set { SetProperty(ref _downloadSpeed, value); }
        }

        IEventAggregator _ea;

        public ImportDbRevProgressDialogViewModel(IEventAggregator ea)
        {
            //CurrentExcuteText = "导出为.lang，等待点击开始按钮执行";
            ProgressbarDisplay = true;
            CloseButtonEnable = false;
            CloseButtonVisibility = Visibility.Collapsed;

            _ea = ea;

            _ea.GetEvent<ImportDbRevDialogStringMainEvent>().Subscribe(UpdateMainString);
            _ea.GetEvent<ImportDbRevDialogStringSubEvent>().Subscribe(UpdateSubString);
            _ea.GetEvent<CloseMainWindowDrawerHostEvent>().Subscribe(ShowCloseButton);

        }

        private void ShowCloseButton()
        {
            CloseButtonEnable = true;
            CloseButtonVisibility = Visibility.Visible;
        }

        private void UpdateSubString(string obj)
        {
            DownloadSpeed = obj;
        }

        private void UpdateMainString(string obj)
        {
            CurrentExcuteText = obj;
        }


    }
}
