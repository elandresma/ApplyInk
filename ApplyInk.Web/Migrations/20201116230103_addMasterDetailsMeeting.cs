using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplyInk.Web.Migrations
{
    public partial class addMasterDetailsMeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterDetailMeeting_Meetings_meetingId",
                table: "MasterDetailMeeting");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterDetailMeeting_AspNetUsers_userId",
                table: "MasterDetailMeeting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MasterDetailMeeting",
                table: "MasterDetailMeeting");

            migrationBuilder.RenameTable(
                name: "MasterDetailMeeting",
                newName: "MasterDetailMeetings");

            migrationBuilder.RenameIndex(
                name: "IX_MasterDetailMeeting_userId",
                table: "MasterDetailMeetings",
                newName: "IX_MasterDetailMeetings_userId");

            migrationBuilder.RenameIndex(
                name: "IX_MasterDetailMeeting_meetingId",
                table: "MasterDetailMeetings",
                newName: "IX_MasterDetailMeetings_meetingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MasterDetailMeetings",
                table: "MasterDetailMeetings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDetailMeetings_Meetings_meetingId",
                table: "MasterDetailMeetings",
                column: "meetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDetailMeetings_AspNetUsers_userId",
                table: "MasterDetailMeetings",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterDetailMeetings_Meetings_meetingId",
                table: "MasterDetailMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterDetailMeetings_AspNetUsers_userId",
                table: "MasterDetailMeetings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MasterDetailMeetings",
                table: "MasterDetailMeetings");

            migrationBuilder.RenameTable(
                name: "MasterDetailMeetings",
                newName: "MasterDetailMeeting");

            migrationBuilder.RenameIndex(
                name: "IX_MasterDetailMeetings_userId",
                table: "MasterDetailMeeting",
                newName: "IX_MasterDetailMeeting_userId");

            migrationBuilder.RenameIndex(
                name: "IX_MasterDetailMeetings_meetingId",
                table: "MasterDetailMeeting",
                newName: "IX_MasterDetailMeeting_meetingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MasterDetailMeeting",
                table: "MasterDetailMeeting",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDetailMeeting_Meetings_meetingId",
                table: "MasterDetailMeeting",
                column: "meetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDetailMeeting_AspNetUsers_userId",
                table: "MasterDetailMeeting",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
