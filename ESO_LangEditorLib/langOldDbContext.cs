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
    
}
