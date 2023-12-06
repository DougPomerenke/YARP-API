


namespace RetirementPlanning
{
    public class BalanceCalculator
    {
        decimal _startingBalance = 0;
        decimal _endingBalance = 0;
        decimal _annualInflationRate = 0;
        decimal _annualInvestmentRateOfReturn = 0;
        decimal _socialSecurityMonthlyIncome = 0;
        decimal _annualSavingsContribution = 0;
        decimal _desiredRetirementMonthlyIncome = 0;
        decimal _annualSavingsChange = 0;
        decimal _annualInvestmentGainLoss = 0;
        decimal _annualWithdraws = 0;

        public decimal StartingBalance
        {
            get { return _startingBalance; }
        }
        public decimal EndingBalance
        {
            get { return _endingBalance; }
        }
        public decimal AnnualSavingsChange
        {
            get { return _annualSavingsChange; }
        }
        public decimal AnnualInvestmentGainLoss
        {
            get { return _annualInvestmentGainLoss; }
        }
        public decimal AnnualInflationRate
        {
            set { _annualInflationRate = value; }
            get { return _annualInflationRate; }
        }
        public decimal AnnualInvestmentRateOfReturn
        {
            set { _annualInvestmentRateOfReturn = value; }
            get { return _annualInvestmentRateOfReturn; }
        }

        public decimal AnnualSavingsContribution
        {
            set { _annualSavingsContribution = value; }
            get { return _annualSavingsContribution; }
        }

        public decimal DesiredRetirementMonthlyIncome
        {
            set { _desiredRetirementMonthlyIncome = value; }
            get { return _desiredRetirementMonthlyIncome; }
        }

        public decimal SocialSecurityMonthlyIncome
        {
            set { _socialSecurityMonthlyIncome = value; }
            get { return _socialSecurityMonthlyIncome; }
        }
        public decimal AnnualWithdraws
        {
            set { _annualWithdraws = value; }
            get { return _annualWithdraws; }
        }


        public BalanceCalculator(decimal initialSavingsBalance)
        {
            _startingBalance = initialSavingsBalance;
        }

        public decimal RunYear()
        {
            // Reset these
            _annualSavingsChange = 0;
            _annualInvestmentGainLoss = 0;

            decimal currentBalance = 0;

            if(_endingBalance != 0)
            {
                _startingBalance = _endingBalance;
            }
            //  Track changes to savings before deposits or withdraws.
            _annualInvestmentGainLoss = _startingBalance * _annualInvestmentRateOfReturn;

            _annualWithdraws = 12 * (_desiredRetirementMonthlyIncome - _socialSecurityMonthlyIncome);

            // Deposits are first, then withdrawls, then investment gain or loss
            // 

            _annualSavingsChange =  _annualSavingsContribution - _annualWithdraws;

            currentBalance = _startingBalance + _annualSavingsChange + _annualInvestmentGainLoss;

            //  Update the desired monthly income due to inflation.
            _desiredRetirementMonthlyIncome = _desiredRetirementMonthlyIncome * (1 + _annualInflationRate);

            // Use 3/4 inflation rate for COLA.
            _socialSecurityMonthlyIncome = _socialSecurityMonthlyIncome * (1 + _annualInflationRate * .75m);

            _endingBalance = currentBalance;

            return (currentBalance);
        }

        //public decimal UpdateBalance()
        //{
        //    decimal balanceChangePerMonth = 0;
        //    _annualSavingsReduction = 0;

        //    Deposits are first, then withdrawls


        //    for (int i = 0; i < 12; i++)
        //    {
        //        Update the desired monthly income due to inflation.
        //        _desiredRetirementMonthlyIncome = _desiredRetirementMonthlyIncome * (1 + _annualInflationRate / 12);

        //        Give SS COLA half of inflation rate.
        //       _socialSecurityMonthlyIncome = _socialSecurityMonthlyIncome * (1 + _annualInflationRate / 24);

        //        decimal monthlyWithdraw = _desiredRetirementMonthlyIncome - _socialSecurityMonthlyIncome;
        //        decimal monthyInvestmentGain = (_savingsBalance + (_annualSavingsContribution / 12)) * (_annualInvestmentRateOfReturn / 12);

        //        balanceChangePerMonth = monthyInvestmentGain - monthlyWithdraw;

        //        _savingsBalance = _savingsBalance + balanceChangePerMonth;

        //        _annualSavingsReduction = _annualSavingsReduction + balanceChangePerMonth;

        //        if (_savingsBalance <= 0)
        //        {
        //            break;
        //        }
        //    }
        //    return (_savingsBalance);
        //}
    }
}