using Microsoft.EntityFrameworkCore.Migrations;

namespace indexer.api.Migrations
{
    public partial class removeid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FileEntry",
                table: "FileEntry");

            migrationBuilder.DropIndex(
                name: "IX_FileEntry_FullName",
                table: "FileEntry");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FileEntry");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileEntry",
                table: "FileEntry",
                column: "FullName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FileEntry",
                table: "FileEntry");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FileEntry",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileEntry",
                table: "FileEntry",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FileEntry_FullName",
                table: "FileEntry",
                column: "FullName",
                unique: true);
        }
    }
}
