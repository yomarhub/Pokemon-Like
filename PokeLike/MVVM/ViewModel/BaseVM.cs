using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokeLike.Model;

namespace PokeLike.MVVM.ViewModel
{
    public abstract class BaseVM : ObservableObject
    {
        //<ImageBrush x:Key="BackgroundImage" AlignmentX="Center" AlignmentY="Center"
        // ImageSource="Ressources/Images/ultra.jpg" Stretch="UniformToFill" />
        public static ImageBrush BackgroundImage { get; private set; } = (ImageBrush)Application.Current.FindResource("background");
        public static string BackgroundPath
        {
            get => ((BitmapImage)BackgroundImage.ImageSource).UriSource.OriginalString;
            set => ((BitmapImage)BackgroundImage.ImageSource).UriSource = new(value, UriKind.Relative);
        }
        protected readonly ExerciceMonsterContext _context = new();
        public ICommand RequestMainView { get; set; }
        public RelayCommand<Type> RequestView { get; set; }
        public BaseVM()
        {
            RequestMainView = new RelayCommand(HandleRequestMainView);
            RequestView = new RelayCommand<Type>(HandleRequestView!);
        }
        private void HandleRequestMainView() => MainWindowVM.OnRequestVMChange?.Invoke(new MainViewVM());
        private void HandleRequestView(Type vmType)
        {
            if (vmType == null || !typeof(BaseVM).IsAssignableFrom(vmType)) return;
            try
            {
                // Dynamically create an instance of the ViewModel
                var viewModel = (BaseVM)Activator.CreateInstance(vmType)!;
                MainWindowVM.OnRequestVMChange?.Invoke(viewModel);
            }
            catch (Exception ex) { MessageBox.Show($"Failed to create ViewModel: {ex.Message}"); }
        }
        public static void ChangeWindowSize(Size size)
        {
            Application.Current.MainWindow.Width = size.Width;
            Application.Current.MainWindow.Height = size.Height;
        }
    }
}
