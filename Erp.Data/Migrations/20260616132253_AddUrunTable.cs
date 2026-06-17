using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUrunTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Urunler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunKodu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UrunAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Alasim = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AlasimNormu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UrunAgirligi = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    UrunKalipNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    KalipCinsi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UrunParcaAdeti = table.Column<int>(type: "int", nullable: true),
                    KalipCevrimSuresi = table.Column<int>(type: "int", nullable: true),
                    BesleyiciStokKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BesleyiciAdeti = table.Column<int>(type: "int", nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urunler", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Urunler");
        }
    }
}
