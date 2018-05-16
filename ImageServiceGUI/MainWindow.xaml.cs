using ImageServiceGUI.Communication;
using System.ComponentModel;
using System.Windows;

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
