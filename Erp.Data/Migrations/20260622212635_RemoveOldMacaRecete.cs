using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOldMacaRecete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrunMacaReceteleri");

            migrationBuilder.CreateTable(
                name: "MacaReceteleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunId = table.Column<int>(type: "int", nullable: false),
                    MacaAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MacaKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    KullanimAdedi = table.Column<int>(type: "int", nullable: true),
                    MacaCinsi = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    MacaCevrimSuresi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacaReceteleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MacaReceteleri_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MacaReceteleri_UrunId",
                table: "MacaReceteleri",
                column: "UrunId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MacaReceteleri");

            migrationBuilder.CreateTable(
                name: "UrunMacaReceteleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunId = table.Column<int>(type: "int", nullable: false),
                    AminGazi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BezirYagiTuketimi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Co2Tuketimi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KumTuketimi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MacaAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MacaCinsi = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    MacaKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MacaKullanimAdedi = table.Column<int>(type: "int", nullable: true),
                    RecineTuketimi = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
                name: "IX_UrunMacaReceteleri_UrunId",
                table: "UrunMacaReceteleri",
                column: "UrunId");
        }
    }
}
