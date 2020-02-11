using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.API.Model
{
    public class Receipt
    {
        public int ID { get; set; }

        /// <summary>
        /// Receipt timestamp(auto-assigned by backend)
        /// </summary>
        public DateTime ReceiptTimestamp { get; set; }

        /// <summary>
        /// Total price(numeric, sum of total prices of all receipt lines, calculated by backend)
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// A list of receipt lines(at least one)
        /// </summary>
        public List<ReceiptLine> ReceiptLines { get; set; }
    }
}
