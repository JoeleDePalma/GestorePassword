using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestioneDb.Migrations
{
    /// <inheritdoc />
    public partial class AddedPKIdtothepasswordsstructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create a new table with the desired column order: Id, App, CryptedPassword
            migrationBuilder.CreateTable(
                name: "Passwords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    App = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CryptedPassword = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passwords", x => x.Id);
                });
        }
    }
}
