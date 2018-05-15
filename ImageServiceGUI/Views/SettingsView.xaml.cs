using ImageServiceGUI.Communication;
using ImageServiceGUI.Model;
using ImageServiceGUI.ViewModel;
using System.Windows.Controls;

namespace ImageServiceGUI.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            GuiChannel c = GuiChannel.Instance;
            DataContext = new SettingsViewModel(new SettingsModel(c));
        }
    }
}
