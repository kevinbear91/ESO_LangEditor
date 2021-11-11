using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class TokenDto
    {
        public string AuthToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
