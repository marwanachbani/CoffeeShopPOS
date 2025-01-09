using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;

namespace CoffeeShopPosUi.Services
{
    public class ReportService : IReportService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IPaymentRepository _paymentRepository;

        public ReportService(IOrderRepository orderRepository, ISessionRepository sessionRepository, IPaymentRepository paymentRepository)
        {
            _orderRepository = orderRepository;
            _sessionRepository = sessionRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<OrderReport> GetOrderReportAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _orderRepository.GetAllAsync();
            var filteredOrders = orders.Where(o => o.Date >= startDate && o.Date <= endDate);

            return new OrderReport
            {
                TotalOrders = filteredOrders.Count(),
                TotalEarnings = filteredOrders.Sum(o => o.TotalAmount),
                ReportPeriod = $"{startDate.ToShortDateString()} - {endDate.ToShortDateString()}"
            };
        }

        public async Task<IEnumerable<SessionReport>> GetSessionReportsAsync(DateTime startDate, DateTime endDate)
        {
            var sessions = await _sessionRepository.GetSessionsByDateAsync(startDate);
            return sessions
                .GroupBy(s => s.ServerId)
                .Select(group => new SessionReport
                {
                    ServerId = group.Key,
                    ServerName = group.First().Server.Username,
                    TotalEarnings = group.Sum(s => s.TotalEarnings),
                    TotalSessions = group.Count()
                })
                .ToList();
        }

        public async Task<IEnumerable<PaymentReport>> GetPaymentReportsAsync(DateTime startDate, DateTime endDate)
        {
            var payments = await _paymentRepository.GetPaymentsByDateAsync(startDate);
            return payments
                .GroupBy(p => new { FromUsername = p.FromUser.Username, ToUsername = p.ToUser.Username })
                .Select(group => new PaymentReport
                {
                    FromUser = group.Key.FromUsername,
                    ToUser = group.Key.ToUsername,
                    TotalAmount = group.Sum(p => p.Amount),
                    ReportPeriod = $"{startDate.ToShortDateString()} - {endDate.ToShortDateString()}"
                })
                .ToList();
        }
    }
}