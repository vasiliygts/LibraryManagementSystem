using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    bookid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    author = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    isbn = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    genre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    year = table.Column<int>(type: "INTEGER", nullable: false),
                    isavailable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.bookid);
                });

            migrationBuilder.CreateTable(
                name: "readers",
                columns: table => new
                {
                    readerid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    @class = table.Column<string>(name: "class", type: "TEXT", maxLength: 50, nullable: false),
                    phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_readers", x => x.readerid);
                });

            migrationBuilder.CreateTable(
                name: "borrowedbooks",
                columns: table => new
                {
                    transactionid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    bookid = table.Column<int>(type: "INTEGER", nullable: false),
                    readerid = table.Column<int>(type: "INTEGER", nullable: false),
                    borrowdate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    returndate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_borrowedbooks", x => x.transactionid);
                    table.ForeignKey(
                        name: "FK_borrowedbooks_books_bookid",
                        column: x => x.bookid,
                        principalTable: "books",
                        principalColumn: "bookid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_borrowedbooks_readers_readerid",
                        column: x => x.readerid,
                        principalTable: "readers",
                        principalColumn: "readerid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_borrowedbooks_bookid",
                table: "borrowedbooks",
                column: "bookid");

            migrationBuilder.CreateIndex(
                name: "IX_borrowedbooks_readerid",
                table: "borrowedbooks",
                column: "readerid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "borrowedbooks");

            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DropTable(
                name: "readers");
        }
    }
}
