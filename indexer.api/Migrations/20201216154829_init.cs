using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace indexer.api.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Path = table.Column<string>(type: "TEXT", maxLength: 4096, nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 4351, nullable: false),
                    Extension = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Size = table.Column<long>(type: "INTEGER", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastAccessTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastWriteTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IndexedDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntry", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileEntry_FullName",
                table: "FileEntry",
                column: "FullName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileEntry");
        }
    }
}
