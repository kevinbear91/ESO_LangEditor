using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESO_LangEditor.API.Migrations
{
    public partial class AddlangUserRef3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LangtextArchive",
                keyColumn: "Id",
                keyValue: new Guid("08d42f95-66ff-4bcb-adc7-b395f436086c"),
                columns: new[] { "ArchiveTimestamp", "ReviewTimestamp" },
                values: new object[] { new DateTime(2021, 3, 28, 0, 5, 44, 839, DateTimeKind.Local).AddTicks(63), new DateTime(2021, 3, 28, 0, 5, 44, 839, DateTimeKind.Local).AddTicks(32) });

            migrationBuilder.UpdateData(
                table: "LangtextArchive",
                keyColumn: "Id",
                keyValue: new Guid("ba652528-900f-437b-8832-eb6d387ad010"),
                columns: new[] { "ArchiveTimestamp", "ReviewTimestamp" },
                values: new object[] { new DateTime(2021, 3, 28, 0, 5, 44, 839, DateTimeKind.Local).AddTicks(95), new DateTime(2021, 3, 28, 0, 5, 44, 839, DateTimeKind.Local).AddTicks(93) });

            migrationBuilder.UpdateData(
                table: "LangtextArchive",
                keyColumn: "Id",
                keyValue: new Guid("d05c0e82-025e-4e31-97ae-b8858ab0a784"),
                columns: new[] { "ArchiveTimestamp", "ReviewTimestamp" },
                values: new object[] { new DateTime(2021, 3, 28, 0, 5, 44, 838, DateTimeKind.Local).AddTicks(9286), new DateTime(2021, 3, 28, 0, 5, 44, 838, DateTimeKind.Local).AddTicks(8596) });

            migrationBuilder.UpdateData(
                table: "LangtextReview",
                keyColumn: "Id",
                keyValue: new Guid("5a1a90b8-183f-4d78-a76b-a00ed7889b84"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 5, 44, 838, DateTimeKind.Local).AddTicks(6606));

            migrationBuilder.UpdateData(
                table: "LangtextReview",
                keyColumn: "Id",
                keyValue: new Guid("9a04bae0-b14a-4015-9b51-f0f04137000a"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 5, 44, 838, DateTimeKind.Local).AddTicks(5810));

            migrationBuilder.UpdateData(
                table: "LangtextReview",
                keyColumn: "Id",
                keyValue: new Guid("c9b3970d-e7d5-4290-939a-563466f13203"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 5, 44, 838, DateTimeKind.Local).AddTicks(6646));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("0762d175-5a6f-43b4-9dea-b9c28d5d4b0e"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 5, 44, 837, DateTimeKind.Local).AddTicks(246));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("3d284691-b117-44ea-bdfa-52e19b7e8612"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 5, 44, 837, DateTimeKind.Local).AddTicks(155));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("4ca5183a-f0dd-4f6f-b664-58a647df535c"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 5, 44, 837, DateTimeKind.Local).AddTicks(213));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("6cfaec5e-be72-4537-890b-696e5cd33b09"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 5, 44, 837, DateTimeKind.Local).AddTicks(231));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("7639d924-b155-4f58-b45e-ff87bf0dba9b"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 5, 44, 836, DateTimeKind.Local).AddTicks(9279));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("7b0a678d-04d1-4442-86ab-e7c3ffb207e0"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 5, 44, 837, DateTimeKind.Local).AddTicks(194));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("cd017b93-0d0d-4c35-8bfc-9e82665ec817"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 5, 44, 837, DateTimeKind.Local).AddTicks(262));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("d7480da5-602f-4b5e-8a03-5a95fbd34c1d"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 5, 44, 837, DateTimeKind.Local).AddTicks(295));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("de09f1b2-7a2c-4bbc-8f16-b5fde4317d84"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 5, 44, 837, DateTimeKind.Local).AddTicks(279));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LangtextArchive",
                keyColumn: "Id",
                keyValue: new Guid("08d42f95-66ff-4bcb-adc7-b395f436086c"),
                columns: new[] { "ArchiveTimestamp", "ReviewTimestamp" },
                values: new object[] { new DateTime(2021, 3, 28, 0, 2, 11, 420, DateTimeKind.Local).AddTicks(1095), new DateTime(2021, 3, 28, 0, 2, 11, 420, DateTimeKind.Local).AddTicks(1082) });

            migrationBuilder.UpdateData(
                table: "LangtextArchive",
                keyColumn: "Id",
                keyValue: new Guid("ba652528-900f-437b-8832-eb6d387ad010"),
                columns: new[] { "ArchiveTimestamp", "ReviewTimestamp" },
                values: new object[] { new DateTime(2021, 3, 28, 0, 2, 11, 420, DateTimeKind.Local).AddTicks(1127), new DateTime(2021, 3, 28, 0, 2, 11, 420, DateTimeKind.Local).AddTicks(1125) });

            migrationBuilder.UpdateData(
                table: "LangtextArchive",
                keyColumn: "Id",
                keyValue: new Guid("d05c0e82-025e-4e31-97ae-b8858ab0a784"),
                columns: new[] { "ArchiveTimestamp", "ReviewTimestamp" },
                values: new object[] { new DateTime(2021, 3, 28, 0, 2, 11, 420, DateTimeKind.Local).AddTicks(354), new DateTime(2021, 3, 28, 0, 2, 11, 419, DateTimeKind.Local).AddTicks(9687) });

            migrationBuilder.UpdateData(
                table: "LangtextReview",
                keyColumn: "Id",
                keyValue: new Guid("5a1a90b8-183f-4d78-a76b-a00ed7889b84"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 2, 11, 419, DateTimeKind.Local).AddTicks(7290));

            migrationBuilder.UpdateData(
                table: "LangtextReview",
                keyColumn: "Id",
                keyValue: new Guid("9a04bae0-b14a-4015-9b51-f0f04137000a"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 2, 11, 419, DateTimeKind.Local).AddTicks(6526));

            migrationBuilder.UpdateData(
                table: "LangtextReview",
                keyColumn: "Id",
                keyValue: new Guid("c9b3970d-e7d5-4290-939a-563466f13203"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 2, 11, 419, DateTimeKind.Local).AddTicks(7331));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("0762d175-5a6f-43b4-9dea-b9c28d5d4b0e"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 2, 11, 418, DateTimeKind.Local).AddTicks(1340));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("3d284691-b117-44ea-bdfa-52e19b7e8612"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 2, 11, 418, DateTimeKind.Local).AddTicks(1245));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("4ca5183a-f0dd-4f6f-b664-58a647df535c"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 2, 11, 418, DateTimeKind.Local).AddTicks(1304));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("6cfaec5e-be72-4537-890b-696e5cd33b09"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 2, 11, 418, DateTimeKind.Local).AddTicks(1321));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("7639d924-b155-4f58-b45e-ff87bf0dba9b"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 2, 11, 418, DateTimeKind.Local).AddTicks(379));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("7b0a678d-04d1-4442-86ab-e7c3ffb207e0"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 2, 11, 418, DateTimeKind.Local).AddTicks(1284));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("cd017b93-0d0d-4c35-8bfc-9e82665ec817"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 2, 11, 418, DateTimeKind.Local).AddTicks(1357));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("d7480da5-602f-4b5e-8a03-5a95fbd34c1d"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 2, 11, 418, DateTimeKind.Local).AddTicks(1390));

            migrationBuilder.UpdateData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("de09f1b2-7a2c-4bbc-8f16-b5fde4317d84"),
                column: "ReviewTimestamp",
                value: new DateTime(2021, 3, 28, 0, 2, 11, 418, DateTimeKind.Local).AddTicks(1374));
        }
    }
}
