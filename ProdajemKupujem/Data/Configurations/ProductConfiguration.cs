using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProdajemKupujem.Models;

namespace ProdajemKupujem.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.
             HasMany(p => p.Images).
             WithOne(p => p.Product).
             HasForeignKey(i => i.ProductId).
             OnDelete(DeleteBehavior.NoAction);

            builder.
                HasMany(p => p.Comments).
                WithOne(c => c.Product).
                HasForeignKey(c => c.ProductId).
                OnDelete(DeleteBehavior.NoAction);
        }
    }
}
