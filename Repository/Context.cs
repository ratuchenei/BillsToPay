using Microsoft.EntityFrameworkCore;
using Models;

namespace Repository
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public Context()
        {
        }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<InterestRule> Interest { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bill>()
                .Property(x => x.ValueOriginal)
                .HasColumnType("decimal(10,3)");

            modelBuilder.Entity<InterestRule>()
                .Property(x => x.InterestPerDay)
                .HasColumnType("decimal(10,3)");

            modelBuilder.Entity<InterestRule>()
                .Property(x => x.Penalty)
                .HasColumnType("decimal(10,3)");
        }
    }
}
