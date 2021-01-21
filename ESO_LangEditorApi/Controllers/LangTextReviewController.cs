using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore.RepositoryWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESO_LangEditor.API.Controllers
{
    [Route("api/langtext/review")]
    [ApiController]
    public class LangTextReviewController : ControllerBase
    {
        public IRepositoryWrapper RepositoryWrapper { get; }
        public IMapper Mapper { get; }


        public LangTextReviewController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            RepositoryWrapper = repositoryWrapper;
            Mapper = mapper;
        }

        //[Authorize(Roles = "Reivewer")]
        public async Task<ActionResult<IEnumerable<LangTextReview>>> GetLangTextAllAsync()
        {
            var langtextList = await RepositoryWrapper.LangTextReviewRepo.GetAllAsync();

            return langtextList.ToList();
        }

        //[Authorize(Roles = "Reivewer")]
        //[AllowAnonymous]
        [HttpGet("{langtextGuid}")]
        public async Task<ActionResult<LangTextReview>> GetLangTextByGuidAsync(Guid langtextGuid)
        {
            var langtext = await RepositoryWrapper.LangTextReviewRepo.GetByIdAsync(langtextGuid);

            if (langtext == null)
            {
                return NotFound();
            }
            else
            {
                return langtext;
            }

        }


        [HttpGet("{langtextGuid}/user/{userId}")]
        public async Task<ActionResult> ReviewLangTextAsync(Guid langtextGuid, Guid userId)
        {
            var langtextReview = await RepositoryWrapper.LangTextReviewRepo.GetByIdAsync(langtextGuid);

            if (langtextReview == null)
            {
                return NotFound();
            }

            langtextReview.ReviewerId = userId;      //Set reviewer ID.
            langtextReview.ReviewTimestamp = DateTime.Now;  //Set review timestamp.

            var langtext = Mapper.Map<LangTextReview, LangText>(langtextReview);


            switch(langtextReview.ReasonFor)
            {
                case ReviewReason.NewAdded:
                    RepositoryWrapper.LangTextRepo.Create(langtext);
                    break;
                case ReviewReason.EnChanged:
                    //Update main table langtext.
                    RepositoryWrapper.LangTextRepo.Update(langtext);
                    break;
                case ReviewReason.ZhChanged:
                    //Update main table langtext.
                    RepositoryWrapper.LangTextRepo.Update(langtext);
                    break;
                case ReviewReason.Deleted:
                    RepositoryWrapper.LangTextRepo.Delete(langtext);
                    break;
                case 0:
                    throw new Exception("闹呢？除了增改删还有啥？langtextID: " + langtextGuid.ToString()
                        + "用户ID：" + userId.ToString());
            }


            //Update main table langtext.
            RepositoryWrapper.LangTextRepo.Update(langtext);    

            if (!await RepositoryWrapper.LangTextRepo.SaveAsync())
            {
                throw new Exception("审核资源 LangtextID: "+ langtextGuid.ToString() + " 失败。" +
                    "操作人：" + userId.ToString() + "。");
            }
            else
            {
                await MoveToArchiveAsync(langtextReview);
                await LangtextReviseAsync(langtextGuid, langtextReview.ReasonFor);

                //Delete langtext after review.
                var langtextReviewDel = await RepositoryWrapper.LangTextReviewRepo.GetByIdAsync(langtextGuid);
                RepositoryWrapper.LangTextReviewRepo.Delete(langtextReviewDel);

                await RepositoryWrapper.LangTextArchiveRepo.SaveAsync();

                return NoContent();
            }

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

        private async Task MoveToArchiveAsync(LangTextReview langTextReview)
        {

            //var langtext = Mapper.Map<LangTextReview, LangText>(langTextReview);

            var langtextArchive = Mapper.Map<LangTextReview, LangTextArchive>(langTextReview);
            langtextArchive.ArchiveTimestamp = DateTime.Now;
            if (langtextArchive.ArchiveReasonFor != ReviewReason.Deleted)
                langtextArchive.Id = new Guid();

            //Move review langtext to archive.
            RepositoryWrapper.LangTextArchiveRepo.Create(langtextArchive);

            if (!await RepositoryWrapper.LangTextArchiveRepo.SaveAsync())
            {
                throw new Exception("归档资源LangtextID: " + langTextReview.Id.ToString() + " 失败。" +
                    "操作人：" + langTextReview.UserId.ToString() + "。");
            }
        }

        private async Task LangtextReviseAsync(Guid langtextId, ReviewReason reason)
        {
            var LangtextRevNumberCurrent = await RepositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(1);
            int newRevNumber = LangtextRevNumberCurrent.LangTextRev + 1;

            LangTextRevisedDto langTextRevisedDto = new LangTextRevisedDto
            {
                LangtextID = langtextId,
                LangTextRevNumber = newRevNumber,
                ReasonFor = reason
            };

            var langtextRevised = Mapper.Map<LangTextRevised>(langTextRevisedDto);

            RepositoryWrapper.LangTextRevisedRepo.Create(langtextRevised);  //Insert to Revised table.

            if (!await RepositoryWrapper.LangTextRevisedRepo.SaveAsync())
            {
                throw new Exception("步进资源 LangtextID: " + langtextId.ToString() + " 失败。");
            }

            //LangTextRevNumberDto langTextRevNumberDto = new LangTextRevNumberDto
            //{
            //    LangTextRev = newRevNumber,
            //};

            //var langtextRevNumber = Mapper.Map<LangTextRevNumber>(langTextRevNumberDto);

            LangtextRevNumberCurrent.LangTextRev = newRevNumber;   //New modify RevNumber.

            RepositoryWrapper.LangTextRevNumberRepo.Update(LangtextRevNumberCurrent);   //Update RevNumber.

            if (!await RepositoryWrapper.LangTextRevNumberRepo.SaveAsync())
            {
                throw new Exception("步进号 " + newRevNumber.ToString() + " 更新失败。");
            }

        }


    }
}
