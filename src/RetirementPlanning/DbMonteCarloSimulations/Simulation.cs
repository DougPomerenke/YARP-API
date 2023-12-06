using System;
using System.Collections.Generic;

namespace RetirementPlanning.DbMonteCarloSimulations;

public partial class Simulation
{
    public Guid Id { get; set; }

    public Guid AccountInfoId { get; set; }

    public Guid MonteCarloTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime RetirementYear { get; set; }

    public DateTime EndingYear { get; set; }

    public decimal TargetIncome { get; set; }

    public decimal? AnnualContributionAmount { get; set; }

    public decimal? AnnualContributionEndingYear { get; set; }

    public DateTime? SocialSecurityStartYear { get; set; }

    public decimal? SocialSecurityMonthlyPayout { get; set; }

    public virtual AccountInfo AccountInfo { get; set; } = null!;

    public virtual MonteCarloType MonteCarloType { get; set; } = null!;
}
