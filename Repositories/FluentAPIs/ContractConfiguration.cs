using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.FluentAPIs
{
    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.HasKey(c => c.ContractId);

            builder.HasOne(c => c.CarUser)
                   .WithMany(cu => cu.Contracts)
                   .HasForeignKey(c => c.CarUserId);
        }
    }
}
