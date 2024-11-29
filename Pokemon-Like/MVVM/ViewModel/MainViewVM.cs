using CommunityToolkit.Mvvm.Input;
using Microsoft.Xaml.Behaviors.Core;
using Pokemon_Like.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pokemon_Like.MVVM.ViewModel
{
    public class MainViewVM : BaseVM
    {
        public ICommand RequestSettingsView { get; set; }
        public ICommand RequestLoginView { get; set; }
        public ICommand RequestAddUser { get; set; }
        public MainViewVM()
        {
            RequestSettingsView = new RelayCommand(HandleRequestSettingsView);
            RequestLoginView = new RelayCommand(HandleRequestLoginView);
            RequestAddUser = new RelayCommand(HandleRequestAddUser);
        }


        private void HandleRequestSettingsView()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new SettingsViewVM());
        }


        private void HandleRequestLoginView()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new LoginViewVM());
        }

        private void HandleRequestAddUser()
        {
            //Login user = DBLogic.AddUser(new Login { Username = "user", PasswordHash = Convert.ToBase64String(Encoding.ASCII.GetBytes("password")) });
            new DBLogic();
        }
    }
}
