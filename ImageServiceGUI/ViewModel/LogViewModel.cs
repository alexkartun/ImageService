using ImageService.Logging.Model;
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
    public class LogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private LogModel model;

        public LogViewModel()
        {
            model = new LogModel();
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

		// Binded to logs list on view.
        public ObservableCollection<MessageRecievedEventArgs> VM_ServiceLogs
        {
            get { return model.ServiceLogs; }
        }
    }
}
