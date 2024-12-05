using System.Windows;
using PokeLike.Model;

namespace PokeLike.Functions
{
    public class DBLogic
    {
        static ExerciceMonsterContext _context = new ExerciceMonsterContext();
        public DBLogic()
        {
            MessageBox.Show(_context.Database.EnsureCreated().ToString());
        }
        public static void RecreateDB()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
        public static void Save() => _context.SaveChanges();
        #region User
        public static Login AddUser(Login User)
        {
            _context.Logins.Add(User);
            _context.SaveChanges();
            return _context.Logins.Find(User) ?? User;
        }
        public static Login FindUser(Login User)
        {
            _context.Logins.Find(User);
            _context.SaveChanges();
            return _context.Logins.Find(User) ?? User;
        }

        public static void RemoveUser(Login User)
        {
            _context.Logins.Remove(User);
            _context.SaveChanges();
        }

        public static void UpdateUser(Login User)
        {
            _context.Logins.Update(User);
            _context.SaveChanges();
        }
        #endregion
        #region Monster

        public static void AddMonster(Monster Monster)
        {
            _context.Monsters.Add(Monster);
            _context.SaveChanges();
        }

        public static Monster FindMonster(Monster Monster)
        {
            _context.Monsters.Find(Monster);
            _context.SaveChanges();
            return _context.Monsters.Find(Monster) ?? Monster;
        }

        public static void RemoveMonster(Monster Monster)
        {
            _context.Monsters.Remove(Monster);
            _context.SaveChanges();
        }

        public static void UpdateMonster(Monster Monster)
        {
            _context.Monsters.Update(Monster);
            _context.SaveChanges();
        }
        #endregion
        #region Player
        public static void AddPlayer(Player Player)
        {
            _context.Players.Add(Player);
            _context.SaveChanges();
        }

        public static Player FindPlayer(Player Player)
        {
            _context.Players.Find(Player);
            _context.SaveChanges();
            return _context.Players.Find(Player) ?? Player;
        }

        public static void RemovePlayer(Player Player)
        {
            _context.Players.Remove(Player);
            _context.SaveChanges();
        }

        public static void UpdatePlayer(Player Player)
        {
            _context.Players.Update(Player);
            _context.SaveChanges();
        }
        #endregion
        #region Spell
        public static void AddSpell(Spell Spell)
        {
            _context.Spells.Add(Spell);
            _context.SaveChanges();
        }

        public static void RemoveSpell(Spell Spell)
        {
            _context.Spells.Remove(Spell);
            _context.SaveChanges();
        }

        public static void UpdateSpell(Spell Spell)
        {
            _context.Spells.Update(Spell);
            _context.SaveChanges();
        }

        public static Spell FindSpell(Spell Spell)
        {
            _context.Spells.Find(Spell);
            _context.SaveChanges();
            return _context.Spells.Find(Spell) ?? Spell;
        }
        #endregion
    }
}
