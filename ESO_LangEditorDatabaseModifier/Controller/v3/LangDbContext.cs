using ESO_LangEditorDatabaseModifier.Model.v3;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorDatabaseModifier.Controller.v3
{
    public class LangDbContextV3 : DbContext 
    {
        public DbSet<LangTextDto_v3> LangData { get; set; }
        //public DbSet<LuaUIData> LuaLang { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Data/LangData_v3.db");
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<LangData>();
        //    modelBuilder.Entity<List<LangData>>();
        //}
    }
}
