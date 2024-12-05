﻿using System;
using System.Collections.Generic;

namespace PokeLike.Model;

public partial class Monster
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Health { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

    public virtual ICollection<Spell> Spells { get; set; } = new List<Spell>();
}
