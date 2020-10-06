using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorLib.Models.Client.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;

namespace ESO_LangEditorLib.Services.Client
{
    public class LangTextRepository
    {
        public IEnumerable<LangTextDto> GetLangTexts(string keyWord, SearchTextType searchType)
        {
            using (var db = new LangDbContext())
            {
                var data = searchType switch
                {
                    SearchTextType.UniqueID => db.LangData.Where(d => d.TextId == keyWord).ToList(),
                    SearchTextType.TextEnglish => db.LangData.Where(d => EF.Functions.Like(d.TextEn, keyWord)).ToList(),
                    SearchTextType.TextChineseS => db.LangData.Where(d => EF.Functions.Like(d.TextZh, keyWord)).ToList(),
                    SearchTextType.UpdateStatus => db.LangData.Where(d => EF.Functions.Like(d.UpdateStats, keyWord)).ToList(),
                    //SearchTextType.ReviewStatus => db.LangData.Where(d => d. == ToInt32(keyWord)).ToList(),
                    SearchTextType.TranslateStatus => db.LangData.Where(d => d.IsTranslated == ToInt32(keyWord)).ToList(),
                    SearchTextType.Guid => throw new NotImplementedException(),
                    SearchTextType.Type => throw new NotImplementedException(),
                    SearchTextType.ByUser => throw new NotImplementedException(),
                    _ => db.LangData.Where(d => EF.Functions.Like(d.TextEn, keyWord)).ToList(),
                };
                return data;
            }
        }

        public IEnumerable<LangTextDto> GeAlltLangTexts()
        {
            using (var db = new LangDbContext())
            {
                return db.LangData.ToList();
                
                //return data;
            }
        }

    }
}
