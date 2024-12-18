using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using PokeLike.Functions;
using PokeLike.Model;

namespace PokeLike.MVVM.ViewModel
{
    public partial class SettingsViewVM : BaseVM
    {
        private string sqlUrl = ExerciceMonsterContext.ServerURL;
        public string SqlUrl
        {
            get => sqlUrl; set
            {
                SetProperty(ref sqlUrl, value);
                OnPropertyChanged(nameof(SqlUrl));
            }
        }
        public SettingsViewVM() : base() { }

        [RelayCommand]
        public void Connect()
        {
            ExerciceMonsterContext.ServerURL = SqlUrl;
            _context = new();
            try
            {
                _context.Database.GetService<IRelationalDatabaseCreator>().Exists();
                MainWindowVM.OnRequestMessage?.Invoke("Connection successfull");
            }
            catch { MainWindowVM.OnRequestMessage?.Invoke("Server doesn't exist : Check for the Server Name"); return; }
            Session.DbConnected = true;
            // Comment this line to avoid the database recreation
            _ = new Init();
        }

    }
}