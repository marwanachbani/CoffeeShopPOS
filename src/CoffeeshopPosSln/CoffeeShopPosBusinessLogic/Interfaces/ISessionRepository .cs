using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopPosBusinessLogic.Interfaces
{
    public interface ISessionRepository :IRepository<Session>
    {
        Task<Session> GetActiveSessionByServerIdAsync(int serverId);
        Task<IEnumerable<Session>> GetSessionsByDateAsync(DateTime date);
    }
}