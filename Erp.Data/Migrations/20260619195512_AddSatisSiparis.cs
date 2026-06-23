using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSatisSiparis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SatisSiparisleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiparisNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SiparisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CariId = table.Column<int>(type: "int", nullable: false),
                    MusteriSiparisNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TerminTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TeslimatAdresi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SatisTemsilcisi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DovizTuru = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Kur = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    OdemeSekli = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TeslimatSekli = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Durum = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SatisSiparisleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SatisSiparisleri_Cariler_CariId",
                        column: x => x.CariId,
                        principalTable: "Cariler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SatisSiparisDetaylari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SatisSiparisId = table.Column<int>(type: "int", nullable: false),
                    StokId = table.Column<int>(type: "int", nullable: false),
                    Miktar = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Birim = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BirimFiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IskontoOrani = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    KdvOrani = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    SatirToplami = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SatisSiparisDetaylari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SatisSiparisDetaylari_SatisSiparisleri_SatisSiparisId",
                        column: x => x.SatisSiparisId,
                        principalTable: "SatisSiparisleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SatisSiparisDetaylari_Stoklar_StokId",
                        column: x => x.StokId,
                        principalTable: "Stoklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SatisSiparisDetaylari_SatisSiparisId",
                table: "SatisSiparisDetaylari",
                column: "SatisSiparisId");

            migrationBuilder.CreateIndex(
                name: "IX_SatisSiparisDetaylari_StokId",
                table: "SatisSiparisDetaylari",
                column: "StokId");

            migrationBuilder.CreateIndex(
                name: "IX_SatisSiparisleri_CariId",
                table: "SatisSiparisleri",
                column: "CariId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SatisSiparisDetaylari");

            migrationBuilder.DropTable(
                name: "SatisSiparisleri");
        }
    }
}
