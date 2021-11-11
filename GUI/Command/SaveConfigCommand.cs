using System;

namespace GUI.Command
{
    public class SaveConfigCommand : CommandBase
    {
        private readonly Action<object> _executeMethod;

        public SaveConfigCommand(Action<object> execute)
        {
            _executeMethod = execute;
        }


        public override void ExecuteCommand(object parameter)
        {
            _executeMethod(parameter);
        }
    }
}
