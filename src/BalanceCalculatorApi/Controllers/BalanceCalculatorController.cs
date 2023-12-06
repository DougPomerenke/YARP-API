using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetirementPlanning;
using RetirementPlanning.Extensions;
using BalanceCalculatorApi;
using System;
using System.Runtime.CompilerServices;
using BalanceCalculatorApi;

namespace BalanceCalculatorApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BalanceCalculatorController : ControllerBase
    {

private readonly ILogger<BalanceCalculatorController> _logger;

        public BalanceCalculatorController(ILogger<BalanceCalculatorController> logger)
        {
            _logger = logger;
        }

        [HttpPost("/api")]
        public List<AccountBalanceMetrix> GetRun(List<InputParameters> fullParameterList)
        {
            //  Result collection to return
            List<AccountBalanceMetrix> accountbalances = new List<AccountBalanceMetrix>();

            try
            {
                InputParameters startingYearParameters = fullParameterList.FirstOrDefault(c => c.LifeEventType == LifeEventType.StartingYear.ToString());

                int evaluationYear = startingYearParameters.LifeEventStartingYear;
                decimal? startingBalance = startingYearParameters.DecimalValue;

                InputParameters[] inputParameters = fullParameterList.Where(c => c.LifeEventType != "StartingYear").OrderBy(c => c.LifeEventStartingYear).ToArray();

                Tuple<decimal, decimal> annualInflationRateRange = new Tuple<decimal, decimal>(0, 0);
                Tuple<decimal, decimal> annualInvestmentRateOfReturnRange = new Tuple<decimal, decimal>(0, 0);

                Random random = new Random();

                BalanceCalculator balanceCalculator = new BalanceCalculator((decimal)startingBalance);

                balanceCalculator.DesiredRetirementMonthlyIncome = 0;
                balanceCalculator.SocialSecurityMonthlyIncome = 0;

                // Run through 50 years
                for (int yearIndex = 1; yearIndex < 50; yearIndex++)
                {
                    // Account balance and other parameters to return for each year are stored in this.
                    AccountBalanceMetrix accountBalance = new AccountBalanceMetrix();

                    // Check to see if there is a change in the scenario parameters due to a life event.
                    int parameterIndex = 0;

                    while (parameterIndex < inputParameters.Count())
                    {
                        //  Check to see what kind of life event is occurring this year.
                        if (inputParameters[parameterIndex].LifeEventStartingYear == evaluationYear)
                        {

                            try
                            {
                                if (inputParameters[parameterIndex].LifeEventType == LifeEventType.RetirementYear.ToString())
                                {
                                    balanceCalculator.DesiredRetirementMonthlyIncome = (decimal)inputParameters[parameterIndex].DecimalValue;
                                }
                                if (inputParameters[parameterIndex].LifeEventType == LifeEventType.InflationRateChange.ToString())
                                {
                                    annualInflationRateRange = new Tuple<decimal, decimal>(inputParameters[parameterIndex].DecimalArray[0], inputParameters[parameterIndex].DecimalArray[1]);
                                }
                                if (inputParameters[parameterIndex].LifeEventType == LifeEventType.SavingChange.ToString())
                                {
                                    balanceCalculator.AnnualSavingsContribution = (decimal)inputParameters[parameterIndex].DecimalValue;
                                }
                                if (inputParameters[parameterIndex].LifeEventType == LifeEventType.InvestmentYieldChange.ToString())
                                {
                                    annualInvestmentRateOfReturnRange = new Tuple<decimal, decimal>(inputParameters[parameterIndex].DecimalArray[0], inputParameters[parameterIndex].DecimalArray[1]);
                                }
                                if (inputParameters[parameterIndex].LifeEventType == LifeEventType.SocialSecurityPayoutYear.ToString())
                                {
                                    balanceCalculator.SocialSecurityMonthlyIncome = (decimal)inputParameters[parameterIndex].DecimalValue;
                                }
                                if (inputParameters[parameterIndex].LifeEventType == LifeEventType.LoanPayOffYear.ToString())
                                {
                                    balanceCalculator.DesiredRetirementMonthlyIncome = (decimal)balanceCalculator.DesiredRetirementMonthlyIncome - (decimal)inputParameters[parameterIndex].DecimalValue;
                                }
                            }                               
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.Message);
                            }
                        }

                        parameterIndex++;   // Done with these parameters.
                    }

                    try
                    {
                        // These two parameters are different every year in this model.
                        balanceCalculator.AnnualInvestmentRateOfReturn = random.NextInRange(annualInvestmentRateOfReturnRange);
                        balanceCalculator.AnnualInflationRate = random.NextInRange(annualInflationRateRange);

                        decimal newBalance = balanceCalculator.RunYear();

                        // Collect balance parameters for this year's run.
                        accountBalance.CurrentYear = evaluationYear;
                        accountBalance.StartingBalance = balanceCalculator.StartingBalance;
                        accountBalance.EndingBalance = balanceCalculator.EndingBalance;
                        accountBalance.AnnualInflationRate = balanceCalculator.AnnualInflationRate;
                        accountBalance.AnnualInvestmentRateOfReturn = balanceCalculator.AnnualInvestmentRateOfReturn;
                        accountBalance.AnnualSavingsChange = balanceCalculator.AnnualSavingsChange;
                        accountBalance.AnnualSavingsContribution = balanceCalculator.AnnualSavingsContribution;
                        accountBalance.DesiredMonthlyIncome = balanceCalculator.DesiredRetirementMonthlyIncome;
                        accountBalance.SocialSecurityMonthlyIncome = balanceCalculator.SocialSecurityMonthlyIncome;
                        accountBalance.AnnualInvestmentChange = balanceCalculator.AnnualInvestmentGainLoss;

                        accountbalances.Add(accountBalance);

                        if (newBalance <= 0)  // Ran out of money.
                        {
                            break;
                        }
                        else
                        {
                            evaluationYear++;
                            //Console.WriteLine($"Year: {evaluationYear}, Age: {age}, - Balance: {balanceCalculator.SavingsBalance.ToUSDollar()}, - AnnualSavingsContribution: {balanceCalculator.AnnualSavingsContribution.ToUSDollar()}, - MonthlyIncome: {balanceCalculator.DesiredMonthlyIncome.ToUSDollar()}, -  SocialSecurityMonthlyIncome: {balanceCalculator.SocialSecurityMonthlyIncome.ToUSDollar()}, -  AnnualSavingsChange: {balanceCalculator.AnnualSavingsChange.ToUSDollar()}, -  AnnualInvestmentChange: {balanceCalculator.AnnualInvestmentChange.ToUSDollar()}, -  AnnualInflationRate: {balanceCalculator.AnnualInflationRate.ToPercent()}, -  AnnualInvestmentRateOfReturn: {balanceCalculator.AnnualInvestmentRateOfReturn.ToPercent()}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }

                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return accountbalances;
        }
    }
}
