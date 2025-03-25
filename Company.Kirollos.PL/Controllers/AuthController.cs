using Company.Kirollos.DAL.Models;
using Company.Kirollos.PL.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.Kirollos.PL.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _UserManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AuthController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager)
        {
            _UserManager = userManager;
            _signInManager = signInManager;
        }

        #region SingUp

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _UserManager.FindByNameAsync(model.UserName);

                if (user == null) 
                {
                    user = await _UserManager.FindByEmailAsync(model.Email);
                    if(user is null)
                    {
                        user = new AppUser()
                        {
                            UserName = model.UserName,
                            Email = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            IsAgree = model.IsAgree,
                        };
                        var result = await _UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SingIn");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                ModelState.AddModelError("","Invalid SingUp");
            }
            return View(model);
        }
        #endregion


        #region SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _UserManager.FindByEmailAsync(model.Email);
                if (user is not null) 
                {
                    bool flag = await _UserManager.CheckPasswordAsync(user, model.Password);
                    if (flag) 
                    {
                        // Sign in using Tokin
                        var  result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RemmemberMe , false);
                        if (result.Succeeded) 
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }                   
                    }
                }
                ModelState.AddModelError("", "Invalid login !");
            }
            return View(model);
        }

        #endregion

        #region SignOut
        [HttpGet]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn) , "Auth");
        }
        #endregion
    }
}
