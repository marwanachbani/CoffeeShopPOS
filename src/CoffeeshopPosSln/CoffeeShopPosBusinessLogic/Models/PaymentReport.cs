using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopPosBusinessLogic.Models
{
    public class PaymentReport
    {
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public decimal TotalAmount { get; set; }
        public string ReportPeriod { get; set; } // Daily, Monthly, Yearly
    }
}