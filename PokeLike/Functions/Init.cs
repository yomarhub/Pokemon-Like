using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PokeLike.Model;
using PokeLike.MVVM.ViewModel;

namespace PokeLike.Functions
{
    public class Init
    {
        private static readonly ExerciceMonsterContext _context = new();
        private static bool _toRecreate = true;
        public Init()
        {
            //DB();
            Task.Run(async () =>
            {
                DBLogic.RecreateDB();
                await AddUsers();
                ImportFromJSON();
            });
        }

        private static readonly string path = "Ressources/jsons/";
        private static void ImportFromJSON()
        {
            string loginString = File.ReadAllText(path + "login.json");
            List<Login> Logins = JsonConvert.DeserializeObject<List<Login>>(loginString)!;

            string playerString = File.ReadAllText(path + "player.json");
            List<Player> Players = JsonConvert.DeserializeObject<List<Player>>(playerString)!;

            string spellString = File.ReadAllText(path + "spell.json");
            List<Spell> Spells = JsonConvert.DeserializeObject<List<Spell>>(spellString)!;

            string monsterString = File.ReadAllText(path + "monster.json");
            List<Monster> Monsters = JsonConvert.DeserializeObject<List<Monster>>(monsterString)!;

            string monsterSpellString = File.ReadAllText(path + "monsterSpell.json");
            List<MonsterSpell> MonsterSpells = JsonConvert.DeserializeObject<List<MonsterSpell>>(monsterSpellString)!;

            var strategy = _context.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using var transaction = _context.Database.BeginTransaction();
                InsertWithIds(Monsters);
                InsertWithIds(Spells);

                MonsterSpells.ForEach((MonsterSpell monsterSpell) =>
                {
                    Monster? monster = _context.Monsters.Find(monsterSpell.MonsterID);
                    Spell? spell = _context.Spells.Find(monsterSpell.SpellID);
                    if (spell != null) monster?.Spells.Add(spell);
                    else MessageBox.Show($"spell not found id = {monsterSpell.SpellID}");
                    if (monster == null) MessageBox.Show($"monster not found id = {monsterSpell.MonsterID}");
                });

                _context.SaveChanges();
                transaction.Commit();
            });
            _context.AddRange(Logins);
            _context.AddRange(Players);
            _context.SaveChanges();
        }
        #region InsertWithIds

        private static readonly string[] _possibleTables = ["Login", "Player", "Monster", "Spell"];
        private static void InsertWithIds<T>(List<T> Table) where T : class
        {
            string? TableName = typeof(T).Name;
            if (TableName == null && _possibleTables.Contains(TableName)) return;
            //MessageBox.Show($"{TableName}");
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT " + TableName + " ON");
            _context.AddRange(Table);
            _context.SaveChanges();
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT " + TableName + " OFF");
        }

        #endregion
        private static async Task AddUsers() => await Task.Run(() =>
        {
            Login[] entities = [
                new Login { Username = "Test5", PasswordHash = Hash.CreateHash("test5") },
                new Login { Username = "Test4", PasswordHash = Hash.CreateHash("test4") },
                new Login { Username = "Test3", PasswordHash = Hash.CreateHash("test3") },
                new Login { Username = "Test2", PasswordHash = Hash.CreateHash("test2") },
                new Login { Username = "Test", PasswordHash = Hash.CreateHash("test"), Players=[new(){Name="play"}] },
            ];
            using ExerciceMonsterContext temp_context = new();
            temp_context.Logins.AddRange(entities);
            temp_context.SaveChanges();
            //MessageBox.Show("Users added !");
            MainWindowVM.OnRequestMessage?.Invoke("Users added !");
        });

        public static void DB()
        {
            bool isValid = _context.Database.CanConnect();
            switch (true)
            {
                case true when !isValid:
                    MessageBox.Show("can't connect now !");
                    _context.Database.EnsureCreated();
                    if (_context.Database.CanConnect()) { MessageBox.Show("can connect now !"); _toRecreate = false; }
                    else { MessageBox.Show("still can't connect !"); return; }
                    break;
                case true when _toRecreate:
                    MessageBox.Show("First Time");
                    DBLogic.RecreateDB();
                    _toRecreate = false;
                    CreateFirstUser();
                    break;
                default:
                    Login login = CreateFirstUser();
                    string testpass = "test";
                    MessageBox.Show($"First User : {login.Username}" +
                        //$"\nPassword : {login.PasswordHash}" +
                        $"\nPassword is equal to {testpass} : {Hash.CreateHash(testpass) == login.PasswordHash}");
                    break;
            }
        }

        private static Login CreateFirstUser()
        {
            Login? login = _context.Logins.FirstOrDefault();
            if (login == null)
            {
                login = _context.Logins.Add(new Login { Username = "Test", PasswordHash = Hash.CreateHash("test") }).Entity;
                _context.SaveChanges();
            }
            return login;
        }
    }
}
