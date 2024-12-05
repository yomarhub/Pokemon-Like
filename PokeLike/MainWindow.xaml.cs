using System.Windows;
using PokeLike.MVVM.ViewModel;

namespace PokeLike
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowVM _mainWindowVM;
        public MainWindow()
        {
            InitializeComponent();
            _mainWindowVM = new MainWindowVM();
            DataContext = _mainWindowVM;
        }
    }
}