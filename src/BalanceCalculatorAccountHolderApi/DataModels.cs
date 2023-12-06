using System.ComponentModel.DataAnnotations;

namespace BalanceCalculatorAccountHolderApi
{
    public class AccountHolder
    {
        [Key]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }

        public decimal AccountStaringBalance { get; set; }

        // Schedule from the Social Security Administration that one receives shortly before the age of 65
        public List<SocialSecurityPayout> SocialSecurityPayouts { get; set; }

        public Scenario Scenario { get; set; }

        public List<FinancialEvent> FinancialEvents { get; set; }
    }

    public class Scenario
    {
        public string Name { get; set; }
        //  Age when retiree stops pre-retirement monthly savings contributions and starts expected monthly retirement income
        public int RetirementAge { get; set; }

        public decimal ExpectedMonthlyRetirementIncome { get; set; }
        public decimal PreRetirementMonthlySavingsContribution { get; set; }

        // Selected from the SocialSecurityPayout list to be used in simulation runs
        public int SocialSecurityPayoutAge { get; set; }
    }
    public class FinancialEvent
    {
        public string Type { get; set; }
        public int Year { get; set; }
        public decimal[] Payload { get; set; }
    }
    public class SocialSecurityPayout
    {
        public int StartingAge { get; set; }
        public decimal MonthlyPayout { get; set; }
    }


}
