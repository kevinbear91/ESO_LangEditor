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
        Task<MessageWithCode> UploadNewGameVersion(GameVersionDto gameVersionDto);
        Task<MessageWithCode> UploadIdTypeDto(LangTypeCatalogDto langTypeCatalogDto);
        Task<MessageWithCode> UploadIdTypeDto(List<LangTypeCatalogDto> langTypeCatalogDto);
        Task<List<LangTypeCatalogDto>> GetIdTypesFromReview();
        Task<MessageWithCode> ApproveIdTypeFromReview(List<int> ids);
        Task<MessageWithCode> DeleteIdTypeFromReview(List<int> ids);
    }
}
