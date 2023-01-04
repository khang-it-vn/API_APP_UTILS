using Microsoft.EntityFrameworkCore.Migrations;

namespace AppUtils.Migrations
{
    public partial class updateColumnAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "USER",
                newName: "MatKhau");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "REPAIRER",
                newName: "MatKhau");

            migrationBuilder.AddColumn<bool>(
                name: "TrangThaiHoatDong",
                table: "REPAIRER",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThaiHoatDong",
                table: "REPAIRER");

            migrationBuilder.RenameColumn(
                name: "MatKhau",
                table: "USER",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "MatKhau",
                table: "REPAIRER",
                newName: "Password");
        }
    }
}
