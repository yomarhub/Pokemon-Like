using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PokeLike.Functions;
using PokeLike.Model;

namespace PokeLike.MVVM.ViewModel
{
    public class GameViewVM : BaseVM
    {
        #region Variables
        public static RelayCommand<Spell>? Attack { get; set; }
        private readonly Dictionary<ImageBrush, string> _backgrounds = [];
        private ImageBrush? currentBack;
        public ImageBrush? CurrentBack
        {
            get => currentBack;
            set
            {
                if (SetProperty(ref currentBack, value))
                {
                    OnPropertyChanged(nameof(CurrentBack));
                }
            }
        }

        private bool isWriting = false;
        public bool IsWriting { get => isWriting; set { SetProperty(ref isWriting, value); OnPropertyChanged(nameof(IsWriting)); } }

        private bool canFight = true;
        public bool CanFight { get => canFight; set { SetProperty(ref canFight, value); OnPropertyChanged(nameof(CanFight)); } }

        private ObservableCollection<Monster> AllMonsters;
        private BattleMonster ally;
        public BattleMonster Ally { get => ally; set { SetProperty(ref ally, value); OnPropertyChanged(nameof(Ally)); } }

        private BattleMonster ennemy;
        public BattleMonster Ennemy { get => ennemy; set { SetProperty(ref ennemy, value); OnPropertyChanged(nameof(Ennemy)); } }
        public double AllyHP { get => (double)Ally.CurrentHP / (double)Ally.Health * 100.0; set { } }
        public double EnnemyHP { get => (double)Ennemy.CurrentHP / (double)Ennemy.Health * 100.0; set { } }
        #endregion
        public GameViewVM() : base()
        {
            new List<string>() { "b00", "b01", "b02", "b03", "b04", "b05", "b06", "b07", "b08", "b09" }.ForEach((b) =>
                {
                    ImageBrush found = (ImageBrush)Application.Current.FindResource(b);
                    _backgrounds?.Add(found, b);
                });
            ChangeBack();
            Attack = new(AttackEnnemy!);
            AllMonsters = new(_context.Monsters.Include(m => m.Spells));
            ally = new(Session.CurrentMonster!);
            ally.OnDamage += HandleAllyDamage;
            ennemy = new(AllMonsters.ElementAt(rand.Next(AllMonsters.Count))) { OnDamage = HandleOnDamage };
            MessageBox.Show($"allyHP : {AllyHP}" +
                $"\nennemyHP : {EnnemyHP}" +
                $"\nAllyPercent : {Ally.CurrentHP / Ally.Health * 100.0}" +
                $"\nEnnemyPercent : {Ennemy.CurrentHP / Ennemy.Health * 100.0}" +
                $"\nAlly.CurrentHP : {Ally.CurrentHP}" +
                $"\n(double)Ennemy.CurrentHP : {(double)Ennemy.CurrentHP}" +
                $"\nAlly.Health : {Ally.Health}");
        }
        public void AttackEnnemy(Spell s)
        {
            if (!CanFight) return;
            if (Ennemy.CurrentHP <= 0) { GetNewEnnemy(); return; }
            CanFight = false;
            s.Attack(Ennemy);
            OnPropertyChanged(nameof(EnnemyHP));
        }
        private readonly Random rand = new();
        public void GetNewEnnemy()
        {
            Ennemy = new(AllMonsters.ElementAt(rand.Next(AllMonsters.Count)));
            Ennemy.OnDamage += HandleOnDamage;
            OnPropertyChanged(nameof(EnnemyHP));
        }

        public async void HandleOnDamage(int health) => await Task.Run(() =>
            {
                if (health <= 0)
                {
                    Ennemy.OnDamage -= HandleOnDamage;
                    OnPropertyChanged(nameof(ViewModel.GameViewVM.EnnemyHP));
                    MessageBox.Show("Ennemy is dead");
                    Session.CurrentScore++;
                    Task.Delay(2000);
                    MessageBox.Show("To generate a new enemy click on any spell");
                }
                else
                {
                    MessageBox.Show($"EnnemyHP : {EnnemyHP}, {Ennemy.CurrentHP}, {Ennemy.Spells.Count}");
                    Spell RandomEnnemySpell = Ennemy.Spells.ElementAt(rand.Next(Ennemy.Spells.Count));
                    RandomEnnemySpell.Attack(Ally, true);
                }
                CanFight = true;
            });

        public void HandleAllyDamage(int health) => MessageBox.Show($"AllyHP : {AllyHP}, {Ally.CurrentHP}, {Ally.Spells.Count}");
        private void ChangeBack()
        {
            if (_backgrounds.Count < 1) return;
            Random rand = new();
            CurrentBack = _backgrounds.ElementAt(rand.Next(_backgrounds.Count)).Key;
        }
    }
}
