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
    public class CarDetailConfiguration : IEntityTypeConfiguration<CarDetail>
    {
        public void Configure(EntityTypeBuilder<CarDetail> builder)
        {
            builder.HasKey(cd => cd.CarDetailId);
        }
    }
}
