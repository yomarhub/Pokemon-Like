using PokeLike.Model;

namespace PokeLike.MVVM.ViewModel
{
    public class SettingsViewVM : BaseVM
    {
        private string sqlUrl = ExerciceMonsterContext.ServerURL;
        public string SqlUrl
        {
            get => sqlUrl; set
            {
                SetProperty(ref sqlUrl, value); OnPropertyChanged(nameof(SqlUrl));
            }
        }
        public SettingsViewVM() : base() { }

    }
}