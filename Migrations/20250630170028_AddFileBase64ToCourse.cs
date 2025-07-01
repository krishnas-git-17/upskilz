using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace theupskilzapi.Migrations
{
    public partial class AddFileBase64ToCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileBase64",
                table: "Courses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Courses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Courses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileBase64",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Courses");
        }
    }
}
