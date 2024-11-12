using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ABC_Retailers.Models;
using System.Threading.Tasks;

namespace ABC_Retailers.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }
    }
}



//code from: https://learn.microsoft.com/en-us/azure/storage/files/storage-files-introduction
// https://www.youtube.com/watch?v=BCzeb0IAy2k
// https://learn.microsoft.com/en-us/aspnet/mvc/overview/security/create-an-aspnet-mvc-5-web-app-with-email-confirmation-and-password-reset
// https://learn.microsoft.com/en-us/azure/storage/files/storage-files-active-directory-overview