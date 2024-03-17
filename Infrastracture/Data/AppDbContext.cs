using Core.Entities;
using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductBrand> ProductBrands { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

    }
}
