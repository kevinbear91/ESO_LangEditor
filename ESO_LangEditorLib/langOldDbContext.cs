using ESO_LangEditorLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESO_LangEditorLib
{
    public class langOldDbContext : DbContext
    {
        public DbSet<LangDataOld> langOldData { get; set; }
        public DbSet<LangOldDataTable> langOldTable { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
           => optionsBuilder.UseSqlite(@"Data Source=Data/CsvData.db");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LangDataOld>()
                .HasNoKey();
            modelBuilder.Entity<LangOldDataTable>()
                .HasNoKey();
        }
    }

    public class langOldLuaDbContext : DbContext
    {
        public DbSet<LuaUIDataOld> langOldLuaData { get; set; }
        public DbSet<LuaUIDataOldTable> langOldLuaTable { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
           => optionsBuilder.UseSqlite(@"Data Source=Data/UI_Str.db");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LuaUIDataOld>()
                .HasNoKey();
            modelBuilder.Entity<LuaUIDataOldTable>()
                .HasNoKey();
        }
    }

}
