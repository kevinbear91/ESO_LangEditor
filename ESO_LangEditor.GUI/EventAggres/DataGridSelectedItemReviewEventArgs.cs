using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ESO_LangEditor.GUI.EventAggres
{
    public class DataGridSelectedItemReviewEventArgs : RoutedEventArgs
    {
        private readonly LangTextForReviewDto _langTextDto;

        public LangTextForReviewDto LangTextDto
        {
            get { return _langTextDto; }
        }

        public DataGridSelectedItemReviewEventArgs(RoutedEvent routedEvent, LangTextForReviewDto langTextDto) : base(routedEvent)
        {
            this._langTextDto = langTextDto;
        }
    }
}
