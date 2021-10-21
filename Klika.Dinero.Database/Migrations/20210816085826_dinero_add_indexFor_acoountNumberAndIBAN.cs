using Microsoft.EntityFrameworkCore.Migrations;

namespace Klika.Dinero.Database.Migrations
{
    public partial class dinero_add_indexFor_acoountNumberAndIBAN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IBAN",
                table: "Accounts",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "Accounts",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountNumber",
                table: "Accounts",
                column: "AccountNumber",
                unique: true,
                filter: "[AccountNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_IBAN",
                table: "Accounts",
                column: "IBAN",
                unique: true,
                filter: "[IBAN] IS NOT NULL");

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Keywords" },
                values: new object[] { "ECOTOK;GAZPROM;HIFA;BENZ;AUTOCESTE" });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Keywords" },
                values: new object[] { "KREDIT;NALOG;KRED.KARTICI" });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Keywords" },
                values: new object[] { "NAKNADA;KAMATA;PROVIZIJA;PROVISION" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_AccountNumber",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_IBAN",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "IBAN",
                table: "Accounts",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(34)",
                oldMaxLength: 34,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "Accounts",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Keywords" },
                values: new object[] { "ECOTOK;GAZPROM;HIFA BENZ;AUTOCESTE" });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Keywords" },
                values: new object[] { "KREDIT;TRAJNI NALOG;NAPLATA PO KRED.KARTICI" });

            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Keywords" },
                values: new object[] { "NAKNADA;TEKUCI RN KAMATA;PROVIZIJA ZA NALOG;PROVISION" });
        }
    }
}
