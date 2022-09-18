using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletAPI.Entities;

namespace WalletAPI
{
    public class WalletSeeder
    {
        private readonly WalletDBContext _dbContext;

        public WalletSeeder(WalletDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Months.Any())
                {
                    var months = GetMonths();
                    _dbContext.Months.AddRange(months);
                    _dbContext.SaveChanges();
                }
            }
        }
        private IEnumerable<Role> GetRoles() 
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Admin"
                },
            };
            return roles;
        }

        private IEnumerable<Month> GetMonths()
        {
            var months = new List<Month>();
            {
                new Month()
                {
                    Name = "August",
                    BeginningOfTheMonth = new DateTime(2022, 08, 01),
                    EndOfTheMonth = new DateTime(2022, 08, 31),
                    Expenses = new List<Expense>()
                    {
                        new Expense()
                        {
                            Name = "Dentist",
                            Amount = 150,
                            DayOfTransaction = new DateTime(2022,08,15)
                        },
                        new Expense()
                        {
                            Name = "Groceries",
                            Amount = 100,
                            DayOfTransaction = new DateTime(2022,08,20)
                        },
                    },
                    Incomes = new List<Income>()
                    {
                        new Income()
                        {
                            Name = "Job salary",
                            Amount = 4000,
                            DayOfTransaction = new DateTime(2022,08,10)
                        }
                    }
                };
                new Month()
                {
                    Name = "September",
                    BeginningOfTheMonth = new DateTime(2022, 09, 01),
                    EndOfTheMonth = new DateTime(2022, 09, 30),
                    Expenses = new List<Expense>()
                    {
                        new Expense()
                        {
                            Name = "Dentist",
                            Amount = 200,
                            DayOfTransaction = new DateTime(2022,09,15)
                        },
                        new Expense()
                        {
                            Name = "Groceries",
                            Amount = 300,
                            DayOfTransaction = new DateTime(2022,09,20)
                        },
                    },
                    Incomes = new List<Income>()
                    {
                        new Income()
                        {
                            Name = "Job salary",
                            Amount = 4000,
                            DayOfTransaction = new DateTime(2022,09,10)
                        }
                    }
                };
                return months;
            };
            
        }
    }
}
