using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESO_LangEditor.Core.Entities
{
    public class GameTextIdCatalog
    {
        [Key]
        public int IdType { get; set; }
        public string IdTypeZH { get; set; }
    }
}
