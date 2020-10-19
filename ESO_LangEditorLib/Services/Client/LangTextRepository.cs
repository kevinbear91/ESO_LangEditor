using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorLib.Models.Client.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;

namespace ESO_LangEditorLib.Services.Client
{
    public class LangTextRepository
    {
        
        public async Task<List<LangTextDto>> GetLangTextsAsync(string keyWord, SearchTextType searchType, SearchPostion searchPostion)
        {
            List<LangTextDto> listData;
            string searchPosAndWord = GetKeywordWithPostion(searchPostion, keyWord);

            using (var db = new LangDbContext())
            {
                listData = searchType switch
                {
                    SearchTextType.UniqueID => await db.LangData.Where(d => d.TextId == searchPosAndWord).ToListAsync(),
                    SearchTextType.TextEnglish => await db.LangData.Where(d => EF.Functions.Like(d.TextEn, searchPosAndWord)).ToListAsync(),
                    SearchTextType.TextChineseS => await db.LangData.Where(d => EF.Functions.Like(d.TextZh, searchPosAndWord)).ToListAsync(),
                    SearchTextType.UpdateStatus => await db.LangData.Where(d => EF.Functions.Like(d.UpdateStats, searchPosAndWord)).ToListAsync(),
                    //SearchTextType.ReviewStatus => db.LangData.Where(d => d. == ToInt32(keyWord)).ToList(),
                    SearchTextType.TranslateStatus => await db.LangData.Where(d => d.IsTranslated == ToInt32(keyWord)).ToListAsync(),
                    //SearchTextType.Guid => throw new NotImplementedException(),
                    //SearchTextType.Type => throw new NotImplementedException(),
                    //SearchTextType.ByUser => throw new NotImplementedException(),
                    _ => await db.LangData.Where(d => EF.Functions.Like(d.TextEn, searchPosAndWord)).ToListAsync(),
                };
            }

            return listData;
        }

        public List<LangTextDto> GetAlltLangTexts(int searchType)
        {
            List<LangTextDto> listData;
            using (var db = new LangDbContext())
            {
                listData = searchType switch
                {
                    0 => db.LangData.Where(d => d.IdType != 100).ToList(),  //搜索游戏内文本
                    1 => db.LangData.Where(d => d.IdType == 100).ToList(),  //搜索Lua UI文本

                };

                return listData;

                //return data;
            }
        }

        public async Task<Dictionary<string, LangTextDto>> GetAlltLangTextsDictionaryAsync()
        {
            using (var db = new LangDbContext())
            {
                return await db.LangData.ToDictionaryAsync(d => d.TextId);
            }
        }

        public async Task<int> UpdateLangZh(LangTextDto lang)
        {
            using var Db = new LangDbContext();
            Db.Attach(lang);
            Db.Entry(lang).Property("TextZh").IsModified = true;
            Db.Entry(lang).Property("IsTranslated").IsModified = true;
            Db.Entry(lang).Property("ZhLastModifyTimestamp").IsModified = true;

            //Db.Update(langList);
            return await Db.SaveChangesAsync();
        }






        private static string GetKeywordWithPostion(SearchPostion searchPostion, string keyWord)
        {
            string searchPosAndWord = searchPostion switch
            {
                SearchPostion.Full => "%" + keyWord + "%",     //任意位置
                SearchPostion.OnlyOnFront => keyWord + "%",           //仅在开头
                SearchPostion.OnlyOnEnd => "%" + keyWord,           //仅在末尾
                _ => "%" + keyWord + "%",     //默认 - 任意位置
            };

            return searchPosAndWord;
        }

    }
}
