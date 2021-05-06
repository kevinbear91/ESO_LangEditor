using ESO_LangEditor.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorDatabaseModifier.Controller.Server
{
    public class LangServerDbContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<LangText> Langtexts { get; set; }
        //public DbSet<LangTextRevNumber> LangtextRevNumber { get; set; }
        //public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"User ID=pguser;Password=pguserpw;Host=localhost;Port=5432;Database=pgDatabaseName;Pooling=true;");
        }
    }
}
