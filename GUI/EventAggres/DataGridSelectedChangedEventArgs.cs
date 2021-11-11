using Core.Models;
using System.Collections.Generic;
using System.Windows;

namespace GUI.EventAggres
{
    public class DataGridSelectedChangedEventArgs : RoutedEventArgs
    {
        private readonly List<LangTextDto> _langTextDtos;

        public List<LangTextDto> LangTextListDto
        {
            get { return _langTextDtos; }
        }

        public DataGridSelectedChangedEventArgs(RoutedEvent routedEvent, List<LangTextDto> langTextDtos) : base(routedEvent)
        {
            _langTextDtos = langTextDtos;
        }
    }
}
