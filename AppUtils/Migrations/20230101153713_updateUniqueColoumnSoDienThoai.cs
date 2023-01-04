using Microsoft.EntityFrameworkCore.Migrations;

namespace AppUtils.Migrations
{
    public partial class updateUniqueColoumnSoDienThoai : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_USER_NumberPhone",
                table: "USER",
                column: "NumberPhone",
                unique: true,
                filter: "[NumberPhone] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_USER_NumberPhone",
                table: "USER");
        }
    }
}
