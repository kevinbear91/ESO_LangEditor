using Core.Models;
using System.Collections.Generic;
using System.Windows;

namespace GUI.EventAggres
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
            _langTextDtos = langTextDtos;
        }
    }
}
