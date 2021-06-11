using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ESO_LangEditor.GUI.EventAggres
{
    public class DataGridReviewSelectedChangedEventArgs : RoutedEventArgs
    {
        private readonly List<LangTextForReviewDto> _langTextDtos;

        public List<LangTextForReviewDto> LangTextListDto
        {
            get { return _langTextDtos; }
        }

        public DataGridReviewSelectedChangedEventArgs(RoutedEvent routedEvent, List<LangTextForReviewDto> langTextDtos) : base(routedEvent)
        {
            this._langTextDtos = langTextDtos;
        }
    }
}
