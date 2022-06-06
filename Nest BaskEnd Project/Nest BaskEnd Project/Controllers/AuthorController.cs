using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nest_BaskEnd_Project.Models;
using Nest_BaskEnd_Project.ViewModel.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nest_BaskEnd_Project.Controllers
{
    public class AuthorController : Controller
    {
        private UserManager<AppUser> _userManager { get; }
        private SignInManager<AppUser> _signIn { get; }

        public AuthorController(UserManager<AppUser> userManager,SignInManager<AppUser> signIn)
        {
            _userManager = userManager;
            _signIn = signIn;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            AppUser user = new AppUser
            {
                Name=register.FirstName,
                Surname=register.LastName,
                Email=register.Email,
                UserName=register.UserName
            };
            IdentityResult result = await _userManager.CreateAsync(user,register.Password);
            if (!ModelState.IsValid) return View();
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            await _signIn.SignInAsync(user, true);
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> SignOut()
        {
            await  _signIn.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
