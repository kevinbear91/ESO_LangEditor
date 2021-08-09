using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.Services
{
    public interface ILangTextRepoClient
    {
        Task<LangTextClient> GetLangTextByGuidAsync(Guid langtextGuid);
        Task<List<LangTextDto>> GetLangTextByConditionAsync(string keyWord, SearchTextType searchType, SearchPostion searchPostion);
        Task<List<LangTextDto>> GetLangTextByConditionAsync(string keyWord, string keyWord2, SearchTextType searchType, SearchTextType searchType2, SearchPostion searchPostion);
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
        Task<bool> UpdateRevNumber(int number);
        Task<UserInClient> GetUserInClient(Guid userId);
        Task<IEnumerable<UserInClient>> GetUsers();
        Task<bool> UpdateUsers(List<UserInClient> users);

    }
}
