using Microsoft.EntityFrameworkCore.Migrations;

namespace ArchiveApp.Migrations
{
    public partial class UpdatedGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_ImageId",
                table: "Groups");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ImageId",
                table: "Groups",
                column: "ImageId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_ImageId",
                table: "Groups");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ImageId",
                table: "Groups",
                column: "ImageId");
        }
    }
}
