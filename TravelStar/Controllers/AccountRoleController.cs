using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelStar.Entities;
using TravelStar.Site.Models.Account;
using TravelStar.Site.Seeds;

namespace TravelStar.Site.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AccountRoleController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountRoleController(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string userId)
        {
            var accountRoles = new List<AccountRolesViewModel>();
            var user = await _userManager.FindByIdAsync(userId);

            foreach (var role in _roleManager.Roles)
            {
                var accountRolesViewModel = new AccountRolesViewModel()
                {
                    RoleName = role.Name

                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    accountRolesViewModel.Selected = true;
                }
                else
                {
                    accountRolesViewModel.Selected = false;
                }

                accountRoles.Add(accountRolesViewModel);
            }

            var model = new ManageAccountRolesViewModel()
            {
                UserId = user.Id,
                Email = user.Email,
                AccountRoles = accountRoles
            };
            return View(model);
        }

        public async Task<IActionResult> Update(string userId, ManageAccountRolesViewModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var roles = await _userManager.GetRolesAsync(user);
                var result = await _userManager.RemoveFromRolesAsync(user, roles);
                result = await _userManager.AddToRolesAsync(user, model.AccountRoles.Where(x => x.Selected).Select(x => x.RoleName));

                var currentUser = await _userManager.GetUserAsync(User);
                await _signInManager.RefreshSignInAsync(currentUser);
                //await DefaultAccounts.SeedSuperAdminAccountAsync(_userManager, _roleManager);

                return Json(new { status = true });
            }
            catch (Exception ex)
            {
                return Json(new { status = true });
            }
        }
    }
}
