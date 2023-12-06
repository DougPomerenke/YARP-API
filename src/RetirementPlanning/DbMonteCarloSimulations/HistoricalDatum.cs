using System;
using System.Collections.Generic;

namespace RetirementPlanning.DbMonteCarloSimulations;

public partial class HistoricalDatum
{
    public Guid Id { get; set; }

    public DateTime Year { get; set; }

    public double AnnualInflationRate { get; set; }

    public double Dow { get; set; }

    public double SandP500 { get; set; }
}
