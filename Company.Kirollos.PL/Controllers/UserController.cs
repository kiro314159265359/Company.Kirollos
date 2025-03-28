using Company.Kirollos.DAL.Models;
using Company.Kirollos.PL.Dtos;
using Company.Kirollos.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Kirollos.PL.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<UserToReturnDto> users;

            if (!string.IsNullOrEmpty(SearchInput))
            {
                users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).Where(U => U.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }
            else
            {
                users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                });
            }
            return View(users);
        }

        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound(new { StatusCode = 404, Message = "Can't Find User !" });

            var dto = new UserToReturnDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            };
            return View(viewName, dto);
        }

        public async Task<IActionResult> Update(string? id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest("Invalid Id");

            return await Details(id, "Update");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] string id, UserToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest(ModelState);

                var user = await _userManager.FindByIdAsync(id);

                var oldUser = await _userManager.FindByEmailAsync(model.Email);
                if( model.Email is null && oldUser is not null)
                {
                    return BadRequest("Email Already Exist");
                }

                if (user is null) { return BadRequest("Invalid operation"); }

                user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                var flag = await _userManager.UpdateAsync(user);
                if (flag.Succeeded)
                {
                    return RedirectToAction(nameof(Index), "User");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserToReturnDto model)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (id != model.Id) return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(id);
            if (user is null) { return BadRequest("Invalid operation"); }

            var flag = await _userManager.DeleteAsync(user);
            if (flag.Succeeded)
            {
                return RedirectToAction(nameof(Index), "User");
            }
            return View(model);
        }
    }
}
