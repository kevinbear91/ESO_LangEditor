using AutoMapper;
using Core.Entities;
using Core.EnumTypes;
using Core.Models;
using EFCore.RepositoryWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/gameVersion")]
    [ApiController]
    public class GameVersionController : ControllerBase
    {
        private IRepositoryWrapper _repositoryWrapper;
        private IMapper _mapper;
        private UserManager<User> _userManager;
        private ILogger<GameVersionController> _logger;
        private string _loggerMessage;

        public GameVersionController(IRepositoryWrapper repositoryWrapper, IMapper mapper,
            UserManager<User> userManager, ILogger<GameVersionController> logger)
        {
             _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;

        }

        [Authorize]
        [HttpGet("{apiId}")]
        public async Task<ActionResult<GameVersionDto>> GetGameVersionById(int apiId)
        {
            var gameVersion = await _repositoryWrapper.GameVersionRepo.GetByIdAsync(apiId);

            if (gameVersion == null)
            {
                return NotFound(new MessageWithCode
                {
                    Code = (int)RespondCode.GameVersionNotFound,
                    Message = RespondCode.GameVersionNotFound.ApiRespondCodeString()
                });

            }

            var gameVersionDto = _mapper.Map<GameVersionDto>(gameVersion);

            return gameVersionDto;
        }

        [Authorize]
        [HttpGet()]
        public async Task<ActionResult<List<GameVersionDto>>> GetGameVersionAll()
        {
            var gameverions = await _repositoryWrapper.GameVersionRepo.GetAllAsync();
            var gameVersionDtos = _mapper.Map<List<GameVersionDto>>(gameverions);

            return gameVersionDtos;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public async Task<IActionResult> AddOrUpdateGameVersion(GameVersionDto gameVersionDto)
        {
            var gameVersion = await _repositoryWrapper.GameVersionRepo.GetByIdAsync(gameVersionDto.GameApiVersion);

            if (gameVersion == null)
            {
                var gameVersionToAdd = _mapper.Map<GameVersion>(gameVersionDto);

                _repositoryWrapper.GameVersionRepo.Create(gameVersionToAdd);
            }
            else
            {
                gameVersion.Version_ZH = gameVersionDto.Version_ZH;
                _repositoryWrapper.GameVersionRepo.Update(gameVersion);
            }

            var gameVersionRev = await _repositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(3);
            gameVersionRev.Rev++;
            _repositoryWrapper.LangTextRevNumberRepo.Update(gameVersionRev);

            if (!await _repositoryWrapper.GameVersionRepo.SaveAsync())
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.GameVersionUpdateFailed,
                    Message = RespondCode.GameVersionUpdateFailed.ApiRespondCodeString()
                });
            }

            if(!await _repositoryWrapper.LangTextRevNumberRepo.SaveAsync())
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextRevNumberUpdateFailed,
                    Message = RespondCode.LangtextRevNumberUpdateFailed.ApiRespondCodeString()
                });
            }

            return Ok(new MessageWithCode
            {
                Code = (int)RespondCode.Success,
                Message = RespondCode.Success.ApiRespondCodeString()
            });

        }

    }
}
