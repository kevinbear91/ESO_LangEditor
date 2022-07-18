using Core.Entities;
using Core.EnumTypes;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Services
{
    public interface ILangTextRepoClient
    {
        Task<LangTextClient> GetLangTextByGuidAsync(Guid langtextGuid);
        Task<List<LangTextDto>> GetLangTextByConditionAsync(string keyWord, SearchTextType searchType, SearchPostion searchPostion);
        Task<List<LangTextDto>> GetLangTextByConditionAsync(string keyWord, string keyWord2, SearchTextType searchType, SearchTextType searchType2, SearchPostion searchPostion);
        /// <summary>
        /// 0 为所有文本，1 为Lua UI文本，2为Langtext文本。
        /// </summary>
        /// <param name="searchType">int</param>
        /// <returns></returns>
        Task<Dictionary<string, LangTextDto>> GetAlltLangTextsDictionaryAsync(int searchType);
        Task<List<LangTextDto>> GetAlltLangTexts();
        Task<bool> AddLangtexts(List<LangTextClient> langtextDto);
        Task<bool> UpdateLangtextEn(List<LangTextForUpdateEnDto> langtextDto);
        Task<bool> UpdateLangtextZh(LangTextForUpdateZhDto langtextDto);
        Task<bool> UpdateLangtextZh(List<LangTextForUpdateZhDto> langtextDto);
        //Task<bool> UpdateLangtextJf(List<LangTextForUpdateZhDto> langtextDto);
        Task<bool> UpdateLangtexts(List<LangTextClient> langtextDto);
        Task<bool> UpdateTranslateStatus(List<LangTextDto> langtexts);
        Task<bool> DeleteLangtexts(List<Guid> langtextId);
        Task<List<LangTextRevNumber>> GetLangtextRevNumber();
        Task<LangTextRevNumber> GetRevNumber(int id);
        /// <summary>
        /// ID 1 为 LangText 步进号，ID 2 为 User 步进号，ID 3为游戏API号， ID 4为Id Type号
        /// </summary>
        /// <param name="id">Int</param>
        /// <param name="number">Int</param>
        /// <returns></returns>
        Task<bool> UpdateRevNumber(int id, int number);
        Task<UserInClient> GetUserInClient(Guid userId);
        Task<IEnumerable<UserInClient>> GetUsers();
        Task<bool> UpdateUsers(List<UserInClient> users);
        Task<bool> UpdateGameVersions(List<GameVersion> gameVersions);
        Task<bool> SaveGameVersion(GameVersion gameVersion);
        Task<bool> UpdateIdTypes(List<LangTypeCatalog> langIdTypes);
        Task<GameVersion> GetGameVersion(int id);
        Task<Dictionary<int, string>> GetGameVersion();
        Task<List<GameVersionDto>> GetGameVersionDtoList();
        Task<Dictionary<int, string>> GetIdTypeDict();

    }
}
