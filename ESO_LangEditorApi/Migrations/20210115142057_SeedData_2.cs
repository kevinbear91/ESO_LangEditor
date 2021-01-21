using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESO_LangEditor.API.Migrations
{
    public partial class SeedData_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LangtextArchive",
                keyColumn: "Id",
                keyValue: new Guid("08d42f95-66ff-4bcb-adc7-b395f436086c"),
                columns: new[] { "ArchiveTimestamp", "ReviewTimestamp" },
                values: new object[] { new DateTime(2021, 1, 15, 22, 20, 56, 713, DateTimeKind.Local).AddTicks(2472), new DateTime(2021, 1, 15, 22, 20, 56, 713, DateTimeKind.Local).AddTicks(2459) });

            migrationBuilder.UpdateData(
                table: "LangtextArchive",
                keyColumn: "Id",
                keyValue: new Guid("ba652528-900f-437b-8832-eb6d387ad010"),
                columns: new[] { "ArchiveTimestamp", "ReviewTimestamp" },
                values: new object[] { new DateTime(2021, 1, 15, 22, 20, 56, 713, DateTimeKind.Local).AddTicks(2504), new DateTime(2021, 1, 15, 22, 20, 56, 713, DateTimeKind.Local).AddTicks(2501) });

            migrationBuilder.UpdateData(
                table: "LangtextArchive",
                keyColumn: "Id",
                keyValue: new Guid("d05c0e82-025e-4e31-97ae-b8858ab0a784"),
                columns: new[] { "ArchiveTimestamp", "ReviewTimestamp" },
                values: new object[] { new DateTime(2021, 1, 15, 22, 20, 56, 713, DateTimeKind.Local).AddTicks(1728), new DateTime(2021, 1, 15, 22, 20, 56, 713, DateTimeKind.Local).AddTicks(1074) });

            migrationBuilder.UpdateData(
                table: "LangtextReview",
                keyColumn: "Id",
                keyValue: new Guid("5a1a90b8-183f-4d78-a76b-a00ed7889b84"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 22, 20, 56, 712, DateTimeKind.Local).AddTicks(8648));

            migrationBuilder.UpdateData(
                table: "LangtextReview",
                keyColumn: "Id",
                keyValue: new Guid("9a04bae0-b14a-4015-9b51-f0f04137000a"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 22, 20, 56, 712, DateTimeKind.Local).AddTicks(7862));

            migrationBuilder.UpdateData(
                table: "LangtextReview",
                keyColumn: "Id",
                keyValue: new Guid("c9b3970d-e7d5-4290-939a-563466f13203"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 22, 20, 56, 712, DateTimeKind.Local).AddTicks(8685));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("0762d175-5a6f-43b4-9dea-b9c28d5d4b0e"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 22, 20, 56, 711, DateTimeKind.Local).AddTicks(2168));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("3d284691-b117-44ea-bdfa-52e19b7e8612"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 22, 20, 56, 711, DateTimeKind.Local).AddTicks(2075));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("4ca5183a-f0dd-4f6f-b664-58a647df535c"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 22, 20, 56, 711, DateTimeKind.Local).AddTicks(2133));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("6cfaec5e-be72-4537-890b-696e5cd33b09"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 22, 20, 56, 711, DateTimeKind.Local).AddTicks(2151));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("7639d924-b155-4f58-b45e-ff87bf0dba9b"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 22, 20, 56, 711, DateTimeKind.Local).AddTicks(1210));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("7b0a678d-04d1-4442-86ab-e7c3ffb207e0"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 22, 20, 56, 711, DateTimeKind.Local).AddTicks(2113));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("cd017b93-0d0d-4c35-8bfc-9e82665ec817"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 22, 20, 56, 711, DateTimeKind.Local).AddTicks(2185));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("d7480da5-602f-4b5e-8a03-5a95fbd34c1d"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 22, 20, 56, 711, DateTimeKind.Local).AddTicks(2218));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("de09f1b2-7a2c-4bbc-8f16-b5fde4317d84"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 22, 20, 56, 711, DateTimeKind.Local).AddTicks(2201));

            migrationBuilder.InsertData(
                table: "Langtexts",
                columns: new[] { "Id", "EnLastModifyTimestamp", "IdType", "IsTranslated", "LangTextType", "ReviewTimestamp", "ReviewerId", "TextEn", "TextId", "TextZh", "UpdateStats", "UserId", "ZhLastModifyTimestamp" },
                values: new object[,]
                {
                    { new Guid("c9b3970d-e7d5-4290-939a-563466f13203"), new DateTime(2020, 11, 2, 21, 48, 38, 582, DateTimeKind.Unspecified).AddTicks(6411), 10860933, (byte)2, (byte)4, new DateTime(2020, 11, 2, 21, 48, 38, 582, DateTimeKind.Unspecified).AddTicks(6411), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "Dungeon: Selene's Web", "10860933-0-964", "组队副本：瑟琳之网", "Update24", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 21, 18, 46, 32, 775, DateTimeKind.Unspecified).AddTicks(4132) },
                    { new Guid("5a1a90b8-183f-4d78-a76b-a00ed7889b84"), new DateTime(2020, 11, 2, 22, 14, 22, 325, DateTimeKind.Unspecified).AddTicks(6276), 100, (byte)2, (byte)2, new DateTime(2020, 11, 2, 22, 14, 22, 325, DateTimeKind.Unspecified).AddTicks(6276), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "All Materials", "SI_GAMEPAD_ALCHEMY_ALL_MATERIALS", "所有材料", "Update28", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 19, 16, 10, 59, 646, DateTimeKind.Unspecified).AddTicks(7889) },
                    { new Guid("9a04bae0-b14a-4015-9b51-f0f04137000a"), new DateTime(2020, 11, 2, 22, 14, 22, 325, DateTimeKind.Unspecified).AddTicks(5292), 100, (byte)2, (byte)3, new DateTime(2020, 11, 2, 22, 14, 22, 325, DateTimeKind.Unspecified).AddTicks(5292), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "Destruction Staff", "SI_EQUIPMENTFILTERTYPE9", "毁灭法杖", "Update28", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 19, 16, 33, 30, 2, DateTimeKind.Unspecified).AddTicks(4484) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("5a1a90b8-183f-4d78-a76b-a00ed7889b84"));

            migrationBuilder.DeleteData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("9a04bae0-b14a-4015-9b51-f0f04137000a"));

            migrationBuilder.DeleteData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("c9b3970d-e7d5-4290-939a-563466f13203"));

            migrationBuilder.UpdateData(
                table: "LangtextArchive",
                keyColumn: "Id",
                keyValue: new Guid("08d42f95-66ff-4bcb-adc7-b395f436086c"),
                columns: new[] { "ArchiveTimestamp", "ReviewTimestamp" },
                values: new object[] { new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(6215), new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(6201) });

            migrationBuilder.UpdateData(
                table: "LangtextArchive",
                keyColumn: "Id",
                keyValue: new Guid("ba652528-900f-437b-8832-eb6d387ad010"),
                columns: new[] { "ArchiveTimestamp", "ReviewTimestamp" },
                values: new object[] { new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(6244), new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(6243) });

            migrationBuilder.UpdateData(
                table: "LangtextArchive",
                keyColumn: "Id",
                keyValue: new Guid("d05c0e82-025e-4e31-97ae-b8858ab0a784"),
                columns: new[] { "ArchiveTimestamp", "ReviewTimestamp" },
                values: new object[] { new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(5482), new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(4826) });

            migrationBuilder.UpdateData(
                table: "LangtextReview",
                keyColumn: "Id",
                keyValue: new Guid("5a1a90b8-183f-4d78-a76b-a00ed7889b84"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(2423));

            migrationBuilder.UpdateData(
                table: "LangtextReview",
                keyColumn: "Id",
                keyValue: new Guid("9a04bae0-b14a-4015-9b51-f0f04137000a"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(1637));

            migrationBuilder.UpdateData(
                table: "LangtextReview",
                keyColumn: "Id",
                keyValue: new Guid("c9b3970d-e7d5-4290-939a-563466f13203"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(2457));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("0762d175-5a6f-43b4-9dea-b9c28d5d4b0e"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5815));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("3d284691-b117-44ea-bdfa-52e19b7e8612"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5727));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("4ca5183a-f0dd-4f6f-b664-58a647df535c"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5782));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("6cfaec5e-be72-4537-890b-696e5cd33b09"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5798));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("7639d924-b155-4f58-b45e-ff87bf0dba9b"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(4879));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("7b0a678d-04d1-4442-86ab-e7c3ffb207e0"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5763));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("cd017b93-0d0d-4c35-8bfc-9e82665ec817"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5833));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("d7480da5-602f-4b5e-8a03-5a95fbd34c1d"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5866));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("de09f1b2-7a2c-4bbc-8f16-b5fde4317d84"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5850));
        }
    }
}
