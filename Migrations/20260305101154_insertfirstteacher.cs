using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RazorTest.Migrations
{
    /// <inheritdoc />
    public partial class insertfirstteacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash" },
                values: new object[] { new Guid("81cce9d1-dd44-4754-b3b7-f150cdd4ff11"), "test@gmail.com", "Test", "VGVzdDEyMzQ=" });

            migrationBuilder.InsertData(
                table: "Teachers",
                column: "Id",
                value: new Guid("81cce9d1-dd44-4754-b3b7-f150cdd4ff11"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: new Guid("81cce9d1-dd44-4754-b3b7-f150cdd4ff11"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("81cce9d1-dd44-4754-b3b7-f150cdd4ff11"));
        }
    }
}
