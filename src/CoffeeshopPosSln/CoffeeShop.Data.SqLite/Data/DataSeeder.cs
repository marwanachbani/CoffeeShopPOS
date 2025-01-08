using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopPosBusinessLogic.Models;

namespace CoffeeShop.Data.SqLite.Data
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Username = "admin", PasswordHash = "admin123", Role = "Admin" },
                    new User { Username = "cashier", PasswordHash = "cashier123", Role = "Cashier" }
                );
            }

            context.SaveChanges();
        }
    }
}
