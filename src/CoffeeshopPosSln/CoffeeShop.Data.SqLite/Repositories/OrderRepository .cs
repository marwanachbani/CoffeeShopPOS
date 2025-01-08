using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Data.SqLite.Data;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Data.SqLite.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Order>> GetOrdersByDateAsync(DateTime date)
        {
            return await _dbSet.Include(o => o.OrderDetails)
                .Where(o => o.Date.Date == date.Date)
                .ToListAsync();
        }
    }
}