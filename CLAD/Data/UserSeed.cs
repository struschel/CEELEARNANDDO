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

            if (userManager.FindByNameAsync("user1@test.nl").Result == null)
            {

                IdentityUser user = new IdentityUser
                {
                    UserName = "user1@test.nl",
                    Email = "user1@test.nl"
                };
                IdentityResult result = userManager.CreateAsync(user, "Test1234!").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }

            }

            if (userManager.FindByNameAsync("user2@test.nl").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "user2@test.nl",
                    Email = "user2@test.nl",
                };
                IdentityResult result = userManager.CreateAsync(user, "Test1234!").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "StandardUser").Wait();
                }


            }

            if (userManager.FindByNameAsync("user3").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "user3@test.nl",
                    Email = "user3@test.nl",
                };
                IdentityResult result = userManager.CreateAsync(user, "Test1234!").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "StandardUser").Wait();
                }
            }

            if (userManager.FindByNameAsync("user4").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "user4@test.nl",
                    Email = "user4@test.nl",
                };
                IdentityResult result = userManager.CreateAsync(user, "Test1234!").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "StandardUser").Wait();
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

