using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.FluentAPIs
{
    public class PercentOwnershipConfiguration : IEntityTypeConfiguration<PercentOwnership>
    {
        public void Configure(EntityTypeBuilder<PercentOwnership> builder)
        {
            builder.HasKey(po => po.PercentOwnershipId);

            builder.HasIndex(po => po.CarUserId)
                   .IsUnique();
        }
    }
}
