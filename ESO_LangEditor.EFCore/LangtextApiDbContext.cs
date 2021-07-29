using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.EFCore.TestData;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.EFCore
{
    public class LangtextApiDbContext : IdentityDbContext<User, Role, Guid>
    {
        public LangtextApiDbContext(DbContextOptions<LangtextApiDbContext> options) : base(options)
        {

        }

        public DbSet<LangText> Langtexts { get; set; }
        public DbSet<LangTextReview> LangtextReview { get; set; }
        public DbSet<LangTextArchive> LangtextArchive { get; set; }
        public DbSet<LangTextRevised> LangtextRevised { get; set; }
        public DbSet<LangTextRevNumber> LangtextRevNumber { get; set; }
        public DbSet<UserRegistrationCode> UserRegistrationCode { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SeedData();
        }
    }
}
