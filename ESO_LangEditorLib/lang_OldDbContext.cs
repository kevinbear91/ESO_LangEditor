using ESO_LangEditorLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESO_LangEditorLib
{
    public class lang_OldDbContext : DbContext
    {
        public DbSet<LangData_Old> langOldData { get; set; }
        public DbSet<LangOldDataTable> langOldTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
           => optionsBuilder.UseSqlite(@"Data Source=Data/CsvData.db");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LangData_Old>()
                .HasNoKey();
            modelBuilder.Entity<LangOldDataTable>()
                .HasNoKey();
        }
    }
    
}
