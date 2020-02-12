using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.UI
{
    public class ReceiptLineViewModel : BindableBase
    {
        private int productID;
        public int ProductID
        {
            get { return productID; }
            set { SetProperty(ref productID, value); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private int amount;
        public int Amount
        {
            get { return amount; }
            set { SetProperty(ref amount, value); }
        }

        private decimal totalPrice;
        public decimal TotalPrice
        {
            get { return totalPrice; }
            set { SetProperty(ref totalPrice, value); }
        }
    }
}
