using Microsoft.EntityFrameworkCore;

namespace CapTest.Order.Service
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }
    }
}
