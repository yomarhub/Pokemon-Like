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
        /*
        <RadialGradientBrush x:Key="Gradient" RadiusX="0.6" RadiusY="10" SpreadMethod="Pad">
            <GradientStop Offset="1" Color="#00FFFF00" />
            <GradientStop Offset="0" Color="Yellow" />
        </RadialGradientBrush>
         */
        private readonly RadialGradientBrush? Gradient;
        #endregion
        public GameViewVM() : base()
        {
            new List<string>() { "b00", "b01", "b02", "b03", "b04", "b05", "b06", "b07", "b08", "b09" }.ForEach((b) =>
                {
                    ImageBrush found = (ImageBrush)Application.Current.FindResource(b);
                    _backgrounds?.Add(found, b);
                });
            ChangeBack();

            /*
            CurrentBack = _backgrounds.ElementAt(9).Key;
            Color CurrentColor = GetBackColor();
            Gradient = new RadialGradientBrush()
            {
                RadiusX = 0.4,
                RadiusY = 10,
                SpreadMethod = GradientSpreadMethod.Pad,
                GradientStops =
                [
                    new GradientStop() { Offset = 1, Color = Colors.Transparent },
                    new GradientStop() { Offset = 0, Color = CurrentColor }
                ]
            };
            MainWindowVM.OnRequestChangeBackground?.Invoke(Gradient);*/
        }
        private void ChangeBack()
        {
            if (_backgrounds.Count < 1) return;
            Random rand = new();
            CurrentBack = _backgrounds.ElementAt(rand.Next(_backgrounds.Count)).Key;
        }
        /*
        private Color GetBackColor()
        {
            Dictionary<string, Color> DictBackColor = new()
            {
                ["b00"] = (Color)ColorConverter.ConvertFromString("#d4d4d4"),
                ["b01"] = (Color)ColorConverter.ConvertFromString("#e0f8e0"),
                ["b02"] = (Color)ColorConverter.ConvertFromString("#f4f4fc"),
                ["b03"] = (Color)ColorConverter.ConvertFromString("#ccb46c"),
                ["b04"] = (Color)ColorConverter.ConvertFromString("#d0e4f4"),
                ["b05"] = (Color)ColorConverter.ConvertFromString("#e3f3f4"),
                ["b06"] = (Color)ColorConverter.ConvertFromString("#eae2c6"),
                ["b07"] = (Color)ColorConverter.ConvertFromString("#ebe3fa"),
                ["b08"] = (Color)ColorConverter.ConvertFromString("#dbebf3"),
                ["b09"] = (Color)ColorConverter.ConvertFromString("#ecebec")
            };
            return DictBackColor[_backgrounds[CurrentBack!]];
        }
        */
        public override void OnShowView()
        {
            base.OnShowView();
            //ChangeWindowSize(new(745, 450));
            Application.Current.MainWindow.Width = 675;
            //ImageBrush img = new(new BitmapImage(new("pack://application:,,,/Ressources/Images/backgrounds/nintendodsCleared.png"))) { Stretch = Stretch.Uniform };
            //MainWindowVM.OnRequestChangeBackground?.Invoke((ImageBrush)Application.Current.FindResource("Nintendo"));
            MainWindowVM.OnRequestChangeBackground?.Invoke(new SolidColorBrush(Colors.White));
        }
        public override void OnHideView()
        {
            base.OnHideView();
            //ChangeWindowSize(new(800, 450));
            MainWindowVM.OnRequestChangeBackground?.Invoke(MainWindowVM.BackgroundImage);
        }
    }
}
