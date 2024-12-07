using PokeLike.Functions;

namespace PokeLike.MVVM.ViewModel
{
    public class MainWindowVM : BaseVM
    {
        #region Variables
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
            _ = new Init();
        }

        public void HandleRequestViewChange(BaseVM a_VMToChange)
        {
            CurrentVM = a_VMToChange;
        }
    }
}