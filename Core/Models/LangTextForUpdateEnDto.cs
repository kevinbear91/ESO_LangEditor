using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class LangTextForUpdateEnDto
    {
        public Guid Id { get; set; }
        public string TextEn { get; set; }
        public int GameApiVersion { get; set; }
    }
}
