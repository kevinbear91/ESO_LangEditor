using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESO_LangEditor.API.Migrations
{
    public partial class AddRoleToEditAndReviewId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LangtextInReivewId",
                table: "Langtexts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Langtexts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("ea7e3901-4aa4-466b-8037-4925ce756fbd"));

            migrationBuilder.CreateIndex(
                name: "IX_Langtexts_LangtextInReivewId",
                table: "Langtexts",
                column: "LangtextInReivewId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Langtexts_LangtextReview_LangtextInReivewId",
                table: "Langtexts",
                column: "LangtextInReivewId",
                principalTable: "LangtextReview",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Langtexts_AspNetRoles_RoleId",
                table: "Langtexts");

            migrationBuilder.DropForeignKey(
                name: "FK_Langtexts_LangtextReview_LangtextInReivewId",
                table: "Langtexts");

            migrationBuilder.DropIndex(
                name: "IX_Langtexts_LangtextInReivewId",
                table: "Langtexts");

            migrationBuilder.DropIndex(
                name: "IX_Langtexts_RoleId",
                table: "Langtexts");

            migrationBuilder.DropColumn(
                name: "LangtextInReivewId",
                table: "Langtexts");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Langtexts");
        }
    }
}
