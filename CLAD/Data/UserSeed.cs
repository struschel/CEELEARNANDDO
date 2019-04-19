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
                    UserName = "Admin",
                    DisplayName = "Admin",
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
                    DisplayName = "Sjaak",
                    Email = "user2@test.nl",
                    Description = "Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc, quis gravida magna mi a libero. Fusce vulputate eleifend sapien. Vestibulum purus quam, scelerisque ut, mollis sed, nonummy id, metus. Nullam accumsan lorem in dui. Cras ultricies mi eu turpis hendrerit fringilla. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; In ac dui quis mi consectetuer lacinia.",
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
                    Description = "Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc, quis gravida magna mi a libero. Fusce vulputate eleifend sapien. Vestibulum purus quam, scelerisque ut, mollis sed, nonummy id, metus. Nullam accumsan lorem in dui. Cras ultricies mi eu turpis hendrerit fringilla. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; In ac dui quis mi consectetuer lacinia.",

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
                    Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui.",
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

