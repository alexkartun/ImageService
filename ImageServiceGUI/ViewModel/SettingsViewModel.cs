using ImageServiceGUI.Model;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ImageServiceGUI.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private SettingsModel sett_model;
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsViewModel()
        {
            sett_model = new SettingsModel();
            sett_model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
            RemoveCommand = new DelegateCommand<object>(OnRemove, CanRemove);
        }

        private void OnRemove(object obj)
        {
            sett_model.RemoveHandler(selected_path);
        }

        private bool CanRemove(object obj)
        {
            if(string.IsNullOrEmpty(selected_path))
            {
                return false;
            }
            return true;
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

        private string selected_path;
        public string VM_SelectedPath
        {
            get { return selected_path; }
            set
            {
                selected_path = value;
                var command = this.RemoveCommand as DelegateCommand<object>;
                command.RaiseCanExecuteChanged();
            }
        }

        public ICommand RemoveCommand { get; set; }

        public ObservableCollection<String> VM_DirectoryHandlers
        {
            get { return sett_model.DirectoryHandlers; }
        }
    }
}
