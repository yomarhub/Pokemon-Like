using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokeLike.Functions;
using PokeLike.Model;

namespace PokeLike.MVVM.ViewModel
{
    public abstract class BaseVM : ObservableObject
    {
        public static bool CanPlay { get => Session.User != null; }
        #region ViewCommands
        public RelayCommand RequestMainView { get; set; }
        public RelayCommand<Type> RequestView { get; set; }
        #endregion

        protected readonly ExerciceMonsterContext _context = new();

        public BaseVM()
        {
            RequestMainView = new RelayCommand(HandleRequestMainView);
            RequestView = new RelayCommand<Type>(HandleRequestView!);
        }
        #region ViewHandlers
        private void HandleRequestMainView() => HandleRequestView(typeof(MainViewVM));
        private void HandleRequestView(Type vmType)
        {
            if (vmType == null || !typeof(BaseVM).IsAssignableFrom(vmType)) return;
            try
            {
                // Dynamically create an instance of the ViewModel
                var viewModel = (BaseVM)Activator.CreateInstance(vmType)!;
                OnHideView();
                MainWindowVM.OnRequestVMChange?.Invoke(viewModel);
                viewModel.OnShowView();
            }
            catch (Exception ex) { MessageBox.Show($"Failed to create ViewModel: {ex.Message}"); }
        }
        #endregion
        public static void ChangeWindowSize(Size size)
        {
            Application.Current.MainWindow.Width = size.Width;
            Application.Current.MainWindow.Height = size.Height;
        }
        #region OnChangeView
        public virtual void OnShowView() { }
        public virtual void OnHideView()
        {
            MainWindowVM.OnRequestClearMessages?.Invoke();
        }
        #endregion
    }
}
