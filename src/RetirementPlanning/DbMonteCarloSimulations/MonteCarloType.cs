using System;
using System.Collections.Generic;

namespace RetirementPlanning.DbMonteCarloSimulations;

public partial class MonteCarloType
{
    public Guid Id { get; set; }

    public string Type { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Simulation> Simulations { get; set; } = new List<Simulation>();
}
