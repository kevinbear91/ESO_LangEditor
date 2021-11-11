using Core.Models;
using System.Windows;

namespace GUI.EventAggres
{
    public class DataGridSelectedItemEventArgs : RoutedEventArgs
    {
        private readonly LangTextDto _langTextDto;

        public LangTextDto LangTextDto
        {
            get { return _langTextDto; }
        }

        public DataGridSelectedItemEventArgs(RoutedEvent routedEvent, LangTextDto langTextDto) : base(routedEvent)
        {
            _langTextDto = langTextDto;
        }
    }
}
