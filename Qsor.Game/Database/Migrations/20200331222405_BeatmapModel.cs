using Microsoft.EntityFrameworkCore.Migrations;

namespace Qsor.Database.Migrations
{
    public partial class BeatmapModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "beatmaps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    File = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Audio = table.Column<string>(nullable: true),
                    Thumbnail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beatmaps", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "beatmaps");
        }
    }
}
