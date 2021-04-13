using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class ProgressDialogViewModel : BaseViewModel
    {
        private string _currentExcuteText;
        private Visibility _beginButtonVisibility;
        private bool _closeButtonEnable;
        private bool _progressbarDisplay;

        public ICommand RunDialogCommand => new ExcuteViewModelMethod(ExportLangAsync);

        public string CurrentExcuteText
        {
            get { return _currentExcuteText; }
            set { _currentExcuteText = value; NotifyPropertyChanged(); }
        }

        public Visibility BeginButtonVisibility
        {
            get { return _beginButtonVisibility; }
            set { _beginButtonVisibility = value; NotifyPropertyChanged(); }
        }

        public bool CloseButtonEnable
        {
            get { return _closeButtonEnable; }
            set { _closeButtonEnable = value; NotifyPropertyChanged(); }
        }

        public bool ProgressbarDisplay
        {
            get { return _progressbarDisplay; }
            set { _progressbarDisplay = value; NotifyPropertyChanged(); }
        }

        public ProgressDialogViewModel()
        {
            CurrentExcuteText = "导出为.lang，等待点击开始按钮执行";
            ProgressbarDisplay = false;
            //BeginButtonEnable = true;
            CloseButtonEnable = true;
        }

        private async void ExportLangAsync(object o)
        {
            var _langTextRepository = new LangTextRepoClientService();
            var _thirdPartSerices = new ThirdPartSerices();
            var exportDbToFile = new ExportDbToFile();

            ProgressbarDisplay = true;
            BeginButtonVisibility = Visibility.Collapsed;
            CloseButtonEnable = false;
            CurrentExcuteText = "正在读取数据库……";
            //ProgressInfo = "正在读取数据库……";
            //ProgressbarVisibility = Visibility.Visible;
            var langtexts = await Task.Run(() => _langTextRepository.GetAlltLangTexts(0));

            CurrentExcuteText = "正在写入文件……";
            await Task.Run(() => exportDbToFile.ExportText(langtexts));

            CurrentExcuteText = "正在转换格式……";
            await Task.Run(() => _thirdPartSerices.ConvertTxTtoLang(false));

            CurrentExcuteText = "完成！";
            CloseButtonEnable = true;
        }

    }
}
