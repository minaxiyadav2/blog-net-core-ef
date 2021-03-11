using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<IdentityUser> _signInManager;

        public AuthController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }
        
        //display page
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // allow us to capture the login form
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            var result = await _signInManager.PasswordSignInAsync(lvm.UserName, lvm.Password, false, false);
            return RedirectToAction("Index", "Panel");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
