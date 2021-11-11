using Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class LangTextRevised
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid LangtextID { get; set; }
        public int LangTextRevNumber { get; set; }
        public ReviewReason ReasonFor { get; set; }

    }
}
