using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore.RepositoryWrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;

namespace ESO_LangEditorGUI.Services
{
    public class LangTextRepoClientService
    {
        private IRepositoryWrapperClient _repositoryWrapper; 
        private IMapper _mapper;


        public LangTextRepoClientService(/*IRepositoryWrapperClient repositoryWrapper, IMapper mapper*/)
        {
            _repositoryWrapper = App.RepoClient; // repositoryWrapper;
            _mapper = App.Mapper; // mapper;
        }

        public async Task<IEnumerable<LangTextDto>> GetLangTextByGuidAsync(Guid langtextGuid)
        {
            var langtext = await _repositoryWrapper.LangTextRepo.GetByIdAsync(langtextGuid);
            var langtextDto = _mapper.Map<List<LangTextDto>>(langtext);



            return langtextDto;
            //if (langtext == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    return langtextDto;
            //}

        }

        public async Task<List<LangTextDto>> GetLangTextByConditionAsync(string keyWord, SearchTextType searchType, SearchPostion searchPostion)
        {
            //var langtext = await _repositoryWrapper.LangTextRepo.GetByIdAsync(langtextGuid);
            //var langtextDto = _mapper.Map<List<LangTextDto>>(langtext);

            string searchPosAndWord = GetKeywordWithPostion(searchPostion, keyWord);

            //return langtextDto;


            var langtext = searchType switch
            {
                SearchTextType.UniqueID => await _repositoryWrapper.LangTextRepo.GetByConditionAsync(d => d.TextId == keyWord),
                SearchTextType.TextEnglish => await _repositoryWrapper.LangTextRepo.GetByConditionAsync(d => EF.Functions.Like(d.TextEn, searchPosAndWord)),
                SearchTextType.TextChineseS => await _repositoryWrapper.LangTextRepo.GetByConditionAsync(d => EF.Functions.Like(d.TextZh, searchPosAndWord)),
                SearchTextType.UpdateStatus => await _repositoryWrapper.LangTextRepo.GetByConditionAsync(d => EF.Functions.Like(d.UpdateStats, searchPosAndWord)),
                SearchTextType.TranslateStatus => await _repositoryWrapper.LangTextRepo.GetByConditionAsync(d => d.IsTranslated == ToInt32(keyWord)),
                SearchTextType.Guid => await _repositoryWrapper.LangTextRepo.GetByConditionAsync(d => d.Id == new Guid(keyWord)),
                SearchTextType.Type => await _repositoryWrapper.LangTextRepo.GetByConditionAsync(d => d.IdType == ToInt32(keyWord)),
                //SearchTextType.ByUser => throw new NotImplementedException(),
                _ => await _repositoryWrapper.LangTextRepo.GetByConditionAsync(d => EF.Functions.Like(d.TextEn, searchPosAndWord)),
            };

            var langtextDto = _mapper.Map<List<LangTextDto>>(langtext);

            return langtextDto;

        }

        public async Task<bool> UpdateLangtextZh(Guid langtextId, LangTextForUpdateZhDto langtextDto)
        {

            var langtext = await _repositoryWrapper.LangTextRepo.GetByIdAsync(langtextId);

            //var langtextUpdate = _mapper.Map<LangText>(langtextDto);

            _mapper.Map(langtextDto, langtext, typeof(LangTextForUpdateZhDto), typeof(LangText));

            _repositoryWrapper.LangTextRepo.Update(langtext);

            return await _repositoryWrapper.LangTextRepo.SaveAsync();
            //Db.Update(langList);
            //return await Db.SaveChangesAsync();
        }


        private static string GetKeywordWithPostion(SearchPostion searchPostion, string keyWord)
        {
            string searchPosAndWord = searchPostion switch
            {
                SearchPostion.Full => "%" + keyWord + "%",     //任意位置
                SearchPostion.OnlyOnFront => keyWord + "%",           //仅在开头
                SearchPostion.OnlyOnEnd => "%" + keyWord,           //仅在末尾
                _ => "%" + keyWord + "%",     //默认 - 任意位置
            };

            return searchPosAndWord;
        }


    }
}
