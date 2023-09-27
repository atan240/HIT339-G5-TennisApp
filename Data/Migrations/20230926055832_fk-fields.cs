using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TennisApp2.Data.Migrations
{
    /// <inheritdoc />
    public partial class fkfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "ScheduleJoin",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "ScheduleJoin",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "ScheduleJoin");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "ScheduleJoin");
        }
    }
}
