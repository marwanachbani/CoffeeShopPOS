using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopPosBusinessLogic.Models
{
    public class SessionReport
    {
        public int ServerId { get; set; }
        public string ServerName { get; set; }
        public decimal TotalEarnings { get; set; }
        public int TotalSessions { get; set; }
    }
}