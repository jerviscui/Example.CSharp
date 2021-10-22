using System;
using CapTest.Depot.Service;
using CapTest.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CapTest.Depot.Host
{
    public class OrderDesignDbContextFactory : IDesignTimeDbContextFactory<DepotDesignDbContext>
    {
        /// <inheritdoc />
        public DepotDesignDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false).Build();

            var options = new DbContextOptionsBuilder<DepotDbContext>()
                .UseNpgsql(configuration.GetConnectionString(DepotConsts.DbContextConnName)).Options;

            return new DepotDesignDbContext(options);
        }
    }

    public class DepotDesignDbContext : DepotDbContext
    {
        /// <inheritdoc />
        public DepotDesignDbContext(DbContextOptions<DepotDbContext> options) : base(options)
        {
        }
    }
}
