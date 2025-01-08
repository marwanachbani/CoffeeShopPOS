using System.Configuration;
using System.Data;
using System.Windows;
using CoffeeShop.Data.SqLite.Data;
using CoffeeShop.Data.SqLite.Features;
using CoffeeShopPosUi.Core;
using CoffeeShopPosUi.ViewModels;
using CoffeeShopPosUi.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeShopPosUi;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static ServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            // Seed the database
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
                DataSeeder.Seed(dbContext);
            }
            // Show the login window
            var loginView = new LoginView
            {
                DataContext = ServiceProvider.GetRequiredService<LoginViewModel>()
            };
            loginView.Show();
            var menuView = new MenuView
            {
                DataContext = ServiceProvider.GetRequiredService<MenuViewModel>()
            };
            menuView.Show();
            var registerView = new RegisterView
            {
                DataContext = ServiceProvider.GetRequiredService<RegisterViewModel>()
            };
            var orderView = new OrderView
            {
                DataContext = ServiceProvider.GetRequiredService<OrderViewModel>()
            };
            orderView.Show();
            registerView.Show();
            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Add DbContext
            
            services.AddSingleton<Messenger>();

            // Register ViewModels
            services.AddScoped<LoginViewModel>();
            services.AddScoped<OrderViewModel>();
            services.AddScoped<MenuViewModel>();
            services.AddScoped<RegisterViewModel>();
            services.AddScoped<IMessenger, Messenger>();            // Add Infrastructure services (repositories, etc.)
            services.AddInfrastructure();
        }
}

