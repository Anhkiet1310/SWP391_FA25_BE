using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;
using Repositories.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FluentAPIs
{
    public class MaintenanceConfiguration : IEntityTypeConfiguration<Maintenance>
    {
        public void Configure(EntityTypeBuilder<Maintenance> builder)
        {
            builder.HasKey(m => m.MaintenanceId);

            builder.Property(m => m.MaintenanceType)
                .IsRequired()
                .HasMaxLength(255);  // Giới hạn độ dài cho MaintenanceType

            builder.Property(m => m.Status)
                .IsRequired();  // Status là bắt buộc (enum int)

            builder.Property(m => m.Description)
                .HasMaxLength(1000);  // Giới hạn độ dài cho mô tả công việc

            builder.Property(m => m.Price)
                .HasColumnType("decimal(10,2)");  // Định dạng chi phí bảo dưỡng

            builder.HasOne(m => m.Car)  // Liên kết với bảng Cars
                .WithMany()
                .HasForeignKey(m => m.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(m => m.CarId);  // Tạo chỉ mục trên CarId (nếu cần)

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");  // Mặc định thời gian tạo

            builder.Property(m => m.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");  // Thời gian cập nhật

            builder.Property(m => m.DeleteAt)
                .IsRequired(false);  // Dùng cho soft delete (có thể null)

            builder.HasData(
                new Maintenance
                {
                    MaintenanceId = 1,
                    CarId = 1,
                    MaintenanceType = "Bảo dưỡng định kỳ",
                    MaintenanceDay = DateTime.UtcNow,
                    Status = MaintenanceStatus.DaLenLich, // Enum
                    Description = "Thay dầu và kiểm tra tổng quát",
                    Price = 500.00m,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
