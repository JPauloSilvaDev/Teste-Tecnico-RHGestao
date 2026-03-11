using Microsoft.EntityFrameworkCore;
using Domain.Entities;
namespace Infrastructure.Context
{
    public class ManagementSystemDbContext : DbContext
    {
        public ManagementSystemDbContext(DbContextOptions<ManagementSystemDbContext> options)
                : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        
        public DbSet<Product> Products { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // Aplicar configurações da Fluent API
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ManagementSystemDbContext).Assembly);

            //// One-to-Many: User has many Orders
            //modelBuilder.Entity<Order>()
            //    .HasOne(o => o.Client)
            //    .WithMany(u => u.Orders)
            //    .HasForeignKey(o => o.ClientId)
            //    .OnDelete(DeleteBehavior.Restrict); // Prevents accidental cascading deletes

            //// One-to-Many: Client has many Orders
            //modelBuilder.Entity<Order>()
            //    .HasOne(o => o.Client)
            //    .WithMany(c => c.Orders)
            //    .HasForeignKey(o => o.ClientId);

            //// Many-to-Many: Orders and Products
            //// EF Core will automatically create a shadow join table (OrderProduct)
            //modelBuilder.Entity<Order>()
            //    .HasMany(o => o.Products);
        }
    }
}
