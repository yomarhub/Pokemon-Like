using PokeLike.Model;

namespace PokeLike.Functions
{
    public class BattleMonster(Monster m) : Monster(m)
    {
        public Action<int>? OnDamage { get; set; }
        public int CurrentHP { get; set; } = m.Health;
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
        public static void Attack(this Spell a, BattleMonster d, bool boosted = false)
        {
            int boostedDamage = (boosted) ? a.Damage * (Session.Score + 1) : a.Damage;
            d.CurrentHP -= (d.CurrentHP > boostedDamage) ? boostedDamage : d.CurrentHP;
            d.OnDamage?.Invoke(boostedDamage);
            //if (d.CurrentHP <= 0) { Death?.Invoke(); }
        }
    }
}
