﻿using System;
using System.Collections.Generic;
using TurboTennisApp.Components.Backend.Models;

namespace TurboTennisApp.Components.Backend.Models;

public partial class Player
{
    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public virtual ICollection<PlayerGame> PlayerGames { get; set; } = new List<PlayerGame>();

    public virtual ICollection<PlayerSet> PlayerSets { get; set; } = new List<PlayerSet>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}
