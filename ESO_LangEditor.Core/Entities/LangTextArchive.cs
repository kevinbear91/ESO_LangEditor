using ESO_LangEditor.Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESO_LangEditor.Core.Entities
{
    public class LangTextArchive : LangtextBase
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ReviewerId { get; set; }

        public DateTime ReviewTimestamp { get; set; }

        public DateTime ArchiveTimestamp { get; set; }

        public ReviewReason ArchiveReasonFor { get; set; }
    }
}
