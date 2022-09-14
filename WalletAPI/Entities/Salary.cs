﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletAPI.Entities
{
    public class Salary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public DateTime DayOfTransaction { get; set; }

        public int MonthID { get; set; }
        public virtual Month Month { get; set; }
    }
}
