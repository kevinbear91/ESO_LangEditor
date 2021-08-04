using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.Services
{
    public interface ILangTextAccess
    {
        Task<IEnumerable<LangTextDto>> GetLangTexts(Guid langtextId);
        Task<IEnumerable<LangTextDto>> GetLangTexts(List<Guid> langtextId);
        Task<List<LangTextDto>> GetLangTexts(int langtextRevised);
        Task<IEnumerable<LangTextDto>> GetLangTextsFromArchive(Guid langtextId);
        Task<IEnumerable<LangTextForReviewDto>> GetLangTextsFromReview(Guid userGuid);
        Task<LangTextForReviewDto> GetLangTextFromReview(Guid langtextGuid);
        Task<ApiMessageWithCode> UpdateLangTextZh(LangTextForUpdateZhDto langTextForUpdateZhDto);
        Task<ApiMessageWithCode> UpdateLangTextZh(List<LangTextForUpdateZhDto> langTextForUpdateZhDtos);
        Task<ApiMessageWithCode> UpdateLangTextEn(List<LangTextForUpdateEnDto> langTextForUpdateEnDtos);
        Task<ApiMessageWithCode> AddLangTexts(List<LangTextForCreationDto> langTextForCreationDtos);
        Task<ApiMessageWithCode> RemoveLangTexts(List<Guid> langTextGuids);
        Task<ApiMessageWithCode> ApproveLangTextsInReview(List<Guid> langTextGuids);
        Task<ApiMessageWithCode> RemoveLangTextsInReview(Guid langTextGuid);
        Task<IEnumerable<Guid>> GetUsersInReview();
        //Task<int> GetLangTextRevisedNumber();
        Task<List<LangTextRevNumberDto>> GetAllRevisedNumber();
        Task<IEnumerable<LangTextRevisedDto>> GetLangTextRevisedDtos(int revNumber);

    }
}
