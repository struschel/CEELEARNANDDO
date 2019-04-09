using Microsoft.EntityFrameworkCore.Migrations;

namespace CLAD.Data.Migrations
{
    public partial class ThumbnailPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailPath",
                table: "Articles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailPath",
                table: "Articles");
        }
    }
}
