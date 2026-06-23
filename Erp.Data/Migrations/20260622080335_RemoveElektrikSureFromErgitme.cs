using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveElektrikSureFromErgitme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirimElektrikTuketimi",
                table: "UrunErgitmeReceteleri");

            migrationBuilder.DropColumn(
                name: "ErgitmeSuresi",
                table: "UrunErgitmeReceteleri");

            migrationBuilder.AlterColumn<string>(
                name: "AnaGrup",
                table: "MasrafMerkezleri",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BirimElektrikTuketimi",
                table: "UrunErgitmeReceteleri",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ErgitmeSuresi",
                table: "UrunErgitmeReceteleri",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AnaGrup",
                table: "MasrafMerkezleri",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
