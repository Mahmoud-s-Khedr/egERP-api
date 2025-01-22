using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EG_ERP.Migrations
{
    /// <inheritdoc />
    public partial class customerupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Customers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Customers",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Customers",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Uuid" },
                values: new object[] { "b9bc38d3-04cc-4379-8e7c-6355c252b4b7", "AQAAAAIAAYagAAAAENtTtvPjUTMTUcmu+orNPvW9eQgskW5NjBDbWnWDX1xU37L84/FPCBdst7gNW9SzvQ==", "47f5ef27-9c4d-4e18-929d-a0046fd99968" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Customers");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Uuid" },
                values: new object[] { "0ee32165-a30a-4e6c-9da6-4c827e8d4b4e", "AQAAAAIAAYagAAAAENQj54JdZU5Lojr7VoSff+QCaK/mS+BSOhs6Dylmcg4h/xxHcnmOUwg1INZsFshalg==", "bf31f00b-ad57-4a29-a235-a9f3501a462a" });
        }
    }
}
