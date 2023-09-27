using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TennisApp2.Data.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleJoin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduleJoinId",
                table: "Schedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduleJoinId",
                table: "Member",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScheduleJoin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleJoin", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_ScheduleJoinId",
                table: "Schedule",
                column: "ScheduleJoinId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_ScheduleJoinId",
                table: "Member",
                column: "ScheduleJoinId");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_ScheduleJoin_ScheduleJoinId",
                table: "Member",
                column: "ScheduleJoinId",
                principalTable: "ScheduleJoin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_ScheduleJoin_ScheduleJoinId",
                table: "Schedule",
                column: "ScheduleJoinId",
                principalTable: "ScheduleJoin",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_ScheduleJoin_ScheduleJoinId",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_ScheduleJoin_ScheduleJoinId",
                table: "Schedule");

            migrationBuilder.DropTable(
                name: "ScheduleJoin");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_ScheduleJoinId",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Member_ScheduleJoinId",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "ScheduleJoinId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "ScheduleJoinId",
                table: "Member");
        }
    }
}
