using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKalipRecete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KalipReceteleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunId = table.Column<int>(type: "int", nullable: false),
                    StokTipi = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StokAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StokKodu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Miktar = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Birim = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KalipReceteleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KalipReceteleri_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KalipReceteleri_UrunId",
                table: "KalipReceteleri",
                column: "UrunId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KalipReceteleri");
        }
    }
}
