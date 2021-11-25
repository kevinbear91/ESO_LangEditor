using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class GameVersionDto
    {
        public int GameApiVersion { get; set; }
        public string Version_EN { get; set; }
        public string Version_ZH { get; set; }
    }
}
