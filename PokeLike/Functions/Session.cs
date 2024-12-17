using System.Windows;
using PokeLike.Model;

namespace PokeLike.Functions
{
    public static class Session
    {
        public static Login? User { get; set; }
        public static Player? CurrentPlayer { get; set; }
        public static Monster? CurrentMonster { get; set; }
        public static void Show()
        {
            MessageBox.Show($"User : {Session.User?.Username}" +
                $"\nPlayer : {Session.CurrentPlayer?.Name}" +
                $"\nMonster : {Session.CurrentMonster?.Name}");
        }
    }
}
