using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.FluentAPIs
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(g => g.GroupId);

            builder.HasIndex(g => g.CarId)
                   .IsUnique();

            builder.HasOne(g => g.Car)
                   .WithOne(c => c.Group)
                   .HasForeignKey<Group>(g => g.CarId);

            builder.HasMany(g => g.Forms)
                   .WithOne(f => f.Group)
                   .HasForeignKey(f => f.GroupId);
        }
    }
}
