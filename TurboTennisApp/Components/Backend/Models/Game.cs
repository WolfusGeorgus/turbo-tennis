using System;
using System.Collections.Generic;

namespace TurboTennisApp.Components.Backend.Models;

public partial class Game
{
    public int Id { get; set; }

    public int? PhaseId { get; set; }

    public int? TournamentId { get; set; }

    public int? GroupId { get; set; }

    public int? Order { get; set; }

    public string? Date { get; set; }

    public virtual Group? Group { get; set; }

    public virtual Phase? Phase { get; set; }

    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();

    public virtual Tournament? Tournament { get; set; }
}
