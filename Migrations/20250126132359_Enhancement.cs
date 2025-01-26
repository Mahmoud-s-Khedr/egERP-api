using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EG_ERP.Migrations
{
    /// <inheritdoc />
    public partial class Enhancement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollPayments_Payments_PaymentId",
                table: "PayrollPayments");
   
            migrationBuilder.DropIndex(
                name: "IX_PayrollPayments_PaymentId",
                table: "PayrollPayments");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.AlterColumn<string>(
                name: "Uuid",
                table: "Users",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateOnly>(
                name: "HireDate",
                table: "Users",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Vertified",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "PaymentDate",
                table: "Payrolls",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "MoueyState",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Departments",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Departments",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Uuid", "Vertified" },
                values: new object[] { "1671121a-f69d-4385-9bbc-84663cb2b2c9", "AQAAAAIAAYagAAAAEGhjK+TEXGfdZWThLS7r3ZhyFK74WtY8+hhjLC4qf46uJvYbENRiXxrgOwHquF+qrw==", "06e798ce-8692-42d7-91a7-3baf1a4217a0", true });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Uuid",
                table: "Users",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayrollPayments_PaymentId",
                table: "PayrollPayments",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollPayments_Payments_PaymentId",
                table: "PayrollPayments",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.CreateIndex(
                name: "IX_PayrollPayments_PayrollId",
                table: "PayrollPayments",
                column: "PayrollId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Uuid",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_PayrollPayments_PaymentId",
                table: "PayrollPayments");

            migrationBuilder.DropIndex(
                name: "IX_PayrollPayments_PayrollId",
                table: "PayrollPayments");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "HireDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Vertified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "MoueyState",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Departments");

            migrationBuilder.AlterColumn<string>(
                name: "Uuid",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 4, null, "HR", "HR" },
                    { 5, null, "IT", "IT" },
                    { 6, null, "Finance", "FINANCE" },
                    { 7, null, "Sales", "SALES" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Uuid" },
                values: new object[] { "b9bc38d3-04cc-4379-8e7c-6355c252b4b7", "AQAAAAIAAYagAAAAENtTtvPjUTMTUcmu+orNPvW9eQgskW5NjBDbWnWDX1xU37L84/FPCBdst7gNW9SzvQ==", "47f5ef27-9c4d-4e18-929d-a0046fd99968" });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollPayments_PaymentId",
                table: "PayrollPayments",
                column: "PaymentId");
        }
    }
}
