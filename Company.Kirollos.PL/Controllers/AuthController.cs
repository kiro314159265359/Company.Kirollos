using Company.Kirollos.DAL.Models;
using Company.Kirollos.PL.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.Kirollos.PL.Controllers
{
    public class AuthController : Controller
    {
        private UserManager<AppUser> _UserManager { get; }
        public AuthController(UserManager<AppUser> userManager)
        {
            _UserManager = userManager;
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

        #endregion

        #region SignOut

        #endregion
    }
}
