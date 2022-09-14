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
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Salary> Salaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<Payment>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(40);

            modelBuilder.Entity<Payment>()
                .Property(p => p.DayOfTransaction)
                .IsRequired();

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .IsRequired();

            modelBuilder.Entity<Salary>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Salary>()
                .Property(p => p.DayOfTransaction)
                .IsRequired();

            modelBuilder.Entity<Salary>()
                .Property(p => p.Amount)
                .IsRequired();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
