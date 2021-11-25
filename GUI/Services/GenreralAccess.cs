using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GUI.Services
{
    public class GenreralAccess : IGeneralAccess
    {
        private readonly HttpClient _langHttpClient;
        private JsonSerializerOptions _jsonOption;

        public GenreralAccess()
        {
            _langHttpClient = App.HttpClient;

            _jsonOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }


        public Task<List<LangTextRevNumberDto>> GetAllRevisedNumber()
        {
            throw new NotImplementedException();
        }

        public Task<List<GameVersionDto>> GetGameVersionDtos()
        {
            throw new NotImplementedException();
        }

        public Task<List<LangTypeCatalogDto>> GetIdtypeDtos()
        {
            throw new NotImplementedException();
        }

        public Task<MessageWithCode> UploadNewGameVersion()
        {
            throw new NotImplementedException();
        }
    }
}
