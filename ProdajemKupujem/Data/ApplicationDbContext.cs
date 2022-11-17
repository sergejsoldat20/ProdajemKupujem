using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProdajemKupujem.Data.Configurations;
using ProdajemKupujem.Models;

namespace ProdajemKupujem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ProdajemKupujem.Models.Product> Product { get; set; }
        public DbSet<ProdajemKupujem.Models.Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            base.OnModelCreating(builder);
        }

        public DbSet<ProdajemKupujem.Models.Comment> Comment { get; set; }
    }

}