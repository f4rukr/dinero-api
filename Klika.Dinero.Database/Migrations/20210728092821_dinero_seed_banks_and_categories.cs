using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Klika.Dinero.Database.Migrations
{
    public partial class dinero_seed_banks_and_categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "TransactionCategories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(7458), "", "UniCredit Bank", new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(7792) },
                    { 2, new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8082), "", "Sberbank Bank", new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8084) },
                    { 3, new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8086), "", "Raiffeisen Bank", new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8087) },
                    { 4, new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8088), "", "Bosna Bank International", new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8089) },
                    { 5, new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8090), "", "Intesa Sanpaolo Bank", new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8091) },
                    { 6, new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8092), "", "NLB Bank", new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8093) },
                    { 7, new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8094), "", "Ziraat Banka", new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8095) },
                    { 8, new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8096), "", "Sparkasse Bank", new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8097) }
                });

            migrationBuilder.InsertData(
                table: "TransactionCategories",
                columns: new[] { "Id", "CreatedAt", "Description", "Keywords", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(1992), "", "", "Other", new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2316) },
                    { 2, new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2604), "", "ECOTOK;GAZPROM;HIFA BENZ;AUTOCESTE", "Vehicle", new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2607) },
                    { 3, new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2609), "", "KREDIT;TRAJNI NALOG;NAPLATA PO KRED.KARTICI", "Loan", new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2610) },
                    { 4, new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2611), "", "APOTEKA;PHARMACY;PHARM", "Pharmacy", new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2616) },
                    { 5, new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2617), "", "NAKNADA;TEKUCI RN KAMATA;PROVIZIJA ZA NALOG;PROVISION", "Bank fee", new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2618) },
                    { 6, new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2619), "", "ATM", "ATM", new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2620) },
                    { 7, new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2624), "", "DOO;KUPOVINA;SHOPPING;BINGO;KONZUM;MERKATOR", "Shopping", new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2625) },
                    { 8, new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2664), "", "PLATA;TOPL;PREVOZ;UPLATA", "Salary", new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2665) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "TransactionCategories");
        }
    }
}
