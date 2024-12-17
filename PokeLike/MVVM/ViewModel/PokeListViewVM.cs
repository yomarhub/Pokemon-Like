using System.Collections.ObjectModel;
using System.Windows;
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
        private ObservableCollection<Monster>? monsters;
        public ObservableCollection<Monster> Monsters
        {
            get => monsters ??= []; set
            {
                if (SetProperty(ref monsters, value))
                {
                    OnPropertyChanged(nameof(Monsters));
                }
            }
        }

        private Monster? selectedMonster;
        public Monster? SelectedMonster
        {
            get => selectedMonster; set
            {
                if (SetProperty(ref selectedMonster, value))
                {
                    OnPropertyChanged(nameof(SelectedMonster));
                }
            }
        }
        public static Visibility CanStartGame
        {
            get => (Session.User != null
                && Session.CurrentPlayer != null
                && Session.CurrentMonster != null) ? Visibility.Visible : Visibility.Collapsed;
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
                    OnPropertyChanged(nameof(CanStartGame));
                    Session.Show();
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

        private Monster _defaultMonster = new() { Name = "No Monster Selected", Spells = new Spell[4] };
        public Monster? PlayersMonster
        {
            get => SelectedPlayer?.Monsters.FirstOrDefault() ?? _defaultMonster;
            set
            {
                Session.CurrentMonster = value;
                OnPropertyChanged(nameof(PlayersMonster));
                OnPropertyChanged(nameof(CanStartGame));
                Session.Show();
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
            SelectedPlayer = Players.FirstOrDefault();

            async Task LoadMonsters() => await Task.Run(() =>
            {
                using ExerciceMonsterContext tmpcontext = new();
                Monsters = new(tmpcontext.Monsters.Include(m => m.Spells));
                SelectedMonster = Monsters.First();
                MessageBox.Show($"{string.Join(", ", SelectedMonster.Spells.Select(s => s.Name))} spells loaded");
                MainWindowVM.OnRequestMessage?.Invoke("Monsters loaded");
            });
            //MessageBox.Show(String.Join("\n", selectedPokemon.Spells.Select(s => s.Name)) +
            //    $"\nCount : {selectedPokemon.Spells.Count}");
        }

        private void HandleChangeMonster()
        {
            if (SelectedMonster is null || SelectedPlayer is null) return;
            Player p = _context.Players.Include(p => p.Monsters).First(p => p.Id == SelectedPlayer.Id);
            p.Monsters.Clear();
            p.Monsters.Add(_context.Monsters.First(m => m.Id == SelectedMonster.Id));
            _context.SaveChanges();
            PlayersMonster = p.Monsters.First();
        }
        /*
        private void HandleChangeMonster()
        {
            if (SelectedMonster is null || SelectedPlayer is null) return;
            Player p = _context.Players.Include(p => p.Monsters).First(p => p.Id == SelectedPlayer.Id);
            p.Monsters.Clear();
            p.Monsters.Add(_context.Monsters.First(m => m.Id == SelectedMonster.Id));
            _context.SaveChanges();
            PlayersMonster = selectedMonster;
        }*/

        private ObservableCollection<Player> LoadPlayers(ExerciceMonsterContext? _context = null)
        {
            _context ??= this._context;
            Players = new(_context.Players.Include(p => p.Monsters).Where(p => Session.User == null || p.LoginId == Session.User!.Id));
            return Players;
        }

        public override void OnHideView()
        {
            base.OnHideView();
            if (Session.User != null && SelectedPlayer != null)
            {
                Player p = _context.Players.Include(p => p.Monsters).First(p => p == SelectedPlayer);
                if (p.Monsters.Count == 0) { MainWindowVM.OnRequestMessage?.Invoke("Current player doesn't have a Monster"); return; }
                Session.CurrentPlayer = p;
                Session.CurrentMonster = p.Monsters.First();
            }
        }

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
            Player p = _context.Players.Include(p => p.Monsters).First(p => p == selectedPlayer);
            p.Monsters.Clear();
            _context.Players.Remove(p);
            _context.SaveChanges();
            LoadPlayers();
            selectedPlayer = Players?.FirstOrDefault();
        }

        #endregion
    }
}
