using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletAPI.Entities;

namespace WalletAPI.Models
{
    public class MonthDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BeginningOfTheMonth { get; set; }
        public DateTime EndOfTheMonth { get; set; }

        public virtual List<Income> Incomes { get; set; }
        public virtual List<Expense> Expenses { get; set; }
    }
}
