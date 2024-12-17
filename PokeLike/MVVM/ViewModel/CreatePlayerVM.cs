using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using PokeLike.Model;

namespace PokeLike.MVVM.ViewModel
{
    public class CreatePlayerVM : INotifyPropertyChanged
    {
        private string? _name;
        private int? _loginId;
        private string? _errorText;
        public string? ErrorText { get => _errorText; set { _errorText = value; OnPropertyChanged(nameof(ErrorText)); } }

        public string? Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public Login Login { get; private set; }

        public int? LoginId
        {
            get => _loginId;
            set { _loginId = value; OnPropertyChanged(nameof(LoginId)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Action? OnDialogClose { get; set; }
        public Player? CreatedPlayer { get; private set; }

        public CreatePlayerVM()
        {
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }
        public CreatePlayerVM(Login l) : this()
        {
            Login = l;
            LoginId = (l.Id > 0) ? l.Id : 1;
        }

        private ExerciceMonsterContext context = new();
        private void Save()
        {
            Login? last = context.Logins.Order().LastOrDefault();
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                ErrorText = "Name is required";
                return;
            }
            else if (this.LoginId < 1)
            {
                ErrorText = "LoginId is required";
                return;
            }
            else if (last == null || LoginId > last.Id) { ErrorText = "invalid LoginId"; return; }
            CreatedPlayer = new Player
            {
                Name = this.Name!,
                LoginId = this.LoginId
            };

            OnDialogClose?.Invoke();
        }

        private void Cancel()
        {
            OnDialogClose?.Invoke();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }
    }
}
