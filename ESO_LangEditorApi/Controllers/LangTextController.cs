using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore.DataRepositories;
using ESO_LangEditor.EFCore.RepositoryWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ESO_LangEditor.API.Controllers
{
    [Route("api/langtext")]
    [ApiController]
    public class LangTextController : ControllerBase
    {
        //public ILangTextRepository LangTextRepository { get; }
        public IRepositoryWrapper RepositoryWrapper { get; }
        public IMapper Mapper { get; }
        private UserManager<User> _userManager;

        public LangTextController(IRepositoryWrapper repositoryWrapper, IMapper mapper, UserManager<User> userManager)
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
            _userManager = userManager;
        }
        
        public async Task<ActionResult<IEnumerable<LangText>>> GetLangTextAllAsync()
        {
            var langtextList = await RepositoryWrapper.LangTextRepo.GetAllAsync();

            return langtextList.ToList();
        }

        [Authorize(Roles = "editor")]
        [HttpGet("{langtextGuid}")]
        public async Task<ActionResult<LangTextDto>> GetLangTextByGuidAsync(Guid langtextGuid)
        {
            var langtext = await RepositoryWrapper.LangTextRepo.GetByIdAsync(langtextGuid);
            var langtextDto = Mapper.Map<LangTextDto>(langtext);

            if (langtext == null)
            {
                return NotFound();
            }
            else
            {
                return langtextDto;
            }

        }

        [Authorize(Roles = "editor")]
        [HttpGet("list")]
        public async Task<ActionResult<List<LangTextDto>>> GetLangTextByGuidListAsync(List<Guid> langtextGuids)
        {
            List<LangText> langTexts = new List<LangText>();

            foreach(var id in langtextGuids)
            {
                var langtext = await RepositoryWrapper.LangTextRepo.GetByIdAsync(id);

                if (langtext != null)
                {
                    langTexts.Add(langtext);
                }
            }

            var langtextsDto = Mapper.Map<List<LangTextDto>>(langTexts);

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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateLangtextAsync(Guid userId, LangTextForCreationDto langTextForCreation)
        {

            //var langtext = Mapper.Map<LangText>(langTextForCreation);
            var langtext = Mapper.Map<LangTextReview>(langTextForCreation);

            //RepositoryWrapper.LangTextRepo.Create(langtext);

            langtext.ReasonFor = ReviewReason.NewAdded;
            langtext.ReviewerId = userId;
            langtext.ReviewTimestamp = DateTime.UtcNow;

            RepositoryWrapper.LangTextReviewRepo.Create(langtext);

            if (!await RepositoryWrapper.LangTextReviewRepo.SaveAsync())
            {
                throw new Exception("创建资源langtext失败");
            }

            //var langtextDto = Mapper.Map<LangTextDto>(langtext);

            return Ok(); //CreatedAtRoute(nameof(GetLangTextByGuidAsync), new { langtextID = langtextDto.Id }, langtextDto);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateLangtextListAsync(List<LangTextForCreationDto> langTextForCreation)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            //var langtext = Mapper.Map<LangText>(langTextForCreation);
            var langtexts = Mapper.Map<List<LangTextReview>>(langTextForCreation);

            foreach(var lang in langtexts)
            {
                lang.ReviewerId = new Guid(userId);
                lang.ReviewTimestamp = DateTime.UtcNow;
                lang.ReasonFor = ReviewReason.NewAdded;
            }

            RepositoryWrapper.LangTextReviewRepo.CreateList(langtexts);

            if (!await RepositoryWrapper.LangTextReviewRepo.SaveAsync())
            {
                throw new Exception("创建审核资源langtext失败");
            }

            return Ok();

        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{langtextID}")]
        public async Task<ActionResult> DeleteLangTextAsync(Guid langtextID)
        {
            var langtext = await RepositoryWrapper.LangTextRepo.GetByIdAsync(langtextID);

            if (langtext == null)
            {
                return NotFound();
            }
            //Check if langtext already in Review.
            if (await RepositoryWrapper.LangTextReviewRepo.GetByIdAsync(langtextID) != null)
            {
                return BadRequest("当前文本已待审核。");
                //throw new Exception("当前文本已待审核。");
            }

            var langtextReview = Mapper.Map<LangTextReview>(langtext);
            langtextReview.ReasonFor = ReviewReason.Deleted;
            //langtext.ReviewerId = userId;
            //langtext.ReviewTimestamp = DateTime.Now;

            //Mapper.Map(langtextReview, typeof(LangTextForUpdateZhDto), typeof(LangTextReview));

            //Move to ReviewRepo wating for review.
            RepositoryWrapper.LangTextReviewRepo.Create(langtextReview);

            if (!await RepositoryWrapper.LangTextReviewRepo.SaveAsync())
            {
                throw new Exception("加入审核表失败，langtextID: " + langtextReview.Id.ToString());
            }

            return NotFound();

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete()]
        public async Task<ActionResult> DeleteLangTextListAsync(List<Guid> langtextIds)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            List<LangTextReview> langTexts = new List<LangTextReview>();

            foreach (var id in langtextIds)
            {
                var langtext = await RepositoryWrapper.LangTextRepo.GetByIdAsync(id);

                if (langtext != null)
                {
                    var langtextToReview = Mapper.Map<LangTextReview>(langtext);

                    langtextToReview.UserId = new Guid(userId);
                    langtextToReview.ReasonFor = ReviewReason.Deleted;
                    langtextToReview.ReviewTimestamp = DateTime.UtcNow;

                    langTexts.Add(langtextToReview);
                }
            }

            RepositoryWrapper.LangTextReviewRepo.CreateList(langTexts);

            if (!await RepositoryWrapper.LangTextReviewRepo.SaveAsync())
            {
                throw new Exception("加入审核表失败");
            }

            return Ok();

        }

        [Authorize(Roles = "editor")]
        [HttpPut("zh/{langtextID}")]
        public async Task<ActionResult> UpdateLangtextZhAsync(LangTextForUpdateZhDto langTextForUpdateZh)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            Guid userIdinGuid = new Guid(userId);

            var langtext = await RepositoryWrapper.LangTextRepo.GetByIdAsync(langTextForUpdateZh.Id);

            if (langtext == null)
            {
                return NotFound();
            }

            //Check if langtext already in Review.
            var langtextInReview = await RepositoryWrapper.LangTextReviewRepo.GetByIdAsync(langTextForUpdateZh.Id);

            if (langtextInReview != null)   //检查是否存在于审核表中
            {
                if (userIdinGuid != langtextInReview.UserId)    //检查如果存在于表中，提交用户是否是之前用户 
                {
                    return BadRequest("当前文本已待审核。"); //如果不是，返回已待审核提示
                }
                else
                {
                    //为否则更新当前待审核文本
                    Mapper.Map(langTextForUpdateZh, langtextInReview, typeof(LangTextForUpdateZhDto), typeof(LangTextReview));

                    RepositoryWrapper.LangTextReviewRepo.Update(langtextInReview);  //Update ReviewRepo wating for review.
                }
            }
            else //如果不在审核表内
            {
                //创建待审核文本
                Mapper.Map(langTextForUpdateZh, langtext, typeof(LangTextForUpdateZhDto), typeof(LangText));

                var langtextReview = Mapper.Map<LangTextReview>(langtext);
                langtextReview.ReasonFor = ReviewReason.ZhChanged;

                RepositoryWrapper.LangTextReviewRepo.Create(langtextReview);    //Move to ReviewRepo wating for review.
            }

            if (!await RepositoryWrapper.LangTextReviewRepo.SaveAsync())
            {
                throw new Exception("加入审核表失败");
            }

            return Ok();

        }

        [Authorize(Roles = "editor")]
        [HttpPut("zh/list")]
        public async Task<ActionResult> UpdateLangtextZhListAsync(List<LangTextForUpdateZhDto> langTextForUpdateZhList)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            Guid userIdinGuid = new Guid(userId);

            //var langtext = await RepositoryWrapper.LangTextRepo.GetByIdAsync(langTextForUpdateZh.Id);

            List<LangText> langTexts = new List<LangText>();
            List<LangTextReview> langTextInReviews = new List<LangTextReview>(); 

            foreach(var lang in langTextForUpdateZhList)
            {
                var langtext = await RepositoryWrapper.LangTextRepo.GetByIdAsync(lang.Id);
                var langtextInReview = await RepositoryWrapper.LangTextReviewRepo.GetByIdAsync(lang.Id);

                if (lang != null)
                {
                    langTexts.Add(langtext);
                }

                if (langtextInReview != null)   //检查列表内文本是否存在于审核表内
                {
                    if (userIdinGuid == langtextInReview.UserId)    //如果用户ID与文本用户ID相同
                    {
                        Mapper.Map(lang, langtextInReview, typeof(LangTextForUpdateZhDto), typeof(LangTextReview));
                        RepositoryWrapper.LangTextReviewRepo.Update(langtextInReview);  //Update ReviewRepo wating for review.                                                           
                    }
                    else  
                    {
                        langTextInReviews.Add(langtextInReview);    //如果ID不相同，加入待返回列表
                    }

                }
                else //如果不在审核表内
                {
                    //创建待审核文本
                    Mapper.Map(lang, langtext, typeof(LangTextForUpdateZhDto), typeof(LangText));

                    var langtextReview = Mapper.Map<LangTextReview>(langtext);
                    langtextReview.ReasonFor = ReviewReason.ZhChanged;

                    RepositoryWrapper.LangTextReviewRepo.Create(langtextReview);    //Move to ReviewRepo wating for review.
                }
            }

            if (!await RepositoryWrapper.LangTextReviewRepo.SaveAsync())
            {
                throw new Exception("加入审核表失败");
            }

            if (langTextInReviews.Count >= 1)
            {
                return Ok(langTextInReviews);
            }

            return Ok();

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("en/{langtextID}")]
        public async Task<ActionResult> UpdateLangtextEnAsync(Guid langtextID, LangTextForUpdateEnDto langTextForUpdateEn)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var langtext = await RepositoryWrapper.LangTextRepo.GetByIdAsync(langtextID);

            if (langtext == null)
            {
                return NotFound();
            }
            Mapper.Map(langTextForUpdateEn, langtext, typeof(LangTextForUpdateEnDto), typeof(LangText));

            var langtextReview = Mapper.Map<LangTextReview>(langtext);
            langtextReview.ReasonFor = ReviewReason.EnChanged;
            langtextReview.UserId = new Guid(userId);
            langtextReview.EnLastModifyTimestamp = DateTime.UtcNow;

            //Move to ReviewRepo wating for review.
            RepositoryWrapper.LangTextReviewRepo.Create(langtextReview);

            if (!await RepositoryWrapper.LangTextRepo.SaveAsync())
            {
                throw new Exception("加入审核表失败，langtextID: " + langtextReview.Id.ToString());
            }

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("en")]
        public async Task<ActionResult> UpdateLangtextEnListAsync(List<LangTextForUpdateEnDto> langTextForUpdateEns)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            List<LangTextReview> langTexts = new List<LangTextReview>();

            foreach (var enChanged in langTextForUpdateEns)
            {
                var langtext = await RepositoryWrapper.LangTextRepo.GetByIdAsync(enChanged.Id);

                if (langtext != null)
                {
                    langtext.UserId = new Guid(userId);
                    langtext.TextEn = enChanged.TextEn;
                    langtext.EnLastModifyTimestamp = DateTime.UtcNow;
                    langtext.UpdateStats = enChanged.UpdateStats;

                    var langtextReview = Mapper.Map<LangTextReview>(langtext);
                    langtextReview.ReasonFor = ReviewReason.EnChanged;

                    langTexts.Add(langtextReview);
                }
            }
            //Move to ReviewRepo wating for review.
            RepositoryWrapper.LangTextReviewRepo.CreateList(langTexts);

            if (!await RepositoryWrapper.LangTextRepo.SaveAsync())
            {
                throw new Exception("加入审核表失败");
            }

            return Ok();
        }


    }
}
