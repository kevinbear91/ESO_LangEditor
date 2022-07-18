using AutoMapper;
using Core.Entities;
using Core.EnumTypes;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Convert;

namespace GUI.Services
{
    public class LangTextRepoClient : ILangTextRepoClient
    {
        private IMapper _mapper;

        public LangTextRepoClient(IMapper Mapper)
        {
            _mapper = Mapper;
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

        public async Task<bool> DeleteLangtexts(List<Guid> langtextId)
        {
            int saveCount = 0;
            var langToDelete = new List<LangTextClient>();

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                foreach (var id in langtextId)
                {
                    var lang = await db.Langtexts.FindAsync(id);
                    langToDelete.Add(lang);
                }
                db.Langtexts.RemoveRange(langToDelete);
                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }

            return saveCount > 0;
        }

        public async Task<List<LangTextDto>> GetAlltLangTexts()
        {
            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {

                var list = await db.Langtexts.FindAsync();

                return _mapper.Map<List<LangTextDto>>(list);

                //return 
                //return searchType switch
                //{
                //    0 => await db.Langtexts.Where(d => d.IdType != 100).ToListAsync(),  //搜索游戏内文本
                //    1 => await db.Langtexts.Where(d => d.IdType == 100).ToListAsync(),  //搜索Lua UI文本

                //};

                //var langtextDto = _mapper.Map<List<LangTextDto>>(listData);

                //return langtextDto;

                //return data;
            }
        }

        public async Task<Dictionary<string, LangTextDto>> GetAlltLangTextsDictionaryAsync(int searchType)
        {
            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {

                //var list = await db.Langtexts.FindAsync();

                //return _mapper.Map<List<LangTextDto>>(list);

                //return 
                var listData = searchType switch
                {
                    0 => await db.Langtexts.ToListAsync(),  //搜索游戏内文本
                    1 => await db.Langtexts.Where(d => d.IdType == 100).ToListAsync(),  //搜索Lua UI文本
                    2 => await db.Langtexts.Where(d => d.IdType != 100).ToListAsync(),  //搜索游戏内文本
                    _ => await db.Langtexts.Where(d => d.IdType != 100).ToListAsync(),  //搜索游戏内文本
                };

                var langtextDto = _mapper.Map<List<LangTextDto>>(listData);

                var langDictDto = langtextDto.ToDictionary(lang => lang.TextId);

                return langDictDto;

                //return data;
            }
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
                    SearchTextType.UpdateStatus => await db.Langtexts.Where(d => d.GameApiVersion == ToInt32(keyWord)).ToListAsync(),
                    //SearchTextType.TranslateStatus => await db.Langtexts.Where(d => d.IsTranslated == ToSByte(keyWord)).ToListAsync(),
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
            SearchTextType searchType, SearchTextType searchType2, SearchPostion searchPostion)
        {
            List<LangTextClient> langtext;

            var firstType = GetSearchTypeToStringRaw(searchType);
            var secondType = GetSearchTypeToStringRaw(searchType2);
            string searchPosAndWord = GetKeywordWithPostion(searchPostion, keyWord2);

            Debug.WriteLine("Key1: {0}, Key2: {1}, fT: {2}, sT: {3}.", keyWord, searchPosAndWord, firstType, secondType);

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                //0 = firstType, 1 = keyWord, 2 = secondType, 3 = searchPosAndWord.
                var query = await db.Langtexts.FromSqlRaw("SELECT * FROM Langtexts WHERE " + firstType
                    + " = " + "'" + keyWord + "'"
                    + " AND " + secondType
                    + " Like '" + searchPosAndWord + "'").ToListAsync();

                langtext = query;

                db.Dispose();
            }

            var langtextDto = _mapper.Map<List<LangTextDto>>(langtext);
            return langtextDto;
        }

        public async Task<LangTextClient> GetLangTextByGuidAsync(Guid langtextGuid)
        {
            LangTextClient langtext;

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                langtext = await db.Langtexts.FindAsync(langtextGuid);
                db.Dispose();
            }

