using AutoMapper;
using Core.Entities;
using Core.Models;
using Core.RequestParameters;
using EFCore.RepositoryWrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/langtext/archive")]
    [ApiController]
    public class LangTextArchiveController : ControllerBase
    {
        private IRepositoryWrapper _repositoryWrapper;
        private IMapper _mapper;

        public LangTextArchiveController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        //public async Task<ActionResult<IEnumerable<LangTextArchive>>> GetLangTextAllAsync([FromQuery] PageParameters pageParameters)
        //{
        //    var langtextList = await _repositoryWrapper.LangTextArchiveRepo.GetAllAsync(pageParameters);

        //    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(langtextList.PageData));

        //    return langtextList.ToList();
        //}

        //[AllowAnonymous]
        //[HttpGet("{langtextGuid}")]
        //public async Task<ActionResult<LangTextArchive>> GetLangTextByGuidAsync(Guid langtextGuid)
        //{
        //    var langtext = await _repositoryWrapper.LangTextArchiveRepo.GetByIdAsync(langtextGuid);

        //    if (langtext == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        return langtext;
        //    }

        //}

        [HttpGet("{langtextId}")]
        public async Task<ActionResult<List<LangTextForArchiveDto>>> GetLangTextByTextIdAsync(string langtextId)
        {
            var langtextArchived = await _repositoryWrapper.LangTextArchiveRepo.GetByConditionAsync(lang => lang.TextId == langtextId);
            var lantextArchivedDto = _mapper.Map<List<LangTextForArchiveDto>>(langtextArchived.ToList());

            return lantextArchivedDto;
        }


    }
}
