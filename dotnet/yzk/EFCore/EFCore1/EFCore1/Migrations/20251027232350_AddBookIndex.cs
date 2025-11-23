using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore1.Migrations
{
    /// <inheritdoc />
    public partial class AddBookIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_T_Books_NameTwo_AuthorName",
                table: "T_Books",
                columns: new[] { "NameTwo", "AuthorName" });

            migrationBuilder.CreateIndex(
                name: "IX_T_Books_Title",
                table: "T_Books",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_T_Books_NameTwo_AuthorName",
                table: "T_Books");

            migrationBuilder.DropIndex(
                name: "IX_T_Books_Title",
                table: "T_Books");
        }
    }
}
