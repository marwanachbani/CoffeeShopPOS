using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Data.SqLite.Data;
using CoffeeShop.Data.SqLite.Features;
using CoffeeShop.Data.SqLite.Repositories;
using CoffeeShopPosBusinessLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Sqlite.Infrastructure.Test
{
    public class DepencyInjection
    {
        [Fact]
        public void AddInfrastructure_Should_Register_GenericRepository_As_IRepository()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddInfrastructure();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert
            var genericRepository = serviceProvider.GetService(typeof(IRepository<object>));
            Assert.NotNull(genericRepository);
            Assert.IsType<GenericRepository<object>>(genericRepository);
        }
        [Fact] public void DbContext_Should_Resolve_Correctly() { var serviceCollection = new ServiceCollection(); serviceCollection.AddInfrastructure(); var serviceProvider = serviceCollection.BuildServiceProvider(); var dbContext = serviceProvider.GetService<AppDbContext>(); Assert.NotNull(dbContext); }
    }
}