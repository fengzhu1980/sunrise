using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SunriseContext : DbContext
    {
        public SunriseContext(DbContextOptions<SunriseContext> options) : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}