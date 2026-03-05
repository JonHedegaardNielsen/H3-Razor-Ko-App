using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RazorTest.Migrations
{
    /// <inheritdoc />
    public partial class addQueueEntryDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "QueueEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "QueueEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "QueueEntries");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "QueueEntries");
        }
    }
}
