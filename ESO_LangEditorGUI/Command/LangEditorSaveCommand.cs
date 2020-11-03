using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorLib.Services.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ESO_LangEditorGUI.Command
{
    class LangEditorSaveCommand : CommandBase
    {
        //private LangTextRepository _langTextRepository = new LangTextRepository();
        //private TextEditorViewModel _textEditorViewModel;

        private readonly Action<object> _executeMethod;

        public LangEditorSaveCommand(Action<object> execute)
        {
            //_textEditorViewModel = textEditorViewModel;
            _executeMethod = execute;
        }

        public override void ExecuteCommand(object parameter)
        {
            //LangTextDto langtext = parameter as LangTextDto;

            _executeMethod(parameter);

            //TextBox langtextZh = parameter as TextBox;
            //LangTextDto lang = _textEditorViewModel.CurrentLangText;

            //if (langtextZh.Text != lang.TextZh)
            //{
            //    lang.TextZh = langtextZh.Text;
            //    lang.IsTranslated = 1;
            //    lang.ZhLastModifyTimestamp = DateTime.Now;

            //    Debug.WriteLine("{0},{1}", lang.TextZh, lang.ZhLastModifyTimestamp);

            //    int update = await _langTextRepository.UpdateLangZh(lang);


            //    _textEditorViewModel.MdNotifyContent = update.ToString();
            //}
            //else
            //{

            //}    

            


            //throw new NotImplementedException();
        }
    }
}
