using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplyInk.Web.Migrations
{
    public partial class addposition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Shops",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Logitude",
                table: "Shops",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Logitude",
                table: "Shops");
        }
    }
}
