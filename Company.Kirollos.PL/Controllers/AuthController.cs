using Company.Kirollos.DAL.Models;
using Company.Kirollos.PL.Dtos;
using Company.Kirollos.PL.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Company.Kirollos.PL.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _UserManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
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
                    if (user is null)
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
                            return RedirectToAction("SignIn");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid SingUp");
            }
            return View(model);
        }
        #endregion

        #region SignIn
        [HttpGet]
        [AllowAnonymous]
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
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RemmemberMe, false);
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
            return RedirectToAction(nameof(SignIn), "Auth");
        }
        #endregion

        #region Forget Password

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendResetUrlPassword(ForgetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _UserManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    // Generate tokin 
                    var token = await _UserManager.GeneratePasswordResetTokenAsync(user);

                    // Create URL
                    var url = Url.Action("ResetPassword", "Auth", new { email = model.Email, token }, Request.Scheme);

                    // Create Email
                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url
                    };
                    // Send Email
                    var flag = EmailSettings.SendEmail(email);
                    if (flag)
                    {
                        return RedirectToAction("CheackYourInbox");
                    }
                }
            }

            ModelState.AddModelError("", "Invalid Resetting for password");
            return View("ForgetPassword", model);
        }

        [HttpGet]
        public IActionResult CheackYourInbox()
        {
            return View();
        }


        #endregion

        #region Reset Password

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDto Password)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                if (email is null || token is null) { return BadRequest("Error in cradentials"); };

                var user = await _UserManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _UserManager.ResetPasswordAsync(user, token, Password.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("SignIn");
                    }
                }

                ModelState.AddModelError("", "Invalid Operation");
            }

            return View();
        }
        #endregion

        #region Google SignIn
        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(prop , GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

            if (!result.Succeeded)
                return RedirectToAction("SignIn", "Auth");

            var externalUser = result.Principal;

            var email = externalUser.FindFirstValue(ClaimTypes.Email);

            var givenName = externalUser.FindFirstValue(ClaimTypes.GivenName);
            var surname = externalUser.FindFirstValue(ClaimTypes.Surname);
            var name = externalUser.FindFirstValue(ClaimTypes.Name);

            var user = await _UserManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = givenName ?? name?.Split(' ').FirstOrDefault() ?? "GoogleUser",
                    LastName = surname ?? (name != null && name.Split(' ').Length > 1 ? name.Split(' ').Last() : "User")
                };

                var createResult = await _UserManager.CreateAsync(user);

                if (!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("SignIn");
                }
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Facebook SignIn
        public IActionResult FacebookLogin()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("FacebookResponse")
            };
            return Challenge(prop, FacebookDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (!result.Succeeded)
                return RedirectToAction("SignIn", "Auth");

            var externalUser = result.Principal;
            var email = externalUser.FindFirstValue(ClaimTypes.Email);
            var givenName = externalUser.FindFirstValue(ClaimTypes.GivenName);
            var surname = externalUser.FindFirstValue(ClaimTypes.Surname);
            var name = externalUser.FindFirstValue(ClaimTypes.Name);

            var user = await _UserManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = givenName ?? name?.Split(' ').FirstOrDefault() ?? "FacebookUser",
                    LastName = surname ?? (name != null && name.Split(' ').Length > 1 ? name.Split(' ').Last() : "User")
                };

                var createResult = await _UserManager.CreateAsync(user);

                if (!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("SignIn");
                }
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion
    }
}
