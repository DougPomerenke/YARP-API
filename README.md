# YARP - Yet Another Retirement Planner

## Summary
YARP is an application for running Monte Carlo simulations to help one make financial decisions regarding retirement. The application is developed on the Microsoft .NET stack. The UI is Blazor, the API is .NET Core, and currently, the Cosmso Db emulator is used for saving the account holders financial data.

### Architecture

![image](https://github.com/DougPomerenke/YARP-API/assets/141588660/e5e61eeb-8631-4a38-b199-4f91812f7c8d)

### Usage
Data inputs are current account balance, retirement age, social security payout age, monthly contributions till retirement, and monthly retirement income. The simulation runs over a 50 year timeframe, or until the account balance goes negative. The age of the account holder when this happens is saved in a statistics component. As more iterations of the simulation are run, the resulting age is added to the history component. The statistical data is displayed. Currently, the minus one sigma value is used as an indicator for a successful plan.

The balance calculations are done in the web API. It is up to code in the UI to set parameters and handle results. The API returns account balance numbers for every year of run. This is displayed by the UI. If more than one simulation is run, the results of the last iteration are displayed. 

For each year of a simulation run, values for inflation and rate of return are randomly generated within a range determined by the input parameters.

### Installation

Currently, there is no installer for yarp. It requires a Windows development environment and some proficiency in Visual Studio and the .NET framework.

OS:
Windows 10 Pro

Platform:
.NET 8

Development Environment:
Microsoft Visual Studio Community 2022 (64-bit) - Preview Version 17.9.0 Preview 1.1 (Or later)

Cosmsos Db Emulator:
https://learn.microsoft.com/en-us/azure/cosmos-db/how-to-develop-emulator?tabs=windows%2Ccsharp&pivots=api-mongodb  (Link valid, 1/9/2024)

Dependencies:
Microsoft.EntityFrameworkCore.Cosmos  (API solution)
Blazor.Bootstrap  (UI solution)

Get your copy of the source code for both projects, YARP-UI and YARP-API
Download and install the Cosmos DB emulation. Follow the link

