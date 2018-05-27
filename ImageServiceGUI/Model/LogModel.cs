using ImageService.Communication.Model;
using ImageService.Infastructure.Enums;
using ImageService.Infastructure.Event;
using ImageService.Logging.Model;
using ImageServiceGUI.Communication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImageServiceGUI.Model
{
    public class LogModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public LogModel()
        {
            ClientConnection.DataRecieved += OnDataRecieved;
            service_logs = new ObservableCollection<MessageRecievedEventArgs>();
            Object thisLock = new Object();
			// For data-binding on concurrent tasks.
            BindingOperations.EnableCollectionSynchronization(service_logs, thisLock);
			// Gets all logs from server and writes it on client GUI.
            CommandMessage req = new CommandMessage((int)CommandEnum.LogCommand);
            ClientConnection.Write(req);
        }

        public GuiChannel ClientConnection
        {
            get
            {
                return GuiChannel.Instance;
            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

		//recieve from command converter
        public void OnDataRecieved(object sender, CommandRecievedEventArgs a)
        {
            if (a.Command == (int) CommandEnum.LogCommand)
            {
                UpdateLogs(a.Args);
            }
        }

        private void UpdateLogs(string[] logs)
        {
            for (int i = 0; i < logs.Length; i += 2)
            {
                string m = logs[i];
                MessageTypeEnum t = MessageRecievedEventArgs.GetTypeEnum(Int32.Parse(logs[i + 1]));
                MessageRecievedEventArgs log = new MessageRecievedEventArgs(m, t);
				// Adds a single log to an observables logs list.
                ServiceLogs.Add(log);
            }
        }

        private ObservableCollection<MessageRecievedEventArgs> service_logs;
        public ObservableCollection<MessageRecievedEventArgs> ServiceLogs
        {
            get { return service_logs; }
            set
            {
                service_logs = value;
                NotifyPropertyChanged("ServiceLogs");
            }
        }
    }
}
