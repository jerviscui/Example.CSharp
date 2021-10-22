using Microsoft.EntityFrameworkCore;

namespace CapTest.Depot.Service
{
    public class DepotDbContext : DbContext
    {
        public DepotDbContext(DbContextOptions<DepotDbContext> options) : base(options)
        {
        }
    }
}
