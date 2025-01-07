using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Data.SqLite.Data;
using CoffeeShop.Data.SqLite.Repositories;
using CoffeeShopPosBusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeShop.Data.SqLite.Features
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Register GenericRepository as the default implementation of IRepository
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=CoffeeShopPOS.db"));
            return services;
              
        }
    }
}