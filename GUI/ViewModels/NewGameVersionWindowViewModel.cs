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
using System.Windows;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public class NewGameVersionWindowViewModel : BindableBase
    {
        //private string _gameApiVersion;
        //private string _gameVersionName_EN;
        //private string _gameVersionName_ZH;
        private GameVersionDto _selectedDto;

        private ObservableCollection<GameVersionDto> _gameVersionDtos = new ObservableCollection<GameVersionDto>();
        
        //public string GameApiVersion
        //{
        //    get => _gameApiVersion;
        //    set => SetProperty(ref _gameApiVersion, value);
        //}

        //public string GameVersionName_EN
        //{
        //    get => _gameVersionName_EN;
        //    set => SetProperty(ref _gameVersionName_EN, value);
        //}

        //public string GameVersionName_ZH
        //{
        //    get => _gameVersionName_ZH;
        //    set => SetProperty(ref _gameVersionName_ZH, value);
        //}

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

        private IGeneralAccess _generalAccess;

        public ICommand SumbitCommand => new ExcuteViewModelMethod(SumbitVersion);


        public NewGameVersionWindowViewModel(IGeneralAccess generalAccess)
        {
            _generalAccess = generalAccess;

            GameVersionDtos.Add(new GameVersionDto { GameApiVersion = 100000, Version_EN = "aaaaaaa", Version_ZH = "bbbbbbb" });
            GameVersionDtos.Add(new GameVersionDto { GameApiVersion = 100001, Version_EN = "ccccccc", Version_ZH = "ddddddd" });
            GameVersionDtos.Add(new GameVersionDto { GameApiVersion = 100002, Version_EN = "fffffff", Version_ZH = "qqqqqqq" });
        }

        private async Task GetGameVersionFromServer()
        {
            var versionDtos = await _generalAccess.GetGameVersionDtos();

            if (GameVersionDtos.Count > 0)
            {
                GameVersionDtos.Clear();

                GameVersionDtos.AddRange(versionDtos);
            }
        }

        private void SumbitVersion(object obj)
        {
            //MessageBox.Show(obj.ToString());
            var selectedItem = obj as GameVersionDto;
            if (selectedItem != null)
            {
                MessageBox.Show($"api: {selectedItem.GameApiVersion}\n en: {selectedItem.Version_EN}\n zh: {selectedItem.Version_ZH}");
            }
        }

    }
}
