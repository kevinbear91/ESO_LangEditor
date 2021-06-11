using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ESO_LangEditor.GUI.Command
{
    public abstract class CommandBaseAsync : ICommand
    {
        private bool _isExecuting;
        public event EventHandler CanExecuteChanged;

        public bool IsExecuting
        {
            get { return _isExecuting; }
            set { _isExecuting = value; CanExecuteChanged?.Invoke(this, new EventArgs()); }
        }


        public bool CanExecute(object parameter)
        {
            return !IsExecuting;
        }

        public async void Execute(object parameter)
        {
            IsExecuting = true;

            await ExecuteAsync(parameter);

            IsExecuting = false;
        }

        public abstract Task ExecuteAsync(object parameter);
    }
}
