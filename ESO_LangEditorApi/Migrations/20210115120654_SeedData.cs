using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESO_LangEditor.API.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LangtextArchive",
                columns: new[] { "Id", "ArchiveTimestamp", "EnLastModifyTimestamp", "IdType", "IsTranslated", "LangTextType", "ReviewTimestamp", "ReviewerId", "TextEn", "TextId", "TextZh", "UpdateStats", "UserId", "ZhLastModifyTimestamp" },
                values: new object[,]
                {
                    { new Guid("d05c0e82-025e-4e31-97ae-b8858ab0a784"), new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(5482), new DateTime(2020, 11, 2, 21, 48, 38, 613, DateTimeKind.Unspecified).AddTicks(4703), 103224356, (byte)2, (byte)4, new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(4826), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "We need to meet with General Renmus as soon as possible. To do this, I must bribe his clerk, obtain forged credentials, and delay the merchant who is currently scheduled to meet with him.", "103224356-0-51125", "我们要尽快与瑞摩斯将军会面。为此，我要贿赂他的雇员，获取伪造的凭证，然后去拖延本应和他会面的商人。", "Update24", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 16, 20, 53, 23, 487, DateTimeKind.Unspecified).AddTicks(1752) },
                    { new Guid("08d42f95-66ff-4bcb-adc7-b395f436086c"), new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(6215), new DateTime(2020, 11, 2, 21, 48, 38, 626, DateTimeKind.Unspecified).AddTicks(4609), 139139780, (byte)2, (byte)4, new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(6201), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "A manifest of items shipped by Arniel Branck.", "139139780-0-7300", "阿尼尔·布兰克运输货品的清单。", "Update25", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 16, 21, 51, 37, 734, DateTimeKind.Unspecified).AddTicks(8102) },
                    { new Guid("ba652528-900f-437b-8832-eb6d387ad010"), new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(6244), new DateTime(2020, 11, 2, 21, 48, 38, 673, DateTimeKind.Unspecified).AddTicks(397), 115740052, (byte)2, (byte)4, new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(6243), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "I don't see Nartise. Let's move on.", "115740052-0-35623", "没看见纳蒂斯，我们继续。", "Update24", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 16, 22, 20, 37, 622, DateTimeKind.Unspecified).AddTicks(9099) }
                });

            migrationBuilder.InsertData(
                table: "LangtextReview",
                columns: new[] { "Id", "EnLastModifyTimestamp", "IdType", "IsTranslated", "LangTextType", "ReviewTimestamp", "ReviewerId", "TextEn", "TextId", "TextZh", "UpdateStats", "UserId", "ZhLastModifyTimestamp" },
                values: new object[,]
                {
                    { new Guid("9a04bae0-b14a-4015-9b51-f0f04137000a"), new DateTime(2020, 11, 2, 22, 14, 22, 325, DateTimeKind.Unspecified).AddTicks(5292), 100, (byte)2, (byte)3, new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(1637), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "Destruction Staff", "SI_EQUIPMENTFILTERTYPE9", "毁灭法杖", "Update28", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 19, 16, 33, 30, 2, DateTimeKind.Unspecified).AddTicks(4484) },
                    { new Guid("5a1a90b8-183f-4d78-a76b-a00ed7889b84"), new DateTime(2020, 11, 2, 22, 14, 22, 325, DateTimeKind.Unspecified).AddTicks(6276), 100, (byte)2, (byte)2, new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(2423), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "All Materials", "SI_GAMEPAD_ALCHEMY_ALL_MATERIALS", "所有材料", "Update28", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 19, 16, 10, 59, 646, DateTimeKind.Unspecified).AddTicks(7889) },
                    { new Guid("c9b3970d-e7d5-4290-939a-563466f13203"), new DateTime(2020, 11, 2, 21, 48, 38, 582, DateTimeKind.Unspecified).AddTicks(6411), 10860933, (byte)2, (byte)4, new DateTime(2021, 1, 15, 20, 6, 53, 943, DateTimeKind.Local).AddTicks(2457), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "Dungeon: Selene's Web", "10860933-0-964", "组队副本：瑟琳之网", "Update24", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 21, 18, 46, 32, 775, DateTimeKind.Unspecified).AddTicks(4132) }
                });

            migrationBuilder.InsertData(
                table: "Langtexts",
                columns: new[] { "Id", "EnLastModifyTimestamp", "IdType", "IsTranslated", "LangTextType", "ReviewTimestamp", "ReviewerId", "TextEn", "TextId", "TextZh", "UpdateStats", "UserId", "ZhLastModifyTimestamp" },
                values: new object[,]
                {
                    { new Guid("7639d924-b155-4f58-b45e-ff87bf0dba9b"), new DateTime(2020, 11, 2, 21, 48, 38, 593, DateTimeKind.Unspecified).AddTicks(6908), 115740052, (byte)2, (byte)4, new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(4879), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "Whatever you say! I'll see what I can find out.", "115740052-0-11491", "按你说的办！我去看看我能发现什么。", "Update24", new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), new DateTime(2020, 11, 17, 8, 55, 12, 335, DateTimeKind.Unspecified).AddTicks(8170) },
                    { new Guid("3d284691-b117-44ea-bdfa-52e19b7e8612"), new DateTime(2020, 11, 2, 21, 48, 38, 684, DateTimeKind.Unspecified).AddTicks(6146), 200879108, (byte)2, (byte)4, new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5727), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "No. Doesn't have a clue. That's what makes it so exciting!\\n\\nThis is an honest-to-goodness secret mission! I always wanted to go on a secret mission! Besides, once you talk to Solgra, you'll understand. She explains things much better than I do.", "200879108-0-40943", "不，不知道怎么回事。这才是让人激动的地方！\\n\\n这可是件货真价实的秘密任务！我一直希望要参与这样的任务来着！还有，你和索尔葛拉交谈的时候就会懂了。她解释起东西来比我好多了。", "Update24", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 17, 8, 33, 10, 457, DateTimeKind.Unspecified).AddTicks(8998) },
                    { new Guid("7b0a678d-04d1-4442-86ab-e7c3ffb207e0"), new DateTime(2020, 11, 2, 21, 48, 38, 684, DateTimeKind.Unspecified).AddTicks(6233), 200879108, (byte)2, (byte)4, new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5763), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "A well? Yeah, I saw the old well. We'll need a rope or something to climb down, though.\\n\\nCheck near the forge. I noticed tools and ropes hanging around. One of those ropes should serve our needs.", "200879108-0-40960", "一口井？对，我看到那口老井了。我们需要绳子之类的才能爬下去。\\n\\n看看熔炉附近，我发现了一些工具和绳子挂在周围。应该会有绳子供我们所需。", "Update24", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 17, 9, 15, 31, 590, DateTimeKind.Unspecified).AddTicks(1130) },
                    { new Guid("4ca5183a-f0dd-4f6f-b664-58a647df535c"), new DateTime(2020, 11, 2, 21, 48, 38, 684, DateTimeKind.Unspecified).AddTicks(6887), 200879108, (byte)2, (byte)4, new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5782), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "Have you seen my bow skills? I'm sure Solgra noticed and said to herself, \"That's a fine young lass! She can put a shaft through an apple balanced on the back of a moving wamasu. I shall recruit her!\"\\n\\nEither that or she was desperate for some help.", "200879108-0-40993", "你没看到我的弓术吗？我确定索尔葛拉注意到了，还自言自语着“那个姑娘可真厉害！她可以射穿放在一只移动中的雷霆蜥蜴背上的苹果，我应该招募她！“\\n\\n或者也可能是她实在是无处求救了。", "Update24", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 17, 8, 37, 34, 984, DateTimeKind.Unspecified).AddTicks(7945) },
                    { new Guid("6cfaec5e-be72-4537-890b-696e5cd33b09"), new DateTime(2020, 11, 2, 21, 48, 38, 632, DateTimeKind.Unspecified).AddTicks(4910), 132143172, (byte)2, (byte)4, new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5798), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "Devastate an enemy with an enhanced charge from your staff, dealing <<1>> and an additional <<2>> over <<3>>.", "132143172-0-29078", "以法杖的强化力量破坏敌人， 造成 <<1>> 并在<<3>>内造成额外<<2>> 。", "Update24", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 10, 1, 50, 9, 975, DateTimeKind.Unspecified).AddTicks(2877) },
                    { new Guid("0762d175-5a6f-43b4-9dea-b9c28d5d4b0e"), new DateTime(2020, 11, 2, 21, 48, 38, 714, DateTimeKind.Unspecified).AddTicks(4141), 198758357, (byte)2, (byte)4, new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5815), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "Brutal Carnage", "198758357-0-137184", "残暴杀戮", "Update26", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 7, 6, 35, 2, 791, DateTimeKind.Unspecified).AddTicks(5322) },
                    { new Guid("cd017b93-0d0d-4c35-8bfc-9e82665ec817"), new DateTime(2020, 11, 2, 21, 48, 38, 132, DateTimeKind.Unspecified).AddTicks(9690), 8290981, (byte)2, (byte)4, new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5833), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "Za'ji^M", "8290981-0-91160", "扎'吉^M", "Update24", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 16, 20, 6, 42, 494, DateTimeKind.Unspecified).AddTicks(8770) },
                    { new Guid("de09f1b2-7a2c-4bbc-8f16-b5fde4317d84"), new DateTime(2020, 11, 2, 21, 48, 37, 913, DateTimeKind.Unspecified).AddTicks(8204), 242841733, (byte)2, (byte)4, new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5850), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "Restoration Staff of Kyne's Wind", "242841733-0-162450", "凯娜之风治疗法杖", "Update26", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 19, 16, 36, 22, 946, DateTimeKind.Unspecified).AddTicks(4880) },
                    { new Guid("d7480da5-602f-4b5e-8a03-5a95fbd34c1d"), new DateTime(2020, 11, 2, 22, 14, 22, 325, DateTimeKind.Unspecified).AddTicks(5283), 100, (byte)2, (byte)3, new DateTime(2021, 1, 15, 20, 6, 53, 941, DateTimeKind.Local).AddTicks(5866), new Guid("8475b578-80f4-4ed0-ae41-c32a45d6d9e6"), "Two-Handed Melee", "SI_EQUIPMENTFILTERTYPE8", "双手武器近战", "Update28", new Guid("18f825ff-46f5-4a1a-9b10-56be5fb2398d"), new DateTime(2020, 11, 19, 16, 28, 39, 667, DateTimeKind.Unspecified).AddTicks(4052) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LangtextArchive",
                keyColumn: "Id",
                keyValue: new Guid("08d42f95-66ff-4bcb-adc7-b395f436086c"));

            migrationBuilder.DeleteData(
                table: "LangtextArchive",
                keyColumn: "Id",
                keyValue: new Guid("ba652528-900f-437b-8832-eb6d387ad010"));

            migrationBuilder.DeleteData(
                table: "LangtextArchive",
                keyColumn: "Id",
                keyValue: new Guid("d05c0e82-025e-4e31-97ae-b8858ab0a784"));

            migrationBuilder.DeleteData(
                table: "LangtextReview",
                keyColumn: "Id",
                keyValue: new Guid("5a1a90b8-183f-4d78-a76b-a00ed7889b84"));

            migrationBuilder.DeleteData(
                table: "LangtextReview",
                keyColumn: "Id",
                keyValue: new Guid("9a04bae0-b14a-4015-9b51-f0f04137000a"));

            migrationBuilder.DeleteData(
                table: "LangtextReview",
                keyColumn: "Id",
                keyValue: new Guid("c9b3970d-e7d5-4290-939a-563466f13203"));

            migrationBuilder.DeleteData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("0762d175-5a6f-43b4-9dea-b9c28d5d4b0e"));

            migrationBuilder.DeleteData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("3d284691-b117-44ea-bdfa-52e19b7e8612"));

            migrationBuilder.DeleteData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("4ca5183a-f0dd-4f6f-b664-58a647df535c"));

            migrationBuilder.DeleteData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("6cfaec5e-be72-4537-890b-696e5cd33b09"));

            migrationBuilder.DeleteData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("7639d924-b155-4f58-b45e-ff87bf0dba9b"));

            migrationBuilder.DeleteData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("7b0a678d-04d1-4442-86ab-e7c3ffb207e0"));

            migrationBuilder.DeleteData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("cd017b93-0d0d-4c35-8bfc-9e82665ec817"));

            migrationBuilder.DeleteData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("d7480da5-602f-4b5e-8a03-5a95fbd34c1d"));

            migrationBuilder.DeleteData(
                table: "Langtexts",
                keyColumn: "Id",
                keyValue: new Guid("de09f1b2-7a2c-4bbc-8f16-b5fde4317d84"));
        }
    }
}
