using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class LangTypeAndLangText
    {
        public int LangCategoryId { get; set; }
        [ForeignKey("LangCategory")]
        public LangTypeCategory LangType { get; set; }

        public Guid LangTextId { get; set; }
        [ForeignKey("LangTextId")]
        public LangText Lang { get; set; }
    }
}
