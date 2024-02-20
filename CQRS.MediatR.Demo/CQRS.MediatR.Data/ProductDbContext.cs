using Microsoft.EntityFrameworkCore;

namespace CQRS.MediatR.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set;}
    }
}
