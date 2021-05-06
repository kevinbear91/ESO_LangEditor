using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorDatabaseModifier.Controller.Server
{
    public class LangTextMappingProfile : Profile
    {
        public LangTextMappingProfile()
        {
            CreateMap<LangText, LangTextDto>();
            CreateMap<LangTextDto, LangText>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
