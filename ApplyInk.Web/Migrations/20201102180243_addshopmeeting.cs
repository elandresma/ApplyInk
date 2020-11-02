using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplyInk.Web.Migrations
{
    public partial class addshopmeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Meetings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_ShopId",
                table: "Meetings",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Shops_ShopId",
                table: "Meetings",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Shops_ShopId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_ShopId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Meetings");
        }
    }
}
