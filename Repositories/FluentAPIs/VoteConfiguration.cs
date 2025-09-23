using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.FluentAPIs
{
    public class VoteConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasKey(v => v.VoteId);

            builder.HasIndex(v => new { v.UserId, v.FormId })
                   .IsUnique();

            builder.HasOne(v => v.User)
                   .WithMany(u => u.Votes)
                   .HasForeignKey(v => v.UserId);

            builder.HasOne(v => v.Form)
                   .WithMany(f => f.Votes)
                   .HasForeignKey(v => v.FormId);
        }
    }
}
