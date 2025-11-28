using Microsoft.UI.Xaml;
using Conversor.ViewModels;

namespace Conversor
{
    public sealed partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; }
        public MainViewModel DataContext { get; }

        public MainWindow()
        {
            ViewModel = new MainViewModel();
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }
    }
}