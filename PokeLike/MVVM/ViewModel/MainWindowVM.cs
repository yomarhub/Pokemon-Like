using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using PokeLike.Functions;

namespace PokeLike.MVVM.ViewModel
{
    public class MainWindowVM : BaseVM
    {
        #region Variables
        #region Background
        public static ImageBrush BackgroundImage { get; private set; } = (ImageBrush)Application.Current.FindResource("background");
        public static string BackgroundPath
        {
            get => ((BitmapImage)BackgroundImage.ImageSource).UriSource.OriginalString;
            set => ((BitmapImage)BackgroundImage.ImageSource).UriSource = new(value, UriKind.Relative);
        }
        private Brush background = BackgroundImage;
        public Brush Background
        {
            get => background;
            set { if (SetProperty(ref background, value)) OnPropertyChanged(nameof(Background)); }
        }
        #endregion
        public static Action<BaseVM>? OnRequestVMChange { get; set; }
        public static Action<string>? OnRequestMessage { get; set; }
        public static Action? OnRequestClearMessages { get; set; }
        public static Action<Brush>? OnRequestChangeBackground { get; set; }
        private BaseVM _currentVM = new MainViewVM();

        public BaseVM CurrentVM
        {
            get => _currentVM;
            set { if (SetProperty(ref _currentVM, value)) OnPropertyChanged(nameof(CurrentVM)); }
        }

        private SnackbarMessageQueue messageQueue = new(TimeSpan.FromSeconds(1));
        public SnackbarMessageQueue MessageQueue
        {
            get => messageQueue; set
            {
                if (SetProperty(ref messageQueue, value)) OnPropertyChanged(nameof(MessageQueue));
            }
        }
        #endregion

        public MainWindowVM()
        {
            OnRequestMessage = (string str) => MessageQueue.Enqueue(str);
            OnRequestClearMessages = () => MessageQueue.Clear();
            OnRequestChangeBackground += (b) => Background = b;
            OnRequestVMChange += HandleRequestViewChange;
            OnRequestVMChange?.Invoke(new MainViewVM());
            _ = new Init();
        }

        public void HandleRequestViewChange(BaseVM a_VMToChange)
        {
            CurrentVM = a_VMToChange;
        }

    }
}