using CommunityToolkit.Mvvm.Input;

namespace PokeLike.MVVM.ViewModel
{
    public class MainViewVM : BaseVM
    {
        #region Variables
        public RelayCommand RequestDB { get; set; }
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
