using Microsoft.EntityFrameworkCore.Migrations;

namespace Klika.Dinero.Database.Migrations
{
    public partial class reverted_trans_categories_and_lowercased_them : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Name" },
                values: new object[] { "Ziraat Bank" });
            
            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Keywords" },
                values: new object[] { "ecotok;gazprom;hifa;benz;autoceste" });

            
            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Keywords" },
                values: new object[] { "kredit;trajni nalog;naplata po kred. kartici" });
            
            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Keywords" },
                values: new object[] { "apoteka;pharmacy;pharm" });
            
            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Keywords" },
                values: new object[] { "naknada;tekuci rn kamata;provizija za nalog;provision" });
            
            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Keywords" },
                values: new object[] { "atm" });
            
            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Keywords" },
                values: new object[] { "doo;kupovina;shopping;bingo;konzum;merkator" });
            
            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Keywords" },
                values: new object[] { "plata;topli;prevoz;uplata" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Name" },
                values: new object[] { "Ziraat Banka" });
            
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
                keyValue: 4,
                columns: new[] { "Keywords" },
                values: new object[] { "APOTEKA;PHARMACY;PHARM" });
            
            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Keywords" },
                values: new object[] { "NAKNADA;KAMATA;PROVIZIJA;PROVISION" });
            
            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Keywords" },
                values: new object[] { "ATM" });
            
            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Keywords" },
                values: new object[] { "DOO;KUPOVINA;SHOPPING;BINGO;KONZUM;MERKATOR" });
            
            migrationBuilder.UpdateData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Keywords" },
                values: new object[] { "PLATA;TOPLI;PREVOZ;UPLATA" });
        }
    }
}
