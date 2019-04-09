using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CLAD.Data
{
    public class UserSeed
    {
        public static void SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);

        }

        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {

            if (userManager.FindByNameAsync("YEEET").Result == null)
            {

                IdentityUser user = new IdentityUser
                {
                    UserName = "YEEET",
                    Email = "user1@test.nl"
                };
                IdentityResult result = userManager.CreateAsync(user, "Test123!").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }

            }

            if (userManager.FindByNameAsync("Sjaak").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "Sjaak",
                    Email = "user2@test.nl",
                };
                IdentityResult result = userManager.CreateAsync(user, "Test123!").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Consultant").Wait();
                }


            }

            if (userManager.FindByNameAsync("Henk").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "Henk",
                    Email = "user3@test.nl",
                };
                IdentityResult result = userManager.CreateAsync(user, "Test123!").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Consultant").Wait();
                }
            }

            if (userManager.FindByNameAsync("Piet").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "Piet",
                    Email = "user4@test.nl",
                };
                IdentityResult result = userManager.CreateAsync(user, "Test123!").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Consultant").Wait();
                }
            }
        }
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("StandardUser").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "StandardUser"
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Admin"
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Consultant").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Consultant"
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}

