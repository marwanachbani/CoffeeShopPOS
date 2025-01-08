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
    public class MenuRepository : GenericRepository<MenuItem>, IMenuRepository
    {
        public MenuRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<MenuItem>> GetMenuByCategoryAsync(string category)
        {
            return await _dbSet.Where(m => m.Category == category).ToListAsync();
        }
    }
}