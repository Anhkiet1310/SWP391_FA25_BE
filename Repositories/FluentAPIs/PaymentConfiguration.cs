using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.FluentAPIs
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.PaymentId);

            builder.HasMany(p => p.Transactions)
                   .WithOne(t => t.Payment)
                   .HasForeignKey(t => t.PaymentId);

            builder.Property(p => p.Status)
                   .HasConversion<int>();
        }
    }
}
