using Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class LangTypeCategory
    {
        public int Id { get; set; }
        public string LangTypeName { get; set; }
        public LangTypeTreeCategory TreeCategory { get; set; }
        public int LangTextIdType { get; set; }
        public int TreeLevel { get; set; }
        //public Guid? LangTextId { get; set; }

    }
}
