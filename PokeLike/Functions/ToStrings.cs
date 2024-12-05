using PokeLike.Model;

namespace PokeLike.Functions
{
    static class ToStrings
    {
        public static string Details(this object data) => data switch
        {
            Login login => login.Details(),
            Player player => player.Details(),
            Monster monster => monster.Details(),
            Spell spell => spell.Details(),
            PlayerMonster playerMonster => playerMonster.Details(),
            MonsterSpell monsterSpell => monsterSpell.Details(),
            _ => data.ToString() ?? "Unknown object",
        };
        public static string Details(this Login login) =>
            $"Id : {login.Id}, Username : {login.Username}, PasswordHash : {login.PasswordHash}";
        public static string Details(this Player player) =>
            $"Id : {player.Id}, Name : {player.Name}, LoginId : {player.LoginId}, Level : {player.Login}";
        public static string Details(this Monster monster) =>
            $"Id : {monster.Id}, Name : {monster.Name}, Health : {monster.Health}";
        public static string Details(this Spell spell) =>
            $"Id : {spell.Id}, Name : {spell.Name}, Damage : {spell.Damage}, Description : {spell.Description}";
        public static string Details(this PlayerMonster playerMonster) =>
            $"PlayerID : {playerMonster.PlayerID}, MonsterID : {playerMonster.MonsterID}";
        public static string Details(this MonsterSpell monsterSpell) =>
            $"MonsterID : {monsterSpell.MonsterID}, SpellID : {monsterSpell.SpellID}";
    }
}
