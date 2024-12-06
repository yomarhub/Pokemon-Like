using System.Windows;

namespace PokeLike.MVVM.ViewModel
{
    public class GameViewVM : BaseVM
    {
        public GameViewVM() : base()
        {
            MainWindowVM.OnRequestChangeSize?.Invoke(new Size(675, 450));
        }

    }
}
