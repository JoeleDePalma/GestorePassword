using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestioneDb.Migrations
{
    /// <inheritdoc />
    public partial class AddedKeySalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeySalt",
                table: "Passwords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeySalt",
                table: "Passwords");
        }
    }
}
