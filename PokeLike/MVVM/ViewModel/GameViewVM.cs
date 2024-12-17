using System.Windows;
using System.Windows.Media;

namespace PokeLike.MVVM.ViewModel
{
    public class GameViewVM : BaseVM
    {
        #region Variables
        private readonly Dictionary<ImageBrush, string> _backgrounds = [];
        private ImageBrush? currentBack;
        public ImageBrush? CurrentBack
        {
            get => currentBack;
            set
            {
                if (SetProperty(ref currentBack, value))
                {
                    OnPropertyChanged(nameof(CurrentBack));
                }
            }
        }

        private bool isWriting = false;
        public bool IsWriting { get => isWriting; set { SetProperty(ref isWriting, value); OnPropertyChanged(nameof(IsWriting)); } }

        private bool isFighting = true;
        public bool IsFighting { get => isFighting; set { SetProperty(ref isFighting, value); OnPropertyChanged(nameof(IsFighting)); } }
        #endregion
        public GameViewVM() : base()
        {
            new List<string>() { "b00", "b01", "b02", "b03", "b04", "b05", "b06", "b07", "b08", "b09" }.ForEach((b) =>
                {
                    ImageBrush found = (ImageBrush)Application.Current.FindResource(b);
                    _backgrounds?.Add(found, b);
                });
            ChangeBack();
        }
        private void ChangeBack()
        {
            if (_backgrounds.Count < 1) return;
            Random rand = new();
            CurrentBack = _backgrounds.ElementAt(rand.Next(_backgrounds.Count)).Key;
        }
        public override void OnShowView()
        {
            base.OnShowView();
        }
        public override void OnHideView()
        {
            base.OnHideView();
        }
    }
}
