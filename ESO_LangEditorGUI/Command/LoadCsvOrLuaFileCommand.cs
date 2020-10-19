using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.View;
using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorLib.Services.Client;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditorGUI.Command
{
    public class LoadCsvAndDbCommand : CommandBaseAsync
    {
        private CompareWindowViewModel _compareWindowViewModel;
        private CompareWithDBWindow _compareWithDBWindow;
        private ParseLangFile parseLangFile = new ParseLangFile();
        private LangTextRepository langTextRepository = new LangTextRepository();

        public LoadCsvAndDbCommand(CompareWindowViewModel compareWindowViewModel)
        {
            _compareWindowViewModel = compareWindowViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _compareWindowViewModel.OpenFileCommand.IsExecuting = true;

            if (_compareWindowViewModel.IsReadMode)
            {
                var filelist = _compareWindowViewModel.FileList;
                var luaList = new List<string>();

                Dictionary<string, LangTextDto> fileContent = new Dictionary<string, LangTextDto>();

                _compareWindowViewModel.DbDict = await Task.Run(() => langTextRepository.GetAlltLangTextsDictionaryAsync());

                foreach (var file in filelist)
                {
                    if (file.EndsWith(".lua"))
                        luaList.Add(file);
                    else
                        fileContent = await parseLangFile.CsvParserToDictionaryAsync(file);
                }
                Dictionary<string, LangTextDto> lualist = await parseLangFile.LuaParser(luaList);

                foreach (var item in lualist)
                {
                    fileContent.Add(item.Key, item.Value);
                }
                //fileContent = await parseLangFile.LuaParser(luaList);

                _compareWindowViewModel.CsvDict = fileContent;
                _compareWindowViewModel.IsReadMode = false;
                _compareWindowViewModel.LoadButtonContent = "开始对比";


                IsExecuting = false;
            }
            else
            {
                IsExecuting = true;

                CompareLangsFromCsvAndDb comparer = new CompareLangsFromCsvAndDb(_compareWindowViewModel);
                comparer.CompareDicts();

                _compareWindowViewModel.IsReadMode = true;
                _compareWindowViewModel.LoadButtonContent = "开始读取";

                IsExecuting = false;
                _compareWindowViewModel.OpenFileCommand.IsExecuting = false;
            }

            //_compareWindowViewModel.OpenFileCommand.IsExecuting = false;

            //throw new NotImplementedException();


        }
    }
}
