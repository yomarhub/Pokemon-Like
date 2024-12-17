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
        #endregion
        public SpellListViewVM() : base() { }

        public override void OnShowView()
        {
            base.OnShowView();
            MainWindowVM.OnRequestMessage?.Invoke("Loading...");
            _ = LoadSpells();

            async Task LoadSpells() => await Task.Run(() =>
            {
                Spells = new(_context.Spells.Include(m => m.Monsters));
                selectedSpell = Spells.First();

                MainWindowVM.OnRequestClearMessages?.Invoke();
                MainWindowVM.OnRequestMessage?.Invoke("Spell loaded");
            });
        }
    }
}
