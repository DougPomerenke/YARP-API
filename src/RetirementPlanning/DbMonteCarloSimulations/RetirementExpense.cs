using System;
using System.Collections.Generic;

namespace RetirementPlanning.DbMonteCarloSimulations;

public partial class RetirementExpense
{
    public Guid Id { get; set; }

    public Guid RetirementExpenseTypeId { get; set; }

    public decimal MonthlyAmount { get; set; }

    public DateTime FinalYear { get; set; }

    public virtual RetirementExpenseType RetirementExpenseType { get; set; } = null!;
}
