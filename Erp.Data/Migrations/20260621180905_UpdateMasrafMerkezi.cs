using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Erp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMasrafMerkezi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UstMasrafMerkezi",
                table: "MasrafMerkezleri");

            migrationBuilder.AlterColumn<string>(
                name: "Kod",
                table: "MasrafMerkezleri",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "AnaGrup",
                table: "MasrafMerkezleri",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnaGrup",
                table: "MasrafMerkezleri");

            migrationBuilder.AlterColumn<string>(
                name: "Kod",
                table: "MasrafMerkezleri",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UstMasrafMerkezi",
                table: "MasrafMerkezleri",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
