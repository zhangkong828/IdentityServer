using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServer.Web.Data.Migrations.IdentityUser
{
    public partial class InitialIdentityUserDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(maxLength: 60, nullable: true),
                    Username = table.Column<string>(maxLength: 60, nullable: true),
                    Password = table.Column<string>(maxLength: 60, nullable: true),
                    NickName = table.Column<string>(maxLength: 60, nullable: true),
                    Avatar = table.Column<string>(maxLength: 200, nullable: true),
                    Email = table.Column<string>(maxLength: 60, nullable: true),
                    PhoneCode = table.Column<string>(maxLength: 20, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 20, nullable: true),
                    LastLoginIp = table.Column<string>(maxLength: 30, nullable: true),
                    LastLoginTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccounts");
        }
    }
}
