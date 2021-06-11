using AutoMapper;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore.RepositoryWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ESO_LangEditor.API.Controllers
{
    [Route("api/revnumber")]
    [ApiController]
    public class LangTextSyncByRevNumberController : ControllerBase
    {
        private IRepositoryWrapper _repositoryWrapper;
        private IMapper _mapper;

        public LangTextSyncByRevNumberController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<int>> GetRevisedNumberAsync()
        {
            var LangRevNumber = await _repositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(1);

            return LangRevNumber.LangTextRev;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<List<LangTextRevisedDto>>> GetRevisedDtoByIDAsync(int id)
        {
            var LangRevList = await _repositoryWrapper.LangTextRevisedRepo.GetByConditionAsync(langRev => langRev.LangTextRevNumber == id);
            var langRevListDto = _mapper.Map<List<LangTextRevisedDto>>(LangRevList);

            return langRevListDto;
        }

    }
}
