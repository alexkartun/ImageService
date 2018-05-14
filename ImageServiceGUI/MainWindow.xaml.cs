using ImageServiceGUI.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageServiceGUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        public MainWindow()
		{
			InitializeComponent();
            GuiChannel c = GuiChannel.Instance;
            if (c.Connect()) // Connected to server.
            {
                c.Start();
                c.GetSettings();
            } 
            else  // Not Connected
            {
                
            }
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            GuiChannel c = GuiChannel.Instance;
            c.Disconnect();
        }
    }
}
