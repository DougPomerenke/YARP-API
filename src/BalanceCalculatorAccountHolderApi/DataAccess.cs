
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BalanceCalculatorAccountHolderApi
{
    public class AccountHoldersContext : DbContext
    {
        public AccountHoldersContext(DbContextOptions<AccountHoldersContext> options) : base(options)
        {
        }

        public DbSet<AccountHolder> AccountHolders { get; set; }
        public DbSet<FinancialEvent> FinancialEvents { get; set; }
        public DbSet<Scenario> ScenarioSets { get; set; }
        public DbSet<SocialSecurityPayout> SocialSecurityPayouts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultContainer("AccountHolder");

            builder.Entity<AccountHolder>()
             .ToContainer(nameof(AccountHolder))
             .HasPartitionKey(c => c.Id);


            builder.Entity<AccountHolder>().OwnsMany(p => p.SocialSecurityPayouts);
            builder.Entity<AccountHolder>().OwnsOne(p => p.Scenario);
            builder.Entity<AccountHolder>().OwnsMany(p => p.FinancialEvents);
        }
    }
}
