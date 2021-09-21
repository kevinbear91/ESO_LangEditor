using ESO_LangEditor.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.EFCore
{
    public class LangtextClientDbContext : DbContext
    {
        public LangtextClientDbContext(DbContextOptions<LangtextClientDbContext> options) : base(options)
        {
            
        }

        public DbSet<LangTextClient> Langtexts { get; set; }
        public DbSet<LangTextRevNumber> LangtextRevNumber { get; set; }
        public DbSet<UserInClient> Users { get; set; }
        //public DbSet<LangTypeAndLangText> LangTypeAndLangTexts { get; set; }
        //public DbSet<LangTypeCategory> LangTypeCategories { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite(@"Data Source=Data/LangData_v4.db");
        //}
    }
}
