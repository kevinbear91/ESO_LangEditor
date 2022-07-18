using Core.Entities;
using Core.Models;
using GUI.Command;
using GUI.EventAggres;
using GUI.Services;
using Prism.Events;
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
    public class NewGameVersionWindowViewModel : BindableBase
    {
        private GameVersionDto _selectedDto;
        private ObservableCollection<GameVersionDto> _gameVersionDtos = new ObservableCollection<GameVersionDto>();

        public ObservableCollection<GameVersionDto> GameVersionDtos
        {
            get => _gameVersionDtos;
            set => SetProperty(ref _gameVersionDtos, value);
        }

        public GameVersionDto SelectedDto
        {
            get => _selectedDto;
            set => SetProperty(ref _selectedDto, value);
        }

        //private ICommand GetGameVersionCommand => new ExcuteViewModelMethod(GetGameVersionFromServer);
        private ICommand GetGameVersionCommand => new ExcuteViewModelMethod(GetGameVersionFromLocal);
        //private IGeneralAccess _generalAccess;
        private IEventAggregator _ea;
        private ILangTextRepoClient _langTextRepoClient;
        public ICommand SumbitCommand => new ExcuteViewModelMethod(SaveGameVersionToLocal);
        public ICommand SelectCommand => new ExcuteViewModelMethod(SelectVersion);

        public NewGameVersionWindowViewModel(IEventAggregator ea, ILangTextRepoClient langTextRepoClient)
        {
            //_generalAccess = generalAccess;
            _langTextRepoClient = langTextRepoClient;
            _ea = ea;

            GetGameVersionCommand.Execute(null);
        }


        //private async void GetGameVersionFromServer(object o)
        //{
        //    var versionDtos = await _generalAccess.GetGameVersionDtos();

        //    if (GameVersionDtos.Count > 0)
        //    {
        //        GameVersionDtos.Clear();

        //        GameVersionDtos.AddRange(versionDtos);
        //    }
        //    else
        //    {
        //        GameVersionDtos.AddRange(versionDtos);
        //    }
        //}

        private async void GetGameVersionFromLocal(object o)
        {
            var versionDtos = await _langTextRepoClient.GetGameVersionDtoList();

            if (GameVersionDtos.Count > 0)
            {
                GameVersionDtos.Clear();

                GameVersionDtos.AddRange(versionDtos);
            }
            else
            {
                GameVersionDtos.AddRange(versionDtos);
            }
        }

        private async void SaveGameVersionToLocal(object obj)
        {
            var selectedItem = obj as GameVersionDto;
            if (selectedItem != null)
            {
                GameVersion gameVersionEntity = new()
                { 
                    GameApiVersion = selectedItem.GameApiVersion,
                    Version_EN = selectedItem.Version_EN,
                    Version_ZH = selectedItem.Version_ZH,
                };

                try
                {
                    if (await _langTextRepoClient.SaveGameVersion(gameVersionEntity))
                    {
                        MessageBox.Show("保存成功！");
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                //MessageBox.Show($"api: {selectedItem.GameApiVersion}\n en: {selectedItem.Version_EN}\n zh: {selectedItem.Version_ZH}");
            }
        }

        //private async void SumbitVersion(object obj)
        //{
        //    //MessageBox.Show(obj.ToString());
        //    var selectedItem = obj as GameVersionDto;
        //    if (selectedItem != null)
        //    {
        //        try
        //        {
        //            var respond = await _generalAccess.UploadNewGameVersion(selectedItem);
        //            MessageBox.Show(respond.Message);
        //        }
        //        catch (HttpRequestException ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }

        //        //MessageBox.Show($"api: {selectedItem.GameApiVersion}\n en: {selectedItem.Version_EN}\n zh: {selectedItem.Version_ZH}");
        //    }
        //}

        private void SelectVersion(object obj)
        {
            var selectedItem = obj as GameVersionDto;
            if (selectedItem != null)
            {
                _ea.GetEvent<SelectGameVersionEvent>().Publish(selectedItem.GameApiVersion);
            }
        }
    }
}
