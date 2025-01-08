using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopPosBusinessLogic.Models;

namespace CoffeeShopPosBusinessLogic.Interfaces
{
    public interface IMenuRepository : IRepository<MenuItem>
    {
        Task<IEnumerable<MenuItem>> GetMenuByCategoryAsync(string category);
    }
}