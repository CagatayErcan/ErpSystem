using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMRPTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlasimHammaddeOranlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlasimSinifi = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HammaddeKodu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HammaddeAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Oran = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlasimHammaddeOranlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnerjiStandartlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsMerkezi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Birim = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Tuketim = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnerjiStandartlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KalipSarfOranlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KalipCinsi = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SarfKodu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SarfAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Birim = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Miktar = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KalipSarfOranlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MacaSarfOranlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MacaCinsi = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SarfKodu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SarfAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Birim = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Miktar = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacaSarfOranlari", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlasimHammaddeOranlari");

            migrationBuilder.DropTable(
                name: "EnerjiStandartlari");

            migrationBuilder.DropTable(
                name: "KalipSarfOranlari");

            migrationBuilder.DropTable(
                name: "MacaSarfOranlari");
        }
    }
}
