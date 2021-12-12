using Core.Models;
using GUI.Command;
using GUI.Services;
using Prism.Mvvm;
using System;
using System.Collections;
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
    public class LangTypeCatalogReviewWindowViewModel : BindableBase
    {
        private List<LangTypeCatalogDto> _langIdListFromServer;
        private ObservableCollection<LangTypeCatalogDto> _langTypeCatalogDtos = new ObservableCollection<LangTypeCatalogDto>();
        public ObservableCollection<LangTypeCatalogDto> LangTypeCatalogDtos
        {
            get => _langTypeCatalogDtos;
            set => SetProperty(ref _langTypeCatalogDtos, value);
        }

        private ICommand GetIdTypeDtosCommand => new ExcuteViewModelMethod(GetIdTypeCatalog);
        private IGeneralAccess _generalAccess;
        public ICommand SubmitApproveItemsCommand => new ExcuteViewModelMethod(SumbitApproveItems);
        public ICommand SubmitDenyItemsCommand => new ExcuteViewModelMethod(SumbitDenyItems);


        public LangTypeCatalogReviewWindowViewModel(IGeneralAccess generalAccess)
        {
            _generalAccess = generalAccess;

            GetIdTypeDtosCommand.Execute(null);
        }

        private async void GetIdTypeCatalog(object o)
        {
            _langIdListFromServer = await _generalAccess.GetIdTypesFromReview();

            if (LangTypeCatalogDtos.Count > 0)
            {
                LangTypeCatalogDtos.Clear();
            }

            if (_langIdListFromServer != null && _langIdListFromServer.Count > 0)
            {
                LangTypeCatalogDtos.AddRange(_langIdListFromServer);
            }
        }

        private async void SumbitApproveItems(object obj)
        {
            IList selectedItems = obj as IList;
            List<LangTypeCatalogDto> list = selectedItems.Cast<LangTypeCatalogDto>().ToList();

            //List<LangTypeCatalogDto> list = (List<LangTypeCatalogDto>)obj;
            List<int> idList = new List<int>();

            foreach(var IdType in list)
            {
                idList.Add(IdType.IdType);
            }

            if (idList.Count >= 1)
            {
                try
                {
                    var respond = await _generalAccess.ApproveIdTypeFromReview(idList);
                    MessageBox.Show(respond.Message);
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("没有选中项。", "警告",MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private async void SumbitDenyItems(object obj)
        {
            var list = (List<LangTypeCatalogDto>)obj;
            List<int> idList = new List<int>();

            foreach (var IdType in list)
            {
                idList.Add(IdType.IdType);
            }

            if (idList.Count >= 1)
            {
                try
                {
                    var respond = await _generalAccess.DeleteIdTypeFromReview(idList);
                    MessageBox.Show(respond.Message);
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("没有选中项。", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
