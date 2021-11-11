using Core.Entities;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface ILangTextService
    {
        Task<LangTextReview> LangtextReviewProcess(LangTextReview langTextReviewEntity);
        Task ArchiveListAsync(Guid userId, List<LangTextArchive> langTextArchive);
        Task LangtextReviseListAsync(LangTextRevNumber RevNumber, List<LangTextRevisedDto> langTextRevisedDtos);
    }
}
