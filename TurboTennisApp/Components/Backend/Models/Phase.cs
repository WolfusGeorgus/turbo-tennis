using System;
using System.Collections.Generic;

namespace TurboTennisApp.Components.Backend.Models;

public partial class Phase
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}
