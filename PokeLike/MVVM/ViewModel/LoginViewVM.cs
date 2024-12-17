using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using PokeLike.Functions;
using PokeLike.Model;

namespace PokeLike.MVVM.ViewModel
{
    public class LoginViewVM : BaseVM
    {
        #region Variables
        public ICommand RequestLogin { get; set; }

        private string userName = "test";
        public string UserName
        {
            get => userName;
            set
            {
                SetProperty(ref userName, value);
                OnPropertyChanged(nameof(UserName));
            }
        }

        private string password = "test";
        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
                OnPropertyChanged(nameof(Password));
            }
        }
        #endregion
        public LoginViewVM() : base()
        {
            RequestLogin = new RelayCommand(HandleRequestLogin);
        }

        private void HandleRequestLogin()
        {
            string hashedPassword = Hash.CreateHash(Password);
            /*
            MessageBox.Show($"Login requested : {UserName}" +
                $"\nPassword : {Password}" +
                $"\nHash : {hashedPassword}");
            */
            Login? found = _context.Logins.FirstOrDefault(u => u.Username == UserName && u.PasswordHash == hashedPassword);
            if (found != null)
            {
                Session.User = found;
                //MessageBox.Show("User found");
                //MessageBox.Show(
                //    $"Id : {found.Id}" +
                //    $"\nUserName : {found.Username}" +
                //    $"\nPassword : {found.PasswordHash}");
                RequestMainView.Execute(null);
            }
            else
            {
                MessageBox.Show($"User not found : {UserName}");
            }
        }

        public override void OnShowView()
        {
            base.OnShowView();
            //MainWindowVM.OnRequestChangeBackground?.Invoke(new SolidColorBrush(Color.FromArgb(35, 0, 0, 0)));
        }
        public override void OnHideView()
        {
            base.OnHideView();
            if (Session.User != null) MainWindowVM.OnRequestMessage?.Invoke($"User {Session.User.Username} logged-in Successfully");
        }
    }
}
