using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public class LangTextRevNumber
    {
        [Key]
        public int Id { get; set; }
        public int Rev { get; set; }
    }
}
