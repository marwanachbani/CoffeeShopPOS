using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopPosBusinessLogic.Models;

namespace CoffeeShopPosBusinessLogic.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetPaymentsByDateAsync(DateTime date);
        Task<decimal> GetTotalPaymentsToUserByDateAsync(int toUserId, DateTime date);
    }
}