using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESO_LangEditor.API.Helpers
{
    public class LangTextMappingProfile : Profile
    {
        public LangTextMappingProfile()
        {
            CreateMap<LangText, LangTextDto>();
            CreateMap<LangText, LangTextReview>();
            CreateMap<LangText, LangTextArchive>();
            CreateMap<LangTextForCreationDto, LangText>();
            CreateMap<LangTextForUpdateZhDto, LangText>();
            CreateMap<LangTextForCreationDto, LangTextReview>();
            CreateMap<LangTextForUpdateZhDto, LangTextReview>();
            CreateMap<LangTextForUpdateEnDto, LangTextReview>();
            CreateMap<LangTextReview, LangText>();
            CreateMap<LangTextReview, LangTextForReviewDto>();
            CreateMap<LangTextReview, LangTextArchive>();
            CreateMap<LangTextRevisedDto, LangTextRevised>();
            CreateMap<LangTextRevised, LangTextRevisedDto>();
            CreateMap<LangTextRevNumberDto, LangTextRevNumber>();
            CreateMap<User, UserDto>();
            CreateMap<User, UserInClientDto>();
            CreateMap<UserProfileModifyBySelfDto, User>();
        }
    }
}
