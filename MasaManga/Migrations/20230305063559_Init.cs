using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasaManga.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    IndexUrl = table.Column<string>(type: "TEXT", nullable: true),
                    TotalSection = table.Column<int>(type: "INTEGER", nullable: false),
                    DownloadSection = table.Column<int>(type: "INTEGER", nullable: false),
                    Cover = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookSection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    TotalPic = table.Column<int>(type: "INTEGER", nullable: false),
                    DownloadPic = table.Column<int>(type: "INTEGER", nullable: false),
                    BookId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookSection_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookPic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    IsDownloaded = table.Column<bool>(type: "INTEGER", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: true),
                    DownloadTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BookSectionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookPic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookPic_BookSection_BookSectionId",
                        column: x => x.BookSectionId,
                        principalTable: "BookSection",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookPic_BookSectionId",
                table: "BookPic",
                column: "BookSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_BookSection_BookId",
                table: "BookSection",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookPic");

            migrationBuilder.DropTable(
                name: "BookSection");

            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
