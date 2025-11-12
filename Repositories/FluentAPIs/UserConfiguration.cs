using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FluentAPIs
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);

            builder.HasMany(u => u.Votes)
                   .WithOne(v => v.User)
                   .HasForeignKey(v => v.UserId);
            
            builder.HasMany(u => u.CarUsers)
                   .WithOne(cu => cu.User)
                   .HasForeignKey(cu => cu.UserId);

            builder.HasMany(u => u.UserGroups)
                   .WithOne(ug => ug.User)
                   .HasForeignKey(ug => ug.UserId);

            builder.HasMany(u => u.Payments)
                   .WithOne(p => p.User)
                   .HasForeignKey(p => p.UserId);
        }
    }
}
