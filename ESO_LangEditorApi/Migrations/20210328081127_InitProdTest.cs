using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ESO_LangEditor.API.Migrations
{
    public partial class InitProdTest : Migration
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
                    { new Guid("f3eb66da-8481-4cf7-9afc-f55fe1089ff4"), "09133d05-3d2f-4c20-956c-7c77d89bcbb8", "InitUser", null },
                    { new Guid("bee2ce3d-ee28-49c5-ab49-23a1f522ab6f"), "50bd0f33-3050-4a2b-aa56-acc2a998fe15", "Editor", null },
                    { new Guid("94cb5476-9d89-4cb0-b9ac-c8b93169580a"), "9f49bdde-38e5-4466-afca-67843f31fc28", "Reviewer", null },
                    { new Guid("0f4f6491-e2a0-49d0-9334-fe05dd4a7bba"), "b3f05ba0-4cc9-41cd-b625-f635f4430507", "Admin", null },
                    { new Guid("91c03833-f7c1-4a59-9563-451a6cffc183"), "b51cc952-1a28-4d41-92eb-9eca14b661fb", "Creater", null }
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
