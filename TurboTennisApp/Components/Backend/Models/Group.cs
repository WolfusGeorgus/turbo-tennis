﻿using System;
using System.Collections.Generic;

namespace TurboTennisApp.Components.Backend.Models;

public partial class Group
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? TournamentId { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual Tournament? Tournament { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
