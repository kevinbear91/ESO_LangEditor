using Core.EnumTypes;
using Core.Models;
using GUI.Command;
using GUI.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public class LangTypeCatalogWindowViewModel : BindableBase
    {
        private List<LangTypeCatalogDto> _langIdListFromServer;
        private ObservableCollection<LangTypeCatalogDto> _langTypeCatalogDtos = new ObservableCollection<LangTypeCatalogDto>();
        public ObservableCollection<LangTypeCatalogDto> LangTypeCatalogDtos
        {
            get => _langTypeCatalogDtos;
            set => SetProperty(ref _langTypeCatalogDtos, value);
        }

        private IGeneralAccess _generalAccess;
        public ICommand SumbitCommand => new ExcuteViewModelMethod(SumbitIdTypeCatalog);
        public ICommand SearchIdTypeCommand => new ExcuteViewModelMethod(SearchIdTypeCatalog);

        public LangTypeCatalogWindowViewModel(IGeneralAccess generalAccess)
        {
            _generalAccess = generalAccess;

            Task.Run(() => GetIdTypeCatalog());
        }

        private async Task GetIdTypeCatalog()
        {
            _langIdListFromServer = await _generalAccess.GetIdtypeDtos();

            if (LangTypeCatalogDtos.Count > 0)
            {
                LangTypeCatalogDtos.Clear();
            }

            if (_langIdListFromServer != null && _langIdListFromServer.Count > 0)
            {
                LangTypeCatalogDtos.AddRange(_langIdListFromServer);
            }
        }

        private async void SumbitIdTypeCatalog(object obj)
        {
            if((LangTypeCatalogDto)obj != null)
            {
                try
                {
                    var respond = await _generalAccess.UploadIdTypeDto((LangTypeCatalogDto)obj);
                    MessageBox.Show(respond.Message);
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SearchIdTypeCatalog(object obj)
        {
            if((string)obj != "")
            {
                var idType = (int)obj;LangTypeCatalogDtos.Add(LangTypeCatalogDtos.FirstOrDefault(id => id.IdType == idType));
            }
            else
            {
                if (LangTypeCatalogDtos.Count > 0)
                {
                    LangTypeCatalogDtos.Clear();
                }

                if (_langIdListFromServer != null && _langIdListFromServer.Count > 0)
                {
                    LangTypeCatalogDtos.AddRange(_langIdListFromServer);
                }
            }
            
        }


    }
}
