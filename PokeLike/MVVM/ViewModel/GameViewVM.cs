using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using PokeLike.Functions;
using PokeLike.Model;

namespace PokeLike.MVVM.ViewModel
{
    public class GameViewVM : BaseVM
    {
        #region Variables
        public static Action<Spell> Attack { get; set; }
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

        private bool isFighting = true;
        public bool IsFighting { get => isFighting; set { SetProperty(ref isFighting, value); OnPropertyChanged(nameof(IsFighting)); } }

        private ObservableCollection<Monster> AllMonsters;
        private BattleMonster ally;
        public BattleMonster Ally { get => ally; set { SetProperty(ref ally, value); OnPropertyChanged(nameof(Ally)); } }

        private BattleMonster ennemy;
        public BattleMonster Ennemy { get => ennemy; set { SetProperty(ref ennemy, value); OnPropertyChanged(nameof(Ennemy)); } }
        #endregion
        public GameViewVM() : base()
        {
            new List<string>() { "b00", "b01", "b02", "b03", "b04", "b05", "b06", "b07", "b08", "b09" }.ForEach((b) =>
                {
                    ImageBrush found = (ImageBrush)Application.Current.FindResource(b);
                    _backgrounds?.Add(found, b);
                });
            ChangeBack();
            Attack = AttackEnnemy;
            AllMonsters = new(_context.Monsters.Include(m => m.Spells));
            if (Session.User != null && Session.CurrentMonster != null) Ally = new(Session.CurrentMonster);
            else Ally = new(AllMonsters.First()) { Name = "No pokemon has been choosen" };
        }
        public void AttackEnnemy(Spell s)
        {
            s.Attack(Ennemy);
        }
        public void GetNewEnnemy()
        {
            Random rand = new();
            Ennemy = new(AllMonsters.ElementAt(rand.Next(AllMonsters.Count))) { OnDamage = HandleOnDamage };
        }
        public void HandleOnDamage(int health)
        {
            if (health <= 0)
            {
                Ennemy.OnDamage -= HandleOnDamage;
                GetNewEnnemy();
                return;
            }
        }
        private void ChangeBack()
        {
            if (_backgrounds.Count < 1) return;
            Random rand = new();
            CurrentBack = _backgrounds.ElementAt(rand.Next(_backgrounds.Count)).Key;
        }
    }
}
