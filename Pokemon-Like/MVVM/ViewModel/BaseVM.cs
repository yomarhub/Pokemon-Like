using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pokemon_Like.Model;

namespace Pokemon_Like.MVVM.ViewModel
{
    abstract public class BaseVM : ObservableObject
    {
        protected readonly ExerciceMonsterContext _context = new();
        public ICommand RequestMainView { get; set; }
        public BaseVM()
        {
            RequestMainView = new RelayCommand(HandleRequestMainView);
        }
        private void HandleRequestMainView() => MainWindowVM.OnRequestVMChange?.Invoke(new MainViewVM());
    }
}
