using Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class LangText : LangtextBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime ReviewTimestamp { get; set; }

        ////修改权限
        //public Guid RoleId { get; set; }
        //[ForeignKey("RoleId")]
        //public Role RoleToEdit { get; set; }

        public Guid? LangtextInReivewId { get; set; }
        [ForeignKey("LangtextInReivewId")]
        public LangTextReview LangtextInReview { get; set; }
    }
}
