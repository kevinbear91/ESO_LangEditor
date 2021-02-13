using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESO_LangEditorDatabaseModifier.Model.v4
{
    public class LangText : LangtextBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        //[ForeignKey("UserId")]
        //public User User { get; set; }


        public Guid ReviewerId { get; set; }

        public DateTime ReviewTimestamp { get; set; }
    }
}
