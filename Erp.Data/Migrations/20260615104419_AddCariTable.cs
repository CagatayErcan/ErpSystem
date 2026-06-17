using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCariTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cariler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CariKodu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CariUnvani = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CariKisaAd = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CariTipi = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Durumu = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    VergiDairesi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VergiNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TCKN = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    MersisNo = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    TicaretSicilNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    YetkiliKisi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EPosta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WebSitesi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Ulke = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Il = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ilce = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostaKodu = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Adres = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SatisTemsilcisi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Bolge = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sektor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DovizTuru = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    RiskLimiti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cariler", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cariler");
        }
    }
}
