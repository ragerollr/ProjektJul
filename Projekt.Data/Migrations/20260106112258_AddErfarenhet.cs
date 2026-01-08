using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddErfarenhet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Erfarenhets_AspNetUsers_UserId",
                table: "Erfarenhets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Erfarenhets",
                table: "Erfarenhets");

            migrationBuilder.RenameTable(
                name: "Erfarenhets",
                newName: "Erfarenheter");

            migrationBuilder.RenameIndex(
                name: "IX_Erfarenhets_UserId",
                table: "Erfarenheter",
                newName: "IX_Erfarenheter_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Erfarenheter",
                table: "Erfarenheter",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Erfarenheter_AspNetUsers_UserId",
                table: "Erfarenheter",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Erfarenheter_AspNetUsers_UserId",
                table: "Erfarenheter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Erfarenheter",
                table: "Erfarenheter");

            migrationBuilder.RenameTable(
                name: "Erfarenheter",
                newName: "Erfarenhets");

            migrationBuilder.RenameIndex(
                name: "IX_Erfarenheter_UserId",
                table: "Erfarenhets",
                newName: "IX_Erfarenhets_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Erfarenhets",
                table: "Erfarenhets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Erfarenhets_AspNetUsers_UserId",
                table: "Erfarenhets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
