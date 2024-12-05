using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PokeLike.Model;

namespace PokeLike.Functions
{
    public class Init
    {
        private static readonly ExerciceMonsterContext _context = new();
        static bool _toRecreate = true;
        public Init()
        {
            //DB();
            //AddUsers();
            //AddOthers();
            ImportFromJSON();
        }

        private static void ImportFromJSON()
        {
            DBLogic.RecreateDB();
            string path = "Ressources/jsons/";
            string spellString = File.ReadAllText(path + "spell.json");
            string monsterString = File.ReadAllText(path + "monster.json");
            string monsterSpellString = File.ReadAllText(path + "monsterSpell.json");
            List<Spell> Spells = JsonConvert.DeserializeObject<List<Spell>>(spellString)!;
            List<Monster> Monsters = JsonConvert.DeserializeObject<List<Monster>>(monsterString)!;
            List<MonsterSpell> MonsterSpells = JsonConvert.DeserializeObject<List<MonsterSpell>>(monsterSpellString)!;

            var strategy = _context.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using var transaction = _context.Database.BeginTransaction();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Monster ON");
                _context.AddRange(Monsters);
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Monster OFF");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Spell ON");
                _context.AddRange(Spells);
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Spell OFF");

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

            /*
            _context.Database.BeginTransaction();
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Monster ON");
            _context.AddRange(Monsters);
            _context.SaveChanges();
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Monster OFF");

            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Spell ON");
            _context.AddRange(Spells);
            _context.SaveChanges();
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Spell OFF");
            _context.Database.CommitTransaction();

            MonsterSpells.ForEach((MonsterSpell monsterSpell) =>
            {
                //MessageBox.Show(monsterSpell.Details() + $"\n found : {_context.Monsters.Find(monsterSpell.MonsterID)?.Details()}");
                Monster? monster = _context.Monsters.Find(monsterSpell.MonsterID);
                Spell? spell = _context.Spells.Find(monsterSpell.SpellID);
                if (spell != null) monster?.Spells.Add(spell);
                else MessageBox.Show($"spell not found id = {monsterSpell.SpellID}");
                if (monster == null) MessageBox.Show($"monster not foounc id = {monsterSpell.MonsterID}");
            });

            _context.SaveChanges();
            //string? text = null; // string.Join("\n", Spells.Slice(0, 100).ConvertAll<string>(m => m.Details()));
            //MessageBox.Show(text ?? "");
            //Environment.Exit(0);
            /*
            string fileName = "Ressources/jsons/monster.json";
            // read file into a string and deserialize JSON to a type
            string jsonString = File.ReadAllText(fileName);
            // Deserialize Data
            List<Monster> monsters = JsonConvert.DeserializeObject<List<Monster>>(jsonString)!;
            // Display Data
            string text = string.Join("\n", monsters.ConvertAll<string>(m => m.Details()));
            MessageBox.Show(text);*/
        }

        private static void AddOthers()
        {
            (Spell fireball, Spell heal) = AddSpells();
            (Monster goblin, Monster dragon) = AddMonsters();
            //Add the spells to the monsters
            goblin.Spells.Add(fireball);
            dragon.Spells.Add(fireball);
            dragon.Spells.Add(heal);

            _context.SaveChanges();
        }
        private static (Monster, Monster) AddMonsters()
        {
            Monster goblin = _context.Monsters.Add(new() { Name = "Goblin", Health = 100, ImageUrl = "https://example.com/goblin.png" }).Entity;
            Monster dragon = _context.Monsters.Add(new() { Name = "Dragon", Health = 500, ImageUrl = "https://example.com/dragon.png" }).Entity;
            return (goblin, dragon);
        }

        private static (Spell, Spell) AddSpells()
        {
            Spell fireball = _context.Spells.Add(new() { Name = "Fireball", Description = "A fiery projectile", Damage = 50 }).Entity;
            Spell heal = _context.Spells.Add(new() { Name = "Heal", Description = "Restores health", Damage = -30 }).Entity;
            return (fireball, heal);
        }


        private static async void AddUsers() => await Task.Run(() =>
        {
            DBLogic.RecreateDB();
            Login[] entities = [
                new Login { Username = "Test5", PasswordHash = Hash.CreateHash("test5") },
                new Login { Username = "Test4", PasswordHash = Hash.CreateHash("test4") },
                new Login { Username = "Test3", PasswordHash = Hash.CreateHash("test3") },
                new Login { Username = "Test2", PasswordHash = Hash.CreateHash("test2") },
                new Login { Username = "Test", PasswordHash = Hash.CreateHash("test") },
            ];
            _context.Logins.AddRange(entities);
            _context.SaveChanges();
            MessageBox.Show("Users added !");
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
