using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMusteriForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_Cariler_MusteriKisaAd",
                table: "Urunler");

            //migrationBuilder.DropIndex(
               // name: "IX_Urunler_MusteriKisaAd",
               // table: "Urunler");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Urunler_MusteriKisaAd",
                table: "Urunler",
                column: "MusteriKisaAd");

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_Cariler_MusteriKisaAd",
                table: "Urunler",
                column: "MusteriKisaAd",
                principalTable: "Cariler",
                principalColumn: "CariKodu");
        }
    }
}
