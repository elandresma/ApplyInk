using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplyInk.Web.Migrations
{
    public partial class AddCityInEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "cityId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_cityId",
                table: "AspNetUsers",
                column: "cityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cities_cityId",
                table: "AspNetUsers",
                column: "cityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cities_cityId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_cityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "cityId",
                table: "AspNetUsers");
        }
    }
}
