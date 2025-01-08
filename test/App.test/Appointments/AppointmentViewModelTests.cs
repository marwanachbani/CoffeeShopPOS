using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopPosBusinessLogic.Interfaces;
using CoffeeShopPosBusinessLogic.Models;
using CoffeeShopPosUi.Core;
using CoffeeShopPosUi.ViewModels;
using Moq;

namespace App.test.Appointments
{
    public class AppointmentViewModelTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMessenger> _messengerMock;

        public AppointmentViewModelTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _messengerMock = new Mock<IMessenger>();
        }

        [Fact]
        public async Task AssignRoleCommand_Should_Add_Appointment_And_Reset_CurrentAppointment()
        {
            // Arrange
            var appointment = new Appointment { UserId = 1, Role = "Server", AppointmentDate = DateTime.Now.Date };

            _appointmentRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Appointment>())).Returns(Task.CompletedTask);

            var viewModel = new AppointmentViewModel(_messengerMock.Object, _appointmentRepositoryMock.Object, _userRepositoryMock.Object)
            {
                CurrentAppointment = appointment
            };

            // Act
            viewModel.AssignRoleCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            _appointmentRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Appointment>(a => a.Role == "Server")), Times.Once);
            Assert.NotNull(viewModel.CurrentAppointment);
            Assert.Equal(0, viewModel.CurrentAppointment.UserId);
        }

        [Fact]
        public async Task AssignRoleCommand_Should_Send_Error_Message_When_Assignment_Details_Are_Invalid()
        {
            // Arrange
            var viewModel = new AppointmentViewModel(_messengerMock.Object, _appointmentRepositoryMock.Object, _userRepositoryMock.Object);

            // Act
            viewModel.AssignRoleCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            _messengerMock.Verify(m => m.Send(It.Is<MessageBase>(msg => msg.MessageType == "AppointmentError" && msg.Payload.ToString().Contains("Invalid assignment details."))), Times.Once);
        }

        [Fact]
        public async Task LoadAppointmentsCommand_Should_Load_Appointments_Into_Collection()
        {
            // Arrange
            var appointments = new[]
            {
                new Appointment { UserId = 1, Role = "Server", AppointmentDate = DateTime.Now.Date },
                new Appointment { UserId = 2, Role = "Manager", AppointmentDate = DateTime.Now.Date }
            };

            _appointmentRepositoryMock.Setup(repo => repo.GetAppointmentsByDateAsync(It.IsAny<DateTime>())).ReturnsAsync(appointments);

            var viewModel = new AppointmentViewModel(_messengerMock.Object, _appointmentRepositoryMock.Object, _userRepositoryMock.Object);

            // Act
            viewModel.LoadAppointmentsCommand.Execute(null);
            await Task.CompletedTask;
            // Assert
            Assert.Equal(2, viewModel.Appointments.Count);
            Assert.Contains(viewModel.Appointments, a => a.Role == "Server");
            Assert.Contains(viewModel.Appointments, a => a.Role == "Manager");
        }
    }
}