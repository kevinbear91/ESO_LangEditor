using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESO_LangEditor.API.Migrations
{
    public partial class UpdateToV5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("367187f5-d112-4b80-b915-6a640f563d18"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8ed126e3-86ee-4e0c-b251-1fe3acb6e9d9"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b6f21761-6c8d-4e15-8f6a-9bd08c930d97"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bb7383b9-aeec-4cd5-b695-c34437069c65"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ea7e3901-4aa4-466b-8037-4925ce756fbd"));

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0e7ece3a-78d5-479b-9bd9-a91d7913c304"), "d7072f4e-5eb5-41a2-a715-67d15d26c8ef", "InitUser", "INITUSER" },
                    { new Guid("08b1a207-286a-4234-ab41-e868bb225705"), "62b84195-eab6-4693-a818-c40eb510ad68", "Editor", "EDITOR" },
                    { new Guid("605b543e-e0bb-4e51-b7eb-feddcdd3699f"), "1d83b1a5-259b-4894-ab25-7f3fa3d5ab17", "Reviewer", "REVIEWER" },
                    { new Guid("86ecc84a-22e1-4b45-ba5d-91c717706f9e"), "31313adf-e71a-418f-aa41-9e9d2c8b22e6", "Admin", "ADMIN" },
                    { new Guid("77c82170-0672-4b33-b191-61abb33b9750"), "29ada10d-6f57-44ef-abec-2f81ba8a835f", "Creater", "CREATER" }
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

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("08b1a207-286a-4234-ab41-e868bb225705"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0e7ece3a-78d5-479b-9bd9-a91d7913c304"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("605b543e-e0bb-4e51-b7eb-feddcdd3699f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("77c82170-0672-4b33-b191-61abb33b9750"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("86ecc84a-22e1-4b45-ba5d-91c717706f9e"));

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("b6f21761-6c8d-4e15-8f6a-9bd08c930d97"), "e9f31f36-22de-4029-8f40-5b84e4459324", "InitUser", "INITUSER" },
                    { new Guid("ea7e3901-4aa4-466b-8037-4925ce756fbd"), "021460b3-50a2-4cfd-9bb8-2b35276eac84", "Editor", "EDITOR" },
                    { new Guid("bb7383b9-aeec-4cd5-b695-c34437069c65"), "3a81e390-9f17-45b9-a994-82343765b9ca", "Reviewer", "REVIEWER" },
                    { new Guid("8ed126e3-86ee-4e0c-b251-1fe3acb6e9d9"), "1dd548d7-f177-478e-bdfc-86b9034c6f62", "Admin", "ADMIN" },
                    { new Guid("367187f5-d112-4b80-b915-6a640f563d18"), "2f8c4a7e-5956-42a2-94cb-6f877f792220", "Creater", "CREATER" }
                });
        }
    }
}