            //var langtextDto = _mapper.Map<LangTextDto>(langtext);
            return langtext;
        }

        public async Task<List<LangTextRevNumber>> GetLangtextRevNumber()
        {
            List<LangTextRevNumber> langtextRev = null;

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                langtextRev = await db.LangtextRevNumber.ToListAsync();
                //langtextRev = langtextRevNumber;
                db.Dispose();
            }

            return langtextRev;
        }

        public async Task<LangTextRevNumber> GetRevNumber(int id)
        {
            LangTextRevNumber rev;

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                rev = await db.LangtextRevNumber.FindAsync(id);
                //langtextRev = langtextRevNumber;
                db.Dispose();
            }

            return rev;
        }

        public async Task<UserInClient> GetUserInClient(Guid userId)
        {
            UserInClient user;
            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                user = await db.Users.FindAsync(userId);
                db.Dispose();
            }

            return user;
        }

        public async Task<IEnumerable<UserInClient>> GetUsers()
        {
            List<UserInClient> user;
            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                user = db.Users.ToList();
                db.Dispose();
            }

            return user;
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

        public async Task<bool> UpdateLangtexts(List<LangTextClient> langtextDto)
        {
            int saveCount = 0;

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                //var lang = _mapper.Map<List<LangTextClient>>(langtextDto);

                db.Langtexts.UpdateRange(langtextDto);
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
                foreach (var lang in langtextDto)
                {
                    var langtext = await db.Langtexts.FindAsync(lang.Id);
                    _mapper.Map(lang, langtext, typeof(LangTextForUpdateZhDto), typeof(LangTextClient));

                    db.Langtexts.Update(langtext);
                }

                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }

            return saveCount > 0;
        }

        public async Task<bool> UpdateRevNumber(int id, int number)
        {
            int saveCount = 0;

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                var langtextRevNumber = await db.LangtextRevNumber.FindAsync(id);
                langtextRevNumber.Rev = number;

                db.LangtextRevNumber.Update(langtextRevNumber);
                saveCount = await db.SaveChangesAsync();

                db.Dispose();
            }
            return saveCount > 0;
        }

        public async Task<bool> UpdateTranslateStatus(List<LangTextDto> langtexts)
        {
            int saveCount = 0;
            var lang = _mapper.Map<List<LangTextClient>>(langtexts);

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                db.Langtexts.UpdateRange(lang);
                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }

            return saveCount > 0;
        }

        public async Task<bool> UpdateUsers(List<UserInClient> users)
        {
            int saveCount = 0;
            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                foreach (var user in users)
                {
                    var isUserExist = await db.Users.FindAsync(user.Id);

                    if (isUserExist != null)
                    {
                        isUserExist.UserNickName = user.UserNickName;
                        //isUserExist.InReviewCount = user.InReviewCount;
                        //isUserExist.RemovedCount = user.RemovedCount;
                        //isUserExist.TranslatedCount = user.RemovedCount;
                        db.Users.Update(isUserExist);
                    }
                    else
                    {
                        db.Users.Add(user);
                    }
                }

                //db.Users.UpdateRange(users);
                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }

            return saveCount > 0;
        }

        public async Task<bool> UpdateGameVersions(List<GameVersion> gameVersions)
        {
            int saveCount = 0;
            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                foreach(var gv in gameVersions)
                {
                    var isGameVersionExist = await db.GameVersion.FindAsync(gv.GameApiVersion);

                    if(isGameVersionExist != null)
                    {
                        isGameVersionExist.Version_EN = gv.Version_EN;
                        isGameVersionExist.Version_ZH = gv.Version_ZH;
                    }
                    else
                    {
                        db.GameVersion.Add(gv);
                    }

                }
                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }
            return saveCount > 0;
        }

        public async Task<bool> SaveGameVersion(GameVersion gameVersion)
        {
            int saveCount = 0;
            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                var isGameVersionExist = await db.GameVersion.FindAsync(gameVersion.GameApiVersion);

                if (isGameVersionExist != null)
                {
                    isGameVersionExist.Version_EN = gameVersion.Version_EN;
                    isGameVersionExist.Version_ZH = gameVersion.Version_ZH;
                }
                else
                {
                    db.GameVersion.Add(gameVersion);
                }

                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }
            return saveCount > 0;
        }

        public async Task<GameVersion> GetGameVersion(int id)
        {
            GameVersion gameVersion;
            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                gameVersion = await db.GameVersion.FindAsync(id);
                db.Dispose();
            }

            return gameVersion;
        }

        public async Task<Dictionary<int, string>> GetGameVersion()
        {
            Dictionary<int, string> gameVersionDict;
            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                var list = await db.GameVersion.ToListAsync();
                db.Dispose();

                gameVersionDict = list.ToDictionary(gv => gv.GameApiVersion, zh => zh.Version_ZH);
            }
            return gameVersionDict;
        }

        public async Task<List<GameVersionDto>> GetGameVersionDtoList()
        {
            List<GameVersionDto> gameVersionList;
            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                var list = await db.GameVersion.ToListAsync();
                db.Dispose();

                gameVersionList = _mapper.Map<List<GameVersionDto>>(list);
            }
            return gameVersionList;
        }

        public async Task<bool> UpdateIdTypes(List<LangTypeCatalog> langIdTypes)
        {
            int saveCount = 0;
            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                foreach(var langType in langIdTypes)
                {
                    var isLangTypeExist = await db.LangIdType.FindAsync(langType.IdType);

                    if (isLangTypeExist != null)
                    {
                        isLangTypeExist.IdTypeZH = langType.IdTypeZH;
                    }
                    else
                    {
                        db.LangIdType.Add(langType);
                    }
                }
                saveCount = await db.SaveChangesAsync();
                db.Dispose();
            }
            return saveCount > 0;
        }

        public async Task<Dictionary<int, string>> GetIdTypeDict()
        {
            Dictionary<int, string> idTypeDict;
            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                var list = await db.LangIdType.ToListAsync();
                db.Dispose();

                idTypeDict = list.ToDictionary(id => id.IdType, zh => zh.IdTypeZH);
            }
            return idTypeDict;
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
                //SearchTextType.TranslateStatus => "IsTranslated",
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
