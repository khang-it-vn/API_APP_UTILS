using Microsoft.EntityFrameworkCore.Migrations;


namespace AppUtils.Migrations
{
    public partial class updateColumnLatitudeLongitude : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "REPAIRER",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "REPAIRER",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "REPAIRER");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "REPAIRER");
        }
    }
}
