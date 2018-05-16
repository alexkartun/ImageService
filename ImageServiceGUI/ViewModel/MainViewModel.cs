using ImageServiceGUI.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    public class MainViewModel
    {
        public GuiChannel ClientConnection
        {
            get
            {
                return GuiChannel.Instance;
            }
        }

        public string IsConnected
        {
            get {
                if (ClientConnection.IsConnected) return "Green";
                else return "Gray";
            }
        }
    }
}
