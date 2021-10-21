using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.Core.RequestParameters;
using ESO_LangEditor.EFCore.RepositoryWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ESO_LangEditor.API.Controllers
{
    [Route("api/langtext")]
    [ApiController]
    public class LangTextController : ControllerBase
    {
        //public ILangTextRepository LangTextRepository { get; }
        private IRepositoryWrapper _repositoryWrapper;
        private IMapper _mapper;
        private UserManager<User> _userManager;
        private ILogger<LangTextController> _logger;
        private string _loggerMessage;

        public LangTextController(IRepositoryWrapper repositoryWrapper, IMapper mapper, 
            UserManager<User> userManager, ILogger<LangTextController> logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult<List<LangTextDto>>> GetLangTextAllAsync()
        {
            Request.Headers.TryGetValue("langTextParameters", out var langTextParameters);
            var para = JsonSerializer.Deserialize<LangTextParameters>(langTextParameters);

            var langtextList = await _repositoryWrapper.LangTextRepo.GetAllAsync(para);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(langtextList.PageData));

            var langtextDto = _mapper.Map<List<LangTextDto>>(langtextList);

            return langtextDto;
        }

        [Authorize]
        [HttpGet("zh")]
        public async Task<ActionResult<List<LangTextDto>>> GetLangTextsZhByConditionAsync()
        {
            Request.Headers.TryGetValue("langTextParameters", out var langTextParameters);
            var para = JsonSerializer.Deserialize<LangTextParameters>(langTextParameters);

            var langtextList = await _repositoryWrapper.LangTextRepo.GetLangTextsZhByConditionAsync(para);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(langtextList.PageData));

            var langtextDto = _mapper.Map<List<LangTextDto>>(langtextList);

            return langtextDto;
        }

        [Authorize]
        [HttpGet("en")]
        public async Task<ActionResult<List<LangTextDto>>> GetLangTextsEnByConditionAsync()
        {
            Request.Headers.TryGetValue("langTextParameters", out var langTextParameters);
            _loggerMessage = "RequestPara: " + langTextParameters;
            _logger.LogError(_loggerMessage);

            var para = JsonSerializer.Deserialize<LangTextParameters>(langTextParameters, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var langtextList = await _repositoryWrapper.LangTextRepo.GetLangTextsEnByConditionAsync(para);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(langtextList.PageData));

            var langtextDto = _mapper.Map<List<LangTextDto>>(langtextList);

            return langtextDto;
        }

        [Authorize(Roles = "Editor")]
        [HttpGet("id/{langtextGuid}")]
        public async Task<ActionResult<LangTextDto>> GetLangTextByGuidAsync(Guid langtextGuid)
        {
            var langtext = await _repositoryWrapper.LangTextRepo.GetByIdAsync(langtextGuid);
            var langtextDto = _mapper.Map<LangTextDto>(langtext);

            if (langtext == null)
            {
                _loggerMessage = "Langtext not exist, guid: " + langtextGuid;
                _logger.LogError(_loggerMessage);
                return NotFound();
            }
            else
            {
                return langtextDto;
            }

        }

        [Authorize]
        [HttpGet("idType")]
        public async Task<ActionResult<List<LangTextDto>>> GetLangTextsByIdTypeAsync()
        {
            Request.Headers.TryGetValue("langTextParameters", out var langTextParameters);
            var para = JsonSerializer.Deserialize<LangTextParameters>(langTextParameters);

            var langtextList = await _repositoryWrapper.LangTextRepo.GetLangTextsByConditionAsync(lang => lang.IdType == Convert.ToInt32(para.SearchTerm), para);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(langtextList.PageData));

            var langtextDto = _mapper.Map<List<LangTextDto>>(langtextList);

            return langtextDto;
        }

        [Authorize]
        [HttpGet("gameupdate")]
        public async Task<ActionResult<List<LangTextDto>>> GetLangTextsByGameVersionAsync()
        {
            Request.Headers.TryGetValue("langTextParameters", out var langTextParameters);
            var para = JsonSerializer.Deserialize<LangTextParameters>(langTextParameters);

            var langtextList = await _repositoryWrapper.LangTextRepo.GetLangTextsByConditionAsync(lang => lang.UpdateStats == para.SearchTerm, para);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(langtextList.PageData));

            var langtextDto = _mapper.Map<List<LangTextDto>>(langtextList);

            return langtextDto;
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<List<LangTextDto>>> GetLangTextsByUserAsync()
        {
            Request.Headers.TryGetValue("langTextParameters", out var langTextParameters);
            var para = JsonSerializer.Deserialize<LangTextParameters>(langTextParameters);

            var langtextList = await _repositoryWrapper.LangTextRepo.GetLangTextsByConditionAsync(lang => lang.UserId == new Guid(para.SearchTerm), para);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(langtextList.PageData));

            var langtextDto = _mapper.Map<List<LangTextDto>>(langtextList);

            return langtextDto;
        }

        [Authorize]
        [HttpGet("reviewer")]
        public async Task<ActionResult<List<LangTextDto>>> GetLangTextsByReviewerAsync()
        {
            Request.Headers.TryGetValue("langTextParameters", out var langTextParameters);
            var para = JsonSerializer.Deserialize<LangTextParameters>(langTextParameters);

            var langtextList = await _repositoryWrapper.LangTextRepo.GetLangTextsByConditionAsync(lang => lang.ReviewerId == new Guid(para.SearchTerm), para);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(langtextList.PageData));

            var langtextDto = _mapper.Map<List<LangTextDto>>(langtextList);

            return langtextDto;
        }


        [Authorize]
        [HttpGet("rev/{revisednumber}")]
        public async Task<ActionResult<List<LangTextDto>>> GetLangTextByRevisedAsync(int revisednumber)
        {
            var langtext = await _repositoryWrapper.LangTextRepo.GetByConditionAsync(lang => lang.Revised == revisednumber);
            var langtextDto = _mapper.Map<List<LangTextDto>>(langtext);

            if (langtextDto.Count == 0)
            {
                _loggerMessage = "Langtext Revised not exist, number: " + revisednumber;
                _logger.LogError(_loggerMessage);
                return NotFound();
            }
            else
            {
                return langtextDto;
            }

        }

        [Authorize(Roles = "Editor")]
        [HttpGet("list")]
        public async Task<ActionResult<List<LangTextDto>>> GetLangTextByGuidListAsync(List<Guid> langtextGuids)
        {
            List<LangText> langTexts = new List<LangText>();

            foreach(var id in langtextGuids)
            {
                var langtext = await _repositoryWrapper.LangTextRepo.GetByIdAsync(id);

                if (langtext != null)
                {
                    langTexts.Add(langtext);
                }
                else
                {
                    _loggerMessage = "Langtext not exist, guid: " + id;
                    _logger.LogError(_loggerMessage);
                }
            }

            var langtextsDto = _mapper.Map<List<LangTextDto>>(langTexts);

            return langtextsDto;

            //var langtext = await RepositoryWrapper.LangTextRepo.GetByIdAsync(langtextGuid);
            //var langtextDto = Mapper.Map<LangTextDto>(langtext);

            //if (langtext == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    return langtextDto;
            //}

        }

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public async Task<ActionResult> CreateLangtextAsync(LangTextForCreationDto langTextForCreation)
        //{
        //    var userIdFromToken = _userManager.GetUserId(HttpContext.User);
        //    //var langtext = Mapper.Map<LangText>(langTextForCreation);
        //    var langtext = _mapper.Map<LangTextReview>(langTextForCreation);

        //    //RepositoryWrapper.LangTextRepo.Create(langtext);

        //    langtext.ReasonFor = ReviewReason.NewAdded;
        //    langtext.ReviewerId = new Guid(userIdFromToken);
        //    langtext.ReviewTimestamp = DateTime.UtcNow;

        //    _repositoryWrapper.LangTextReviewRepo.Create(langtext);

        //    if (!await _repositoryWrapper.LangTextReviewRepo.SaveAsync())
        //    {
        //        _loggerMessage = "Langtext create faild, guid: " + langtext.Id 
        //            + ", textId: " + langtext.TextId
        //            + ", textEn: " + langtext.TextEn
        //            + ", User: " + userIdFromToken;
        //        _logger.LogError(_loggerMessage);

        //        throw new Exception("创建资源langtext失败");
        //    }

        //    //var langtextDto = Mapper.Map<LangTextDto>(langtext);

        //    return Ok(); //CreatedAtRoute(nameof(GetLangTextByGuidAsync), new { langtextID = langtextDto.Id }, langtextDto);

        //}

        [Authorize(Roles = "Admin")]
        [DisableRequestSizeLimit]
        [HttpPost("new/list")]
        public async Task<IActionResult> CreateLangtextListAsync(List<LangTextForCreationDto> langTextsForCreation)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            //var langtext = Mapper.Map<LangText>(langTextForCreation);
            var langtexts = _mapper.Map<List<LangTextReview>>(langTextsForCreation);

            foreach(var lang in langtexts)
            {
                lang.UserForModify = user;
                lang.UserForModify = user;
                lang.ReviewerId = new Guid(userId);
                lang.ReviewTimestamp = DateTime.UtcNow;
                lang.ReasonFor = ReviewReason.NewAdded;
            }

            _repositoryWrapper.LangTextReviewRepo.CreateList(langtexts);

            if (!await _repositoryWrapper.LangTextReviewRepo.SaveAsync())
            {
                _loggerMessage = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.LangtextAddedFailed)
                    + ", User: " + userId;
                _logger.LogError(_loggerMessage);

                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextAddedFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.LangtextAddedFailed)
                });
            }

            return Ok(new MessageWithCode
            {
                Code = (int)RespondCode.Success,
                Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.Success)
            });

        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{langtextID}")]
        public async Task<IActionResult> DeleteLangTextAsync(Guid langtextID)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var langtext = await _repositoryWrapper.LangTextRepo.GetByIdAsync(langtextID);

            if (langtext == null)
            {
                return NotFound(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextNotOnServer,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.LangtextNotOnServer)
                });
            }
            //Check if langtext already in Review.
            if (await _repositoryWrapper.LangTextReviewRepo.GetByIdAsync(langtextID) != null)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextInReview,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.LangtextInReview)
                });
                //throw new Exception("当前文本已待审核。");
            }

            var langtextReview = _mapper.Map<LangTextReview>(langtext);
            langtextReview.ReasonFor = ReviewReason.Deleted;
            langtext.ReviewerId = new Guid(userId);
            langtext.ReviewTimestamp = DateTime.Now;

            //Mapper.Map(langtextReview, typeof(LangTextForUpdateZhDto), typeof(LangTextReview));

            //Move to ReviewRepo wating for review.
            _repositoryWrapper.LangTextReviewRepo.Create(langtextReview);

            if (!await _repositoryWrapper.LangTextReviewRepo.SaveAsync())
            {
                _loggerMessage = "Create Langtext review fro delete faild, list count: " + langtextID
                    + ", User: " + userId;
                _logger.LogError(_loggerMessage);

                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextUpdateFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.LangtextUpdateFailed)
                });

                //throw new Exception("加入审核表失败，langtextID: " + langtextReview.Id.ToString());
            }

            return Ok(new MessageWithCode
            {
                Code = (int)RespondCode.Success,
                Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.Success)
            });

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete()]
        public async Task<IActionResult> DeleteLangTextListAsync(List<Guid> langtextIds)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            List<LangTextReview> langTexts = new List<LangTextReview>();

            foreach (var id in langtextIds)
            {
                var langtext = await _repositoryWrapper.LangTextRepo.GetByIdAsync(id);

                if (langtext != null)
                {
                    var langtextToReview = _mapper.Map<LangTextReview>(langtext);

                    langtextToReview.UserId = new Guid(userId);
                    langtextToReview.ReasonFor = ReviewReason.Deleted;
                    langtextToReview.ReviewTimestamp = DateTime.UtcNow;

                    langTexts.Add(langtextToReview);
                }
            }

            _repositoryWrapper.LangTextReviewRepo.CreateList(langTexts);

            if (!await _repositoryWrapper.LangTextReviewRepo.SaveAsync())
            {
                _loggerMessage = "Create Langtext review for delete list faild, list count: " + langtextIds.Count
                    + ", User: " + userId;
                _logger.LogError(_loggerMessage);

                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextUpdateFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.LangtextUpdateFailed)
                });
                //throw new Exception("加入审核表失败");
            }

            return Ok(new MessageWithCode
            {
                Code = (int)RespondCode.Success,
                Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.Success)
            });

        }

        [Authorize(Roles = "Editor")]
        [HttpPost("zh/{langtextID}")]
        public async Task<IActionResult> UpdateLangtextZhAsync(Guid langtextID, LangTextForUpdateZhDto langTextForUpdateZh)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            //_userManager.IsInRoleAsync()


            var langtext = await _repositoryWrapper.LangTextRepo.GetByIdAsync(langtextID);

            if (langtext == null)
            {
                _loggerMessage = "当前更新文本不存在于Langtext表内, langtext Id: " + langtextID
                    + ", User: " + userId;
                _logger.LogError(_loggerMessage);

                return NotFound(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextNotOnServer,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.LangtextNotOnServer)
                });
            }

            //Check if langtext already in Review.
            var langtextInReview = await _repositoryWrapper.LangTextReviewRepo.GetByIdAsync(langtextID);

            if (langtextInReview != null)   //检查是否存在于审核表中
            {
                if (user.Id != langtextInReview.UserId)    //检查如果存在于表中，提交用户是否是之前用户 
                {
                    return BadRequest(new MessageWithCode
                    {
                        Code = (int)RespondCode.LangtextInReview,
                        Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.LangtextInReview)
                    }); //如果不是，返回已待审核提示
                }
                else
                {
                    //为否则更新当前待审核文本
                    _mapper.Map(langTextForUpdateZh, langtextInReview, typeof(LangTextForUpdateZhDto), typeof(LangTextReview));

                    _repositoryWrapper.LangTextReviewRepo.Update(langtextInReview);  //Update ReviewRepo wating for review.
                }
            }
            else //如果不在审核表内
            {
                //创建待审核文本
                var langtextReview = _mapper.Map<LangTextReview>(langtext);
                langtextReview.ReasonFor = ReviewReason.ZhChanged;

                _mapper.Map(langTextForUpdateZh, langtextReview, typeof(LangTextForUpdateZhDto), typeof(LangTextReview));
                _repositoryWrapper.LangTextReviewRepo.Create(langtextReview);    //Move to ReviewRepo wating for review.
            }

            if (!await _repositoryWrapper.LangTextReviewRepo.SaveAsync())
            {
                _loggerMessage = "当前更新文本保存至LangtextReview表失败，langtext Id: " + langtextID
                    + ", User: " + userId;
                _logger.LogError(_loggerMessage);

                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextUpdateFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.LangtextInReview)
                });
                
            }

            return Ok(new MessageWithCode
            {
                Code = (int)RespondCode.Success,
                Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.Success)
            });

        }

        [Authorize(Roles = "Editor")]
        [HttpPost("zh/list")]
        public async Task<IActionResult> UpdateLangtextZhListAsync(List<LangTextForUpdateZhDto> langTextForUpdateZhList)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            Guid userIdinGuid = new Guid(userId);

            //var langtext = await RepositoryWrapper.LangTextRepo.GetByIdAsync(langTextForUpdateZh.Id);

            List<LangText> langTexts = new List<LangText>();
            List<LangTextReview> langTextInReviews = new List<LangTextReview>(); 

            foreach(var lang in langTextForUpdateZhList)
            {
                var langtext = await _repositoryWrapper.LangTextRepo.GetByIdAsync(lang.Id);
                var langtextInReview = await _repositoryWrapper.LangTextReviewRepo.GetByIdAsync(lang.Id);

                if (lang != null)
                {
                    langTexts.Add(langtext);
                }

                if (langtextInReview != null)   //检查列表内文本是否存在于审核表内
                {
                    if (userIdinGuid == langtextInReview.UserId)    //如果用户ID与文本用户ID相同
                    {
                        _mapper.Map(lang, langtextInReview, typeof(LangTextForUpdateZhDto), typeof(LangTextReview));
                        _repositoryWrapper.LangTextReviewRepo.Update(langtextInReview);  //Update ReviewRepo wating for review.                                                           
                    }
                    else
                    {
                        langTextInReviews.Add(langtextInReview);    //如果ID不相同，加入待返回列表
                    }

                }
                else //如果不在审核表内
                {
                    //创建待审核文本
                    var langtextReview = _mapper.Map<LangTextReview>(langtext);
                    langtextReview.ReasonFor = ReviewReason.ZhChanged;

                    _mapper.Map(lang, langtextReview, typeof(LangTextForUpdateZhDto), typeof(LangTextReview));

                    _repositoryWrapper.LangTextReviewRepo.Create(langtextReview);    //Move to ReviewRepo wating for review.
                }
            }

            if (!await _repositoryWrapper.LangTextReviewRepo.SaveAsync())
            {
                _loggerMessage = "当前更新文本保存至LangtextReview表失败， langtext count: " + langTextForUpdateZhList.Count
                    + ", User: " + userId;
                _logger.LogError(_loggerMessage);

                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextUpdateFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.LangtextInReview)
                });
            }

            return Ok(new MessageWithCode
            {
                Code = (int)RespondCode.Success,
                Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.Success)
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{langtextID}/en")]
        public async Task<IActionResult> UpdateLangtextEnAsync(Guid langtextID, LangTextForUpdateEnDto langTextForUpdateEn)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var langtext = await _repositoryWrapper.LangTextRepo.GetByIdAsync(langtextID);

            if (langtext == null)
            {
                _loggerMessage = "当前更新文本不存在于Langtext表内, langtext Id: " + langtextID
                    + ", User: " + userId;
                _logger.LogError(_loggerMessage);

                return NotFound(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextNotOnServer,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.LangtextNotOnServer)
                });
            }
            _mapper.Map(langTextForUpdateEn, langtext, typeof(LangTextForUpdateEnDto), typeof(LangText));

            var langtextReview = _mapper.Map<LangTextReview>(langtext);
            langtextReview.ReasonFor = ReviewReason.EnChanged;
            langtextReview.UserId = new Guid(userId);
            langtextReview.EnLastModifyTimestamp = DateTime.UtcNow;

            //Move to ReviewRepo wating for review.
            _repositoryWrapper.LangTextReviewRepo.Create(langtextReview);

            if (!await _repositoryWrapper.LangTextRepo.SaveAsync())
            {
                _loggerMessage = "当前更新文本保存至LangtextReview表失败，langtext Id: " + langtextID
                    + ", User: " + userId;
                _logger.LogError(_loggerMessage);

                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextUpdateFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.LangtextInReview)
                });
            }

            return Ok(new MessageWithCode
            {
                Code = (int)RespondCode.Success,
                Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.Success)
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("en/list")]
        public async Task<IActionResult> UpdateLangtextEnListAsync(List<LangTextForUpdateEnDto> langTextForUpdateEns)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            List<LangTextReview> langTexts = new List<LangTextReview>();

            foreach (var enChanged in langTextForUpdateEns)
            {
                var langtext = await _repositoryWrapper.LangTextRepo.GetByIdAsync(enChanged.Id);

                if (langtext != null)
                {
                    langtext.UserId = new Guid(userId);
                    langtext.TextEn = enChanged.TextEn;
                    langtext.EnLastModifyTimestamp = DateTime.UtcNow;
                    langtext.UpdateStats = enChanged.UpdateStats;

                    var langtextReview = _mapper.Map<LangTextReview>(langtext);
                    langtextReview.ReasonFor = ReviewReason.EnChanged;

                    langTexts.Add(langtextReview);
                }
            }
            //Move to ReviewRepo wating for review.
            _repositoryWrapper.LangTextReviewRepo.CreateList(langTexts);

            if (!await _repositoryWrapper.LangTextRepo.SaveAsync())
            {
                _loggerMessage = "当前更新文本保存至LangtextReview表失败，langtext count: " + langTextForUpdateEns.Count
                    + ", User: " + userId;
                _logger.LogError(_loggerMessage);

                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextUpdateFailed,
                    Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.LangtextInReview)
                });
            }

            return Ok(new MessageWithCode
            {
                Code = (int)RespondCode.Success,
                Message = ApiRespondCodeExtensions.ApiRespondCodeString(RespondCode.Success)
            });
        }


    }
}
