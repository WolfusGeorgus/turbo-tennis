using System;
using System.Collections.Generic;

namespace TurboTennisApp.Components.Backend.Models;

public partial class PlayerGame
{
    public int Id { get; set; }

    public int? PlayerId { get; set; }

    public int? GameId { get; set; }

    public int? StatusId { get; set; }

    public virtual Game? Game { get; set; }

    public virtual Player? Player { get; set; }

    public virtual GameStatus? Status { get; set; }
}
