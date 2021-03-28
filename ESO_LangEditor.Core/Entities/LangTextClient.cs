using ESO_LangEditor.Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESO_LangEditor.Core.Entities
{
    public class LangTextClient : LangtextBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime ReviewTimestamp { get; set; }
    }
}
