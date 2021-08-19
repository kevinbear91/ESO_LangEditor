using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ESO_LangEditor.EFCore.TestData
{
    public static class ModelBuilderExtension
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {

            
            //modelBuilder.Entity<LangTextRevNumber>().HasData(
            //    new LangTextRevNumber 
            //    { 
            //        Id = 1, 
            //        LangTextRev = 1 
            //    });

            //modelBuilder.Entity<Role>().HasData(
            //    new Role
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "InitUser",
            //        NormalizedName = "INITUSER",
            //    },
            //    new Role
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Editor",
            //        NormalizedName = "EDITOR",
            //    },
            //    new Role
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Reviewer",
            //        NormalizedName = "REVIEWER",
            //    },
            //    new Role
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Admin",
            //        NormalizedName = "ADMIN",
            //    },
            //    new Role
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Creater",
            //        NormalizedName = "CREATER",
            //    });


            //modelBuilder.Entity<User>().HasData(
            //    new User
            //    {
            //        Id = new Guid("8475B578-80F4-4ED0-AE41-C32A45D6D9E6"),
            //        UserName = "Bevisbear",
            //    });

        }
    }
}
