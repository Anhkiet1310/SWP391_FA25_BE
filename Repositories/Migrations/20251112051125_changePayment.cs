using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class changePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<int>(
                name: "CarUserId",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
            name: "UserId",
            table: "Payments",
            type: "int",
            nullable: true, 
            oldClrType: typeof(int),
            oldType: "int");

            migrationBuilder.UpdateData(
                table: "Maintenances",
                keyColumn: "MaintenanceId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "MaintenanceDay" },
                values: new object[] { new DateTime(2025, 11, 12, 5, 11, 24, 801, DateTimeKind.Utc).AddTicks(1775), new DateTime(2025, 11, 12, 5, 11, 24, 801, DateTimeKind.Utc).AddTicks(1764) });

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_CarUsers_CarUserId",
                table: "Transactions",
                column: "CarUserId",
                principalTable: "CarUsers",
                principalColumn: "CarUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_CarUsers_CarUserId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Payments",
                newName: "CarUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                newName: "IX_Payments_CarUserId");

            migrationBuilder.AlterColumn<int>(
                name: "CarUserId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Maintenances",
                keyColumn: "MaintenanceId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "MaintenanceDay" },
                values: new object[] { new DateTime(2025, 11, 9, 17, 52, 52, 756, DateTimeKind.Utc).AddTicks(6726), new DateTime(2025, 11, 9, 17, 52, 52, 756, DateTimeKind.Utc).AddTicks(6721) });

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CarUsers_CarUserId",
                table: "Payments",
                column: "CarUserId",
                principalTable: "CarUsers",
                principalColumn: "CarUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_CarUsers_CarUserId",
                table: "Transactions",
                column: "CarUserId",
                principalTable: "CarUsers",
                principalColumn: "CarUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
