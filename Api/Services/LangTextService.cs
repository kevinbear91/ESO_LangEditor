using AutoMapper;
using Core.Entities;
using Core.Models;
using EFCore.RepositoryWrapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class LangTextService : ILangTextService
    {
        private IRepositoryWrapper _repositoryWrapper;
        private ILogger<LangTextService> _logger;
        private IMapper _mapper;
        private string _loggerMessage;

        public LangTextService(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILogger<LangTextService> logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task ArchiveListAsync(Guid userId, List<LangTextArchive> langTextArchive)
        {
            //var userId = _userManager.GetUserId(HttpContext.User);

            _repositoryWrapper.LangTextArchiveRepo.CreateList(langTextArchive);

            if (!await _repositoryWrapper.LangTextArchiveRepo.SaveAsync())
            {
                _loggerMessage = "将审核后的文本加入存档库失败， 文本条目总计： " + langTextArchive.Count
                                + "， 操作人：" + userId;
                _logger.LogError(_loggerMessage);
                throw new Exception("归档资源 失败。" +
                    "操作人：" + userId.ToString() + "。");
            }
        }

        public Task<LangTextReview> LangtextReviewProcess(LangTextReview langTextReviewEntity)
        {
            throw new NotImplementedException();
        }

        public async Task LangtextReviseListAsync(LangTextRevNumber RevNumber, List<LangTextRevisedDto> langTextRevisedDtos)
        {
            var langtextRevised = _mapper.Map<List<LangTextRevised>>(langTextRevisedDtos);
            _repositoryWrapper.LangTextRevisedRepo.CreateList(langtextRevised);  //Insert to Revised table.

            if (!await _repositoryWrapper.LangTextRevisedRepo.SaveAsync())
            {
                _loggerMessage = "更新步进资源失败，资源的步进号为：" + RevNumber.ToString();
                _logger.LogError(_loggerMessage);
                throw new Exception("步进资源失败。");
            }

            _repositoryWrapper.LangTextRevNumberRepo.Update(RevNumber);   //Update RevNumber.

            if (!await _repositoryWrapper.LangTextRevNumberRepo.SaveAsync())
            {
                _loggerMessage = "步进号 " + RevNumber.Rev.ToString() + " 更新失败。";
                _logger.LogError(_loggerMessage);
                throw new Exception("步进号 " + RevNumber.Rev.ToString() + " 更新失败。");
            }
        }
    }
}
