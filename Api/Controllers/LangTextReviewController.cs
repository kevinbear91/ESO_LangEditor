using API.Services;
using AutoMapper;
using Core.Entities;
using Core.EnumTypes;
using Core.Models;
using Core.RequestParameters;
using EFCore.RepositoryWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/langtext/review")]
    [ApiController]
    public class LangTextReviewController : ControllerBase
    {
        private IRepositoryWrapper _repositoryWrapper;
        private IMapper _mapper;
        private UserManager<User> _userManager;
        private ILogger<LangTextReviewController> _logger;
        private string _loggerMessage;
        private ILangTextService _langTextService;

        public LangTextReviewController(IRepositoryWrapper repositoryWrapper, IMapper mapper, UserManager<User> userManager,
            ILogger<LangTextReviewController> logger, ILangTextService langTextService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
            _langTextService = langTextService;
        }

        [Authorize(Roles = "Reviewer")]
        public async Task<ActionResult<IEnumerable<LangTextReview>>> GetLangTextAllAsync([FromQuery] PageParameters pageParameters)
        {
            var langtextList = await _repositoryWrapper.LangTextReviewRepo.GetAllAsync(pageParameters);

            return langtextList.ToList();
        }

        [Authorize(Roles = "Editor")]
        [HttpGet("{langtextGuid}")]
        public async Task<ActionResult<LangTextForReviewDto>> GetLangTextByGuidAsync(Guid langtextGuid)
        {
            var langtext = await _repositoryWrapper.LangTextReviewRepo.GetByIdAsync(langtextGuid);

            if (langtext == null)
            {
                return NotFound(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextNotOnServer,
                    Message = RespondCode.LangtextNotOnServer.ApiRespondCodeString()
                });
            }
            else
            {
                //var langDto = _mapper.Map<LangTextDto>(langtext);
                return _mapper.Map<LangTextForReviewDto>(langtext);
            }

        }

        [Authorize(Roles = "Editor")]
        [HttpGet("user/{userGuid}")]
        public async Task<ActionResult<List<LangTextForReviewDto>>> GetLangTextByUserGuidAsync(Guid userGuid)
        {
            var langtext = await _repositoryWrapper.LangTextReviewRepo.GetByConditionAsync(lang => lang.UserId == userGuid);
            //var langlist = langtext.ToList();

            if (langtext == null)
            {
                _loggerMessage = "当前用户待审核列表查找失败，用户Id： " + userGuid;
                _logger.LogError(_loggerMessage);

                return NotFound(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextNotOnServer,
                    Message = RespondCode.LangtextNotOnServer.ApiRespondCodeString()
                });
            }
            else
            {
                var LangTextReviewDtolist = _mapper.Map<List<LangTextForReviewDto>>(langtext);
                return LangTextReviewDtolist;
            }

        }

        [Authorize(Roles = "Editor")]
        [HttpGet("users")]
        public async Task<ActionResult<List<Guid>>> GetReviewUserListAsync()
        {
            var langtext = await _repositoryWrapper.LangTextReviewRepo.SelectByConditionWithDistinctAsync(u => u.UserId);

            if (langtext == null)
            {
                _loggerMessage = "当前待审核列表为空！ ";
                _logger.LogError(_loggerMessage);

                return NotFound(new MessageWithCode
                {
                    Code = (int)RespondCode.UserNotFound,
                    Message = RespondCode.LangtextNotOnServer.ApiRespondCodeString()
                });
            }
            else
            {
                return langtext.ToList();
            }

        }

        //[Authorize(Roles = "Reviewer")]
        //[HttpGet("{langtextGuid}")]
        //public async Task<ActionResult> ReviewLangTextAsync(Guid langtextGuid)
        //{
        //    var userId = _userManager.GetUserId(HttpContext.User);
        //    var langtextReview = await _repositoryWrapper.LangTextReviewRepo.GetByIdAsync(langtextGuid);

        //    if (langtextReview == null)
        //    {
        //        _loggerMessage = "Pass Langtext in review not found, lang id: " + langtextGuid.ToString();
        //        _logger.LogError(_loggerMessage);
        //        return NotFound();
        //    }

        //    langtextReview.ReviewerId = new Guid(userId);      //Set reviewer ID.
        //    langtextReview.ReviewTimestamp = DateTime.UtcNow;  //Set review timestamp.

        //    var langtext = _mapper.Map<LangTextReview, LangText>(langtextReview);


        //    switch(langtextReview.ReasonFor)
        //    {
        //        case ReviewReason.NewAdded:
        //            _repositoryWrapper.LangTextRepo.Create(langtext);
        //            break;
        //        case ReviewReason.EnChanged:
        //            //Update main table langtext.
        //            _repositoryWrapper.LangTextRepo.Update(langtext);
        //            break;
        //        case ReviewReason.ZhChanged:
        //            //Update main table langtext.
        //            _repositoryWrapper.LangTextRepo.Update(langtext);
        //            break;
        //        case ReviewReason.Deleted:
        //            _repositoryWrapper.LangTextRepo.Delete(langtext);
        //            break;
        //        case 0:
        //            _loggerMessage = "Pass Langtext in review Error, lang id: " + langtextGuid.ToString() 
        //                + ", Reason: " + langtextReview.ReasonFor
        //                + ", User: " + userId;
        //            _logger.LogError(_loggerMessage);
        //            throw new Exception("闹呢？除了增改删还有啥？langtextID: " + langtextGuid.ToString()
        //                + "用户ID：" + userId.ToString());
        //    }


        //    //Update main table langtext.
        //    _repositoryWrapper.LangTextRepo.Update(langtext);    

        //    if (!await _repositoryWrapper.LangTextRepo.SaveAsync())
        //    {
        //        _loggerMessage = "Pass Langtext in review failed, lang id: " + langtextGuid.ToString()
        //                + ", User: " + userId;
        //        _logger.LogError(_loggerMessage);
        //        throw new Exception("审核资源 LangtextID: "+ langtextGuid.ToString() + " 失败。" +
        //            "操作人：" + userId.ToString() + "。");
        //    }
        //    else
        //    {
        //        await MoveToArchiveAsync(langtextReview);
        //        //await LangtextReviseAsync(langtextGuid, langtextReview.ReasonFor);

        //        //Delete langtext after review.
        //        var langtextReviewDel = await _repositoryWrapper.LangTextReviewRepo.GetByIdAsync(langtextGuid);
        //        _repositoryWrapper.LangTextReviewRepo.Delete(langtextReviewDel);

        //        await _repositoryWrapper.LangTextArchiveRepo.SaveAsync();

        //        return NoContent();
        //    }

        //}

        [Authorize(Roles = "Reviewer")]
        [HttpPost()]
        public async Task<ActionResult> ReviewLangTextAsync(List<Guid> langtextIdList)
        {
            var userId = _userManager.GetUserId(HttpContext.User);  //获取操作者ID
            List<LangTextArchive> langTextArchives = new List<LangTextArchive>();   //建立待归档列表
            List<LangTextRevisedDto> langTextRevisedDtos = new List<LangTextRevisedDto>();  //建立步进状态DTO列表

            var LangtextRevNumberCurrent = await _repositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(1); //查询获取新的步进号
            LangtextRevNumberCurrent.Rev++;
            int revisedNumber = LangtextRevNumberCurrent.Rev;

            foreach (var id in langtextIdList)
            {
                var langtextReview = await _repositoryWrapper.LangTextReviewRepo.GetByIdAsync(id);   //查询待审核文本
                var langtextReviewDel = langtextReview; //设置当前审核文本待删除

                if (langtextReview != null)
                {
                    langtextReview.ReviewerId = new Guid(userId);       //Set reviewer ID.
                    langtextReview.ReviewTimestamp = DateTime.UtcNow;      //Set review timestamp.
                    langtextReview.Revised = revisedNumber;

                    var langtext = _mapper.Map<LangTextReview, LangText>(langtextReview);
                    //langtext.LangtextInReview = null;
                    //langtext.LangtextInReivewId = null;

                    var langtextArchive = _mapper.Map<LangTextReview, LangTextArchive>(langtextReview);

                    //检查审核原因
                    switch (langtextReview.ReasonFor)
                    {
                        case ReviewReason.NewAdded:     //如果为新增
                            _repositoryWrapper.LangTextRepo.Create(langtext);    //主库创建文本
                            break;
                        case ReviewReason.EnChanged:    //如果为英文修改
                            _repositoryWrapper.LangTextRepo.Update(langtext);    //主库更新文本
                            break;
                        case ReviewReason.ZhChanged:    //如果为中文修改
                            _repositoryWrapper.LangTextRepo.Update(langtext);    //主库更新文本
                            break;
                        case ReviewReason.Deleted:  //如果为移除项
                            _repositoryWrapper.LangTextRepo.Delete(langtext);    //主库删除文本
                            break;
                        case 0: //如果枚举出错
                            _loggerMessage = "Pass Langtext in review failed, lang id: " + langtextReview.Id.ToString()
                                + ", Reason: " + langtextReview.ReasonFor
                                + ", User: " + userId;
                            _logger.LogError(_loggerMessage);
                            throw new Exception("闹呢？除了增改删还有啥？langtextID: "
                                + langtextReview.Id.ToString()
                                + "用户ID：" + userId.ToString());
                    }

                    langTextArchives.Add(MakeReivewToLangtextArchive(langtextArchive)); //将当前审核文本设置为归档
                    langTextRevisedDtos.Add(MakeLangtextRevisedDto(LangtextRevNumberCurrent, langtextReview));  //步进库增加步进号与langtext ID

                    //删除当前待审核文本
                    _repositoryWrapper.LangTextReviewRepo.Delete(langtextReviewDel);
                }

            }

            if (!await _repositoryWrapper.LangTextRepo.SaveAsync())
            {
                _loggerMessage = "Pass Langtext in review failed, lang count: " + langtextIdList.Count
                                + ", User: " + userId;
                _logger.LogError(_loggerMessage);

                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextReviewFailed,
                    Message = RespondCode.LangtextReviewFailed.ApiRespondCodeString()
                });

                //throw new Exception("审核资源失败。" + "操作人：" + userId.ToString() + "。");
            }
            else
            {
                await _langTextService.LangtextReviseListAsync(LangtextRevNumberCurrent, langTextRevisedDtos);
                await _langTextService.ArchiveListAsync(new Guid(userId), langTextArchives);

                return Ok(new MessageWithCode
                {
                    Code = (int)RespondCode.Success,
                    Message = RespondCode.Success.ApiRespondCodeString()
                });
            }


            static LangTextArchive MakeReivewToLangtextArchive(LangTextArchive langArchive)
            {
                if (langArchive.ReasonFor != ReviewReason.Deleted)
                    langArchive.Id = Guid.NewGuid();

                langArchive.ArchiveTimestamp = DateTime.UtcNow;

                return langArchive;
            }

            static LangTextRevisedDto MakeLangtextRevisedDto(LangTextRevNumber RevNumber, LangTextReview langReview)
            {
                LangTextRevisedDto langTextRevisedDto = new LangTextRevisedDto
                {
                    LangtextID = langReview.Id,
                    LangTextRevNumber = RevNumber.Rev,
                    ReasonFor = langReview.ReasonFor
                };

                return langTextRevisedDto;
            }

        }

        [Authorize(Roles = "Editor")]
        [HttpPost("del/list")]
        public async Task<IActionResult> DeleteLangTextInReview(List<Guid> langtextIds)
        {
            var userId = _userManager.GetUserId(HttpContext.User);  //获取操作者ID
            var user = await _userManager.FindByIdAsync(userId);

            bool isAdmin = false;

            List<LangTextReview> langTextReviewsNoPermission = new List<LangTextReview>();

            if (user == null)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.UserNotFound,
                    Message = RespondCode.UserNotFound.ApiRespondCodeString()
                });
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                isAdmin = true;
            }

            foreach (var langId in langtextIds)
            {

                var langtext = await _repositoryWrapper.LangTextReviewRepo.GetByIdAsync(langId);

                if (langtext != null)
                {
                    if (langtext.UserId == user.Id)
                    {
                        _repositoryWrapper.LangTextReviewRepo.Delete(langtext);
                    }
                    else if (isAdmin)
                    {
                        _repositoryWrapper.LangTextReviewRepo.Delete(langtext);
                    }
                    else
                    {
                        langTextReviewsNoPermission.Add(langtext);
                    }
                }
            }

            if (langtextIds.Count == langTextReviewsNoPermission.Count)
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextReviewDeleteNoPermission,
                    Message = RespondCode.LangtextReviewDeleteNoPermission.ApiRespondCodeString()
                });
            }

            if (!await _repositoryWrapper.LangTextReviewRepo.SaveAsync())
            {
                return BadRequest(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextReviewFailed,
                    Message = RespondCode.LangtextReviewFailed.ApiRespondCodeString()
                });
            }

            if (langTextReviewsNoPermission.Count >= 1)
            {
                return Ok(new MessageWithCode
                {
                    Code = (int)RespondCode.LangtextReviewDeleteIncludeNoPermission,
                    Message = RespondCode.LangtextReviewDeleteIncludeNoPermission.ApiRespondCodeString()
                });
            }

            return Ok(new MessageWithCode
            {
                Code = (int)RespondCode.Success,
                Message = RespondCode.Success.ApiRespondCodeString()
            });


        }

        //private async Task<LangText> NewAddedLangtextReview(Guid userId, LangTextReview langTextReview)
        //{

        //    langTextReview.ReviewerId = userId;      //Set reviewer ID.
        //    langTextReview.ReviewTimestamp = DateTime.Now;  //Set review timestamp.

        //    LangText langtext = null;

        //    switch (langTextReview.ReasonFor)
        //    {
        //        case ReviewReason.NewAdded:
        //            langtext = Mapper.Map<LangTextReview, LangText>(langTextReview);
        //            break;
        //        case ReviewReason.EnChanged:

        //            break;
        //        case ReviewReason.ZhChanged:

        //            break;
        //        case ReviewReason.Deleted:

        //            break;
        //    }





        //    return langtext;
        //}

        //private async Task MoveToArchiveAsync(LangTextReview langTextReview)
        //{

        //    //var langtext = Mapper.Map<LangTextReview, LangText>(langTextReview);

        //    var langtextArchive = _mapper.Map<LangTextReview, LangTextArchive>(langTextReview);
        //    langtextArchive.ArchiveTimestamp = DateTime.UtcNow;
        //    if (langtextArchive.ReasonFor != ReviewReason.Deleted)
        //        langtextArchive.Id = Guid.NewGuid();

        //    //Move review langtext to archive.
        //    _repositoryWrapper.LangTextArchiveRepo.Create(langtextArchive);

        //    if (!await _repositoryWrapper.LangTextArchiveRepo.SaveAsync())
        //    {
        //        throw new Exception("归档资源LangtextID: " + langTextReview.Id.ToString() + " 失败。" +
        //            "操作人：" + langTextReview.UserId.ToString() + "。");
        //    }
        //}

        //private async Task ListMoveToArchiveAsync(List<LangTextArchive> langTextArchive)
        //{
        //    var userId = _userManager.GetUserId(HttpContext.User);

        //    _repositoryWrapper.LangTextArchiveRepo.CreateList(langTextArchive);

        //    if (!await _repositoryWrapper.LangTextArchiveRepo.SaveAsync())
        //    {
        //        _loggerMessage = "Pass Langtext in review to archive failed, lang count: " + langTextArchive.Count
        //                        + ", User: " + userId;
        //        _logger.LogError(_loggerMessage);
        //        throw new Exception("归档资源 失败。" +
        //            "操作人：" + userId.ToString() + "。");
        //    }
        //}

        //private async Task LangtextReviseAsync(Guid langtextId, ReviewReason reason)
        //{
        //    var LangtextRevNumberCurrent = await _repositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(1);
        //    int newRevNumber = LangtextRevNumberCurrent.LangTextRev + 1;

        //    LangTextRevisedDto langTextRevisedDto = new LangTextRevisedDto
        //    {
        //        LangtextID = langtextId,
        //        LangTextRevNumber = newRevNumber,
        //        ReasonFor = reason
        //    };

        //    var langtextRevised = _mapper.Map<LangTextRevised>(langTextRevisedDto);

        //    _repositoryWrapper.LangTextRevisedRepo.Create(langtextRevised);  //Insert to Revised table.

        //    if (!await _repositoryWrapper.LangTextRevisedRepo.SaveAsync())
        //    {
        //        _loggerMessage = "Pass Langtext in review for revised failed, lang id: " + langtextId.ToString();
        //        _logger.LogError(_loggerMessage);

        //        throw new Exception("步进资源 LangtextID: " + langtextId.ToString() + " 失败。");
        //    }

        //    //LangTextRevNumberDto langTextRevNumberDto = new LangTextRevNumberDto
        //    //{
        //    //    LangTextRev = newRevNumber,
        //    //};

        //    //var langtextRevNumber = Mapper.Map<LangTextRevNumber>(langTextRevNumberDto);

        //    LangtextRevNumberCurrent.LangTextRev = newRevNumber;   //New modify RevNumber.

        //    _repositoryWrapper.LangTextRevNumberRepo.Update(LangtextRevNumberCurrent);   //Update RevNumber.

        //    if (!await _repositoryWrapper.LangTextRevNumberRepo.SaveAsync())
        //    {
        //        _loggerMessage = "Update revised number failed, revised number: " + newRevNumber.ToString();
        //        _logger.LogError(_loggerMessage);
        //        throw new Exception("步进号 " + newRevNumber.ToString() + " 更新失败。");
        //    }

        //}

        //private async Task<LangTextRevNumber> GetNewLangtextRevNumber()
        //{
        //    //var LangtextRevNumberCurrent = await RepositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(1);
        //    //int newRevNumber = LangtextRevNumberCurrent.LangTextRev + 1;

        //    return await RepositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(1);
        //}

        //private async Task LangtextReviseAsync(LangTextRevNumber RevNumber, List<LangTextRevisedDto> langTextRevisedDtos)
        //{
        //    var langtextRevised = _mapper.Map<List<LangTextRevised>>(langTextRevisedDtos);
        //    _repositoryWrapper.LangTextRevisedRepo.CreateList(langtextRevised);  //Insert to Revised table.

        //    if (!await _repositoryWrapper.LangTextRevisedRepo.SaveAsync())
        //    {
        //        _loggerMessage = "Update revised number failed, revised number: " + RevNumber.ToString();
        //        _logger.LogError(_loggerMessage);
        //        throw new Exception("步进资源失败。");
        //    }

        //    //LangTextRevNumber LangtextRevNumberCurrent = new LangTextRevNumber
        //    //{
        //    //    Id = 1,
        //    //    LangTextRev = newRevNumber,
        //    //};

        //    //var langtextRevNumberCurrent = Mapper.Map<LangTextRevNumber>(LangtextRevNumberCurrentDto);
        //    _repositoryWrapper.LangTextRevNumberRepo.Update(RevNumber);   //Update RevNumber.

        //    if (!await _repositoryWrapper.LangTextRevNumberRepo.SaveAsync())
        //    {
        //        _loggerMessage = "Update revised number failed, Rev number: " + RevNumber.LangTextRev.ToString();
        //        _logger.LogError(_loggerMessage);
        //        throw new Exception("步进号 " + RevNumber.LangTextRev.ToString() + " 更新失败。");
        //    }

        //}


    }
}
