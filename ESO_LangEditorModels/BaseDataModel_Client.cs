using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ESO_LangEditorModels
{
    public abstract class BaseDataModelClient : INotifyPropertyChanged
    {
        protected BaseDataModelClient() { }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        //public abstract override string ToString();

        #region INotifyPropertyChanged Members

        /// <summary>
        /// The event that is raised when a property that calls the NotifyPropertyChanged method is changed.
        /// </summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event alerting the WPF Framework to update the UI.
        /// </summary>
        /// <param name="propertyNames">The names of the properties to update in the View.</param>
        //protected virtual void NotifyPropertyChanged(params string[] propertyNames)
        //{
        //    if (PropertyChanged != null) propertyNames.ForEach(p => PropertyChanged(this, new PropertyChangedEventArgs(p)));
        //}

        /// <summary>
        /// Raises the PropertyChanged event alerting the WPF Framework to update the UI.
        /// </summary>
        /// <param name="propertyName">The optional name of the property to update in the View. If this is left blank, the name will be taken from the calling member via the CallerMemberName attribute.</param>
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
