using System.Collections.ObjectModel;
using PokeLike.Model;

namespace PokeLike.MVVM.ViewModel
{
    class PokeListViewVM : BaseVM
    {
        public PokeListViewVM() : base()
        {
            pokemons = new(_context.Monsters);
            selectedPokemon = pokemons.First();
        }

        private ObservableCollection<Monster> pokemons;
        public ObservableCollection<Monster> Pokemons
        {
            get => pokemons;
            set
            {
                if (SetProperty(ref pokemons, value))
                {
                    OnPropertyChanged(nameof(Pokemons));
                }
            }
        }

        private Monster selectedPokemon;
        public Monster SelectedPokemon
        {
            get => selectedPokemon;
            set
            {
                if (SetProperty(ref selectedPokemon, value))
                {
                    OnPropertyChanged(nameof(SelectedPokemon));
                }
            }
        }
    }
}
