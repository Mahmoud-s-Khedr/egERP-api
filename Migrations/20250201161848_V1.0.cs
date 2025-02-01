using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EG_ERP.Migrations
{
    /// <inheritdoc />
    public partial class V10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Uuid" },
                values: new object[] { "8db5c2f3-f9a2-4901-b39a-16d14c721778", "AQAAAAIAAYagAAAAEIX44RJRnrVVbc/IUSdkIsrHUfK52btKcg09xaQ8dtg76iQMDgvUUkE9+zgPKX1Oqg==", "a3cfb077-a9e5-48c8-83a1-c8d85c94a3d0" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiedDate", "SecurityStamp", "TwoFactorEnabled", "UserName", "Uuid", "Vertified" },
                values: new object[] { 2, 0, "7a640305-c58b-4c3e-ac02-43b7cabe89bb", "Admin", "yousef@joo.com", true, false, null, "Admin", "YOUSEF@JOO.COM", "YOUSEF", "AQAAAAIAAYagAAAAEH6FpozgyUdP/7TKMtnRTZQGYsXLye/GIrlub1TWbWwsrws9mesAbTJB+m9boU4Iyg==", null, false, null, null, null, false, "Yousef", "5902cdbd-8d09-4d2a-ae9f-47bad7dd55fd", true });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, 2 });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Uuid" },
                values: new object[] { "71e402b7-e11c-4194-9702-ee95c6a73b40", "AQAAAAIAAYagAAAAEJqiYh2vWtwM9FzXFjPc8V9m5N5ZbL9b+Vsc0MVThsjmpzoDUbhXjSejxb84aCbw7w==", "0961f45a-9d8d-4994-8601-daebcbd7ec85" });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
