using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ESO_LangEditor.API.Migrations
{
    public partial class InitialProdlang : Migration
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
                    { new Guid("52ffbdd2-784c-47fd-b0a8-954c19d1dc0e"), "e0c069e3-b1fc-4c2c-afca-229a5452c076", "InitUser", "INITUSER" },
                    { new Guid("22bd81fb-f232-48b7-a36d-97ed7fe9776a"), "3a6c3f86-6b4c-4660-afea-54bf599c2504", "Editor", "EDITOR" },
                    { new Guid("cf29c56b-07b6-4a56-b1dc-ba680676a416"), "498b1e83-19e2-4b3f-921d-8cb357855c69", "Reviewer", "REVIEWER" },
                    { new Guid("38f07190-d0e7-426d-9dbf-9435f6d96f67"), "e877b2dd-c7b2-4458-9a36-7351b7c73731", "Admin", "ADMIN" },
                    { new Guid("11d9aa1e-b9d3-4fea-ab6a-16c8380e30b1"), "473e7648-d500-4d3e-8260-c2c7d658ef82", "Creater", "CREATER" }
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
