using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CosmosEFDemoApi
{

    public class DemoContext : DbContext
    {
        public DemoContext(DbContextOptions<DemoContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultContainer("Orders");

            builder.Entity<Customer>()
             .ToContainer(nameof(Customer))
             .HasPartitionKey(c => c.Id)
             .HasNoDiscriminator();

            builder.Entity<Order>()
             .ToContainer(nameof(Order))
             .HasPartitionKey(o => o.CustomerId)
             .HasNoDiscriminator();

            builder.Entity<Address>()
             .ToContainer(nameof(Address))
             .HasPartitionKey(c => c.CustomerId)
             .HasNoDiscriminator();
        }
    }

    public class Address
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    public class DefaultAddress
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public DefaultAddress() { }

        public DefaultAddress(Address address)
        {
            Street = address.Street;
            City = address.City;
            State = address.State;
            ZipCode = address.ZipCode;
        }
    }


    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid TrackingNumber { get; set; }

        public DefaultAddress ShipToAddress { get; set; }
    }

    public class Customer
    {
        [Key]
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public DefaultAddress? DefaultBillingAddress { get; set; }
        public DefaultAddress? DefaultShippingAddress { get; set; }

        public ICollection<Order>? RecentOrders { get; set; }
    }

}
