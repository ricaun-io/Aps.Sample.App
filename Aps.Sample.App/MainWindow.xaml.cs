using System.Windows;

namespace Aps.Sample.App
{
    public partial class MainWindow : Window
    {
        ApsService ApsService = new ApsService();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(ApsService);
        }
    }
}