using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.FluentAPIs
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasKey(s => s.ScheduleId);

            builder.HasOne(s => s.CarUser)
                   .WithMany(cu => cu.Schedules)
                   .HasForeignKey(s => s.CarUserId);
        }
    }
}
