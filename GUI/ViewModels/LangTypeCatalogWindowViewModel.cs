using Core.EnumTypes;
using Core.Models;
using GUI.Command;
using GUI.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private ObservableCollection<LangTypeCatalogDto> _langIdTypeDtos = new ObservableCollection<LangTypeCatalogDto>();
        public ObservableCollection<LangTypeCatalogDto> LangIdTypeDtos
        {
            get => _langIdTypeDtos;
            set => SetProperty(ref _langIdTypeDtos, value);
        }

        private IGeneralAccess _generalAccess;
        public ICommand SumbitCommand => new ExcuteViewModelMethod(SumbitIdTypeCatalog);
        public ICommand SearchIdTypeCommand => new ExcuteViewModelMethod(SearchIdTypeCatalog);
        private ICommand GetIdTypeDtosCommand => new ExcuteViewModelMethod(GetIdTypeCatalog);

        public LangTypeCatalogWindowViewModel(IGeneralAccess generalAccess)
        {
            _generalAccess = generalAccess;
            GetIdTypeDtosCommand.Execute(null);
        }

        private async void GetIdTypeCatalog(object o)
        {
            _langIdListFromServer = await _generalAccess.GetIdtypeDtos();

            if (LangIdTypeDtos.Count > 0)
            {
                LangIdTypeDtos.Clear();
            }

            LangIdTypeDtos.AddRange(_langIdListFromServer);
            Debug.WriteLine(LangIdTypeDtos.Count);
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
                var idType = Convert.ToInt32(obj);
                Debug.WriteLine(idType);

                var result = LangIdTypeDtos.First(id => id.IdType == idType);

                if (LangIdTypeDtos.Count > 0)
                {
                    LangIdTypeDtos.Clear();
                }

                LangIdTypeDtos.Add(result);
            }
            else
            {
                if (LangIdTypeDtos.Count > 0)
                {
                    LangIdTypeDtos.Clear();
                }

                if (_langIdListFromServer != null && _langIdListFromServer.Count > 0)
                {
                    LangIdTypeDtos.AddRange(_langIdListFromServer);
                }
            }
            
        }


    }
}
