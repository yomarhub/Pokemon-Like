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
        private string? _errorText;
        public string? ErrorText { get => _errorText; set { _errorText = value; OnPropertyChanged(nameof(ErrorText)); } }

        public string? Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public Login Login { get; private set; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Action? OnDialogClose { get; set; }
        public Player? CreatedPlayer { get; private set; }

        public CreatePlayerVM(Login l)
        {
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            Login = l;
        }

        private ExerciceMonsterContext context = new();
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                ErrorText = "Name is required";
                return;
            }
            CreatedPlayer = new Player
            {
                Name = this.Name!,
                LoginId = this.Login.Id
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

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
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
