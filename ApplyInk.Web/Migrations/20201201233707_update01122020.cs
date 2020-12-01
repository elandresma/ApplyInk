using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplyInk.Web.Migrations
{
    public partial class update01122020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterDetailMeetings_Meetings_MeetingId",
                table: "MasterDetailMeetings");

            migrationBuilder.RenameColumn(
                name: "MeetingId",
                table: "MasterDetailMeetings",
                newName: "meetingId");

            migrationBuilder.RenameIndex(
                name: "IX_MasterDetailMeetings_MeetingId",
                table: "MasterDetailMeetings",
                newName: "IX_MasterDetailMeetings_meetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDetailMeetings_Meetings_meetingId",
                table: "MasterDetailMeetings",
                column: "meetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterDetailMeetings_Meetings_meetingId",
                table: "MasterDetailMeetings");

            migrationBuilder.RenameColumn(
                name: "meetingId",
                table: "MasterDetailMeetings",
                newName: "MeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_MasterDetailMeetings_meetingId",
                table: "MasterDetailMeetings",
                newName: "IX_MasterDetailMeetings_MeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterDetailMeetings_Meetings_MeetingId",
                table: "MasterDetailMeetings",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
