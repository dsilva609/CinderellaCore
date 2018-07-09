using Microsoft.EntityFrameworkCore.Migrations;

namespace CinderellaCore.Web.Data.Migrations
{
    public partial class addedenableimportfieldtouser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableImport",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableImport",
                table: "AspNetUsers");
        }
    }
}
