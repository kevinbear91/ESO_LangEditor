using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorGUI.Services
{
    public class LangTextMappingProfile : Profile
    {
        public LangTextMappingProfile()
        {
            CreateMap<LangTextClient, LangTextDto>();
            CreateMap<LangTextDto, LangTextClient>();
            CreateMap<LangTextForCreationDto, LangTextClient>();
            CreateMap<LangTextForUpdateZhDto, LangTextClient>();
            CreateMap<LangTextForUpdateZhDto, LangTextReview>();
            CreateMap<LangTextForUpdateEnDto, LangTextReview>();
            CreateMap<User, UserDto>();
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
