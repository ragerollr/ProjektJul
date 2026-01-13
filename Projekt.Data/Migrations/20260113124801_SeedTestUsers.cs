using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Projekt.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsPrivate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImagePath", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "test-user-1", 0, null, "1a8ed0c5-d624-405f-8c58-b05442b87098", "user1@test.se", true, "Test User One", false, false, null, "USER1@TEST.SE", "USER1@TEST.SE", "AQAAAAIAAYagAAAAEGT6IfH1vyij10AOcxm69+WqvB9yDv0K9u/Lsdw0068JKrkbmycW6kAA9fw1KzQo/A==", null, false, null, "c7077cc7-cf7e-4cb7-8cfc-45af010b79ca", false, "user1@test.se" },
                    { "test-user-2", 0, null, "36cc494d-fb83-4b06-8214-fb2b0d14bc38", "user2@test.se", true, "Test User Two", false, false, null, "USER2@TEST.SE", "USER2@TEST.SE", "AQAAAAIAAYagAAAAEDB5iQh2hNXd3FwtqoTS2PmI5WN1h9f3kL4/EwpScSSsBIXRRuCoRnvG0pLGdi2uxw==", null, false, null, "f1445eee-3453-4a71-bd39-519e2615bc09", false, "user2@test.se" },
                    { "test-user-3", 0, null, "ef75e6bc-7e5f-411f-840c-2df1e2266f01", "user3@test.se", true, "Test User Three", true, false, null, "USER3@TEST.SE", "USER3@TEST.SE", "AQAAAAIAAYagAAAAEC8nyD7wnUv1b236Frlx0E8f+KXganOg95AU8qpCy4mRdUZTnCA7oe0z9rzmjufyxg==", null, false, null, "d1d75146-fcde-4552-96ff-b22f0d18bf18", false, "user3@test.se" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-3");
        }
    }
}
