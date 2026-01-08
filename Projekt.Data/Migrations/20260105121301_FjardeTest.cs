using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Projekt.Data.Migrations
{
    /// <inheritdoc />
    public partial class FjardeTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Projekts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Projekts",
                keyColumn: "Id",
                keyValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Projekts",
                columns: new[] { "Id", "Description", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, "Beskrivning av projekt 1", "Projekt 1", "user1" },
                    { 2, "Beskrivning av projekt 2", "Projekt 2", "user2" }
                });
        }
    }
}
