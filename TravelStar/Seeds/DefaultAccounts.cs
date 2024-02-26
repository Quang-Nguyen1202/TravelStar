using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TravelStar.Entities;
using TravelStar.Site.Constants;

namespace TravelStar.Site.Seeds
{
    public static class DefaultAccounts
    {
        public static async Task SeedBasicAccountAsync(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            //Seed default Account
            var defaultAccount = new AppUser
            {
                UserName = "basicaccount@gmail.com",
                Email = "basicaccount@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };

            if (userManager.Users.All(u => u.Id != defaultAccount.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultAccount.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultAccount, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultAccount, Roles.Basic.ToString());
                }
            }
        }

        public static async Task SeedSuperAdminAccountAsync(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            //Seed default Account
            var defaultAccount = new AppUser
            {
                UserName = "superadminaccount@gmail.com",
                Email = "superadminaccount@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };

            if (userManager.Users.All(u => u.Id != defaultAccount.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultAccount.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultAccount, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultAccount, Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultAccount, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultAccount, Roles.SuperAdmin.ToString());
                }
                await roleManager.SeedClaimForSuperAdmin();
            }
        }

        private async static Task SeedClaimForSuperAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("SuperAdmin");
            await roleManager.AddPermissionClaim(adminRole, new List<string>() {"ManageHotel", "Users"});
        }

        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, List<string> modules)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(modules);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }

            }
        }
    }
}
