using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore.RepositoryWrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ESO_LangEditor.API.Controllers
{
    [Route("api/langtext/review")]
    [ApiController]
    public class LangTextReviewController : ControllerBase
    {
        private IRepositoryWrapper _repositoryWrapper;
        private IMapper _mapper;
        private UserManager<User> _userManager;

        public LangTextReviewController(IRepositoryWrapper repositoryWrapper, IMapper mapper, UserManager<User> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _userManager = userManager;
        }

        //[Authorize(Roles = "Reivewer")]
        public async Task<ActionResult<IEnumerable<LangTextReview>>> GetLangTextAllAsync()
        {
            var langtextList = await _repositoryWrapper.LangTextReviewRepo.GetAllAsync();

            return langtextList.ToList();
        }

        //[Authorize(Roles = "Reivewer")]
        //[AllowAnonymous]
        [HttpGet("{langtextGuid}")]
        public async Task<ActionResult<LangTextReview>> GetLangTextByGuidAsync(Guid langtextGuid)
        {
            var langtext = await _repositoryWrapper.LangTextReviewRepo.GetByIdAsync(langtextGuid);

            if (langtext == null)
            {
                return NotFound();
            }
            else
            {
                return langtext;
            }

        }

        [HttpGet("user/{userGuid}")]
        public async Task<ActionResult<IEnumerable<LangTextForReviewDto>>> GetLangTextByUserGuidAsync(Guid userGuid)
        {
            var langtext = await _repositoryWrapper.LangTextReviewRepo.GetByConditionAsync(lang => lang.UserId == userGuid);
            //var langlist = langtext.ToList();

            if (langtext == null)
            {
                return NotFound();
            }
            else
            {
                var LangTextReviewDtolist = _mapper.Map<List<LangTextForReviewDto>>(langtext);
                return LangTextReviewDtolist;
            }

        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<Guid>>> GetReviewUserListAsync()
        {
            var langtext = await _repositoryWrapper.LangTextReviewRepo.SelectByConditionWithDistinctAsync(u => u.UserId);

            if (langtext == null)
            {
                return NotFound();
            }
            else
            {
                return langtext.ToList();
            }

        }


        [HttpGet("{langtextGuid}")]
        public async Task<ActionResult> ReviewLangTextAsync(Guid langtextGuid)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var langtextReview = await _repositoryWrapper.LangTextReviewRepo.GetByIdAsync(langtextGuid);

            if (langtextReview == null)
            {
                return NotFound();
            }

            langtextReview.ReviewerId = new Guid(userId);      //Set reviewer ID.
            langtextReview.ReviewTimestamp = DateTime.UtcNow;  //Set review timestamp.

            var langtext = _mapper.Map<LangTextReview, LangText>(langtextReview);


            switch(langtextReview.ReasonFor)
            {
                case ReviewReason.NewAdded:
                    _repositoryWrapper.LangTextRepo.Create(langtext);
                    break;
                case ReviewReason.EnChanged:
                    //Update main table langtext.
                    _repositoryWrapper.LangTextRepo.Update(langtext);
                    break;
                case ReviewReason.ZhChanged:
                    //Update main table langtext.
                    _repositoryWrapper.LangTextRepo.Update(langtext);
                    break;
                case ReviewReason.Deleted:
                    _repositoryWrapper.LangTextRepo.Delete(langtext);
                    break;
                case 0:
                    throw new Exception("闹呢？除了增改删还有啥？langtextID: " + langtextGuid.ToString()
                        + "用户ID：" + userId.ToString());
            }


            //Update main table langtext.
            _repositoryWrapper.LangTextRepo.Update(langtext);    

            if (!await _repositoryWrapper.LangTextRepo.SaveAsync())
            {
                throw new Exception("审核资源 LangtextID: "+ langtextGuid.ToString() + " 失败。" +
                    "操作人：" + userId.ToString() + "。");
            }
            else
            {
                await MoveToArchiveAsync(langtextReview);
                await LangtextReviseAsync(langtextGuid, langtextReview.ReasonFor);

                //Delete langtext after review.
                var langtextReviewDel = await _repositoryWrapper.LangTextReviewRepo.GetByIdAsync(langtextGuid);
                _repositoryWrapper.LangTextReviewRepo.Delete(langtextReviewDel);

                await _repositoryWrapper.LangTextArchiveRepo.SaveAsync();

                return NoContent();
            }

        }

        [HttpPut()]
        public async Task<ActionResult> ReviewLangTextAsync(List<Guid> langtextIdList)
        {
            var userId = _userManager.GetUserId(HttpContext.User);  //获取操作者ID
            List<LangTextArchive> langTextArchives = new List<LangTextArchive>();   //建立待归档列表
            List<LangTextRevisedDto> langTextRevisedDtos = new List<LangTextRevisedDto>();  //建立步进状态DTO列表
            var LangtextRevNumberCurrent = await _repositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(1); //查询获取新的步进号
            LangtextRevNumberCurrent.LangTextRev++;

            foreach (var id in langtextIdList)
            {
                var langtextReview = await _repositoryWrapper.LangTextReviewRepo.GetByIdAsync(id);   //查询待审核文本Dto
                var langtextReviewDel = langtextReview; //设置当前审核文本待删除

                if (langtextReview != null)
                {
                    langtextReview.ReviewerId = new Guid(userId);       //Set reviewer ID.
                    langtextReview.ReviewTimestamp = DateTime.UtcNow;      //Set review timestamp.

                    var langtext = _mapper.Map<LangTextReview, LangText>(langtextReview);
                    var langtextArchive = _mapper.Map<LangTextReview, LangTextArchive>(langtextReview);

                    //检查审核原因
                    switch (langtextReview.ReasonFor)
                    {
                        case ReviewReason.NewAdded:     //如果为新增
                            _repositoryWrapper.LangTextRepo.Create(langtext);    //主库创建文本
                            langTextArchives.Add(MakeReivewToLangtextArchive(langtextArchive)); //将当前审核文本设置为归档
                            langTextRevisedDtos.Add(MakeLangtextRevisedDto(LangtextRevNumberCurrent, langtextReview));  //步进库增加步进号与langtext ID
                            break;
                        case ReviewReason.EnChanged:    //如果为英文修改
                            _repositoryWrapper.LangTextRepo.Update(langtext);    //主库更新文本
                            langTextArchives.Add(MakeReivewToLangtextArchive(langtextArchive)); //将当前审核文本设置为归档
                            langTextRevisedDtos.Add(MakeLangtextRevisedDto(LangtextRevNumberCurrent, langtextReview));  //步进库增加步进号与langtext ID
                            break;
                        case ReviewReason.ZhChanged:    //如果为中文修改
                            _repositoryWrapper.LangTextRepo.Update(langtext);    //主库更新文本
                            langTextArchives.Add(MakeReivewToLangtextArchive(langtextArchive)); //将当前审核文本设置为归档
                            langTextRevisedDtos.Add(MakeLangtextRevisedDto(LangtextRevNumberCurrent, langtextReview)); 
                            break;
                        case ReviewReason.Deleted:  //如果为移除项
                            _repositoryWrapper.LangTextRepo.Delete(langtext);    //主库删除文本
                            langTextArchives.Add(MakeReivewToLangtextArchive(langtextArchive)); //将当前审核文本设置为归档
                            langTextRevisedDtos.Add(MakeLangtextRevisedDto(LangtextRevNumberCurrent, langtextReview));
                            break;
                        case 0: //如果枚举出错
                            throw new Exception("闹呢？除了增改删还有啥？langtextID: "
                                + langtextReview.Id.ToString()
                                + "用户ID：" + userId.ToString());
                    }
                    
                    //删除当前待审核文本
                    _repositoryWrapper.LangTextReviewRepo.Delete(langtextReviewDel);
                }

            }

            if (!await _repositoryWrapper.LangTextRepo.SaveAsync())
            {
                throw new Exception("审核资源失败。" + "操作人：" + userId.ToString() + "。");
            }
            else
            {
                await LangtextReviseAsync(LangtextRevNumberCurrent, langTextRevisedDtos);
                await ListMoveToArchiveAsync(langTextArchives);

                //await RepositoryWrapper.LangTextReviewRepo.SaveAsync();
                //if (!await RepositoryWrapper.LangTextReviewRepo.SaveAsync())
                //{
                //    throw new Exception("删除审核资源 失败。操作人：" + userId.ToString() + "。");
                //}

                return Ok();
            }


            static LangTextArchive MakeReivewToLangtextArchive(LangTextArchive langArchive)
            {
                if (langArchive.ReasonFor != ReviewReason.Deleted)
                    langArchive.Id = new Guid();

                langArchive.ArchiveTimestamp = DateTime.UtcNow;
                
                return langArchive;
            }

            static LangTextRevisedDto MakeLangtextRevisedDto(LangTextRevNumber RevNumber, LangTextReview langReview)
            {
                LangTextRevisedDto langTextRevisedDto = new LangTextRevisedDto
                {
                    LangtextID = langReview.Id,
                    LangTextRevNumber = RevNumber.LangTextRev,
                    ReasonFor = langReview.ReasonFor
                };

                return langTextRevisedDto;
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

            var langtextArchive = _mapper.Map<LangTextReview, LangTextArchive>(langTextReview);
            langtextArchive.ArchiveTimestamp = DateTime.UtcNow;
            if (langtextArchive.ReasonFor != ReviewReason.Deleted)
                langtextArchive.Id = new Guid();

            //Move review langtext to archive.
            _repositoryWrapper.LangTextArchiveRepo.Create(langtextArchive);

            if (!await _repositoryWrapper.LangTextArchiveRepo.SaveAsync())
            {
                throw new Exception("归档资源LangtextID: " + langTextReview.Id.ToString() + " 失败。" +
                    "操作人：" + langTextReview.UserId.ToString() + "。");
            }
        }

        private async Task ListMoveToArchiveAsync(List<LangTextArchive> langTextArchive)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            _repositoryWrapper.LangTextArchiveRepo.CreateList(langTextArchive);

            if (!await _repositoryWrapper.LangTextArchiveRepo.SaveAsync())
            {
                throw new Exception("归档资源 失败。" +
                    "操作人：" + userId.ToString() + "。");
            }
        }

        private async Task LangtextReviseAsync(Guid langtextId, ReviewReason reason)
        {
            var LangtextRevNumberCurrent = await _repositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(1);
            int newRevNumber = LangtextRevNumberCurrent.LangTextRev + 1;

            LangTextRevisedDto langTextRevisedDto = new LangTextRevisedDto
            {
                LangtextID = langtextId,
                LangTextRevNumber = newRevNumber,
                ReasonFor = reason
            };

            var langtextRevised = _mapper.Map<LangTextRevised>(langTextRevisedDto);

            _repositoryWrapper.LangTextRevisedRepo.Create(langtextRevised);  //Insert to Revised table.

            if (!await _repositoryWrapper.LangTextRevisedRepo.SaveAsync())
            {
                throw new Exception("步进资源 LangtextID: " + langtextId.ToString() + " 失败。");
            }

            //LangTextRevNumberDto langTextRevNumberDto = new LangTextRevNumberDto
            //{
            //    LangTextRev = newRevNumber,
            //};

            //var langtextRevNumber = Mapper.Map<LangTextRevNumber>(langTextRevNumberDto);

            LangtextRevNumberCurrent.LangTextRev = newRevNumber;   //New modify RevNumber.

            _repositoryWrapper.LangTextRevNumberRepo.Update(LangtextRevNumberCurrent);   //Update RevNumber.

            if (!await _repositoryWrapper.LangTextRevNumberRepo.SaveAsync())
            {
                throw new Exception("步进号 " + newRevNumber.ToString() + " 更新失败。");
            }

        }

        //private async Task<LangTextRevNumber> GetNewLangtextRevNumber()
        //{
        //    //var LangtextRevNumberCurrent = await RepositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(1);
        //    //int newRevNumber = LangtextRevNumberCurrent.LangTextRev + 1;

        //    return await RepositoryWrapper.LangTextRevNumberRepo.GetByIdAsync(1);
        //}

        private async Task LangtextReviseAsync(LangTextRevNumber RevNumber, List<LangTextRevisedDto> langTextRevisedDtos)
        {
            var langtextRevised = _mapper.Map<List<LangTextRevised>>(langTextRevisedDtos);
            _repositoryWrapper.LangTextRevisedRepo.CreateList(langtextRevised);  //Insert to Revised table.

            if (!await _repositoryWrapper.LangTextRevisedRepo.SaveAsync())
            {
                throw new Exception("步进资源失败。");
            }

            //LangTextRevNumber LangtextRevNumberCurrent = new LangTextRevNumber
            //{
            //    Id = 1,
            //    LangTextRev = newRevNumber,
            //};

            //var langtextRevNumberCurrent = Mapper.Map<LangTextRevNumber>(LangtextRevNumberCurrentDto);
            _repositoryWrapper.LangTextRevNumberRepo.Update(RevNumber);   //Update RevNumber.

            if (!await _repositoryWrapper.LangTextRevNumberRepo.SaveAsync())
            {
                throw new Exception("步进号 " + RevNumber.LangTextRev.ToString() + " 更新失败。");
            }

        }


    }
}
