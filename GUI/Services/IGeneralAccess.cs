using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Services
{
    public interface IGeneralAccess
    {
        Task<List<LangTextRevNumberDto>> GetAllRevisedNumber();
        Task<List<GameVersionDto>> GetGameVersionDtos();
        Task<List<LangTypeCatalogDto>> GetIdtypeDtos();
        Task<MessageWithCode> UploadNewGameVersion();
        Task<MessageWithCode> UploadIdTypeDto();
        Task<MessageWithCode> GetIdTypeFromReview();
        Task<MessageWithCode> ApproveIdTypeFromReview();
        Task<MessageWithCode> DeleteIdTypeFromReview();
    }
}
