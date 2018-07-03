using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CinderellaCore.Data.Migrations
{
    public partial class addeditemmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    UserNum = table.Column<int>(nullable: false),
                    Genre = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    YearReleased = table.Column<int>(nullable: false),
                    IsPhysical = table.Column<bool>(nullable: false),
                    IsNew = table.Column<bool>(nullable: false),
                    LocationPurchased = table.Column<string>(nullable: true),
                    DatePurchased = table.Column<DateTime>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    LastCompleted = table.Column<DateTime>(nullable: false),
                    CompletionStatus = table.Column<int>(nullable: false),
                    DateStarted = table.Column<DateTime>(nullable: false),
                    DateCompleted = table.Column<DateTime>(nullable: false),
                    CheckedOut = table.Column<bool>(nullable: false),
                    TimesCompleted = table.Column<int>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    CountryOfOrigin = table.Column<string>(nullable: true),
                    CountryPurchased = table.Column<string>(nullable: true),
                    IsShowcased = table.Column<bool>(nullable: false),
                    IsQueued = table.Column<bool>(nullable: false),
                    QueueRank = table.Column<int>(nullable: false),
                    Artist = table.Column<string>(nullable: false),
                    MediaType = table.Column<int>(nullable: false),
                    Style = table.Column<string>(nullable: true),
                    Speed = table.Column<int>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    RecordLabel = table.Column<string>(nullable: true),
                    DiscogsID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    UserNum = table.Column<int>(nullable: false),
                    Genre = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    YearReleased = table.Column<int>(nullable: false),
                    IsPhysical = table.Column<bool>(nullable: false),
                    IsNew = table.Column<bool>(nullable: false),
                    LocationPurchased = table.Column<string>(nullable: true),
                    DatePurchased = table.Column<DateTime>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    LastCompleted = table.Column<DateTime>(nullable: false),
                    CompletionStatus = table.Column<int>(nullable: false),
                    DateStarted = table.Column<DateTime>(nullable: false),
                    DateCompleted = table.Column<DateTime>(nullable: false),
                    CheckedOut = table.Column<bool>(nullable: false),
                    TimesCompleted = table.Column<int>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    CountryOfOrigin = table.Column<string>(nullable: true),
                    CountryPurchased = table.Column<string>(nullable: true),
                    IsShowcased = table.Column<bool>(nullable: false),
                    IsQueued = table.Column<bool>(nullable: false),
                    QueueRank = table.Column<int>(nullable: false),
                    Author = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Hardcover = table.Column<bool>(nullable: false),
                    IsFirstEdition = table.Column<bool>(nullable: false),
                    Publisher = table.Column<string>(nullable: false),
                    PageCount = table.Column<int>(nullable: false),
                    ISBN10 = table.Column<string>(nullable: true),
                    ISBN13 = table.Column<string>(nullable: true),
                    IsReissue = table.Column<bool>(nullable: false),
                    GoogleBookID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    UserNum = table.Column<int>(nullable: false),
                    Genre = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    YearReleased = table.Column<int>(nullable: false),
                    IsPhysical = table.Column<bool>(nullable: false),
                    IsNew = table.Column<bool>(nullable: false),
                    LocationPurchased = table.Column<string>(nullable: true),
                    DatePurchased = table.Column<DateTime>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    LastCompleted = table.Column<DateTime>(nullable: false),
                    CompletionStatus = table.Column<int>(nullable: false),
                    DateStarted = table.Column<DateTime>(nullable: false),
                    DateCompleted = table.Column<DateTime>(nullable: false),
                    CheckedOut = table.Column<bool>(nullable: false),
                    TimesCompleted = table.Column<int>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    CountryOfOrigin = table.Column<string>(nullable: true),
                    CountryPurchased = table.Column<string>(nullable: true),
                    IsShowcased = table.Column<bool>(nullable: false),
                    IsQueued = table.Column<bool>(nullable: false),
                    QueueRank = table.Column<int>(nullable: false),
                    Developer = table.Column<string>(nullable: false),
                    Publisher = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Rating = table.Column<int>(nullable: false),
                    Platform = table.Column<int>(nullable: false),
                    IsDLC = table.Column<bool>(nullable: false),
                    PartOfSeries = table.Column<bool>(nullable: false),
                    Series = table.Column<string>(nullable: true),
                    GiantBombID = table.Column<int>(nullable: false),
                    BGGID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    UserNum = table.Column<int>(nullable: false),
                    Genre = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    YearReleased = table.Column<int>(nullable: false),
                    IsPhysical = table.Column<bool>(nullable: false),
                    IsNew = table.Column<bool>(nullable: false),
                    LocationPurchased = table.Column<string>(nullable: true),
                    DatePurchased = table.Column<DateTime>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    LastCompleted = table.Column<DateTime>(nullable: false),
                    CompletionStatus = table.Column<int>(nullable: false),
                    DateStarted = table.Column<DateTime>(nullable: false),
                    DateCompleted = table.Column<DateTime>(nullable: false),
                    CheckedOut = table.Column<bool>(nullable: false),
                    TimesCompleted = table.Column<int>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    CountryOfOrigin = table.Column<string>(nullable: true),
                    CountryPurchased = table.Column<string>(nullable: true),
                    IsShowcased = table.Column<bool>(nullable: false),
                    IsQueued = table.Column<bool>(nullable: false),
                    QueueRank = table.Column<int>(nullable: false),
                    Director = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Distributor = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    TMDBID = table.Column<int>(nullable: false),
                    SeasonNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Pops",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    UserNum = table.Column<int>(nullable: false),
                    Genre = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    YearReleased = table.Column<int>(nullable: false),
                    IsPhysical = table.Column<bool>(nullable: false),
                    IsNew = table.Column<bool>(nullable: false),
                    LocationPurchased = table.Column<string>(nullable: true),
                    DatePurchased = table.Column<DateTime>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    LastCompleted = table.Column<DateTime>(nullable: false),
                    CompletionStatus = table.Column<int>(nullable: false),
                    DateStarted = table.Column<DateTime>(nullable: false),
                    DateCompleted = table.Column<DateTime>(nullable: false),
                    CheckedOut = table.Column<bool>(nullable: false),
                    TimesCompleted = table.Column<int>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    CountryOfOrigin = table.Column<string>(nullable: true),
                    CountryPurchased = table.Column<string>(nullable: true),
                    IsShowcased = table.Column<bool>(nullable: false),
                    IsQueued = table.Column<bool>(nullable: false),
                    QueueRank = table.Column<int>(nullable: false),
                    Series = table.Column<string>(nullable: false),
                    PopLine = table.Column<string>(nullable: false),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pops", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Wishes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    UserID = table.Column<string>(nullable: false),
                    ApiID = table.Column<string>(nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    ItemType = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    Owned = table.Column<bool>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AlbumID = table.Column<int>(nullable: false),
                    duration = table.Column<string>(nullable: true),
                    position = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tracks_Albums_AlbumID",
                        column: x => x.AlbumID,
                        principalTable: "Albums",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_AlbumID",
                table: "Tracks",
                column: "AlbumID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Pops");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "Wishes");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.ID);
                });
        }
    }
}
