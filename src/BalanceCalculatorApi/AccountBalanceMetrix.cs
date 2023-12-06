namespace BalanceCalculatorApi
{
    public class AccountBalanceMetrix
    {
        // Data returned by api
        public int CurrentYear { get; set; }
        public decimal StartingBalance { get; set; }
        public decimal EndingBalance { get; set; }
        public decimal AnnualSavingsContribution { get; set; }
        public decimal DesiredMonthlyIncome { get; set; }
        public decimal SocialSecurityMonthlyIncome { get; set; }
        public decimal AnnualSavingsChange { get; set; }
        public decimal AnnualInvestmentChange { get; set; }
        public decimal AnnualInflationRate { get; set; }
        public decimal AnnualInvestmentRateOfReturn { get; set; }

    }

    public class InputParameters
    {
        //  LifeEventTypes

        //"StartingYear"
        //"RetirementYear"
        //"SavingChange"
        //"InvestmentYieldChange"
        //"InflationRateChange"
        //"SocialSecurityPayoutYear"
        //"LoanPayOffYear"

        protected string lifeEventType;

        public string LifeEventType
        {
            get { return this.lifeEventType; }
            set { this.lifeEventType = value; }
        }
        public int LifeEventStartingYear { get; set; }
        public decimal? DecimalValue { get; set; }
        public int? IntValue { get; set; }
        public decimal[]? DecimalArray { get; set; }

        public InputParameters(string lifeEventType)
        {
            this.lifeEventType = lifeEventType;
        }
    }
    public enum LifeEventType
    {
        StartingYear,
        RetirementYear,
        SavingChange,
        InvestmentYieldChange,
        InflationRateChange,
        SocialSecurityPayoutYear,
        LoanPayOffYear
    }
}
