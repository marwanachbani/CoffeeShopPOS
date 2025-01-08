using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopPosBusinessLogic.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Role { get; set; } // E.g., Server, Manager
        public int UserId { get; set; }
        public User User { get; set; } // Navigation property
    }
}