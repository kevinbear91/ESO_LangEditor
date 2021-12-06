using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class LangTextForUpdateZhDto
    {
        public Guid Id { get; set; }

        public string TextZh { get; set; }

        public DateTime ZhLastModifyTimestamp { get; set; }

        public Guid UserId { get; set; }

    }
}
