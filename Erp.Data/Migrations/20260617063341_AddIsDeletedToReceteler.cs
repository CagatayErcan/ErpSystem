using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToReceteler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UrunMacaReceteleri",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UrunIslemeReceteleri",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UrunErgitmeReceteleri",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UrunMacaReceteleri");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UrunIslemeReceteleri");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UrunErgitmeReceteleri");
        }
    }
}
