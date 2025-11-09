using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintenanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Maintenances",
                keyColumn: "MaintenanceId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "MaintenanceDay" },
                values: new object[] { new DateTime(2025, 11, 9, 17, 52, 52, 756, DateTimeKind.Utc).AddTicks(6726), new DateTime(2025, 11, 9, 17, 52, 52, 756, DateTimeKind.Utc).AddTicks(6721) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Maintenances",
                keyColumn: "MaintenanceId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "MaintenanceDay" },
                values: new object[] { new DateTime(2025, 11, 9, 17, 52, 28, 552, DateTimeKind.Utc).AddTicks(8125), new DateTime(2025, 11, 9, 17, 52, 28, 552, DateTimeKind.Utc).AddTicks(8120) });
        }
    }
}
