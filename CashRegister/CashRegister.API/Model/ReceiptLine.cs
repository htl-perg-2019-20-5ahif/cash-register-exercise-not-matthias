using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.API.Model
{
    public class ReceiptLine
    {
        public int ID { get; set; }

        /// <summary>
        /// Reference to the bought product
        /// </summary>
        public Product BoughtProduct { get; set; }

        /// <summary>
        /// Amount of pieces bought.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Total price(numeric, amount * product's unit price, calculated by backend)
        /// </summary>
        public decimal TotalPrice { get; set; }

    }
}
