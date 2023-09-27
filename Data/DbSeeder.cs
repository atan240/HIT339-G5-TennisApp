using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TennisApp2.Constants;

namespace TennisApp2.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();

            // Add roles to the database
            await roleMgr.CreateAsync(new IdentityRole(Roles.Member.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.Coach.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));


            // Create admin user
            var admin = new IdentityUser
            {
                UserName = "admin1@gmail.com",
                Email = "admin1@gmail.com",
                EmailConfirmed = true
            };

            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb is null)
            {
                await userMgr.CreateAsync(admin, "Admin@123");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
            }

            // Create members
            var regularMember = new[]
            {
                new IdentityUser
                {
                    UserName = "abc@abc.com",
                    Email = "abc@abc.com",
                    EmailConfirmed = true
                },
                new IdentityUser
                {
                    UserName = "def@abc.com",
                    Email = "def@abc.com",
                    EmailConfirmed = true
                },
                new IdentityUser
                {
                    UserName = "ghi@abc.com",
                    Email = "ghi@abc.com",
                    EmailConfirmed = true
                },
            };
            foreach (var member in regularMember)
            {
                var existingMember = await userMgr.FindByEmailAsync(member.Email);
                if (existingMember is null)
                {
                    await userMgr.CreateAsync(member, "Alpha-50");
                    await userMgr.AddToRoleAsync(member, Roles.Member.ToString());
                }
            }

            // Create coach
            var regularCoach = new[]
            {
                new IdentityUser
                {
                    UserName = "jkl@abc.com",
                    Email = "jkl@abc.com",
                    EmailConfirmed = true
                },
                new IdentityUser
                {
                    UserName = "mno@abc.com",
                    Email = "mno@abc.com",
                    EmailConfirmed = true
                },
                new IdentityUser
                {
                    UserName = "pqr@abc.com",
                    Email = "pqr@abc.com",
                    EmailConfirmed = true
                },
            };
            foreach (var coach in regularCoach)
            {
                var existingCoach = await userMgr.FindByEmailAsync(coach.Email);
                if (existingCoach is null)
                {
                    await userMgr.CreateAsync(coach, "Alpha-50");
                    await userMgr.AddToRoleAsync(coach, Roles.Coach.ToString());
                }
            }

        }
    }
}
