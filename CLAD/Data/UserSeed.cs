using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLAD.Models;
using Microsoft.AspNetCore.Identity;

namespace CLAD.Data
{
    public class UserSeed
    {
        public static void SeedData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);

        }

        public static void SeedUsers(UserManager<User> userManager)
        {

            if (userManager.FindByEmailAsync("user1@test.nl").Result == null)
            {

                User user = new User
                {
                    UserName = "YEEET",
                    DisplayName = "YEEET",
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
                User user = new User
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
                User user = new User
                {
                    UserName = "Henk",
                    DisplayName = "Henk",
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
                User user = new User
                {
                    UserName = "Piet",
                    DisplayName = "Piet",
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

            if (!roleManager.RoleExistsAsync("Admiin").Result)
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

