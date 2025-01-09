using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopPosBusinessLogic.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public int FromUserId { get; set; } // Reference to the payer (Server or Manager)
        public User FromUser { get; set; }
        public int ToUserId { get; set; } // Reference to the payee (Manager or Admin)
        public User ToUser { get; set; }
        public decimal Amount { get; set; }
    }
}