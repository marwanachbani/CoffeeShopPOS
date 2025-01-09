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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        public SessionRepository(AppDbContext context) : base(context) { }

        public async Task<Session> GetActiveSessionByServerIdAsync(int serverId)
        {
            return await _dbSet
                .Include(s => s.Orders)
                .FirstOrDefaultAsync(s => s.ServerId == serverId && s.EndTime == null);
        }

        public async Task<IEnumerable<Session>> GetSessionsByDateAsync(DateTime date)
        {
            return await _dbSet
                .Include(s => s.Orders)
                .Where(s => s.StartTime.Date == date.Date)
                .ToListAsync();
        }
    }
}