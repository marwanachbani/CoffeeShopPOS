using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopPosBusinessLogic.Models
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int ServerId { get; set; } // Reference to User (Server)
        public User Server { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public decimal TotalEarnings { get; set; }

        public void AddOrder(Order order)
        {
            Orders.Add(order);
            TotalEarnings += order.TotalAmount;
        }

        public bool IsActive => EndTime == null;
    }
}