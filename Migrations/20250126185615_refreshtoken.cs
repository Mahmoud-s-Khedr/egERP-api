using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EG_ERP.Migrations
{
    /// <inheritdoc />
    public partial class refreshtoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RefreshToken", "Uuid", "Vertified" },
                values: new object[] { "ad7ddc1b-f5da-4ed8-8631-3c6b6a52123c", "AQAAAAIAAYagAAAAEEUSJ2ofoUXwGbspMArY5Ug77YTX2LLQSt+aQc/T0hgL3E46QrF2eAxDOLPumb60AA==", null, "dd276067-9c28-4a27-a480-6d59a8f01906", true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Uuid", "Vertified" },
                values: new object[] { "1671121a-f69d-4385-9bbc-84663cb2b2c9", "AQAAAAIAAYagAAAAEGhjK+TEXGfdZWThLS7r3ZhyFK74WtY8+hhjLC4qf46uJvYbENRiXxrgOwHquF+qrw==", "06e798ce-8692-42d7-91a7-3baf1a4217a0", false });
        }
    }
}
