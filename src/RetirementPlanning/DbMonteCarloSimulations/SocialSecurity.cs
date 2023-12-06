using System;
using System.Collections.Generic;

namespace RetirementPlanning.DbMonteCarloSimulations;

public partial class SocialSecurity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime StartYear { get; set; }

    public decimal MonthlyPayout { get; set; }

    public virtual User User { get; set; } = null!;
}
