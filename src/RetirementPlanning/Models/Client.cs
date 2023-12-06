using System.Security.Permissions;

namespace RetirementPlanning.Models
{
    public class Client
    {
        public Guid Id { get; set; }

        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;

        public required string DateOfBirth  { get; set; }

        public string AccountName  { get; set; } = string.Empty;

        public required AccountIfo accountIfo { get; set; }

        public required SocialSecurity socialSecurity { get; set; }

    }

    public class AccountIfo
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;

        public decimal StartingBalance { get; set; }
    }

    public class SocialSecurity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string StartYear { get; set; } = string.Empty;

        public decimal MonthlyPayout { get; set; }
    }

    public class Simulation
    {
        public Guid Id { get; set; }

        public Guid AccountIfoId { get; set; }
    }

    public class MonteCarloType
    {
        public Guid Id { get; set; }

    }

}
