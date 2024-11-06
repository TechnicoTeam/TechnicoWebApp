using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Technico.Main.Migrations
{
    /// <inheritdoc />
    public partial class M2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "E9",
                table: "Properties",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_E9",
                table: "Properties",
                column: "E9",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Properties_E9",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "E9",
                table: "Properties");
        }
    }
}
