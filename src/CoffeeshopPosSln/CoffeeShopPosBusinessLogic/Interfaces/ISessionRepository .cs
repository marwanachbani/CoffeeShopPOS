using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
<<<<<<< HEAD
using CoffeeShopPosBusinessLogic.Models;

namespace CoffeeShopPosBusinessLogic.Interfaces
{
    public interface ISessionRepository : IRepository<Session>
=======

namespace CoffeeShopPosBusinessLogic.Interfaces
{
    public interface ISessionRepository :IRepository<Session>
>>>>>>> feature/session-management
    {
        Task<Session> GetActiveSessionByServerIdAsync(int serverId);
        Task<IEnumerable<Session>> GetSessionsByDateAsync(DateTime date);
    }
}