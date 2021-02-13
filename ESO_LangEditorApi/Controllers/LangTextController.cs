using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore.DataRepositories;
using ESO_LangEditor.EFCore.RepositoryWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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


        public LangTextController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
        }
        
        public async Task<ActionResult<IEnumerable<LangText>>> GetLangTextAllAsync()
        {
            var langtextList = await RepositoryWrapper.LangTextRepo.GetAllAsync();

            return langtextList.ToList();
        }

        [Authorize(Roles = "editor")]
        //[AllowAnonymous]
        [HttpGet("{langtextGuid}")]
        public async Task<ActionResult<IEnumerable<LangTextDto>>> GetLangTextByGuidAsync(Guid langtextGuid)
        {
            var langtext = await RepositoryWrapper.LangTextRepo.GetByIdAsync(langtextGuid);
            var langtextDto = Mapper.Map<List<LangTextDto>>(langtext);


            if (langtext == null)
            {
                return NotFound();
            }
            else
            {
                return langtextDto;
            }

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
            langtext.ReviewTimestamp = DateTime.Now;

            RepositoryWrapper.LangTextReviewRepo.Create(langtext);

            if (!await RepositoryWrapper.LangTextReviewRepo.SaveAsync())
            {
                throw new Exception("创建资源langtext失败");
            }

            //var langtextDto = Mapper.Map<LangTextDto>(langtext);

            return Ok(); //CreatedAtRoute(nameof(GetLangTextByGuidAsync), new { langtextID = langtextDto.Id }, langtextDto);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{langtextID}/user/{userId}")]
        public async Task<ActionResult> DeleteLangTextAsync(Guid langtextID, Guid userId)
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

            //var langtextArchive = Mapper.Map<LangTextArchive>(langtext);
            //langtextArchive.ArchiveReasonFor = ReviewReason.Deleted;

            //Mapper.Map(langTextForArchive, langtextArchive, typeof(LangTextForArchiveDto), typeof(LangTextArchive));

            //RepositoryWrapper.LangTextArchiveRepo.Create(langtextArchive);

            //RepositoryWrapper.LangTextRepo.Delete(langtext);

            return NotFound();

        }

        //[Authorize(Roles = "Editor")]
        [HttpPut("{langtextID}/user/{userId}")]
        public async Task<ActionResult> UpdateLangtextZHAsync(Guid langtextID, Guid userId, LangTextForUpdateZhDto langTextForUpdateZh)
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
            langtextReview.ReasonFor = ReviewReason.ZhChanged;
            //langtext.ReviewerId = userId;
            //langtext.ReviewTimestamp = DateTime.Now;

            Mapper.Map(langTextForUpdateZh, langtextReview, typeof(LangTextForUpdateZhDto), typeof(LangTextReview));

            //Move to ReviewRepo wating for review.
            RepositoryWrapper.LangTextReviewRepo.Create(langtextReview);

            if (!await RepositoryWrapper.LangTextReviewRepo.SaveAsync())
            {
                throw new Exception("加入审核表失败，langtextID: " + langtextReview.Id.ToString());
            }

            return NoContent();

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{langtextID}/en/user/{userId}")]
        public async Task<ActionResult> UpdateLangtextEnAsync(Guid langtextID,Guid userId, LangTextForUpdateEnDto langTextForUpdateEn)
        {
            var langtext = await RepositoryWrapper.LangTextRepo.GetByIdAsync(langtextID);

            if (langtext == null)
            {
                return NotFound();
            }

            var langtextReview = Mapper.Map<LangTextReview>(langtext);
            langtextReview.ReasonFor = ReviewReason.EnChanged;
            //langtext.ReviewerId = userId;
            //langtext.ReviewTimestamp = DateTime.Now;

            //Mapper.Map(langTextForUpdateEn, langtext, typeof(LangTextForUpdateEnDto), typeof(LangText));
            Mapper.Map(langTextForUpdateEn, langtextReview, typeof(LangTextForUpdateEnDto), typeof(LangTextReview));

            //RepositoryWrapper.LangTextRepo.Update(langtext);

            //Move to ReviewRepo wating for review.
            RepositoryWrapper.LangTextReviewRepo.Create(langtextReview);

            if (!await RepositoryWrapper.LangTextRepo.SaveAsync())
            {
                throw new Exception("加入审核表失败，langtextID: " + langtextReview.Id.ToString());
            }

            return NoContent();
        }


    }
}
