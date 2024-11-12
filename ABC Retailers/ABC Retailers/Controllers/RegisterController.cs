using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ABC_Retailers.Models;
using System.Threading.Tasks;

namespace ABC_Retailers.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Determine role based on email domain
                    string role = model.Email.EndsWith("@admin.com") ? "Admin" : "Client";

                    // Ensure the role exists, create if not
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }

                    // Assign the role to the user
                    await _userManager.AddToRoleAsync(user, role);

                    // Sign in the user after registration
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
    }
}



//code from: https://learn.microsoft.com/en-us/azure/storage/files/storage-files-introduction
// https://www.youtube.com/watch?v=BCzeb0IAy2k
// https://learn.microsoft.com/en-us/aspnet/mvc/overview/security/create-an-aspnet-mvc-5-web-app-with-email-confirmation-and-password-reset
// https://learn.microsoft.com/en-us/azure/storage/files/storage-files-active-directory-overview