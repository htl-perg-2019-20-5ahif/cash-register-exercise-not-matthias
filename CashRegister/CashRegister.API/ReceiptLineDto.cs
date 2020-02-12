using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.API
{
    public class ReceiptLineDto
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
