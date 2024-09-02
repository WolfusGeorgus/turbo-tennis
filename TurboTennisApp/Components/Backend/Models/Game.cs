using System;
using System.Collections.Generic;
using TurboTennisApp.Components.Backend.Models;

namespace TurboTennisApp.Components.Backend.Models;

public partial class Game
{
    public int Id { get; set; }

    public int? PhaseId { get; set; }

    public int? TournamentId { get; set; }

    public int? GroupId { get; set; }

    public int? Order { get; set; }

    public string? Date { get; set; }

    public int? StatusId { get; set; }

    public virtual Group? Group { get; set; }

    public virtual Phase? Phase { get; set; }

    public virtual ICollection<PlayerGame> PlayerGames { get; set; } = new List<PlayerGame>();

    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();

    public virtual GameStatus? Status { get; set; }

    public virtual Tournament? Tournament { get; set; }
}
