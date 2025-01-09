using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopPosBusinessLogic.Models
{
    public class OrderReport
    {
        public int TotalOrders { get; set; }
        public decimal TotalEarnings { get; set; }
        public string ReportPeriod { get; set; } // Daily, Monthly, Yearly
    }
}