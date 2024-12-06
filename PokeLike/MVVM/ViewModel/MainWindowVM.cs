using System.Windows;
using PokeLike.Functions;

namespace PokeLike.MVVM.ViewModel
{
    public class MainWindowVM : BaseVM
    {
        #region Variables
        public static Action<Size>? OnRequestChangeSize { get; set; }
        public static Action<BaseVM>? OnRequestVMChange { get; set; }
        private BaseVM _currentVM = new MainViewVM();
        public BaseVM CurrentVM
        {
            get => _currentVM;
            set
            {
                SetProperty(ref _currentVM, value);
                OnPropertyChanged(nameof(CurrentVM));
            }
        }
        #endregion

        public MainWindowVM()
        {
            OnRequestVMChange += HandleRequestViewChange;
            OnRequestVMChange?.Invoke(new MainViewVM());
            OnRequestChangeSize += HandleRequestChangeSize;
            _ = new Init();
        }
        public static void HandleRequestChangeSize(Size size)
        {
            Application.Current.MainWindow.Width = size.Width;
            Application.Current.MainWindow.Height = size.Height;
        }
        public void HandleRequestViewChange(BaseVM a_VMToChange)
        {
            CurrentVM = a_VMToChange;
        }
    }
}