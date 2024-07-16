using System.Windows;
using Aps.Sample.App.Services;

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