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
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Payment>> GetPaymentsByDateAsync(DateTime date)
        {
            return await _dbSet
                .Include(p => p.FromUser)
                .Include(p => p.ToUser)
                .Where(p => p.PaymentDate.Date == date.Date)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPaymentsToUserByDateAsync(int toUserId, DateTime date)
        {
            return await _dbSet
                .Where(p => p.ToUserId == toUserId && p.PaymentDate.Date == date.Date)
                .SumAsync(p => p.Amount);
        }
    }
}