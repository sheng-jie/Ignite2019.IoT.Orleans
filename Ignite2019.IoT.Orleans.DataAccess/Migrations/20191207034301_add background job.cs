using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ignite2019.IoT.Orleans.DataAccess.Migrations
{
    public partial class addbackgroundjob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FrameworkActions");

            migrationBuilder.DropTable(
                name: "FrameworkModules");

            migrationBuilder.DropTable(
                name: "FrameworkAreas");

            migrationBuilder.CreateTable(
                name: "BackgroundJobs",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    DeviceId = table.Column<string>(nullable: true),
                    JobStatus = table.Column<int>(nullable: false),
                    Command = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    Period = table.Column<TimeSpan>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: true),
                    ExecutedCount = table.Column<int>(nullable: false),
                    LastExecuteTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundJobs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PersistedGrants",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(maxLength: 50, nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: false),
                    RefreshToken = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PersistedGrants_FrameworkUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "FrameworkUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_UserId",
                table: "PersistedGrants",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackgroundJobs");

            migrationBuilder.DropTable(
                name: "PersistedGrants");

            migrationBuilder.CreateTable(
                name: "FrameworkAreas",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreaName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Prefix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkAreas", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FrameworkModules",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModuleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameSpace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkModules", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FrameworkModules_FrameworkAreas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "FrameworkAreas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FrameworkActions",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MethodName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Parameter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkActions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FrameworkActions_FrameworkModules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "FrameworkModules",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FrameworkActions_ModuleId",
                table: "FrameworkActions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_FrameworkModules_AreaId",
                table: "FrameworkModules",
                column: "AreaId");
        }
    }
}
