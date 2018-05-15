using ImageService.Infastructure.Event;
using ImageServiceGUI.Communication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    public class LogModel : INotifyPropertyChanged
    {
        private GuiChannel channel;
        public event PropertyChangedEventHandler PropertyChanged;
        public LogModel(GuiChannel c)
        {
            channel = c;
            ServiceLogs = new ObservableCollection<Log>();
            channel.Converter.LogsRecieved += OnLogsRecieved;
        }

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void OnLogsRecieved(object sender, CommandRecievedEventArgs a)
        {
            UpdateLogs(a.Args);
        }

        private void UpdateLogs(string[] logs)
        {
            for (int i = 0; i < logs.Length; i += 2)
            {
                Log log = new Log(logs[i], Int32.Parse(logs[i + 1]));
                service_logs.Add(log);
            }
        }

        private ObservableCollection<Log> service_logs;
        public ObservableCollection<Log> ServiceLogs
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
