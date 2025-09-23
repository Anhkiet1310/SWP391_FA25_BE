using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.FluentAPIs
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.TransactionId);

            builder.HasOne(t => t.CarUser)
                   .WithMany(cu => cu.Transactions)
                   .HasForeignKey(t => t.CarUserId);

            builder.HasOne(t => t.Payment)
                   .WithOne(p => p.Transaction)
                   .HasForeignKey<Payment>(p => p.TransactionId);
        }
    }
}
