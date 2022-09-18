using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletAPI.Entities
{
    public class WalletDBContext : DbContext
    {
        private string _connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=WalletDb;Trusted_Connection=True;";
        public DbSet<Month> Months { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder.Entity<Month>()
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(15);

            modelBuilder.Entity<Month>()
                .Property(m =>m.BeginningOfTheMonth)
                .IsRequired();

            modelBuilder.Entity<Month>()
                .Property(m => m.EndOfTheMonth)
                .IsRequired();

            modelBuilder.Entity<Expense>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(40);

            modelBuilder.Entity<Expense>()
                .Property(p => p.DayOfTransaction)
                .IsRequired();

            modelBuilder.Entity<Expense>()
                .Property(p => p.Amount)
                .IsRequired();

            modelBuilder.Entity<Income>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Income>()
                .Property(p => p.DayOfTransaction)
                .IsRequired();

            modelBuilder.Entity<Income>()
                .Property(p => p.Amount)
                .IsRequired();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
