using ImageServiceGUI.Communication;
using ImageServiceGUI.Model;
using ImageServiceGUI.ViewModel;
using System.Windows.Controls;

namespace ImageServiceGUI.Views
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        public LogView()
        {
            InitializeComponent();
            GuiChannel c = GuiChannel.Instance;
            DataContext = new LogViewModel(new LogModel(c));
        }
    }
}
