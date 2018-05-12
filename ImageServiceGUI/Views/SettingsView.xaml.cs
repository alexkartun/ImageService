using ImageServiceGUI.ViewModel;
using System.Windows.Controls;

namespace ImageServiceGUI.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private SettingsViewModel settings_vm;
        public SettingsView()
        {
            InitializeComponent();
            settings_vm = new SettingsViewModel();
            DataContext = settings_vm;
        }
    }
}
