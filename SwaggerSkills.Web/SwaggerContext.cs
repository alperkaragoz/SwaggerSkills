using Microsoft.EntityFrameworkCore;
using SwaggerSkills.Web.Entities;

namespace SwaggerSkills.Web
{
    public class SwaggerContext : DbContext
    {
        public SwaggerContext(DbContextOptions<SwaggerContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable(nameof(Customer));
            base.OnModelCreating(modelBuilder);
        }
    }
}
