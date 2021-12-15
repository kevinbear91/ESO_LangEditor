using Core.Entities;
using Core.EnumTypes;
using Core.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestParameters.Extensions
{
    public static class LangTextFilterExtensions
    {
        public static IQueryable<LangText> FilterLangTexts(this IQueryable<LangText> langTexts, LangTextParameters langTextParameters)
        {
            if (langTextParameters.GameApiVersion != 0)
            {
                langTexts.Where(lang => lang.GameApiVersion == langTextParameters.GameApiVersion);
            }

            if (langTextParameters.IdType != 0)
            {
                langTexts.Where(lang => lang.IdType == langTextParameters.IdType);
            }

            if (langTextParameters.Revised != 0)
            {
                langTexts.Where(lang => lang.Revised == langTextParameters.Revised);
            }

            if (langTextParameters.UserId != Guid.Empty)
            {
                langTexts.Where(lang => lang.UserId == langTextParameters.UserId);
            }

            if (langTextParameters.UniqueId != "")
            {
                langTexts.Where(lang => lang.TextId == langTextParameters.UniqueId);
            }

            return langTexts;

        }

        public static IQueryable<LangText> SearchLangTextsEn(this IQueryable<LangText> langTexts, LangTextParameters langTextParameters)
        {

            if (langTextParameters.SearchPostion == SearchPostion.Full)
            {
                return langTextParameters.CaseSensitive
                    ? langTexts.Where(lang => lang.TextEn.Contains(langTextParameters.SearchTerm))
                    : langTexts.Where(lang => lang.TextEn.ToLower().Contains(langTextParameters.SearchTerm.ToLower()));

            }

            if (langTextParameters.SearchPostion == SearchPostion.OnlyOnFront)
            {
                return langTextParameters.CaseSensitive
                    ? langTexts.Where(lang => lang.TextEn.StartsWith(langTextParameters.SearchTerm))
                    : langTexts.Where(lang => lang.TextEn.ToLower().StartsWith(langTextParameters.SearchTerm.ToLower()));
            }

            if (langTextParameters.SearchPostion == SearchPostion.OnlyOnEnd)
            {
                return langTextParameters.CaseSensitive
                    ? langTexts.Where(lang => lang.TextEn.EndsWith(langTextParameters.SearchTerm))
                    : langTexts.Where(lang => lang.TextEn.ToLower().EndsWith(langTextParameters.SearchTerm.ToLower()));
            }

            return langTexts;

        }

        public static IQueryable<LangText> SearchLangTextsZh(this IQueryable<LangText> langTexts, LangTextParameters langTextParameters)
        {
            if (langTextParameters.SearchPostion == SearchPostion.Full)
            {
                return langTexts.Where(lang => lang.TextZh.Contains(langTextParameters.SearchTerm));
            }

            if (langTextParameters.SearchPostion == SearchPostion.OnlyOnFront)
            {
                return langTexts.Where(lang => lang.TextZh.StartsWith(langTextParameters.SearchTerm));
            }

            if (langTextParameters.SearchPostion == SearchPostion.OnlyOnEnd)
            {
                return langTexts.Where(lang => lang.TextZh.EndsWith(langTextParameters.SearchTerm));
            }

            return langTexts;

        }

    }
}
