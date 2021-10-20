using System;
using CapTest.Order.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CapTest.Order.Host
{
    public class OrderDesignDbContextFactory : IDesignTimeDbContextFactory<OrderDesignDbContext>
    {
        /// <inheritdoc />
        public OrderDesignDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false).Build();

            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseNpgsql(configuration.GetConnectionString(OrderConsts.DbContextConnName)).Options;

            return new OrderDesignDbContext(options);
        }
    }

    public class OrderDesignDbContext : OrderDbContext
    {
        /// <inheritdoc />
        public OrderDesignDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }
    }
}
