using PokeLike.Model;

namespace PokeLike.Functions
{
    public static class Session
    {
        public static Login? User { get; set; }
        public static Monster? CurrentMonster { get; set; }
    }
}
