using System;
using System.Collections.Generic;

namespace TurboTennisApp.Components.Backend.Models;

public partial class Tournament
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? PhaseId { get; set; }

    public DateOnly? Created { get; set; }

    public DateOnly? Finished { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual Phase? Phase { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
