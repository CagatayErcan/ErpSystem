using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUrunReceteler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrunErgitmeReceteleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunId = table.Column<int>(type: "int", nullable: false),
                    StokKodu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StokAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Miktar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Birim = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BirimElektrikTuketimi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ErgitmeSuresi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunErgitmeReceteleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunErgitmeReceteleri_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UrunIslemeReceteleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunId = table.Column<int>(type: "int", nullable: false),
                    OperasyonSırası = table.Column<int>(type: "int", nullable: true),
                    OperasyonKodu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OperasyonAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsMerkezi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TakimTuketimi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StokKodu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StokAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BirimElektrikTuketimi = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunIslemeReceteleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunIslemeReceteleri_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UrunMacaReceteleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunId = table.Column<int>(type: "int", nullable: false),
                    MacaAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MacaKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MacaKullanimAdedi = table.Column<int>(type: "int", nullable: true),
                    MacaCinsi = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    KumTuketimi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecineTuketimi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Co2Tuketimi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AminGazi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BezirYagiTuketimi = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunMacaReceteleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunMacaReceteleri_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrunErgitmeReceteleri_UrunId",
                table: "UrunErgitmeReceteleri",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunIslemeReceteleri_UrunId",
                table: "UrunIslemeReceteleri",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunMacaReceteleri_UrunId",
                table: "UrunMacaReceteleri",
                column: "UrunId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrunErgitmeReceteleri");

            migrationBuilder.DropTable(
                name: "UrunIslemeReceteleri");

            migrationBuilder.DropTable(
                name: "UrunMacaReceteleri");
        }
    }
}
