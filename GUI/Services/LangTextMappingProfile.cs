using AutoMapper;
using Core.Entities;
using Core.Models;

namespace GUI.Services
{
    public class LangTextMappingProfile : Profile
    {
        public LangTextMappingProfile()
        {
            CreateMap<LangTextClient, LangTextDto>();
            CreateMap<LangTextDto, LangTextClient>();
            CreateMap<LangTextDto, LangTextForUpdateZhDto>();
            CreateMap<LangTextDto, LangTextForUpdateEnDto>();
            CreateMap<LangTextDto, LangTextForCreationDto>();
            CreateMap<LangTextForCreationDto, LangTextClient>();
            CreateMap<LangTextForUpdateZhDto, LangTextClient>();
            CreateMap<User, UserDto>();
            CreateMap<UserInClient, UserInClientDto>();
            CreateMap<UserInClientDto, UserInClient>();
            CreateMap<UserProfileModifyBySelfDto, User>();
            CreateMap<LangTextReview, LangTextClient>();
            CreateMap<LangTextClient, LangTextReview>();
            CreateMap<LangTextClient, LangTextArchive>();
            CreateMap<LangTextReview, LangTextArchive>();
            CreateMap<LangTextRevisedDto, LangTextRevised>();
            CreateMap<LangTextRevNumberDto, LangTextRevNumber>();

        }
    }
}
