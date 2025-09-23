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
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(c => c.CarId);

            builder.HasMany(c => c.CarDetails)
                   .WithOne(cd => cd.Car)
                   .HasForeignKey(cd => cd.CarId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.CarUsers)
                   .WithOne(cu => cu.Car)
                   .HasForeignKey(cu => cu.CarId);

            builder.HasOne(c => c.Group)
                   .WithOne(g => g.Car)
                   .HasForeignKey<Group>(g => g.CarId);
        }
    }
}
