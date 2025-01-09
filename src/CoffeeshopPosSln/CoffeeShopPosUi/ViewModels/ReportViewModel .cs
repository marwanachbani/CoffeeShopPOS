using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;
using CoffeeShopPosUi.Core;

namespace CoffeeShopPosUi.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        private readonly IReportService _reportService;

        public ObservableCollection<OrderReport> OrderReports { get; set; } = new ObservableCollection<OrderReport>();
        public ObservableCollection<SessionReport> SessionReports { get; set; } = new ObservableCollection<SessionReport>();
        public ObservableCollection<PaymentReport> PaymentReports { get; set; } = new ObservableCollection<PaymentReport>();

        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-7);
        public DateTime EndDate { get; set; } = DateTime.Now;

        public ICommand GenerateReportsCommand { get; }

        public ReportViewModel(IMessenger messenger, IReportService reportService) : base(messenger)
        {
            _reportService = reportService;
            GenerateReportsCommand = new RelayCommand(async (obj) => await GenerateReportsAsync());
        }

        private async Task GenerateReportsAsync()
        {
            OrderReports.Clear();
            SessionReports.Clear();
            PaymentReports.Clear();

            var orderReport = await _reportService.GetOrderReportAsync(StartDate, EndDate);
            OrderReports.Add(orderReport);

            var sessionReports = await _reportService.GetSessionReportsAsync(StartDate, EndDate);
            foreach (var report in sessionReports) SessionReports.Add(report);

            var paymentReports = await _reportService.GetPaymentReportsAsync(StartDate, EndDate);
            foreach (var report in paymentReports) PaymentReports.Add(report);

            SendMessage("ReportsGenerated", null);
        }
    }
}