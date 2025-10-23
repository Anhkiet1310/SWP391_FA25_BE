using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.FluentAPIs
{
    public class FormConfiguration : IEntityTypeConfiguration<Form>
    {
        public void Configure(EntityTypeBuilder<Form> builder)
        {
            builder.HasKey(f => f.FormId);

            builder.HasMany(f => f.Votes)
                   .WithOne(v => v.Form)
                   .HasForeignKey(v => v.FormId);
        }
    }
}
