using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CashRegister.API
{
    public class Product
    {
        public int ID { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }
    }
}
