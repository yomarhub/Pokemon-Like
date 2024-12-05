using System.ComponentModel;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using PokeLike.Model;

namespace PokeLike.Functions
{
    public class PlayerMonster
    {
        public int PlayerID { get; set; }
        public int MonsterID { get; set; }
    }
    public class MonsterSpell
    {
        public int MonsterID { get; set; }
        public int SpellID { get; set; }
    }
    public class GameData
    {
        public List<Login> Login { get; set; }
        public List<Player> Player { get; set; }
        public List<Monster> Monster { get; set; }
        public List<Spell> Spell { get; set; }
        public List<PlayerMonster> PlayerMonster { get; set; }
        public List<MonsterSpell> MonsterSpell { get; set; }
        public GameData()
        {
            Login = [];
            Player = [];
            Monster = [];
            Spell = [];
            PlayerMonster = [];
            MonsterSpell = [];
        }

        public static void GetDataFromJson(ExerciceMonsterContext context)
        {
            string fileName = "Ressources\\jsons\\monster.json";
            // read file into a string and deserialize JSON to a type
            string jsonString = File.ReadAllText(fileName);
            // Deserialize Data
            GameData gameData = JsonConvert.DeserializeObject<GameData>(jsonString)!;
            // Display Data
            MessageBox.Show(string.Join("\n", gameData.GameDataToString()));
        }

        public List<string> GameDataToString()
        {
            List<string> datas = new List<string>();
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                string name = descriptor.Name;
                object? value = descriptor.GetValue(this);
                if (value is IEnumerable<object> collection)
                {
                    foreach (var item in collection)
                    {
                        datas.Add($"{name}={item.Details()}");
                    }
                }
                else
                {
                    datas.Add($"{name}={value?.Details()}");
                }
            }

            return datas;
        }
    }
}