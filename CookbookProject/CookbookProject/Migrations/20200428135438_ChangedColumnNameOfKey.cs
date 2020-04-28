using Microsoft.EntityFrameworkCore.Migrations;

namespace CookbookProject.Migrations
{
    public partial class ChangedColumnNameOfKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Verification");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Verification",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Verification");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Verification",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
