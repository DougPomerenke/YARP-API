using RetirementPlanning;
using RetirementPlanning.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// See https://aka.ms/new-console-template for more information

//StartYear MonthlyPayout
//65 2720.00
//66 2944.00
//66.66 3099.00
//68 3281.00
//69 3549.00
//70 3986.00

decimal startingBalance = 555000;
Tuple<decimal, decimal> annualInflationRateRange = new Tuple<decimal, decimal>(.01m, .06m);
Tuple<decimal, decimal> agressiveAnnualInvestmentRateOfReturnRange = new Tuple<decimal, decimal>(-.05m, .08m);
Tuple<decimal, decimal> moderateAnnualInvestmentRateOfReturnRange = new Tuple<decimal, decimal>(-.02m, .04m);
Tuple<decimal, decimal> conservativeAnnualInvestmentRateOfReturnRange = new Tuple<decimal, decimal>(.01m, .03m);
decimal annualSavingsContribution = 0;
decimal desiredMonthlyIncome = 4100;
int retirementAge = 66;
decimal socialSecurityMonthlyIncome = 2944; //66
int yearOfBirth = 1958;
int lifeExpectancy =95;
int mortgagePayment = 1680;
int mortgagePaidYear = 2041;

int targetReachedCount = 0;

//  Run 100 iterations

