using System.Configuration;
using System.Data;
using System.Windows;
using CoffeeShop.Data.SqLite.Features;
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

            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Add DbContext
            

            // Add Infrastructure services (repositories, etc.)
            services.AddInfrastructure();
        }
}

