using ImageServiceGUI.Communication;
using ImageServiceGUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private SettingsModel sett_model;
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsViewModel(SettingsModel m)
        {
            sett_model = m;
            sett_model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public string VM_OutputDir
        {
            get { return sett_model.OutputDir; }
        }

        public string VM_SourceName
        {
            get { return sett_model.SourceName; }
        }

        public string VM_LogName
        {
            get { return sett_model.LogName; }
        }

        public string VM_ThumbnailSize
        {
            get { return sett_model.ThumbnailSize; }
        }

        public ObservableCollection<String> VM_DirectoryHandlers
        {
            get { return sett_model.DirectoryHandlers; }
        }
    }
}
