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
            CreateMap<LangTextForCreationDto, LangText>();
            CreateMap<LangTextForUpdateZhDto, LangText>();
            CreateMap<LangTextForUpdateZhDto, LangTextReview>();
            CreateMap<LangTextForUpdateEnDto, LangTextReview>();
            CreateMap<User, UserDto>();
            CreateMap<UserProfileModifyBySelfDto, User>();
            CreateMap<LangTextReview, LangText>();
            CreateMap<LangText, LangTextReview>();
            CreateMap<LangText, LangTextArchive>();
            CreateMap<LangTextReview, LangTextArchive>();
            CreateMap<LangTextRevisedDto, LangTextRevised>();
            CreateMap<LangTextRevNumberDto, LangTextRevNumber>();

        }
    }
}
