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
        private RelayCommand? changeMonster;
        public ICommand ChangeMonster => changeMonster ??= new RelayCommand(HandleChangeMonster);

        private Visibility canStartGame = Visibility.Collapsed;
        public Visibility CanStartGame
        {
            get => canStartGame; set
            {
                if (SetProperty(ref canStartGame, value))
                {
                    OnPropertyChanged(nameof(CanStartGame));
                }
            }
        }
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
        private Player? selectedPlayer;
        public Player? SelectedPlayer
        {
            get => selectedPlayer; set
            {
                if (SetProperty(ref selectedPlayer, value))
                {
                    OnPropertyChanged(nameof(SelectedPlayer));
                    OnPropertyChanged(nameof(PlayersMonster));
                    Session.CurrentPlayer = SelectedPlayer;
                    Session.CurrentMonster = SelectedPlayer?.Monsters.FirstOrDefault();
                    UpdateCanStartGame();
                }
            }
        }

        private void UpdateCanStartGame()
        {
            CanStartGame = (Session.User != null && Session.CurrentPlayer != null && Session.CurrentMonster != null)
                ? Visibility.Visible : Visibility.Collapsed;
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

        private readonly Monster _defaultMonster = new() { Name = "No Monster", Spells = new Spell[4] };
        public Monster? PlayersMonster
        {
            get => SelectedPlayer?.Monsters.FirstOrDefault() ?? _defaultMonster;
            set
            {
                OnPropertyChanged(nameof(PlayersMonster));
                Session.CurrentMonster = PlayersMonster;
                UpdateCanStartGame();
            }
        }
        #endregion

        public PokeListViewVM() : base() { }

        private void HandleChangeMonster()
        {
            if (SelectedPlayer is null || SelectedMonster is null) return;
            Player p = _context.Players.Include(p => p.Monsters).ThenInclude(m => m.Spells).First(p => p.Id == SelectedPlayer.Id);
            p.Monsters.Clear();
            p.Monsters.Add(_context.Monsters.Include(m => m.Spells).First(m => m.Id == SelectedMonster.Id));
            _context.SaveChanges();
            PlayersMonster = p.Monsters.First();
            Session.CurrentMonster = (PlayersMonster != _defaultMonster) ? PlayersMonster : null;
            UpdateCanStartGame();
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
        public override void OnShowView()
        {
            base.OnShowView();
            _ = LoadMonsters();
            LoadPlayers();
            SelectedPlayer = Players.FirstOrDefault();

            async Task LoadMonsters() => await Task.Run(() =>
            {
                using ExerciceMonsterContext tmpcontext = new();
                Monsters = new(tmpcontext.Monsters.Include(m => m.Spells));
                SelectedMonster = Monsters.FirstOrDefault();
                MainWindowVM.OnRequestMessage?.Invoke("Monsters loaded");
            });
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

        private ObservableCollection<Player> LoadPlayers(ExerciceMonsterContext? _context = null)
        {
            if (Session.User is null) return [];
            _context ??= this._context;
            Players = new(_context.Players.Include(p => p.Monsters).ThenInclude(m => m.Spells).Where(p => p.LoginId == Session.User.Id));
            return Players;
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
