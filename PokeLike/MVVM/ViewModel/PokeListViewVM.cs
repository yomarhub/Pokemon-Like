using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using PokeLike.Functions;
using PokeLike.Model;

namespace PokeLike.MVVM.ViewModel
{
    internal partial class PokeListViewVM : BaseVM
    {
        #region Variables
        private ObservableCollection<Monster>? pokemons;
        public ObservableCollection<Monster> Pokemons
        {
            get => pokemons ??= []; set
            {
                if (SetProperty(ref pokemons, value))
                {
                    OnPropertyChanged(nameof(Pokemons));
                }
            }
        }

        private Monster? selectedPokemon;
        public Monster? SelectedPokemon
        {
            get => selectedPokemon; set
            {
                if (SetProperty(ref selectedPokemon, value))
                {
                    OnPropertyChanged(nameof(SelectedPokemon));
                }
            }
        }
        private Player? selectedPlayer;
        public Player? SelectedPlayer
        {
            get => selectedPlayer; set
            {
                if (SetProperty(ref selectedPlayer, value))
                {
                    OnPropertyChanged(nameof(SelectedPlayer));
                    OnPropertyChanged(nameof(PlayersMonster));
                }
            }
        }

        private ObservableCollection<Player>? players;
        public ObservableCollection<Player> Players
        {
            get => players ??= []; set
            {
                SetProperty(ref players, value);
                OnPropertyChanged(nameof(Players));
            }
        }

        public Monster? PlayersMonster
        {
            get => SelectedPlayer?.Monsters.FirstOrDefault() ?? new() { Name = "No Monster Selected", Spells = new Spell[4] }; set
            {
                OnPropertyChanged(nameof(PlayersMonster));
            }
        }

        private RelayCommand? changeMonster;
        public ICommand ChangeMonster => changeMonster ??= new RelayCommand(HandleChangeMonster);
        #endregion
        public PokeListViewVM() : base()
        {
            //Pokemons = new(_context.Monsters.Include(m => m.Spells).Take(2));
            //selectedPokemon = Pokemons.First();
            _ = LoadMonsters();
            LoadPlayers();
            selectedPlayer = Players.FirstOrDefault();

            async Task LoadMonsters() => await Task.Run(() =>
            {
                using ExerciceMonsterContext tmpcontext = new();
                Pokemons = new(tmpcontext.Monsters.Include(m => m.Spells));
                selectedPokemon = Pokemons.First();
                MainWindowVM.OnRequestMessage?.Invoke("Monsters loaded");
            });
            //MessageBox.Show(String.Join("\n", selectedPokemon.Spells.Select(s => s.Name)) +
            //    $"\nCount : {selectedPokemon.Spells.Count}");
        }

        private void HandleChangeMonster()
        {
            if (SelectedPokemon == null) return;
            Player p = _context.Players.First(p => p == selectedPlayer);
            p.Monsters.Clear();
            p.Monsters.Add(SelectedPokemon);
            _context.SaveChanges();
            PlayersMonster = null;
        }

        private ObservableCollection<Player> LoadPlayers(ExerciceMonsterContext? _context = null)
        {
            _context ??= this._context;
            Players = new(_context.Players.Include(p => p.Monsters).Where(p => Session.User == null || p.LoginId == Session.User!.Id));
            return Players;
        }
        #region addremove Monster
        /*
        private RelayCommand? addMonster;
        public ICommand AddMonster => addMonster ??= new RelayCommand(HandleAddMonster);

        private RelayCommand? removeMonster;
        public ICommand RemoveMonster => removeMonster ??= new RelayCommand(HandleRemoveMonster);
        private void HandleAddMonster()
        {
            Player p = _context.Players.First(p => p.Login == Session.User);
            p.Monsters.Add(selectedPokemon);
            _context.SaveChanges();
            PlayersMonster = null;
        }
        private void HandleRemoveMonster()
        {
            Player p = _context.Players.First(p => p.Login == Session.User);
            bool r = p.Monsters.Remove(selectedPokemon);
            //MessageBox.Show(r ? "Monster removed" : "Monster not removed");
            _context.SaveChanges();
            PlayersMonster = null;
        } */
        #endregion
        #region Dialog

        [RelayCommand]
        private async Task OpenDialog()
        {
            var dialogViewModel = new CreatePlayerVM(Session.User ?? new());
            var dialog = new View.CreatePlayerDialog { DataContext = dialogViewModel };

            dialogViewModel.OnDialogClose = () =>
            {
                DialogHost.Close("RootDialog");
                if (dialogViewModel.CreatedPlayer != null)
                {
                    using ExerciceMonsterContext context = new();
                    Player createdPlayer = dialogViewModel.CreatedPlayer;
                    context.Players.Add(createdPlayer);
                    context.SaveChanges();
                    LoadPlayers(context);
                }
            };

            await DialogHost.Show(dialog, "RootDialog");
        }

        [RelayCommand]
        private void DeletePlayer()
        {
            if (selectedPlayer == null) return;
            Player p = _context.Players.First(p => p == selectedPlayer);
            //Players.Remove(p);
            _context.Players.Remove(p);
            _context.SaveChanges();
            LoadPlayers();
            selectedPlayer = Players?.FirstOrDefault();
        }

        #endregion
    }
}
