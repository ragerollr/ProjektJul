using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt.Data.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utildningar_AspNetUsers_UserId",
                table: "Utildningar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Utildningar",
                table: "Utildningar");

            migrationBuilder.RenameTable(
                name: "Utildningar",
                newName: "Utbildningar");

            migrationBuilder.RenameIndex(
                name: "IX_Utildningar_UserId",
                table: "Utbildningar",
                newName: "IX_Utbildningar_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Utbildningar",
                table: "Utbildningar",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Utbildningar_AspNetUsers_UserId",
                table: "Utbildningar",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utbildningar_AspNetUsers_UserId",
                table: "Utbildningar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Utbildningar",
                table: "Utbildningar");

            migrationBuilder.RenameTable(
                name: "Utbildningar",
                newName: "Utildningar");

            migrationBuilder.RenameIndex(
                name: "IX_Utbildningar_UserId",
                table: "Utildningar",
                newName: "IX_Utildningar_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Utildningar",
                table: "Utildningar",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Utildningar_AspNetUsers_UserId",
                table: "Utildningar",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
