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
    public class PaymentViewModel : BaseViewModel
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISessionRepository _sessionRepository;

        public ObservableCollection<Payment> Payments { get; set; } = new ObservableCollection<Payment>();
        public Payment CurrentPayment { get; set; } = new Payment();
        public decimal TotalToAdmin { get; set; }

        public ICommand ProcessServerToManagerCommand { get; }
        public ICommand ProcessManagerToAdminCommand { get; }
        public ICommand LoadPaymentsCommand { get; }

        public PaymentViewModel(IMessenger messenger, IPaymentRepository paymentRepository, ISessionRepository sessionRepository)
            : base(messenger)
        {
            _paymentRepository = paymentRepository;
            _sessionRepository = sessionRepository;

            ProcessServerToManagerCommand = new RelayCommand(async (obj) => await ProcessServerToManagerAsync());
            ProcessManagerToAdminCommand = new RelayCommand(async (obj) => await ProcessManagerToAdminAsync());
            LoadPaymentsCommand = new RelayCommand(async (obj) => await LoadPaymentsAsync());
        }

        private async Task ProcessServerToManagerAsync()
        {
            var activeSessions = await _sessionRepository.GetSessionsByDateAsync(DateTime.Now);

            foreach (var session in activeSessions)
            {
                if (!session.IsActive)
                {
                    var payment = new Payment
                    {
                        PaymentDate = DateTime.Now,
                        FromUserId = session.ServerId,
                        ToUserId = CurrentPayment.ToUserId, // Manager ID
                        Amount = session.TotalEarnings
                    };

                    await _paymentRepository.AddAsync(payment);
                    Payments.Add(payment);
                }
            }

            SendMessage("ServerToManagerProcessed", null);
        }

        private async Task ProcessManagerToAdminAsync()
        {
            var totalPayments = await _paymentRepository.GetTotalPaymentsToUserByDateAsync(CurrentPayment.FromUserId, DateTime.Now);

            var payment = new Payment
            {
                PaymentDate = DateTime.Now,
                FromUserId = CurrentPayment.FromUserId, // Manager ID
                ToUserId = CurrentPayment.ToUserId, // Admin ID
                Amount = totalPayments
            };

            await _paymentRepository.AddAsync(payment);
            Payments.Add(payment);

            TotalToAdmin += payment.Amount;

            SendMessage("ManagerToAdminProcessed", null);
        }

        private async Task LoadPaymentsAsync()
        {
            var todayPayments = await _paymentRepository.GetPaymentsByDateAsync(DateTime.Now);
            Payments.Clear();

            foreach (var payment in todayPayments)
            {
                Payments.Add(payment);
            }
        }
    }
}