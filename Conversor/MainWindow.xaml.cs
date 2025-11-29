using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Conversor.ViewModels;

namespace Conversor
{
    public sealed partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; }

        public MainWindow()
        {
            this.InitializeComponent();

            ViewModel = new MainViewModel();

            if (this.Content is FrameworkElement root)
            {
                root.DataContext = ViewModel;
            }
            else
            {
                var grid = new Grid();
                this.Content = grid;
                grid.DataContext = ViewModel;
            }
        }
    }
}
