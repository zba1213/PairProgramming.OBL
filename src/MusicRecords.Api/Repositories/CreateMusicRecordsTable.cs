using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicRecords.Api.Repositories;

[DbContext(typeof(MusicRecordsDbContext))]
[Migration("20261116000000_CreateMusicRecordsTable")]
public sealed class CreateMusicRecordsTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "MusicRecords",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1")
                    .Annotation("Sqlite:Autoincrement", true),
                Title = table.Column<string>(maxLength: 200, nullable: false),
                Artist = table.Column<string>(maxLength: 200, nullable: false),
                Duration = table.Column<int>(nullable: false),
                PublicationYear = table.Column<int>(nullable: false),
                SearchTitle = table.Column<string>(maxLength: 200, nullable: false),
                SearchArtist = table.Column<string>(maxLength: 200, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_MusicRecords", record => record.Id));

        migrationBuilder.Sql("""
            INSERT INTO MusicRecords (Title, Artist, Duration, PublicationYear, SearchTitle, SearchArtist) VALUES
                ('Kind of Blue', 'Miles Davis', 2757, 1959, 'KIND OF BLUE', 'MILES DAVIS'),
                ('Abbey Road', 'The Beatles', 2813, 1969, 'ABBEY ROAD', 'THE BEATLES'),
                ('Hounds of Love', 'Kate Bush', 2847, 1985, 'HOUNDS OF LOVE', 'KATE BUSH');
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
        => migrationBuilder.DropTable("MusicRecords");
}
