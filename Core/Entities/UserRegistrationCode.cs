using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class UserRegistrationCode
    {
        [Key]
        public string Code { get; set; }
        public Guid UserRequest { get; set; }
        [ForeignKey("UserRequest")]
        public User UserForRequest { get; set; }

        public Guid? UserUsed { get; set; }
        [ForeignKey("UserUsed")]
        public User UserForUsed { get; set; }

        public DateTime RequestTimestamp { get; set; }
        public DateTime? UsedTimestamp { get; set; }
    }
}
