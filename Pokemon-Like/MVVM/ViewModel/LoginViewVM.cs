using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Pokemon_Like.Functions;
using Pokemon_Like.Model;

namespace Pokemon_Like.MVVM.ViewModel
{
    public class LoginViewVM : BaseVM
    {

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

        public LoginViewVM()
        {
            RequestLogin = new RelayCommand(PerformRequestLogin);
        }

        private void PerformRequestLogin()
        {
            string hashedPassword = Hash.CreateHash(Password);
            MessageBox.Show($"Login requested : {UserName}" +
                $"\nPassword : {Password}" +
                $"\nHash : {hashedPassword}");
            Login? found = _context.Logins.FirstOrDefault(u => u.Username == UserName && u.PasswordHash == hashedPassword);
            if (found != null)
            {
                MessageBox.Show($"found : {found.Id}, {found.Username}, {found.PasswordHash}");
                MessageBox.Show("User found");
                MessageBox.Show(
                    String.Format(
                        "UserName : {0}, password : {1}",
                        found.Username,
                        found.PasswordHash
                    )
                );
            }
            else
            {
                MessageBox.Show("User not found");
            }
        }

    }
}
