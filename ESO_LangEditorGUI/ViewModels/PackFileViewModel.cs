using ESO_LangEditorGUI.Command;
using ESO_LangEditorLib.Models.Client.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESO_LangEditorGUI.ViewModels
{
    public class PackFileViewModel : BaseViewModel
    {
        private string _addonVersion;
        private string _apiVersion;

        public string AddonVersion
        {
            get { return _addonVersion; }
            set { _addonVersion = value; NotifyPropertyChanged(); }
        }

        public string ApiVersion
        {
            get { return _apiVersion; }
            set { _apiVersion = value; NotifyPropertyChanged(); }
        }

        public IEnumerable<CHSorCHT> ChsOrChtList
        {
            get { return Enum.GetValues(typeof(CHSorCHT)).Cast<CHSorCHT>(); }
        }

        public ExportToFileCommand exportToFileCommand { get; }


        public PackFileViewModel()
        {
            //exportToFileCommand = new ExportToFileCommand();


        }

    }
}
