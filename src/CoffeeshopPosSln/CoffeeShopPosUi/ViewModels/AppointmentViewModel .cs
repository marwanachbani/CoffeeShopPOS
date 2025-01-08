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
    public class AppointmentViewModel : BaseViewModel
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;

        public ObservableCollection<User> AvailableUsers { get; set; } = new ObservableCollection<User>();
        public ObservableCollection<Appointment> Appointments { get; set; } = new ObservableCollection<Appointment>();
        public Appointment CurrentAppointment { get; set; } = new Appointment();

        public ICommand AssignRoleCommand { get; }
        public ICommand LoadAppointmentsCommand { get; }

        public AppointmentViewModel(IMessenger messenger, IAppointmentRepository appointmentRepository, IUserRepository userRepository) 
            : base(messenger)
        {
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;

            AssignRoleCommand = new RelayCommand(async (obj) => await AssignRoleAsync());
            LoadAppointmentsCommand = new RelayCommand(async (obj) => await LoadAppointmentsAsync());
        }

        private async Task AssignRoleAsync()
        {
            if (CurrentAppointment.UserId == 0 || string.IsNullOrWhiteSpace(CurrentAppointment.Role))
            {
                SendMessage("AppointmentError", "Invalid assignment details.");
                return;
            }

            CurrentAppointment.AppointmentDate = DateTime.Now.Date;
            await _appointmentRepository.AddAsync(CurrentAppointment);
            Appointments.Add(CurrentAppointment);

            CurrentAppointment = new Appointment();
            OnPropertyChanged(nameof(CurrentAppointment));
        }

        private async Task LoadAppointmentsAsync()
        {
            var appointments = await _appointmentRepository.GetAppointmentsByDateAsync(DateTime.Now.Date);
            Appointments.Clear();

            foreach (var appointment in appointments)
            {
                Appointments.Add(appointment);
            }
        }
    }
}