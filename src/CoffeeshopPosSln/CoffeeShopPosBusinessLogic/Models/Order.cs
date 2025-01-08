using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopPosBusinessLogic.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        
    }
}