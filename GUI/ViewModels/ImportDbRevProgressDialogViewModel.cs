using GUI.EventAggres;
using MaterialDesignThemes.Wpf;
using Prism.Events;
using Prism.Mvvm;
using System.Windows;

namespace GUI.ViewModels
{
    public class ImportDbRevProgressDialogViewModel : BindableBase
    {
        private string _currentExcuteText;
        private Visibility _closeButtonVisibility;
        private bool _closeButtonEnable;
        private bool _progressbarDisplay;
        private string _subExcuteText;

        public string CurrentExcuteText
        {
            get => _currentExcuteText;
            set => SetProperty(ref _currentExcuteText, value);
        }

        public Visibility CloseButtonVisibility
        {
            get => _closeButtonVisibility;
            set => SetProperty(ref _closeButtonVisibility, value);
        }

        public bool CloseButtonEnable
        {
            get => _closeButtonEnable;
            set => SetProperty(ref _closeButtonEnable, value);
        }

        public bool ProgressbarDisplay
        {
            get => _progressbarDisplay;
            set => SetProperty(ref _progressbarDisplay, value);
        }

        public string SubExcuteText
        {
            get => _subExcuteText;
            set => SetProperty(ref _subExcuteText, value);
        }

        private IEventAggregator _ea;

        public ImportDbRevProgressDialogViewModel(IEventAggregator ea)
        {
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
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void UpdateSubString(string obj)
        {
            SubExcuteText = obj;
        }

        private void UpdateMainString(string obj)
        {
            CurrentExcuteText = obj;
        }


    }
}
