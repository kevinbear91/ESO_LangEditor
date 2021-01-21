using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.EFCore.TestData
{
    public static class ModelBuilderExtension
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LangText>().HasData(
                new LangText
                {
                    Id = new Guid("7639d924-b155-4f58-b45e-ff87bf0dba9b"),
                    TextId = "115740052-0-11491",
                    IdType = 115740052,
                    TextEn = "Whatever you say! I'll see what I can find out.",
                    TextZh = "按你说的办！我去看看我能发现什么。",
                    LangTextType = LangType.LangText,
                    IsTranslated = 2,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T21:48:38.5936908"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-17T08:55:12.335817"),
                    UserId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                },
                new LangText
                {
                    Id = new Guid("3d284691-b117-44ea-bdfa-52e19b7e8612"),
                    TextId = "200879108-0-40943",
                    IdType = 200879108,
                    TextEn = "No. Doesn\u0027t have a clue. That\u0027s what makes it so exciting!\\n\\nThis is an honest-to-goodness secret mission! I always wanted to go on a secret mission! Besides, once you talk to Solgra, you\u0027ll understand. She explains things much better than I do.",
                    TextZh = "不，不知道怎么回事。这才是让人激动的地方！\\n\\n这可是件货真价实的秘密任务！我一直希望要参与这样的任务来着！还有，你和索尔葛拉交谈的时候就会懂了。她解释起东西来比我好多了。",
                    LangTextType = LangType.LangText,
                    IsTranslated = 2,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T21:48:38.6846146"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-17T08:33:10.4578998"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                },
                new LangText
                {
                    Id = new Guid("7b0a678d-04d1-4442-86ab-e7c3ffb207e0"),
                    TextId = "200879108-0-40960",
                    IdType = 200879108,
                    TextEn = "A well? Yeah, I saw the old well. We\u0027ll need a rope or something to climb down, though.\\n\\nCheck near the forge. I noticed tools and ropes hanging around. One of those ropes should serve our needs.",
                    TextZh = "一口井？对，我看到那口老井了。我们需要绳子之类的才能爬下去。\\n\\n看看熔炉附近，我发现了一些工具和绳子挂在周围。应该会有绳子供我们所需。",
                    LangTextType = LangType.LangText,
                    IsTranslated = 2,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T21:48:38.6846233"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-17T09:15:31.590113"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                },
                new LangText
                {
                    Id = new Guid("4ca5183a-f0dd-4f6f-b664-58a647df535c"),
                    TextId = "200879108-0-40993",
                    IdType = 200879108,
                    TextEn = "Have you seen my bow skills? I\u0027m sure Solgra noticed and said to herself, \u0022That\u0027s a fine young lass! She can put a shaft through an apple balanced on the back of a moving wamasu. I shall recruit her!\u0022\\n\\nEither that or she was desperate for some help.",
                    TextZh = "你没看到我的弓术吗？我确定索尔葛拉注意到了，还自言自语着“那个姑娘可真厉害！她可以射穿放在一只移动中的雷霆蜥蜴背上的苹果，我应该招募她！“\\n\\n或者也可能是她实在是无处求救了。",
                    LangTextType = LangType.LangText,
                    IsTranslated = 2,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T21:48:38.6846887"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-17T08:37:34.9847945"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                },
                new LangText
                {
                    Id = new Guid("6cfaec5e-be72-4537-890b-696e5cd33b09"),
                    TextId = "132143172-0-29078",
                    IdType = 132143172,
                    TextEn = "Devastate an enemy with an enhanced charge from your staff, dealing \u003C\u003C1\u003E\u003E and an additional \u003C\u003C2\u003E\u003E over \u003C\u003C3\u003E\u003E.",
                    TextZh = "以法杖的强化力量破坏敌人， 造成 \u003C\u003C1\u003E\u003E 并在\u003C\u003C3\u003E\u003E内造成额外\u003C\u003C2\u003E\u003E 。",
                    LangTextType = LangType.LangText,
                    IsTranslated = 2,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T21:48:38.632491"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-10T01:50:09.9752877"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                },
                new LangText
                {
                    Id = new Guid("0762d175-5a6f-43b4-9dea-b9c28d5d4b0e"),
                    TextId = "198758357-0-137184",
                    IdType = 198758357,
                    TextEn = "Brutal Carnage",
                    TextZh = "残暴杀戮",
                    LangTextType = LangType.LangText,
                    IsTranslated = 2,
                    UpdateStats = "Update26",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T21:48:38.7144141"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-07T06:35:02.7915322"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                },
                new LangText
                {
                    Id = new Guid("cd017b93-0d0d-4c35-8bfc-9e82665ec817"),
                    TextId = "8290981-0-91160",
                    IdType = 8290981,
                    TextEn = "Za\u0027ji^M",
                    TextZh = "扎\u0027吉^M",
                    LangTextType = LangType.LangText,
                    IsTranslated = 2,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T21:48:38.132969"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-16T20:06:42.494877"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                },
                new LangText
                {
                    Id = new Guid("de09f1b2-7a2c-4bbc-8f16-b5fde4317d84"),
                    TextId = "242841733-0-162450",
                    IdType = 242841733,
                    TextEn = "Restoration Staff of Kyne\u0027s Wind",
                    TextZh = "凯娜之风治疗法杖",
                    LangTextType = LangType.LangText,
                    IsTranslated = 2,
                    UpdateStats = "Update26",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T21:48:37.9138204"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-19T16:36:22.946488"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                },
                new LangText
                {
                    Id = new Guid("d7480da5-602f-4b5e-8a03-5a95fbd34c1d"),
                    TextId = "SI_EQUIPMENTFILTERTYPE8",
                    IdType = 100,
                    TextEn = "Two-Handed Melee",
                    TextZh = "双手武器近战",
                    LangTextType = LangType.LuaBoth,
                    IsTranslated = 2,
                    UpdateStats = "Update28",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T22:14:22.3255283"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-19T16:28:39.6674052"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                },
                new LangText
                {
                    Id = new Guid("9a04bae0-b14a-4015-9b51-f0f04137000a"),
                    TextId = "SI_EQUIPMENTFILTERTYPE9",
                    IdType = 100,
                    TextEn = "Destruction Staff",
                    TextZh = "毁灭法杖",
                    LangTextType = LangType.LuaBoth,
                    IsTranslated = 2,
                    UpdateStats = "Update28",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T22:14:22.3255292"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-19T16:33:30.0024484"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Parse("2020-11-02T22:14:22.3255292"),
                },
                new LangText
                {
                    Id = new Guid("5a1a90b8-183f-4d78-a76b-a00ed7889b84"),
                    TextId = "SI_GAMEPAD_ALCHEMY_ALL_MATERIALS",
                    IdType = 100,
                    TextEn = "All Materials",
                    TextZh = "所有材料",
                    LangTextType = LangType.LuaClient,
                    IsTranslated = 2,
                    UpdateStats = "Update28",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T22:14:22.3256276"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-19T16:10:59.6467889"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Parse("2020-11-02T22:14:22.3256276"),
                },
                new LangText
                {
                    Id = new Guid("c9b3970d-e7d5-4290-939a-563466f13203"),
                    TextId = "10860933-0-964",
                    IdType = 10860933,
                    TextEn = "Dungeon: Selene\u0027s Web",
                    TextZh = "组队副本：瑟琳之网",
                    LangTextType = LangType.LangText,
                    IsTranslated = 2,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T21:48:38.5826411"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-21T18:46:32.7754132"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Parse("2020-11-02T21:48:38.5826411"),
                }
                );
            modelBuilder.Entity<LangTextReview>().HasData(
                new LangTextReview 
                {
                    Id = new Guid("9a04bae0-b14a-4015-9b51-f0f04137000a"),
                    TextId = "SI_EQUIPMENTFILTERTYPE9",
                    IdType = 100,
                    TextEn = "Destruction Staff",
                    TextZh = "毁灭法杖",
                    LangTextType = LangType.LuaBoth,
                    IsTranslated = 2,
                    UpdateStats = "Update28",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T22:14:22.3255292"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-19T16:33:30.0024484"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                },
                new LangTextReview
                {
                    Id = new Guid("5a1a90b8-183f-4d78-a76b-a00ed7889b84"),
                    TextId = "SI_GAMEPAD_ALCHEMY_ALL_MATERIALS",
                    IdType = 100,
                    TextEn = "All Materials",
                    TextZh = "所有材料",
                    LangTextType = LangType.LuaClient,
                    IsTranslated = 2,
                    UpdateStats = "Update28",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T22:14:22.3256276"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-19T16:10:59.6467889"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                },
                new LangTextReview
                {
                    Id = new Guid("c9b3970d-e7d5-4290-939a-563466f13203"),
                    TextId = "10860933-0-964",
                    IdType = 10860933,
                    TextEn = "Dungeon: Selene\u0027s Web",
                    TextZh = "组队副本：瑟琳之网",
                    LangTextType = LangType.LangText,
                    IsTranslated = 2,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T21:48:38.5826411"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-21T18:46:32.7754132"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                }
                );
            modelBuilder.Entity<LangTextArchive>().HasData(
                new LangTextArchive
                {
                    Id = new Guid("d05c0e82-025e-4e31-97ae-b8858ab0a784"),
                    TextId = "103224356-0-51125",
                    IdType = 103224356,
                    TextEn = "We need to meet with General Renmus as soon as possible. To do this, I must bribe his clerk, obtain forged credentials, and delay the merchant who is currently scheduled to meet with him.",
                    TextZh = "我们要尽快与瑞摩斯将军会面。为此，我要贿赂他的雇员，获取伪造的凭证，然后去拖延本应和他会面的商人。",
                    LangTextType = LangType.LangText,
                    IsTranslated = 2,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T21:48:38.6134703"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-16T20:53:23.4871752"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                    ArchiveTimestamp = DateTime.Now,
                },
                new LangTextArchive
                {
                    Id = new Guid("08d42f95-66ff-4bcb-adc7-b395f436086c"),
                    TextId = "139139780-0-7300",
                    IdType = 139139780,
                    TextEn = "A manifest of items shipped by Arniel Branck.",
                    TextZh = "阿尼尔·布兰克运输货品的清单。",
                    LangTextType = LangType.LangText,
                    IsTranslated = 2,
                    UpdateStats = "Update25",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T21:48:38.6264609"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-16T21:51:37.7348102"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                    ArchiveTimestamp = DateTime.Now,
                },
                new LangTextArchive
                {
                    Id = new Guid("ba652528-900f-437b-8832-eb6d387ad010"),
                    TextId = "115740052-0-35623",
                    IdType = 115740052,
                    TextEn = "I don\u0027t see Nartise. Let\u0027s move on.",
                    TextZh = "没看见纳蒂斯，我们继续。",
                    LangTextType = LangType.LangText,
                    IsTranslated = 2,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Parse("2020-11-02T21:48:38.6730397"),
                    ZhLastModifyTimestamp = DateTime.Parse("2020-11-16T22:20:37.6229099"),
                    UserId = new Guid("18F825FF-46F5-4A1A-9B10-56BE5FB2398D"),
                    ReviewerId = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
                    ReviewTimestamp = DateTime.Now,
                    ArchiveTimestamp = DateTime.Now,
                }
                );

            modelBuilder.Entity<LangTextRevNumber>().HasData(
                new LangTextRevNumber 
                { 
                    Id = 1, 
                    LangTextRev = 1 
                });
        }
    }
}
