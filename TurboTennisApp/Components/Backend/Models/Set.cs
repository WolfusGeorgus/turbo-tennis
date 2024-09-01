using System;
using System.Collections.Generic;

namespace TurboTennisApp.Components.Backend.Models;

public partial class Set
{
    public int Id { get; set; }

    public int? Setnumber { get; set; }

    public int? GameId { get; set; }

    public virtual Game? Game { get; set; }

    public virtual ICollection<PlayerSet> PlayerSets { get; set; } = new List<PlayerSet>();
}
