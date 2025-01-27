using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EG_ERP.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceRefressToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiedDate",
                table: "Users",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RefreshTokenExpiedDate", "Uuid" },
                values: new object[] { "71e402b7-e11c-4194-9702-ee95c6a73b40", "AQAAAAIAAYagAAAAEJqiYh2vWtwM9FzXFjPc8V9m5N5ZbL9b+Vsc0MVThsjmpzoDUbhXjSejxb84aCbw7w==", null, "0961f45a-9d8d-4994-8601-daebcbd7ec85" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiedDate",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Uuid" },
                values: new object[] { "ad7ddc1b-f5da-4ed8-8631-3c6b6a52123c", "AQAAAAIAAYagAAAAEEUSJ2ofoUXwGbspMArY5Ug77YTX2LLQSt+aQc/T0hgL3E46QrF2eAxDOLPumb60AA==", "dd276067-9c28-4a27-a480-6d59a8f01906" });
        }
    }
}
