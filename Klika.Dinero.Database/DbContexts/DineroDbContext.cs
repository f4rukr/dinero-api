using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityFrameworkCore.Triggers;
using Klika.Dinero.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Klika.Dinero.Database.DbContexts
{
    public class DineroDbContext : DbContextWithTriggers
    {
        public DineroDbContext(DbContextOptions<DineroDbContext> options) : base(options) { }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        /// <summary>
        /// Use this method for FluentAPI configuration.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<Transaction>()
                .HasIndex(p => new { p.AccountId, p.DateOfTransaction })
                .IsUnique(true);

            modelBuilder.Entity<Account>()
                .Property(a => a.CurrencyCode)
                .HasConversion<string>();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.AccountNumber)
                .IsUnique(true);

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.IBAN)
                .IsUnique(true);
        }
        
        public async override Task<Int32> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var changeSet = ChangeTracker.Entries<IEntityBase>();
            if (changeSet != null)
            {
                foreach (var entry in changeSet.Where(c => c.Entity is IEntityBase &&
                                                           (c.State == EntityState.Modified || c.State == EntityState.Added)))
                {
                    if(entry.State == EntityState.Added)
                        entry.Entity.CreatedAt = DateTime.Now;
                    
                    entry.Entity.UpdatedAt = DateTime.Now;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
