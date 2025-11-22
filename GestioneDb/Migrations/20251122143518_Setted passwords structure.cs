using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestioneDb.Migrations
{
    /// <inheritdoc />
    public partial class Settedpasswordsstructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Passwords",
                columns: table => new
                {
                    App = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CryptedPassword = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Passwords");
        }
    }
}
