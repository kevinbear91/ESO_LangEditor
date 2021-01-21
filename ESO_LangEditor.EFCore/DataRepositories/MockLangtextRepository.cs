using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ESO_LangEditor.EFCore.DataRepositories
{
    public class MockLangtextRepository : BaseRepository<LangText, Guid>, ILangTextRepository
    {
        private readonly List<LangText> _langTexts;

        public MockLangtextRepository(DbContext dbcontext) : base(dbcontext)
        {
            _langTexts = new List<LangText>()
            {
                 new LangText { Id = new Guid("F9593612-661A-49A5-BF31-82AA163B7ECB"),
                    TextId = "242841733-0-134110",
                    IdType = 242841733,
                    TextEn = "Jewelry: Nikulas' Heavy Armor",
                    TextZh = "饰品：尼古拉斯的重甲",
                    IsTranslated = 0,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Today,
                    ZhLastModifyTimestamp = DateTime.Today,
                    UserId = new Guid("E77232D7-CC85-4CBE-B5E3-7595822C4922") },

                new LangText { Id = new Guid("B1CE7D9D-ED4E-400B-BD5F-A2DE102C676C"),
                    TextId = "242841733-0-134112",
                    IdType = 242841733,
                    TextEn = "Weapons: Noble Duelist's Silks",
                    TextZh = "武器：高贵决斗者丝袍",
                    IsTranslated = 0,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Today,
                    ZhLastModifyTimestamp = DateTime.Today,
                    UserId = new Guid("E77232D7-CC85-4CBE-B5E3-7595822C4922") },

                new LangText { Id = new Guid("4221E2A4-EDE3-406D-A8EE-E58316D03A12"),
                    TextId = "7949764-0-63965",
                    IdType = 7949764,
                    TextEn = "Explore the Unhallowed Grave",
                    TextZh = "探索不洁墓窟",
                    IsTranslated = 0,
                    UpdateStats = "Update25",
                    EnLastModifyTimestamp = DateTime.Today,
                    ZhLastModifyTimestamp = DateTime.Today,
                    UserId = new Guid("E77232D7-CC85-4CBE-B5E3-7595822C4922") },

                new LangText { Id = new Guid("23CCA81A-6F43-4905-8025-BA280E4B5B58"),
                    TextId = "55049764-6-4956",
                    IdType = 55049764,
                    TextEn = @"We will meet in the Smokefrosts, my friend. If not there, then in Sovngarde.\n\nKyne speed you.",
                    TextZh = @"我们将在烟霜相遇，我的朋友。不在那的话，便是松加德了。\n\n凯娜祝你好运。",
                    IsTranslated = 0,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Today,
                    ZhLastModifyTimestamp = DateTime.Today,
                    UserId = new Guid("E77232D7-CC85-4CBE-B5E3-7595822C4922") },

                new LangText { Id = new Guid("52774D43-734D-4385-9E4E-170336AEE228"),
                    TextId = "198758357-0-126651",
                    IdType = 198758357,
                    TextEn = "Shrouded Dagger",
                    TextZh = "隐秘之匕",
                    IsTranslated = 0,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Today,
                    ZhLastModifyTimestamp = DateTime.Today,
                    UserId = new Guid("148ED451-BF19-43E9-A8D3-55F922CD349E") },

                new LangText { Id = new Guid("FAE9BC73-1BE7-4AD9-B582-9B2EA3FBF3EB"),
                    TextId = "198758357-0-126534",
                    IdType = 198758357,
                    TextEn = "Minor Endurance",
                    TextZh = "耐久(弱)",
                    IsTranslated = 0,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Today,
                    ZhLastModifyTimestamp = DateTime.Today,
                    UserId = new Guid("148ED451-BF19-43E9-A8D3-55F922CD349E") },

                new LangText { Id = new Guid("ECB63039-8E4A-4679-8FBA-F1EF3BFA8D33"),
                    TextId = "188155806-0-372",
                    IdType = 188155806,
                    TextEn = "Defeat Fight-Master Grel and his adepts at Sanguine's Demesne.",
                    TextZh = "在血腥领域击败战斗高手格雷尔和他的修行者。",
                    IsTranslated = 0,
                    UpdateStats = "Update24",
                    EnLastModifyTimestamp = DateTime.Now,
                    ZhLastModifyTimestamp = DateTime.Now,
                    UserId = new Guid("148ED451-BF19-43E9-A8D3-55F922CD349E") },
            };
        }


        public IEnumerable<LangText> GetAllLangTexts()
        {
            return _langTexts;
        }

        public LangText GetLangText(Guid guid)
        {
            return _langTexts.FirstOrDefault(l => l.Id == guid);
        }

        public void Insert(LangTextDto langText)
        {
            langText.Id = new Guid();
            //langText.IsTranslated = 0;
            //langText.EnLastModifyTimestamp = DateTime.Now;
            //langText.ZhLastModifyTimestamp = DateTime.Now;


            //_langTexts.Add(langText);
        }

        public LangText Update(LangText updateLangText)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid langTextID)
        {
            //_langTexts.Remove(langText);
        }

        public IEnumerable<LangText> FindByCondition(Expression<Func<LangText, bool>> expression)
        {
            throw new NotImplementedException();
        }

        
    }
}
