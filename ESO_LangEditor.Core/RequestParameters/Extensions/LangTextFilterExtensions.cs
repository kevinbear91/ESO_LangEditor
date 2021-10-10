using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.Core.RequestParameters.Extensions
{
    public static class LangTextFilterExtensions
    {
        public static IQueryable<LangText> FilterLangTexts(this IQueryable<LangText> langTexts, LangTextParameters langTextParameters)
        {
            if (!string.IsNullOrEmpty(langTextParameters.GameVersionInfo))
            {
                langTexts.Where(lang => lang.UpdateStats == langTextParameters.GameVersionInfo);
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

            //if (langTextParameters. != Guid.Empty)
            //{
            //    langTexts.Where(lang => lang.UserId == langTextParameters.UserId);
            //}

            return langTexts;

        }

        public static IQueryable<LangText> SearchLangTextsEn(this IQueryable<LangText> langTexts, LangTextParameters langTextParameters, string searchTerm)
        {
            //if (string.IsNullOrEmpty(searchTerm))
            //{
            //    return langTexts;
            //}

            if (langTextParameters.SearchPostion == SearchPostion.Full)
            {
                return langTexts.Where(lang => lang.TextEn.Contains(searchTerm));
            }

            if (langTextParameters.SearchPostion == SearchPostion.OnlyOnFront)
            {
                return langTexts.Where(lang => lang.TextEn.StartsWith(searchTerm));
            }

            if (langTextParameters.SearchPostion == SearchPostion.OnlyOnEnd)
            {
                return langTexts.Where(lang => lang.TextEn.EndsWith(searchTerm));
            }

            return langTexts;

        }

        public static IQueryable<LangText> SearchLangTextsZh(this IQueryable<LangText> langTexts, LangTextParameters langTextParameters, string searchTerm)
        {
            if (langTextParameters.SearchPostion == SearchPostion.Full)
            {
                return langTexts.Where(lang => lang.TextZh.Contains(searchTerm));
            }

            if (langTextParameters.SearchPostion == SearchPostion.OnlyOnFront)
            {
                return langTexts.Where(lang => lang.TextZh.StartsWith(searchTerm));
            }

            if (langTextParameters.SearchPostion == SearchPostion.OnlyOnEnd)
            {
                return langTexts.Where(lang => lang.TextZh.EndsWith(searchTerm));
            }

            return langTexts;

        }

    }
}
