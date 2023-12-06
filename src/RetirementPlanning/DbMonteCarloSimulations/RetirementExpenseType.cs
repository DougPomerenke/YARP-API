using System;
using System.Collections.Generic;

namespace RetirementPlanning.DbMonteCarloSimulations;

public partial class RetirementExpenseType
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<RetirementExpense> RetirementExpenses { get; set; } = new List<RetirementExpense>();
}
