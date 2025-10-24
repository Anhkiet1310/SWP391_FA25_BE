using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.FluentAPIs
{
    public class CarUserConfiguration : IEntityTypeConfiguration<CarUser>
    {
        public void Configure(EntityTypeBuilder<CarUser> builder)
        {
            builder.HasKey(cu => cu.CarUserId);

            builder.HasIndex(cu => new { cu.CarId, cu.UserId })
                   .IsUnique();

            builder.HasOne(cu => cu.Car)
                   .WithMany(c => c.CarUsers)
                   .HasForeignKey(cu => cu.CarId);

            builder.HasOne(cu => cu.User)
                   .WithMany(u => u.CarUsers)
                   .HasForeignKey(cu => cu.UserId);

            builder.HasMany(cu => cu.Schedules)
                   .WithOne(s => s.CarUser)
                   .HasForeignKey(s => s.CarUserId);

            builder.HasMany(cu => cu.Contracts)
                   .WithOne(c => c.CarUser)
                   .HasForeignKey(c => c.CarUserId);

            builder.HasOne(cu => cu.PercentOwnership)
                   .WithOne(po => po.CarUser)
                   .HasForeignKey<PercentOwnership>(po => po.CarUserId);

            builder.HasMany(cu => cu.Transactions)
                   .WithOne(t => t.CarUser)
                   .HasForeignKey(t => t.CarUserId);
        }
    }
}
