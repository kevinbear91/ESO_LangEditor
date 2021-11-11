using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class UpdateToV5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpireTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserAvatarPath",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserEsoId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "LangTextRev",
                table: "LangtextRevNumber",
                newName: "Rev");

            migrationBuilder.CreateTable(
                name: "UserRegistrationCode",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    UserRequest = table.Column<Guid>(type: "uuid", nullable: false),
                    UserUsed = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestTimestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UsedTimestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRegistrationCode", x => x.Code);
                    table.ForeignKey(
                        name: "FK_UserRegistrationCode_AspNetUsers_UserRequest",
                        column: x => x.UserRequest,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRegistrationCode_AspNetUsers_UserUsed",
                        column: x => x.UserUsed,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRegistrationCode_UserRequest",
                table: "UserRegistrationCode",
                column: "UserRequest");

            migrationBuilder.CreateIndex(
                name: "IX_UserRegistrationCode_UserUsed",
                table: "UserRegistrationCode",
                column: "UserUsed");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRegistrationCode");

            migrationBuilder.RenameColumn(
                name: "Rev",
                table: "LangtextRevNumber",
                newName: "LangTextRev");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpireTime",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserAvatarPath",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEsoId",
                table: "AspNetUsers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
