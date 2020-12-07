using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreTutorial.Lesson_01.Migrations
{
    public partial class Change_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Teachers_TeacherId",
                schema: "lesson_01",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Schools_SchoolId",
                schema: "lesson_01",
                table: "Teachers");

            migrationBuilder.DropTable(
                name: "Schools",
                schema: "lesson_01");

            migrationBuilder.DropTable(
                name: "Students",
                schema: "lesson_01");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_SchoolId",
                schema: "lesson_01",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                schema: "lesson_01",
                table: "Teachers");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "lesson_01",
                table: "Classes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Teachers_TeacherId",
                schema: "lesson_01",
                table: "Classes",
                column: "TeacherId",
                principalSchema: "lesson_01",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Teachers_TeacherId",
                schema: "lesson_01",
                table: "Classes");

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                schema: "lesson_01",
                table: "Teachers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "lesson_01",
                table: "Classes",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "Schools",
                schema: "lesson_01",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    City = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                schema: "lesson_01",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Classes_ClassId",
                        column: x => x.ClassId,
                        principalSchema: "lesson_01",
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SchoolId",
                schema: "lesson_01",
                table: "Teachers",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                schema: "lesson_01",
                table: "Students",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Teachers_TeacherId",
                schema: "lesson_01",
                table: "Classes",
                column: "TeacherId",
                principalSchema: "lesson_01",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Schools_SchoolId",
                schema: "lesson_01",
                table: "Teachers",
                column: "SchoolId",
                principalSchema: "lesson_01",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
