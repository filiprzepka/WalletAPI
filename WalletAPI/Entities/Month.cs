using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletAPI.Entities
{
    public class Month
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BeginningOfTheMonth { get; set; }
        public DateTime EndOfTheMonth { get; set; }

        public virtual List<Payment> Payments { get; set; }

        public virtual List<Salary> Salaries { get; set; }
    }
}
