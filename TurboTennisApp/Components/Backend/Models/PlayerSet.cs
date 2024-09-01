using System;
using System.Collections.Generic;

namespace TurboTennisApp.Components.Backend.Models;

public partial class PlayerSet
{
    public int PlayerId { get; set; }

    public int SetId { get; set; }

    public int? Points { get; set; }

    public virtual Player Player { get; set; } = null!;

    public virtual Set Set { get; set; } = null!;
}
