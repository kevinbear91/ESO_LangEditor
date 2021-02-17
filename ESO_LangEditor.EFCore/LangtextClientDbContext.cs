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

        public DbSet<LangText> Langtexts { get; set; }
        public DbSet<LangTextRevNumber> LangtextRevNumber { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite(@"Data Source=Data/LangData_v4.db");
        //}
    }
}
