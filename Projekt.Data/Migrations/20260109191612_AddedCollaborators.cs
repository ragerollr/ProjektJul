using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCollaborators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projekts_AspNetUsers_UserId",
                table: "Projekts");

            migrationBuilder.CreateTable(
                name: "ProjectCollaborators",
                columns: table => new
                {
                    CollaboratingProjectsId = table.Column<int>(type: "int", nullable: false),
                    CollaboratorsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCollaborators", x => new { x.CollaboratingProjectsId, x.CollaboratorsId });
                    table.ForeignKey(
                        name: "FK_ProjectCollaborators_AspNetUsers_CollaboratorsId",
                        column: x => x.CollaboratorsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectCollaborators_Projekts_CollaboratingProjectsId",
                        column: x => x.CollaboratingProjectsId,
                        principalTable: "Projekts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCollaborators_CollaboratorsId",
                table: "ProjectCollaborators",
                column: "CollaboratorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projekts_AspNetUsers_UserId",
                table: "Projekts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projekts_AspNetUsers_UserId",
                table: "Projekts");

            migrationBuilder.DropTable(
                name: "ProjectCollaborators");

            migrationBuilder.AddForeignKey(
                name: "FK_Projekts_AspNetUsers_UserId",
                table: "Projekts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
