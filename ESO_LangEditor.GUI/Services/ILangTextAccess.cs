using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.Services
{
    public interface ILangTextAccess
    {
        Task<IEnumerable<LangTextDto>> GetLangTexts(Guid langtextId);
        Task<IEnumerable<LangTextDto>> GetLangTexts(List<Guid> langtextId);
        Task<List<LangTextDto>> GetLangTexts(int langtextRevised);
        Task<IEnumerable<LangTextDto>> GetLangTextsFromArchive(Guid langtextId);
        Task<List<LangTextForReviewDto>> GetLangTextsFromReviewer(Guid userGuid);
        Task<LangTextForReviewDto> GetLangTextFromReview(Guid langtextGuid);
        Task<MessageWithCode> UpdateLangTextZh(LangTextForUpdateZhDto langTextForUpdateZhDto);
        Task<MessageWithCode> UpdateLangTextZh(List<LangTextForUpdateZhDto> langTextForUpdateZhDtos);
        Task<MessageWithCode> UpdateLangTextEn(List<LangTextForUpdateEnDto> langTextForUpdateEnDtos);
        Task<MessageWithCode> AddLangTexts(List<LangTextForCreationDto> langTextForCreationDtos);
        Task<MessageWithCode> RemoveLangTexts(List<Guid> langTextGuids);
        Task<MessageWithCode> ApproveLangTextsInReview(List<Guid> langTextGuids);
        Task<MessageWithCode> RemoveLangTextsInReview(Guid langTextGuid);
        Task<MessageWithCode> RemoveLangTextsInReview(List<Guid> langTextGuids);
        Task<List<Guid>> GetUsersInReview();
        //Task<int> GetLangTextRevisedNumber();
        Task<List<LangTextRevNumberDto>> GetAllRevisedNumber();
        Task<IEnumerable<LangTextRevisedDto>> GetLangTextRevisedDtos(int revNumber);

    }
}
