using Microsoft.EntityFrameworkCore;

namespace CapTest.Order.Service
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; } = null!;

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }
    }
}
