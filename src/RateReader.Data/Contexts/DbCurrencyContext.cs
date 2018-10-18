using Microsoft.EntityFrameworkCore;
using RateReader.Data.Entities;

namespace RateReader.Data.Contexts
{
    public class DbCurrencyContext : DbContext
    {
        internal DbCurrencyContext(DbContextOptions<DbCurrencyContext> options)
            :base(options)
        {
            
        }

        public DbSet<DbCurrency> Currencies { get; set; }
        public DbSet<DbCurrencyValue> DbCurrencyValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbCurrency>()
                .ToTable("Currencies");
            modelBuilder.Entity<DbCurrency>()
                .HasMany(e => e.Values)
                .WithOne(e => e.Currency)
                .HasForeignKey(e => e.CurrencyId);
            modelBuilder.Entity<DbCurrency>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<DbCurrency>()
                .Property(e => e.CreatedDate)
                .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<DbCurrency>()
                .HasIndex(e => e.Code).IsUnique();

            modelBuilder.Entity<DbCurrencyValue>()
                .ToTable("CurrencyValues");
            modelBuilder.Entity<DbCurrencyValue>()
                .Property(e => e.Rate)
                .HasColumnType("decimal(7,3)");
            modelBuilder.Entity<DbCurrencyValue>()
                .HasIndex(e => e.Id);
            modelBuilder.Entity<DbCurrencyValue>()
                .Property(e => e.CreatedDate)
                .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<DbCurrencyValue>()
                .HasIndex(e => new {e.CurrencyId, e.Date}).IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}