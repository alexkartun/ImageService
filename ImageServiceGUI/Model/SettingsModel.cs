using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageService.Infastructure.Event;
using ImageServiceGUI.Communication;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImageServiceGUI.Model
{
    class SettingsModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsModel()
        {
            directory_handlers = new ObservableCollection<String>();
            Object thisLock = new Object();
            BindingOperations.EnableCollectionSynchronization(directory_handlers, thisLock);
            ClientConnection.DataRecieved += OnDataRecieved;
            CommandMessage req = new CommandMessage((int) CommandEnum.GetConfigCommand);
            ClientConnection.Write(req);
            ClientConnection.Read();
        }

        public GuiChannel ClientConnection
        {
            get
            {
                return GuiChannel.Instance;
            }
        }

        public void OnDataRecieved(object sender, CommandRecievedEventArgs a)
        {
            if (a.Command == (int) CommandEnum.GetConfigCommand)
            {
                SetSettings(a.Args);
            }
            else if (a.Command == (int)CommandEnum.CloseCommand)
			{
                DirectoryHandlers.Remove(a.Args[0]);
            }
        }


		// recieves args from 
        private void SetSettings(string[] args)
        {
            OutputDir = args[0];
            SourceName = args[1];
            LogName = args[2];
            ThumbnailSize = args[3];
            for (int i = 4; i < args.Length; i++)
            {
                DirectoryHandlers.Add(args[i]);
            }
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
