using CommunityToolkit.Mvvm.Input;

namespace PokeLike.MVVM.ViewModel
{
    public class MainViewVM : BaseVM
    {
        #region Variables
        public RelayCommand RequestDB { get; set; }
        // true = is not animated / false = is animated / null = default
        //public static bool SpellAnim { get; set; } = true;
        #endregion
        public MainViewVM() : base()
        {
            RequestDB = new RelayCommand(HandleRequestDB);
        }

        #region Handlers
        private void HandleRequestDB()
        {
            //Functions.Init.DB();
            //GameData.GetDataFromJson(new Model.ExerciceMonsterContext());
        }
        #endregion
    }
}
