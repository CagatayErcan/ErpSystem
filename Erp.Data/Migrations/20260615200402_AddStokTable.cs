using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStokTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stoklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StokKodu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StokAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AnaGrup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AltGrup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StokTipi = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AnaBirim = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    VarsayilanTedarikci = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AlternatifTedarikci = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SonAlisFiyati = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SatisFiyati = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KdvOrani = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MinimumStok = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KritikStok = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiderKategorisi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaliyetTuru = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MasrafMerkezi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stoklar", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stoklar");
        }
    }
}
