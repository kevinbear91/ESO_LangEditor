using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ESO_LangEditor.API.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    UserNickName = table.Column<string>(maxLength: 20, nullable: true),
                    UserEsoId = table.Column<string>(maxLength: 20, nullable: true),
                    UserAvatarPath = table.Column<string>(nullable: true),
                    TranslatedCount = table.Column<int>(nullable: false),
                    InReviewCount = table.Column<int>(nullable: false),
                    RemovedCount = table.Column<int>(nullable: false),
                    RefreshToken = table.Column<string>(nullable: true),
                    RefreshTokenExpireTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LangtextRevised",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LangtextID = table.Column<Guid>(nullable: false),
                    LangTextRevNumber = table.Column<int>(nullable: false),
                    ReasonFor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LangtextRevised", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LangtextRevNumber",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LangTextRev = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LangtextRevNumber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LangtextArchive",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TextId = table.Column<string>(nullable: true),
                    IdType = table.Column<int>(nullable: false),
                    TextEn = table.Column<string>(nullable: true),
                    TextZh = table.Column<string>(nullable: true),
                    LangTextType = table.Column<byte>(nullable: false),
                    IsTranslated = table.Column<byte>(nullable: false),
                    UpdateStats = table.Column<string>(nullable: true),
                    EnLastModifyTimestamp = table.Column<DateTime>(nullable: false),
                    ZhLastModifyTimestamp = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ReviewerId = table.Column<Guid>(nullable: false),
                    Revised = table.Column<int>(nullable: false),
                    ReviewTimestamp = table.Column<DateTime>(nullable: false),
                    ArchiveTimestamp = table.Column<DateTime>(nullable: false),
                    ReasonFor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LangtextArchive", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LangtextArchive_AspNetUsers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LangtextArchive_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LangtextReview",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TextId = table.Column<string>(nullable: true),
                    IdType = table.Column<int>(nullable: false),
                    TextEn = table.Column<string>(nullable: true),
                    TextZh = table.Column<string>(nullable: true),
                    LangTextType = table.Column<byte>(nullable: false),
                    IsTranslated = table.Column<byte>(nullable: false),
                    UpdateStats = table.Column<string>(nullable: true),
                    EnLastModifyTimestamp = table.Column<DateTime>(nullable: false),
                    ZhLastModifyTimestamp = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ReviewerId = table.Column<Guid>(nullable: false),
                    Revised = table.Column<int>(nullable: false),
                    ReviewTimestamp = table.Column<DateTime>(nullable: false),
                    ReasonFor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LangtextReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LangtextReview_AspNetUsers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LangtextReview_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Langtexts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TextId = table.Column<string>(nullable: true),
                    IdType = table.Column<int>(nullable: false),
                    TextEn = table.Column<string>(nullable: true),
                    TextZh = table.Column<string>(nullable: true),
                    LangTextType = table.Column<byte>(nullable: false),
                    IsTranslated = table.Column<byte>(nullable: false),
                    UpdateStats = table.Column<string>(nullable: true),
                    EnLastModifyTimestamp = table.Column<DateTime>(nullable: false),
                    ZhLastModifyTimestamp = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ReviewerId = table.Column<Guid>(nullable: false),
                    Revised = table.Column<int>(nullable: false),
                    ReviewTimestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Langtexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Langtexts_AspNetUsers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Langtexts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LangtextArchive_ReviewerId",
                table: "LangtextArchive",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_LangtextArchive_UserId",
                table: "LangtextArchive",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LangtextReview_ReviewerId",
                table: "LangtextReview",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_LangtextReview_UserId",
                table: "LangtextReview",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Langtexts_ReviewerId",
                table: "Langtexts",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Langtexts_UserId",
                table: "Langtexts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "LangtextArchive");

            migrationBuilder.DropTable(
                name: "LangtextReview");

            migrationBuilder.DropTable(
                name: "LangtextRevised");

            migrationBuilder.DropTable(
                name: "LangtextRevNumber");

            migrationBuilder.DropTable(
                name: "Langtexts");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
