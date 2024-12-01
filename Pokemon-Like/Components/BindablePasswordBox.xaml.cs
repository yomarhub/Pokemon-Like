using System.Windows;
using System.Windows.Controls;

namespace Pokemon_Like.Components
{
    /// <summary>
    /// Logique d'interaction pour BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {


        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox), new PropertyMetadata(string.Empty, GetDefault));

        private static void GetDefault(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBox)
            {
                passwordBox.PasswordUpdate();
            }
        }

        public BindablePasswordBox()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = passwordBox.Password;
        }
        private void PasswordUpdate()
        {
            passwordBox.Password = Password;
        }
    }
}
