using ESO_LangEditorDatabaseModifier.Model.v2;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorDatabaseModifier.Controller.v2
{
    public class LangDbContextV2 : DbContext 
    {
        public DbSet<LangTextDto_v2> LangData { get; set; }
        public DbSet<LuaUIData_v2> LuaLang { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Data/LangData.db");
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<LangData>();
        //    modelBuilder.Entity<List<LangData>>();
        //}
    }
}
