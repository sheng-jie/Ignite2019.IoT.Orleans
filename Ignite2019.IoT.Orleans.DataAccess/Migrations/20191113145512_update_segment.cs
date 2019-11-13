using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignite2019.IoT.Orleans.DataAccess.Migrations
{
    public partial class update_segment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatestNum",
                table: "Segment");

            migrationBuilder.AddColumn<decimal>(
                name: "InitialNum",
                table: "Segment",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxNum",
                table: "Segment",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Remain",
                table: "Segment",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialNum",
                table: "Segment");

            migrationBuilder.DropColumn(
                name: "MaxNum",
                table: "Segment");

            migrationBuilder.DropColumn(
                name: "Remain",
                table: "Segment");

            migrationBuilder.AddColumn<decimal>(
                name: "LatestNum",
                table: "Segment",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
