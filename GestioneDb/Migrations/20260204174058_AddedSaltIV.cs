using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestioneDb.Migrations
{
    /// <inheritdoc />
    public partial class AddedSaltIV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SaltIV",
                table: "Passwords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaltIV",
                table: "Passwords");
        }
    }
}
