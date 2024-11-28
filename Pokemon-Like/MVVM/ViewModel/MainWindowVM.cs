using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Like.MVVM.ViewModel
{
    public class MainWindowVM : BaseVM
    {
        static public Action<BaseVM> OnRequestVMChange;

        #region Variables
        private BaseVM _currentVM;
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
            MainWindowVM.OnRequestVMChange += HandleRequestViewChange;
            MainWindowVM.OnRequestVMChange?.Invoke(new MainViewVM());

        }

        public void HandleRequestViewChange(BaseVM a_VMToChange)
        {
            CurrentVM = a_VMToChange;
        }
    }
}
