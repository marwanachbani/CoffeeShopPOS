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
    public class SessionViewModel : BaseViewModel
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IOrderRepository _orderRepository;

        public ObservableCollection<Session> Sessions { get; set; } = new ObservableCollection<Session>();
        public Session CurrentSession { get; set; }

        public ICommand StartSessionCommand { get; }
        public ICommand EndSessionCommand { get; }
        public ICommand LoadSessionsCommand { get; }

        public SessionViewModel(IMessenger messenger, ISessionRepository sessionRepository, IOrderRepository orderRepository)
            : base(messenger)
        {
            _sessionRepository = sessionRepository;
            _orderRepository = orderRepository;

            StartSessionCommand = new RelayCommand(async (obj) => await StartSessionAsync());
            EndSessionCommand = new RelayCommand(async (obj) => await EndSessionAsync());
            LoadSessionsCommand = new RelayCommand(async (obj) => await LoadSessionsAsync());
        }

        private async Task StartSessionAsync()
        {
            if (CurrentSession != null && CurrentSession.IsActive)
            {
                SendMessage("SessionError", "An active session already exists.");
                return;
            }

            var newSession = new Session
            {
                StartTime = DateTime.Now,
                ServerId = CurrentSession.ServerId
            };

            await _sessionRepository.AddAsync(newSession);
            CurrentSession = newSession;
            Sessions.Add(newSession);

            SendMessage("SessionStarted", newSession);
        }

        private async Task EndSessionAsync()
        {
            if (CurrentSession == null || !CurrentSession.IsActive)
            {
                SendMessage("SessionError", "No active session to end.");
                return;
            }

            CurrentSession.EndTime = DateTime.Now;

            foreach (var order in CurrentSession.Orders)
            {
                CurrentSession.TotalEarnings += order.TotalAmount;
            }

            await _sessionRepository.UpdateAsync(CurrentSession);
            SendMessage("SessionEnded", CurrentSession);

            CurrentSession = null;
        }

        private async Task LoadSessionsAsync()
        {
            var todaySessions = await _sessionRepository.GetSessionsByDateAsync(DateTime.Now);
            Sessions.Clear();

            foreach (var session in todaySessions)
            {
                Sessions.Add(session);
            }
        }
    }
}