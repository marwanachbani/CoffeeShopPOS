using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopPosBusinessLogic.Models;

namespace CoffeeShopPosBusinessLogic.Interfaces
{
    public interface IReportService
    {
        Task<OrderReport> GetOrderReportAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<SessionReport>> GetSessionReportsAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<PaymentReport>> GetPaymentReportsAsync(DateTime startDate, DateTime endDate);
    }
}