using PokeLike.Model;

namespace PokeLike.Functions
{
    public class BattleMonster(Monster m) : Monster(m)
    {
        public int CurrentHP { get; set; }
    }
    public class Battle()
    {

    }
    public static class BattleExtention
    {
        // a = attacker, d = defender
        //public static Action? Death { get; set; }
        public static void RandomAttack(this BattleMonster a, BattleMonster d)
        {
            Random r = new();
            int i = r.Next(a.Spells.Count);
            Attack(a.Spells.ElementAt(i), d);
        }
        public static void Attack(this Spell a, BattleMonster d)
        {
            d.CurrentHP -= (d.CurrentHP > a.Damage) ? a.Damage : d.CurrentHP;
            //if (d.CurrentHP <= 0) { Death?.Invoke(); }
        }
    }
}
