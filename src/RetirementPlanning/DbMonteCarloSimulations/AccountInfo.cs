using System;
using System.Collections.Generic;

namespace RetirementPlanning.DbMonteCarloSimulations;

public partial class AccountInfo
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public decimal StartingBalance { get; set; }

    public virtual ICollection<Simulation> Simulations { get; set; } = new List<Simulation>();

    public virtual User User { get; set; } = null!;
}
