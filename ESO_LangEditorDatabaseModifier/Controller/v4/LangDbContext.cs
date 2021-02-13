using ESO_LangEditorDatabaseModifier.Model.v4;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorDatabaseModifier.Controller.v4
{
    public class LangDbContextV4 : DbContext
    {
        public DbSet<LangText> Langtexts { get; set; }
        public DbSet<LangTextRevNumber> LangtextRevNumber { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Data/LangData_v4.db");
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<LangData>();
        //    modelBuilder.Entity<List<LangData>>();
        //}
    }
}
