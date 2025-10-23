using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.FluentAPIs
{
    public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.HasKey(ug => ug.UserGroupId);

            builder.HasOne(ug => ug.User)
                   .WithMany(u => u.UserGroups)
                   .HasForeignKey(ug => ug.UserId);

            builder.HasOne(ug => ug.Group)
                   .WithMany(g => g.UserGroups)
                   .HasForeignKey(ug => ug.GroupId);
        }
    }
}
