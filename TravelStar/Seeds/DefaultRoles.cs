using Microsoft.AspNetCore.Identity;
using TravelStar.Entities;
using TravelStar.Site.Constants;

namespace TravelStar.Site.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
        }
    }
}