for (int iterations =  1; iterations<100; iterations++)
{
    int evaluationYear = 2023;
    Random random = new Random();

    Tuple<decimal, decimal> investmentRateOfReturnRange = agressiveAnnualInvestmentRateOfReturnRange;

    Console.BackgroundColor = ConsoleColor.DarkBlue;
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
    Console.WriteLine($"Iteration: {iterations}, Balance: {startingBalance.ToUSDollar()}, -  AnnualInflationRateRange: {annualInflationRateRange.Item1.ToPercent()} to {annualInflationRateRange.Item2.ToPercent()}, -  AnnualInvestmentRateOfReturn: {investmentRateOfReturnRange.Item1.ToPercent()} to {investmentRateOfReturnRange.Item2.ToPercent()}, -  SocialSecurityMonthlyIncome: {socialSecurityMonthlyIncome.ToUSDollar()}, - AnnualSavingsContribution: {annualSavingsContribution.ToUSDollar()}, - DesiredMonthlyIncome: {desiredMonthlyIncome.ToUSDollar()}");

    Console.ResetColor();

    // This is where a REST api call could go.

    string path = "https://localhost:7077/BalanceCalculator";
    HttpClient client = new HttpClient();

    HttpResponseMessage response = await client.GetAsync(path);
    AccountBalanceMetrix accountBalanceMetrix = null;
    List<AccountBalanceMetrix> AccountBalanceMetris = new List<AccountBalanceMetrix>();

    if (response.IsSuccessStatusCode)
    {
       // AccountBalanceMetris = await response.Content.ReadAsStreamAsync<AccountBalanceMetrix>();
    }
    else
    {
        Console.WriteLine(response.ToString());
    }


    BalanceCalculator balanceCalculator = new BalanceCalculator(startingBalance);

    //  Run iteration over a time span up to 50 years, or until the life expectancy is met, or until the savings balance goes to zero.

    for (int i = 1; i < 50; i++)
    {
        int age = evaluationYear - yearOfBirth;

        // Changing parameters at retirement
        if (evaluationYear == yearOfBirth + retirementAge)
        {
            investmentRateOfReturnRange = moderateAnnualInvestmentRateOfReturnRange;
            balanceCalculator.DesiredRetirementMonthlyIncome = desiredMonthlyIncome;
            balanceCalculator.SocialSecurityMonthlyIncome = socialSecurityMonthlyIncome;
            balanceCalculator.AnnualSavingsContribution= 0;
            Console.ForegroundColor = ConsoleColor.Blue;
        }
        // Changing parameters when mortgage is paid
        else if (evaluationYear == mortgagePaidYear)
        {
            investmentRateOfReturnRange = conservativeAnnualInvestmentRateOfReturnRange;
            // The current desired income can be reduced by the mortgage payment amount.
            balanceCalculator.DesiredRetirementMonthlyIncome = balanceCalculator.DesiredRetirementMonthlyIncome - mortgagePayment;
            Console.ForegroundColor = ConsoleColor.Blue;
        }

        if(Console.ForegroundColor == ConsoleColor.Blue)
        {
            Console.WriteLine($"SocialSecurityMonthlyIncome: {balanceCalculator.SocialSecurityMonthlyIncome.ToUSDollar()}, - AnnualSavingsContribution: {balanceCalculator.AnnualSavingsContribution.ToUSDollar()}, - DesiredMonthlyIncome: {balanceCalculator.DesiredRetirementMonthlyIncome.ToUSDollar()}, - AnnualInflationRateRange: {annualInflationRateRange.Item1.ToPercent()} to {annualInflationRateRange.Item2.ToPercent()}, -  AnnualInvestmentRateOfReturn: {investmentRateOfReturnRange.Item1.ToPercent()} to {investmentRateOfReturnRange.Item2.ToPercent()}");
            Console.ResetColor();
        }

        //  Set the investment rate of return and inflation rate for this year of the iteration

        balanceCalculator.AnnualInflationRate = random.NextInRange(annualInflationRateRange);
        balanceCalculator.AnnualInvestmentRateOfReturn = random.NextInRange(investmentRateOfReturnRange);

        decimal newBalance = balanceCalculator.RunYear();

        if (newBalance<=0 )  // Ran out of money before life expectancy reached.
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Year: {evaluationYear}, Age: {age}, - Balance: {balanceCalculator.SavingsBalance.ToUSDollar()}, - AnnualSavingsContribution: {balanceCalculator.AnnualSavingsContribution.ToUSDollar()}, - MonthlyIncome: {balanceCalculator.DesiredRetirementMonthlyIncome.ToUSDollar()}, -  SocialSecurityMonthlyIncome: {balanceCalculator.SocialSecurityMonthlyIncome.ToUSDollar()}, -  AnnualSavingsChange: {balanceCalculator.AnnualSavingsChange.ToUSDollar()}, -  AnnualInvestmentChange: {balanceCalculator.AnnualInvestmentChange.ToUSDollar()}, -  AnnualInflationRate: {balanceCalculator.AnnualInflationRate.ToPercent()}, -  AnnualInvestmentRateOfReturn: {balanceCalculator.AnnualInvestmentRateOfReturn.ToPercent()}");
            Console.ResetColor();
            
            break;
        }
        else if(age > lifeExpectancy) // Reached life expectancy with money left over, we're done.
        {
            targetReachedCount++;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Year: {evaluationYear}, Age: {age}, - Balance: {balanceCalculator.SavingsBalance.ToUSDollar()}, - AnnualSavingsContribution: {balanceCalculator.AnnualSavingsContribution.ToUSDollar()}, - MonthlyIncome: {balanceCalculator.DesiredRetirementMonthlyIncome.ToUSDollar()}, -  SocialSecurityMonthlyIncome: {balanceCalculator.SocialSecurityMonthlyIncome.ToUSDollar()}, -  AnnualSavingsChange: {balanceCalculator.AnnualSavingsChange.ToUSDollar()}, -  AnnualInvestmentChange: {balanceCalculator.AnnualInvestmentChange.ToUSDollar()}, -  AnnualInflationRate: {balanceCalculator.AnnualInflationRate.ToPercent()}, -  AnnualInvestmentRateOfReturn: {balanceCalculator.AnnualInvestmentRateOfReturn.ToPercent()}");
            Console.ResetColor();

            break;
        }
        else
        {
            Console.WriteLine($"Year: {evaluationYear}, Age: {age}, - Balance: {balanceCalculator.SavingsBalance.ToUSDollar()}, - AnnualSavingsContribution: {balanceCalculator.AnnualSavingsContribution.ToUSDollar()}, - MonthlyIncome: {balanceCalculator.DesiredRetirementMonthlyIncome.ToUSDollar()}, -  SocialSecurityMonthlyIncome: {balanceCalculator.SocialSecurityMonthlyIncome.ToUSDollar()}, -  AnnualSavingsChange: {balanceCalculator.AnnualSavingsChange.ToUSDollar()}, -  AnnualInvestmentChange: {balanceCalculator.AnnualInvestmentChange.ToUSDollar()}, -  AnnualInflationRate: {balanceCalculator.AnnualInflationRate.ToPercent()}, -  AnnualInvestmentRateOfReturn: {balanceCalculator.AnnualInvestmentRateOfReturn.ToPercent()}");
        }
        evaluationYear++;
    }

    Thread.Sleep(1000);


    // The value in success could be the output of a REST api
    decimal success = (decimal)targetReachedCount / (decimal)iterations;


    Console.WriteLine();
    Console.WriteLine();
    Console.BackgroundColor= ConsoleColor.DarkCyan;
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine($"#####     Success Confidence to reach {lifeExpectancy}: {success.ToPercent()}");
    Console.ResetColor();
    Console.WriteLine();
}


public class AccountBalanceMetrix
{
    // Data returned by api
    public int CurrentYear { get; set; }
    public int CurrentAge { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal AnnualSavingsContribution { get; set; }
    public decimal DesiredMonthlyIncome { get; set; }
    public decimal SocialSecurityMonthlyIncome { get; set; }
    public decimal AnnualSavingsChange { get; set; }
    public decimal AnnualInvestmentChange { get; set; }
    public decimal AnnualInflationRate { get; set; }
    public decimal AnnualInvestmentRateOfReturn { get; set; }

}