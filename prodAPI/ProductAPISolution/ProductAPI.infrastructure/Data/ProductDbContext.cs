using Microsoft.EntityFrameworkCore;
using ProductAPI.domain.Entity;
namespace ProductAPI.infrastructure.Data
{
    public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
    }
}
