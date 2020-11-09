using ESO_LangEditorModels;
using Microsoft.EntityFrameworkCore;

namespace ESO_LangEditorGUI.Services
{
    public class LangDbContext : DbContext
    {
        public DbSet<LangTextDto> LangData { get; set; }
        //public DbSet<LuaUIData> LuaLang { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
           => optionsBuilder.UseSqlite(@"Data Source=Data/LangData_v3.db");

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<LangData>();
        //    modelBuilder.Entity<List<LangData>>();
        //}
    }
}
