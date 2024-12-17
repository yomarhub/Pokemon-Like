namespace PokeLike.Model;

public partial class Monster
{
    public Monster() { }
    public Monster(Monster m)
    {
        Id = m.Id;
        Name = m.Name;
        Health = m.Health;
        ImageUrl = m.ImageUrl;
        Players = m.Players;
        Spells = m.Spells;
    }
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Health { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<Player> Players { get; set; } = [];

    public virtual ICollection<Spell> Spells { get; set; } = [];
}
