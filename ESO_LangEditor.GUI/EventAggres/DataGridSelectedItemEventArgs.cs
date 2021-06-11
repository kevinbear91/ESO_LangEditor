using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ESO_LangEditor.GUI.EventAggres
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
            this._langTextDto = langTextDto;
        }
    }
}
