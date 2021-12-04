using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.Services
{
    public class LangtextClientDbContext : DbContext
    {
        public LangtextClientDbContext(DbContextOptions<LangtextClientDbContext> options) : base(options)
        {

        }

        public DbSet<LangTextClient> Langtexts { get; set; }
        public DbSet<LangTextRevNumber> LangtextRevNumber { get; set; }
        public DbSet<UserInClient> Users { get; set; }
        public DbSet<GameVersion> GameVersion { get; set; }
        public DbSet<LangTypeCatalog> LangIdType { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite(@"Data Source=Data/LangData_v4.db");
        //}
    }
}
