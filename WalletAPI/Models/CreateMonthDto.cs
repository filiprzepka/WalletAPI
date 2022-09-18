using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletAPI.Models
{
    public class CreateMonthDto
    {
        [Required]
        [MaxLength(15)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BeginningOfTheMonth { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndOfTheMonth { get; set; }
        public int UserId { get; set; }
    }
}
