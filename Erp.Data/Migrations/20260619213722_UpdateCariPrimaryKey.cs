using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCariPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SatisSiparisleri_Cariler_CariId",
                table: "SatisSiparisleri");

            migrationBuilder.DropIndex(
                name: "IX_SatisSiparisleri_CariId",
                table: "SatisSiparisleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cariler",
                table: "Cariler");

            migrationBuilder.DropColumn(
                name: "CariId",
                table: "SatisSiparisleri");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Cariler");

            migrationBuilder.AddColumn<string>(
                name: "CariKodu",
                table: "SatisSiparisleri",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cariler",
                table: "Cariler",
                column: "CariKodu");

            migrationBuilder.CreateIndex(
                name: "IX_SatisSiparisleri_CariKodu",
                table: "SatisSiparisleri",
                column: "CariKodu");

            migrationBuilder.AddForeignKey(
                name: "FK_SatisSiparisleri_Cariler_CariKodu",
                table: "SatisSiparisleri",
                column: "CariKodu",
                principalTable: "Cariler",
                principalColumn: "CariKodu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SatisSiparisleri_Cariler_CariKodu",
                table: "SatisSiparisleri");

            migrationBuilder.DropIndex(
                name: "IX_SatisSiparisleri_CariKodu",
                table: "SatisSiparisleri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cariler",
                table: "Cariler");

            migrationBuilder.DropColumn(
                name: "CariKodu",
                table: "SatisSiparisleri");

            migrationBuilder.AddColumn<int>(
                name: "CariId",
                table: "SatisSiparisleri",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Cariler",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cariler",
                table: "Cariler",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SatisSiparisleri_CariId",
                table: "SatisSiparisleri",
                column: "CariId");

            migrationBuilder.AddForeignKey(
                name: "FK_SatisSiparisleri_Cariler_CariId",
                table: "SatisSiparisleri",
                column: "CariId",
                principalTable: "Cariler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
