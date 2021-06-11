using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ESO_LangEditor.GUI.EventAggres
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
            this._langTextDtos = langTextDtos;
        }
    }
}
