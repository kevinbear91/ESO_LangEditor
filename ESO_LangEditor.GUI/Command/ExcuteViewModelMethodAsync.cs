using System;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.Command
{
    public class ExcuteViewModelMethodAsync : CommandBaseAsync
    {
        private readonly Action<object> _executeMethod;

        public ExcuteViewModelMethodAsync(Action<object> execute)
        {
            _executeMethod = execute;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            //await _executeMethod(parameter);
        }
    }
}
