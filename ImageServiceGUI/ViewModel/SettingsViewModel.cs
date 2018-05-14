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
        private ISettingsModel sett_model;
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsViewModel(GuiChannel channel)
        {
            sett_model = new SettingsModel(channel);
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

        private string output_dir;
        public string VM_OutputDir
        {
            get { return output_dir; }
            set => output_dir = value;
        }

        private string source_name;
        public string VM_SourceName
        {
            get { return source_name; }
            set => source_name = value;
        }

        private string log_name;
        public string VM_LogName
        {
            get { return log_name; }
            set => log_name = value;
        }

        private string thumbnail_size;
        public string VM_ThumbnailSize
        {
            get { return thumbnail_size; }
            set => thumbnail_size = value;
        }

        private ObservableCollection<String> directory_handlers;
        public ObservableCollection<String> VM_DirectoryHandlers
        {
            get { return directory_handlers;  }
            set => directory_handlers = value;
        }
    }
}
