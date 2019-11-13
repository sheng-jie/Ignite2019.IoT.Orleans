using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignite2019.IoT.Orleans.DataAccess.Migrations
{
    public partial class updateproductaddnettype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NetType",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NetType",
                table: "Products");
        }
    }
}
