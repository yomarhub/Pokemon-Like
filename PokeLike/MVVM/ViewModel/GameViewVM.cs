using System.Windows;
using System.Windows.Media;

namespace PokeLike.MVVM.ViewModel
{
    public class GameViewVM : BaseVM
    {

        private List<ImageBrush> _backgrounds { get; set; } = [];
        public GameViewVM() : base()
        {
            new List<string>() { "b00", "b01", "b02", "b03", "b04", "b05", "b06", "b07", "b08", "b09" }.ForEach((b) =>
            {
                _backgrounds?.Append(Application.Current.FindResource(b));
            });
            ChangeWindowSize(new(675, 450));
            ChangeBack();
        }

        private async void ChangeBack()
        {
            ;
        }
    }
}
