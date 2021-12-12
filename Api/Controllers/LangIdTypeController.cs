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
    [Route("api/langIdType")]
    [ApiController]
    public class LangIdTypeController : ControllerBase
    {
        private IRepositoryWrapper _repositoryWrapper;
        private IMapper _mapper;
        private UserManager<User> _userManager;
        private ILogger<LangIdTypeController> _logger;
        private string _loggerMessage;


        public LangIdTypeController(IRepositoryWrapper repositoryWrapper, IMapper mapper,
            UserManager<User> userManager, ILogger<LangIdTypeController> logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        [HttpGet()]
        public async Task<ActionResult<List<LangTypeCatalogDto>>> GetLangIdTypeAllAsync()
        {
            var langIdTypeList = await _repositoryWrapper.LangTypeCatalogRepo.GetAllAsync();

            var langIdTypeListDto = _mapper.Map<List<LangTypeCatalogDto>>(langIdTypeList);

            return langIdTypeListDto;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public async Task<IActionResult> AddNewLangIdTypeAsync(LangTypeCatalogDto langTypeCatalogDto)
        {
            var langIdType = _mapper.Map<LangTypeCatalogReview>(langTypeCatalogDto);
            //var userId = _userManager.GetUserId(HttpContext.User);  //获取操作者ID

            var langIdTypeInReview = await _repositoryWrapper.LangTypeCatalogReviewRepo.GetByIdAsync(langTypeCatalogDto.IdType);

            if (langIdTypeInReview != null)
            {
                langIdTypeInReview.IdTypeZH = langTypeCatalogDto.IdTypeZH;
                _repositoryWrapper.LangTypeCatalogReviewRepo.Update(langIdTypeInReview);
            }
            else
            {
                _repositoryWrapper.LangTypeCatalogReviewRepo.Create(langIdType);
            }

            if (!await _repositoryWrapper.LangTypeCatalogReviewRepo.SaveAsync())
            {
                _loggerMessage = RespondCode.LangtextLangTypeCatalogUpdateFailed.ApiRespondCodeString();
                _logger.LogError(_loggerMessage);

                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextLangTypeCatalogUpdateFailed,
                    Message = RespondCode.LangtextLangTypeCatalogUpdateFailed.ApiRespondCodeString()
                });
            }

            return Ok(new MessageWithCode
            {
                Code = (int)RespondCode.Success,
                Message = RespondCode.Success.ApiRespondCodeString()
            });
        }

        [Authorize]
        [HttpGet("review")]
        public async Task<ActionResult<List<LangTypeCatalogDto>>> GetLangIdTypeInReviewAllAsync()
        {
            var langIdTypeList = await _repositoryWrapper.LangTypeCatalogReviewRepo.GetAllAsync();

            var langIdTypeListDto = _mapper.Map<List<LangTypeCatalogDto>>(langIdTypeList);

            return langIdTypeListDto;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("review")]
        public async Task<IActionResult> LangIdTypeReviewAsync(List<int> Ids)
        {
            foreach(var i in Ids)
            {
                var langIdTypeInRewiew = await _repositoryWrapper.LangTypeCatalogReviewRepo.GetByIdAsync(i);

                if (langIdTypeInRewiew != null)
                {
                    var langIdType = await _repositoryWrapper.LangTypeCatalogRepo.GetByIdAsync(i);

                    if (langIdType != null)
                    {
                        langIdType.IdTypeZH = langIdTypeInRewiew.IdTypeZH;
                        _repositoryWrapper.LangTypeCatalogRepo.Update(langIdType);
                    }
                    else
                    {
                        var langIdTypeFromReview = _mapper.Map<LangTypeCatalog>(langIdTypeInRewiew);
                        _repositoryWrapper.LangTypeCatalogRepo.Create(langIdTypeFromReview);
                    }

                    _repositoryWrapper.LangTypeCatalogReviewRepo.Delete(langIdTypeInRewiew);
                }
            }

            var langIdTypeRev = await _repositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(4);
            langIdTypeRev.Rev++;
            _repositoryWrapper.LangTextRevNumberRepo.Update(langIdTypeRev);

            if (!await _repositoryWrapper.LangTypeCatalogRepo.SaveAsync())
            {
                _loggerMessage = RespondCode.LangtextLangTypeCatalogUpdateFailed.ApiRespondCodeString();
                _logger.LogError(_loggerMessage);

                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextLangTypeCatalogUpdateFailed,
                    Message = RespondCode.LangtextLangTypeCatalogUpdateFailed.ApiRespondCodeString()
                });
            }
            else
            {
                if (!await _repositoryWrapper.LangTextRevNumberRepo.SaveAsync())
                {
                    _loggerMessage = RespondCode.LangtextRevNumberUpdateFailed.ApiRespondCodeString();
                    _logger.LogError(_loggerMessage);

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

        [Authorize(Roles = "Admin")]
        [HttpPost("review/del")]
        public async Task<IActionResult> DeleteLangIdTypeInReview(List<int> Ids)
        {
            foreach(int i in Ids)
            {
                var langIdTypeInRewiew = await _repositoryWrapper.LangTypeCatalogReviewRepo.GetByIdAsync(i);

                if (langIdTypeInRewiew != null)
                {
                    _repositoryWrapper.LangTypeCatalogReviewRepo.Delete(langIdTypeInRewiew);
                }
            }

            if (!await _repositoryWrapper.LangTypeCatalogReviewRepo.SaveAsync())
            {
                _loggerMessage = RespondCode.LangtextLangTypeCatalogReviewUpdateFailed.ApiRespondCodeString();
                _logger.LogError(_loggerMessage);

                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextLangTypeCatalogReviewUpdateFailed,
                    Message = RespondCode.LangtextLangTypeCatalogReviewUpdateFailed.ApiRespondCodeString()
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
