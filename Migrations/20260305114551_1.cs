using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RazorTest.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Queues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QueueName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QueueEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QueueId = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TeacherId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueueEntries_Queues_QueueId",
                        column: x => x.QueueId,
                        principalTable: "Queues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QueueEntries_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QueueEntries_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TeacherQueues",
                columns: table => new
                {
                    QueueId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeacherId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherQueues", x => new { x.QueueId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_TeacherQueues_Queues_QueueId",
                        column: x => x.QueueId,
                        principalTable: "Queues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherQueues_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Queues",
                columns: new[] { "Id", "QueueName" },
                values: new object[,]
                {
                    { 1, "Serverside" },
                    { 2, "Gui-programmering" },
                    { 3, "Intro" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "amalie@techcollege.dk", "Amalie", "VGVhY2hlcjEyMzQ=" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "elev1@techcollege.dk", "Elev 1", "U3R1ZGVudDEyMzQ=" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "elev2@techcollege.dk", "Elev 2", "U3R1ZGVudDEyMzQ=" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "elev3@techcollege.dk", "Elev 3", "U3R1ZGVudDEyMzQ=" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "elev4@techcollege.dk", "Elev 4", "U3R1ZGVudDEyMzQ=" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "elev5@techcollege.dk", "Elev 5", "U3R1ZGVudDEyMzQ=" },
                    { new Guid("81cce9d1-dd44-4754-b3b7-f150cdd4ff11"), "test@gmail.com", "Test", "VGVzdDEyMzQ=" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                column: "Id",
                values: new object[]
                {
                    new Guid("22222222-2222-2222-2222-222222222222"),
                    new Guid("33333333-3333-3333-3333-333333333333"),
                    new Guid("44444444-4444-4444-4444-444444444444"),
                    new Guid("55555555-5555-5555-5555-555555555555"),
                    new Guid("66666666-6666-6666-6666-666666666666")
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                column: "Id",
                values: new object[]
                {
                    new Guid("11111111-1111-1111-1111-111111111111"),
                    new Guid("81cce9d1-dd44-4754-b3b7-f150cdd4ff11")
                });

            migrationBuilder.InsertData(
                table: "TeacherQueues",
                columns: new[] { "QueueId", "TeacherId" },
                values: new object[,]
                {
                    { 1, new Guid("81cce9d1-dd44-4754-b3b7-f150cdd4ff11") },
                    { 2, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 3, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 3, new Guid("81cce9d1-dd44-4754-b3b7-f150cdd4ff11") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_QueueEntries_QueueId_StudentId",
                table: "QueueEntries",
                columns: new[] { "QueueId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QueueEntries_StudentId",
                table: "QueueEntries",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_QueueEntries_TeacherId",
                table: "QueueEntries",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherQueues_TeacherId",
                table: "TeacherQueues",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QueueEntries");

            migrationBuilder.DropTable(
                name: "TeacherQueues");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Queues");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
