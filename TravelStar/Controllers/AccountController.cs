using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelStar.Business.Interfaces;
using TravelStar.Entities;
using TravelStar.Site.Models.Account;

namespace TravelStar.Site.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public AccountController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
          
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var allUsersExceptCurrentUser = await _userManager.Users.Where(x => x.Id != currentUser.Id).ToListAsync();
            return View(allUsersExceptCurrentUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountList()
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            List<AppUser> allUsersExceptCurrentUser = await _userManager.Users.Where(x => x.Id != currentUser.Id).ToListAsync();
            return Json(new { data = allUsersExceptCurrentUser });
        }

        [HttpPost]
        public async Task<IActionResult> AddAccount(AppUser user)
        {
            bool status = false;
            bool isExist = true;
            try
            {
                var userExists = await _userManager.FindByEmailAsync(user.Email);
                if (userExists == null)
                {
                    user.UserName = user.Email;
                    user.CreatedDate = DateTime.Now;
                    user.EmailConfirmed = true;
                    user.PhoneNumberConfirmed = true;
                    user.IsActive = true;
                    await _userManager.CreateAsync(user, "123Pa$$word!");
                    //await _emailService.SendInfoBookingSuccessEmailAsync(user.Email, "Tín Phát", "123Pa$$word!", "https://tinphat.azurewebsites.net/");
                    //await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                    isExist = false;
                }

                status = true;
                return Json(new { status = status, isExist = isExist });
            }
            catch (Exception ex)
            {
                return Json(new { status = status, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(AppUser user)
        {
            bool status = false;
            try
            {
                var userExists = await _userManager.FindByIdAsync(user.Id);
                if (userExists != null)
                {
                    userExists.UserFullName = user.UserFullName;
                    userExists.UserName = user.Email;
                    userExists.Email = user.Email;

                    await _userManager.UpdateAsync(userExists);
                }

                status = true;
                return Json(new { status = status });
            }
            catch (Exception ex)
            {
                return Json(new { status });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            bool status = false;
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var result = await _userManager.ChangePasswordAsync(currentUser, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    status = true;
                }
                return Json(new { status = status });
            }
            catch (Exception ex)
            {
                return Json(new { status });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(AppUser model)
        {
            bool status = false;
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                var rolesForUser = await _userManager.GetRolesAsync(model);

                if (rolesForUser.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, rolesForUser);
                }

                await _userManager.DeleteAsync(user);
                status = true;
                return Json(new { status });
            }
            catch (Exception ex)
            {
                return Json(new { status });
            }
        }
    }
}
