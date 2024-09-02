using System;
using System.Collections.Generic;

namespace TurboTennisApp.Components.Backend.Models;

public partial class GameStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<PlayerGame> PlayerGames { get; set; } = new List<PlayerGame>();
}
