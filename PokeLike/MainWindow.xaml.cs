using System.Windows;
using PokeLike.MVVM.ViewModel;

namespace PokeLike
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowVM MainWindowVM { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            MainWindowVM = new MainWindowVM();
            DataContext = MainWindowVM;
        }
    }
}