using System;

namespace ESO_LangEditor.GUI.Command
{
    public class ExcuteViewModelMethod : CommandBase
    {
        private readonly Action<object> _executeMethod;

        public ExcuteViewModelMethod(Action<object> execute)
        {
            _executeMethod = execute;
        }

        public override void ExecuteCommand(object parameter)
        {
            //ExportToLang();
            _executeMethod(parameter);
        }
    }
}
