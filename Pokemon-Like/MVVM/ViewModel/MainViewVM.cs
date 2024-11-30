using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Pokemon_Like.Functions;

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
            GameData.GetDataFromJson(new Model.ExerciceMonsterContext());
        }
    }
}
