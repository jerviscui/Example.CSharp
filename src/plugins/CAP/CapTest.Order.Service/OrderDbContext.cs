using Microsoft.EntityFrameworkCore;

namespace CapTest.Order.Service
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }
    }
}
