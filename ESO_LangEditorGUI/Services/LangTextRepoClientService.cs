using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore;
using ESO_LangEditor.EFCore.RepositoryWrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;

namespace ESO_LangEditorGUI.Services
{
    public class LangTextRepoClientService
    {
        private IRepositoryWrapperClient _repositoryWrapper; 
        private IMapper _mapper;
        private LangtextClientDbContext _langtextClientDb;

        public LangTextRepoClientService(/*IRepositoryWrapperClient repositoryWrapper, IMapper mapper*/)
        {
            //_langtextClientDb = new LangtextClientDbContext(App.DbOptionsBuilder);
            //_repositoryWrapper = new RepositoryWrapperClient(LangtextClientDb); // repositoryWrapper;
            _mapper = App.Mapper; // mapper;
        }

        public async Task<IEnumerable<LangTextDto>> GetLangTextByGuidAsync(Guid langtextGuid)
        {
            LangText langtext;

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                langtext = await db.Langtexts.FindAsync(langtextGuid);
                db.Dispose();
            }

            var langtextDto = _mapper.Map<List<LangTextDto>>(langtext);
            return langtextDto;
        }

        public async Task<List<LangTextDto>> GetLangTextByConditionAsync(string keyWord, SearchTextType searchType, SearchPostion searchPostion)
        {
            string searchPosAndWord = GetKeywordWithPostion(searchPostion, keyWord);
            List<LangText> langtext;

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

        public async Task<bool> AddLangtexts(List<LangText> langtextDto)
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
            //LangText langtext;
            int saveCount = 0;

            var lang = _mapper.Map<List<LangText>>(langtextDto);

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

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                var lang = _mapper.Map<LangText>(langtextDto);

                db.Langtexts.Update(lang);
                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }

            return saveCount > 0;
        }

        public async Task<bool> UpdateLangtextZh(List<LangTextForUpdateZhDto> langtextDto)
        {
            int saveCount = 0;

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                var lang = _mapper.Map<List<LangText>>(langtextDto);

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
            var lang = _mapper.Map<List<LangText>>(langtexts);

            using(var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                db.Langtexts.UpdateRange(lang);
                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }

            return saveCount > 0;
        }

        public async Task<bool> DeleteLangtexts(List<LangText> langtextDto)
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
