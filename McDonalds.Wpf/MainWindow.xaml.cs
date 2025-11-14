using System.Windows;
using McDonalds.ViewModels;

namespace McDonalds.Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}