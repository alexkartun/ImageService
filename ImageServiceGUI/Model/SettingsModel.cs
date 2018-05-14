using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageService.Infastructure.Event;
using ImageServiceGUI.Communication;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    class SettingsModel : ISettingsModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private GuiChannel channel;

        public SettingsModel(GuiChannel c)
        {
            channel = c;
            channel.Converter.SettingsRecieved += OnSettingsRecieved;
        }

        public void OnSettingsRecieved(object sender, CommandRecievedEventArgs a)
        {
            if (a.Command == (int) CommandEnum.GetConfigCommand)
            {
                SetSettings(a.Args);
            }
            else // Command is remove handler.
            {
                RemoveHandler(a.Args[0]);
            }
        }

        private void RemoveHandler(string directory)
        {
            directory_handlers.Remove(directory);
        }

        private void SetSettings(string[] args)
        {
            OutputDir = args[0];
            SourceName = args[1];
            LogName = args[2];
            ThumbnailSize = args[3];
            ObservableCollection<String> handlers = new ObservableCollection<String>();
            for (int i = 4; i < args.Length; i++)
            {
                handlers.Add(args[i]);
            }
            DirectoryHandlers = handlers;
        }
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private string output_dir;
        public string OutputDir
        {
            get { return output_dir; }
            set
            {
                output_dir = value;
                NotifyPropertyChanged("OutputDir");
            }
        }

        private string source_name;
        public string SourceName
        {
            get { return source_name; }
            set
            {
                source_name = value;
                NotifyPropertyChanged("SourceName");
            }
        }

        private string log_name;
        public string LogName
        {
            get { return log_name; }
            set
            {
                log_name = value;
                NotifyPropertyChanged("LogName");
            }
        }

        private string thumbnail_size;
        public string ThumbnailSize
        {
            get { return thumbnail_size; }
            set
            {
                thumbnail_size = value;
                NotifyPropertyChanged("ThumbnailSize");
            }
        }

        private ObservableCollection<String> directory_handlers;
        public ObservableCollection<String> DirectoryHandlers
        {
            get { return directory_handlers; }
            set
            {
                directory_handlers = value;
                NotifyPropertyChanged("DirectoryHandlers");
            }
        }
    }
}
