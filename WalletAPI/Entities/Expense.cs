using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WalletAPI.Entities
{
    public class Expense
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime DayOfTransaction { get; set; }

        public int UserId { get; set; }
        public int MonthID { get; set; }
        [JsonIgnore]
        public virtual Month Month { get; set; }
    }
}
