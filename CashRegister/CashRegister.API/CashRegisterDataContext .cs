using CashRegister.API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.API
{
    public class CashRegisterDataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptLine> ReceiptLines { get; set; }


        public CashRegisterDataContext (DbContextOptions<CashRegisterDataContext > options) : base(options)
        { }
    }
}
