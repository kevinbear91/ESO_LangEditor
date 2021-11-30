using Core.Models;
using GUI.Command;
using GUI.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public class LangTypeCatalogReviewWindowViewModel : BindableBase
    {
        private ObservableCollection<LangTypeCatalogDto> _langTypeCatalogDtos = new ObservableCollection<LangTypeCatalogDto>();
        public ObservableCollection<LangTypeCatalogDto> LangTypeCatalogDtos
        {
            get => _langTypeCatalogDtos;
            set => SetProperty(ref _langTypeCatalogDtos, value);
        }

        private IGeneralAccess _generalAccess;
        public ICommand SubmitApproveItemsCommand => new ExcuteViewModelMethod(SumbitApproveItems);
        public ICommand SubmitDenyItemsCommand => new ExcuteViewModelMethod(SumbitDenyItems);


        public LangTypeCatalogReviewWindowViewModel(IGeneralAccess generalAccess)
        {
            _generalAccess = generalAccess;

        }

        private void SumbitApproveItems(object obj)
        {
            throw new NotImplementedException();
        }
        private void SumbitDenyItems(object obj)
        {
            throw new NotImplementedException();
        }

    }
}
