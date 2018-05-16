using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    public class Log : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Log(string m, int t)
        {
            Message = m;
            Type = t;
        }


        private string message;
        public String Message
        {
            get { return message; }
            set
            {
                message = value;
                NotifyPropertyChanged("Message");
            }
        }

        private int type;
        public Int32 Type
        {
            get { return type; }
            set
            {
                type = value;
                NotifyPropertyChanged("Type");
            }
        }
		// TODO: check INOTIFY relevance.
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
