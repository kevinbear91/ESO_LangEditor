using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class removeRoleIdFromLangTexts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Langtexts_AspNetRoles_RoleId",
                table: "Langtexts");

            migrationBuilder.DropIndex(
                name: "IX_Langtexts_RoleId",
                table: "Langtexts");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Langtexts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Langtexts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Langtexts_RoleId",
                table: "Langtexts",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Langtexts_AspNetRoles_RoleId",
                table: "Langtexts",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
