using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Klika.Dinero.Database.Migrations
{
    public partial class dinero_add_dateOfTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfTransaction",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(15), new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(373) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(4039), new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(4043) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(4054), new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(4055) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(4061), new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(4061) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(4067), new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(4068) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(4074), new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(4075) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(4080), new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(4081) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(4086), new DateTime(2021, 8, 3, 8, 41, 3, 298, DateTimeKind.Utc).AddTicks(4087) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8292), new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8300) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8341), new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8342) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8348), new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8349) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8355), new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8355) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8361), new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8362) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8368), new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8369) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8376), new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8376) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "Keywords", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8382), "PLATA;TOPLI;PREVOZ;UPLATA", new DateTime(2021, 8, 3, 8, 41, 3, 299, DateTimeKind.Utc).AddTicks(8383) });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId_DateOfTransaction",
                table: "Transactions",
                columns: new[] { "AccountId", "DateOfTransaction" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_AccountId_DateOfTransaction",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "DateOfTransaction",
                table: "Transactions");

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(7458), new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(7792) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8082), new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8084) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8086), new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8087) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8088), new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8089) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8090), new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8091) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8092), new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8093) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8094), new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8095) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8096), new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8097) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(1992), new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2316) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2604), new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2607) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2609), new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2610) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2611), new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2616) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2617), new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2618) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2619), new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2620) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2624), new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2625) });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "Keywords", "UpdatedAt" },
                values: new object[] { new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2664), "PLATA;TOPL;PREVOZ;UPLATA", new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2665) });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");
        }
    }
}
