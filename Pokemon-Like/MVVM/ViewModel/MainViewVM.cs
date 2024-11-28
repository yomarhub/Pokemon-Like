using CommunityToolkit.Mvvm.Input;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pokemon_Like.MVVM.ViewModel
{
    public class MainViewVM : BaseVM
    {
        public ICommand RequestSettingsView { get; set; }
        public ICommand RequestLoginView { get; set; }
        public MainViewVM() {
            RequestSettingsView = new RelayCommand(HandleRequestSettingsView);
            RequestLoginView = new RelayCommand(HandleRequestLoginView);
        }


        private void HandleRequestSettingsView()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new SettingsViewVM());
        }


        private void HandleRequestLoginView()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new LoginViewVM());
        }
    }
}
