using System.Windows;

namespace PokeLike.MVVM.ViewModel
{
    public class GameViewVM : BaseVM
    {
        public GameViewVM() : base()
        {
            MainWindowVM.OnRequestChangeSize?.Invoke(new Size(800, 750));
        }

    }
}
