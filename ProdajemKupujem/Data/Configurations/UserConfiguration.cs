using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProdajemKupujem.Models;

namespace ProdajemKupujem.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
             .HasMany(u => u.MessagesRecieved)
             .WithOne(m => m.Receiver)
             .HasForeignKey(m => m.ReceiverId)
             .OnDelete(DeleteBehavior.NoAction);

            builder
              .HasMany(u => u.MessagesSent)
              .WithOne(m => m.Sender)
              .HasForeignKey(m => m.SenderId)
              .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(u => u.Products)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            builder
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);
        }

    }
}
