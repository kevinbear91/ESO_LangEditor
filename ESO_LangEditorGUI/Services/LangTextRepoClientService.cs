using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore;
using ESO_LangEditor.EFCore.RepositoryWrapper;
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
    public class LangTextRepoClientService
    {
        private IMapper _mapper;

        public LangTextRepoClientService()
        {
            _mapper = App.Mapper; // mapper;
        }

        public async Task<LangTextDto> GetLangTextByGuidAsync(Guid langtextGuid)
        {
            LangTextClient langtext;

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                langtext = await db.Langtexts.FindAsync(langtextGuid);
                db.Dispose();
            }

            var langtextDto = _mapper.Map<LangTextDto>(langtext);
            return langtextDto;
        }

        public async Task<List<LangTextDto>> GetLangTextByConditionAsync(string keyWord, SearchTextType searchType, SearchPostion searchPostion)
        {
            string searchPosAndWord = GetKeywordWithPostion(searchPostion, keyWord);
            List<LangTextClient> langtext;

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                langtext = searchType switch
                {
                    SearchTextType.UniqueID => await db.Langtexts.Where(d => d.TextId == keyWord).ToListAsync(),
                    SearchTextType.TextEnglish => await db.Langtexts.Where(d => EF.Functions.Like(d.TextEn, searchPosAndWord)).ToListAsync(),
                    SearchTextType.TextChineseS => await db.Langtexts.Where(d => EF.Functions.Like(d.TextZh, searchPosAndWord)).ToListAsync(),
                    SearchTextType.UpdateStatus => await db.Langtexts.Where(d => EF.Functions.Like(d.UpdateStats, searchPosAndWord)).ToListAsync(),
                    SearchTextType.TranslateStatus => await db.Langtexts.Where(d => d.IsTranslated == ToSByte(keyWord)).ToListAsync(),
                    SearchTextType.Guid => await db.Langtexts.Where(d => d.Id == new Guid(keyWord)).ToListAsync(),
                    SearchTextType.Type => await db.Langtexts.Where(d => d.IdType == ToInt32(keyWord)).ToListAsync(),
                    //SearchTextType.ByUser => throw new NotImplementedException(),
                    _ => await db.Langtexts.Where(d => EF.Functions.Like(d.TextEn, searchPosAndWord)).ToListAsync(),
                };

                db.Dispose();
            }

            var langtextDto = _mapper.Map<List<LangTextDto>>(langtext);
            return langtextDto;
        }

        public async Task<List<LangTextDto>> GetLangTextByConditionAsync(string keyWord, string keyWord2, 
            SearchTextType searchType, SearchTextType searchType2, 
            SearchPostion searchPostion)
        {

            var firstType = GetSearchTypeToStringRaw(searchType);
            var secondType = GetSearchTypeToStringRaw(searchType2);
            string searchPosAndWord = GetKeywordWithPostion(searchPostion, keyWord2);

            List <LangTextClient> langtext;

            Debug.WriteLine("Key1: {0}, Key2: {1}, fT: {2}, sT: {3}.", keyWord, searchPosAndWord, firstType, secondType);

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                //0 = firstType, 1 = keyWord, 2 = secondType, 3 = searchPosAndWord.
                var query = await db.Langtexts.FromSqlRaw("SELECT * FROM Langtexts WHERE " + firstType 
                    + " = " + keyWord 
                    + " AND " + secondType 
                    + " Like '" + searchPosAndWord + "'").ToListAsync();

                langtext = query;

                db.Dispose();
            }

            var langtextDto = _mapper.Map<List<LangTextDto>>(langtext);
            return langtextDto;
        }

        public async Task<Dictionary<string, LangTextDto>> GetAlltLangTextsDictionaryAsync()
        {
            List<LangTextClient> langtext;

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                langtext = await db.Langtexts.ToListAsync();
                db.Dispose();
            }

            var langtextDto = _mapper.Map<List<LangTextDto>>(langtext);

            var langtextDirct = langtextDto.ToDictionary(d => d.TextId);
            Debug.WriteLine(langtextDirct.Count);
            return langtextDirct;
        }


        public async Task<bool> AddLangtexts(List<LangTextClient> langtextDto)
        {
            int saveCount = 0;

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                db.Langtexts.AddRange(langtextDto);
                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }

            return saveCount > 0;
        }

        public async Task<bool> UpdateLangtextEn(List<LangTextForUpdateEnDto> langtextDto)
        {
            int saveCount = 0;

            var lang = _mapper.Map<List<LangTextClient>>(langtextDto);

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                db.Langtexts.UpdateRange(lang);
                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }

            return saveCount > 0;
        }


        public async Task<bool> UpdateLangtextZh(LangTextForUpdateZhDto langtextDto)
        {
            int saveCount = 0;
            //Debug.WriteLine("ID:{0}, zh: {1}, modifytimer: {2}", langtextDto.Id, langtextDto.TextZh, langtextDto.ZhLastModifyTimestamp);

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                var langtext = await db.Langtexts.FindAsync(langtextDto.Id);
                _mapper.Map(langtextDto, langtext, typeof(LangTextForUpdateZhDto), typeof(LangTextClient));

                db.Langtexts.Update(langtext);
                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }

            return saveCount > 0;
        }

        public async Task<bool> UpdateLangtextZh(List<LangTextForUpdateZhDto> langtextDto)
        {
            int saveCount = 0;
            //Debug.WriteLine("ID:{0}, zh: {1}, modifytimer: {2}", langtextDto.Id, langtextDto.TextZh, langtextDto.ZhLastModifyTimestamp);

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                foreach(var lang in langtextDto)
                {
                    var langtext = await db.Langtexts.FindAsync(lang.Id);
                    _mapper.Map(langtextDto, langtext, typeof(LangTextForUpdateZhDto), typeof(LangTextClient));

                    db.Langtexts.Update(langtext);
                }
                
                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }

            return saveCount > 0;
        }

        public async Task<bool> UpdateLangtexts(List<LangTextDto> langtextDto)
        {
            int saveCount = 0;

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                var lang = _mapper.Map<List<LangTextClient>>(langtextDto);

                db.Langtexts.UpdateRange(lang);
                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }

            return saveCount > 0;
        }

        public async Task<bool> UpdateTranslateStatus(List<LangTextDto> langtexts)
        {
            int saveCount = 0;
            foreach (var langtext in langtexts)
            {
                langtext.IsTranslated = 2;
            }
            var lang = _mapper.Map<List<LangTextClient>>(langtexts);

            using(var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                db.Langtexts.UpdateRange(lang);
                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }

            return saveCount > 0;
        }

        public async Task<bool> DeleteLangtexts(List<LangTextClient> langtextDto)
        {
            int saveCount = 0;

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                db.Langtexts.RemoveRange(langtextDto);
                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }

            return saveCount > 0;
        }

        public async Task<int> GetLangtextRevNumber()
        {
            int langtextRev = 0;

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
               var langtextRevNumber = await db.LangtextRevNumber.FindAsync(1);
                langtextRev = langtextRevNumber.LangTextRev;
                db.Dispose();
            }

            return langtextRev;
        }

        private string GetSearchTypeToStringRaw(SearchTextType searchTextType)
        {

            string rawString = searchTextType switch
            {
                SearchTextType.Guid => "Id",
                SearchTextType.UniqueID => "TextId",
                SearchTextType.Type => "IdType",
                SearchTextType.TextEnglish => "TextEn",
                SearchTextType.TextChineseS => "TextZh",
                SearchTextType.TranslateStatus => "IsTranslated",
                SearchTextType.UpdateStatus => "UpdateStats",
                //SearchTextType.ReviewStatus => ""
                SearchTextType.ByUser => "UserId",
                _ => "TextEn",
            };
            return rawString;
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
