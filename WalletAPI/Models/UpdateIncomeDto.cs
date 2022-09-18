using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletAPI.Models
{
    public class UpdateIncomeDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime DayOfTransaction { get; set; }

        public int UserId { get; set; }
        public int MonthID { get; set; }
    }
}
