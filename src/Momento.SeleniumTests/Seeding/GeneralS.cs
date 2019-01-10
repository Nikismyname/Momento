using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Momento.Data;

namespace Momento.SeleniumTests.Seeding
{
    public static class GeneralS
    {
        public static void SeedRoles(MomentoDbContext context)
        {
            var roles = new IdentityRole[]
            {
                new IdentityRole
                {
                   Name = "User",
                   NormalizedName = "USER",
                },
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                },
                new IdentityRole
                {
                    Name = "Moderator",
                    NormalizedName = "MODERATOR",
                },
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }
    }
}
