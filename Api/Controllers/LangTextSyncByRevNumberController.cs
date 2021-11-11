using AutoMapper;
using Core.Models;
using Core.RequestParameters;
using EFCore.RepositoryWrapper;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/revise")]
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

        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<List<LangTextRevNumberDto>>> GetRevisedNumberAsync([FromQuery] PageParameters pageParameters)
        {
            var LangRevNumber = await _repositoryWrapper.LangTextRevNumberRepo.GetAllAsync(pageParameters);
            var LangRevNumebrDto = _mapper.Map<List<LangTextRevNumberDto>>(LangRevNumber.ToList());

            return LangRevNumebrDto;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<LangTextRevNumberDto>> GetRevisedNumberByIdAsync(int id)
        {
            var LangRevNumber = await _repositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(id);
            var LangRevNumebrDto = _mapper.Map<LangTextRevNumberDto>(LangRevNumber);

            return LangRevNumebrDto;
        }

        [Authorize]
        [HttpGet("LangTextRev/{id}")]
        public async Task<ActionResult<List<LangTextRevisedDto>>> GetRevisedDtoByIDAsync(int id)
        {
            var LangRevList = await _repositoryWrapper.LangTextRevisedRepo.GetByConditionAsync(langRev => langRev.LangTextRevNumber == id);
            var langRevListDto = _mapper.Map<List<LangTextRevisedDto>>(LangRevList);

            return langRevListDto;
        }

    }
}
