using ESO_LangEditorModels;
using ESO_LangEditorModels.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;

namespace ESO_LangEditorGUI.Services
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
                    SearchTextType.UniqueID => await db.LangData.Where(d => d.TextId == keyWord).ToListAsync(),
                    SearchTextType.TextEnglish => await db.LangData.Where(d => EF.Functions.Like(d.TextEn, searchPosAndWord)).ToListAsync(),
                    SearchTextType.TextChineseS => await db.LangData.Where(d => EF.Functions.Like(d.TextZh, searchPosAndWord)).ToListAsync(),
                    SearchTextType.UpdateStatus => await db.LangData.Where(d => EF.Functions.Like(d.UpdateStats, searchPosAndWord)).ToListAsync(),
                    SearchTextType.TranslateStatus => await db.LangData.Where(d => d.IsTranslated == ToInt32(keyWord)).ToListAsync(),
                    SearchTextType.Guid => await db.LangData.Where(d => d.Id == new Guid(keyWord)).ToListAsync(),
                    SearchTextType.Type => await db.LangData.Where(d => d.IdType == ToInt32(keyWord)).ToListAsync(),
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
            Db.Entry(lang).Property("UserId").IsModified = true;

            //Db.Update(langList);
            return await Db.SaveChangesAsync();
        }

        public async Task<int> UpdateTranslated(List<LangTextDto> langs)
        {
            using var Db = new LangDbContext();

            foreach (var l in langs)
            {
                l.IsTranslated = 2;
                Db.Attach(l);
                Db.Entry(l).Property("IsTranslated").IsModified = true;
            }

            return await Db.SaveChangesAsync();
        }

        public async Task<int> AddNewLangs(List<LangTextDto> langList)
        {
            using var Db = new LangDbContext();
            Db.AddRange(langList);
            return await Db.SaveChangesAsync();
        }

        public async Task<int> DeleteLangs(List<LangTextDto> langList)
        {
            using var Db = new LangDbContext();
            Db.RemoveRange(langList);
            return await Db.SaveChangesAsync();
        }

        public async Task UpdateLangsEN(List<LangTextDto> langList)
        {
            using var Db = new LangDbContext();

            //int i = 0;
            foreach (var l in langList)    //EF core 不支持List批量标记，但循环速度不差，2700条执行时间基本在1秒左右。
            {
                //i += 1;
                Db.Attach(l);

                Db.Entry(l).Property("TextEn").IsModified = true;
                Db.Entry(l).Property("UpdateStats").IsModified = true;
                Db.Entry(l).Property("IsTranslated").IsModified = true;
                Db.Entry(l).Property("EnLastModifyTimestamp").IsModified = true;
                Db.Entry(l).Property("UserId").IsModified = true;

                //Debug.WriteLine("UniqueID: " + l.UniqueID + ", "
                //    + "Text_EN: " + l.Text_EN + ", "
                //    + "UpdateStats: " + l.UpdateStats + ", "
                //    + "Counter: " + i);
            }

            //Db.UpdateRange(langList);
            await Db.SaveChangesAsync();
        }

        public async Task UpdateLangsZH(List<LangTextDto> langList)
        {
            using var Db = new LangDbContext();

            //int i = 0;
            foreach (var l in langList)    //EF core 不支持List批量标记，但循环速度不差，2700条执行时间基本在1秒左右。
            {
                //i += 1;
                Db.Attach(l);

                Db.Entry(l).Property("TextZh").IsModified = true;
                Db.Entry(l).Property("UpdateStats").IsModified = true;
                Db.Entry(l).Property("IsTranslated").IsModified = true;
                Db.Entry(l).Property("ZhLastModifyTimestamp").IsModified = true;
                Db.Entry(l).Property("UserId").IsModified = true;

                //Debug.WriteLine("UniqueID: " + l.UniqueID + ", "
                //    + "Text_EN: " + l.Text_EN + ", "
                //    + "UpdateStats: " + l.UpdateStats + ", "
                //    + "Counter: " + i);
            }

            //Db.UpdateRange(langList);
            await Db.SaveChangesAsync();
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
