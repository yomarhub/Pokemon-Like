using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PokeLike.Model;

namespace PokeLike.MVVM.ViewModel
{
    internal partial class SpellListViewVM : BaseVM
    {
        #region Variables
        private ObservableCollection<Spell>? spells;
        public ObservableCollection<Spell> Spells
        {
            get => spells ??= []; set
            {
                if (SetProperty(ref spells, value))
                {
                    OnPropertyChanged(nameof(Spells));
                }
            }
        }

        private Spell? selectedSpell;
        public Spell? SelectedSpell
        {
            get => selectedSpell; set
            {
                if (SetProperty(ref selectedSpell, value))
                {
                    OnPropertyChanged(nameof(SelectedSpell));
                }
            }
        }

        private ObservableCollection<Monster> monstersList;
        public ObservableCollection<Monster> MonstersList { get => monstersList; set { SetProperty(ref monstersList, value); OnPropertyChanged(nameof(MonstersList)); } }

        private string monsterFilter;
        public string MonsterFilter { get => monsterFilter; set { SetProperty(ref monsterFilter, value); OnPropertyChanged(nameof(MonsterFilter)); UpdateMonsterList(); } }

        private void UpdateMonsterList()
        {
            if (string.IsNullOrWhiteSpace(MonsterFilter))
            {
                MonstersList = new(_context.Monsters);
                _ = LoadSpells();
            }
            else
            {
                // Monstres potenciel qui contienne la sous string MonsterFilter dans leurs nom
                MonstersList = new(_context.Monsters.Where(m => m.Name.Contains(MonsterFilter)));
                // Si Un monstre avec le nom MonsterFilter existe sans respect de la case alors mExist != null
                Monster? mExist = MonstersList.FirstOrDefault(m => m.Name.Equals(MonsterFilter, StringComparison.CurrentCultureIgnoreCase));
                // Affiche (les sorts que le monstre MonsterFilter connait)
                // ou (les sorts que les monstres potenciels de MonsterList connaissent)
                Spells = new(_context.Spells.Include(s => s.Monsters)
                    .Where(s => !s.Monsters.All(m => (mExist != null) ? !(m == mExist) : !MonstersList.Contains(m)))
                    );
                SelectedSpell = Spells.FirstOrDefault();
            }
        }
        #endregion
        public SpellListViewVM() : base() { }

        public override void OnShowView()
        {
            base.OnShowView();
            MainWindowVM.OnRequestMessage?.Invoke("Loading...");
            _ = LoadSpells(true);

        }
        private async Task LoadSpells(bool showMessage = false) => await Task.Run(() =>
            {
                Spells = new(_context.Spells.Include(s => s.Monsters));
                SelectedSpell = Spells.First();
                if (showMessage)
                {
                    MainWindowVM.OnRequestClearMessages?.Invoke();
                    MainWindowVM.OnRequestMessage?.Invoke("Spell loaded");
                }
            });
    }
}
