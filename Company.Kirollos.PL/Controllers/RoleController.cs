using Company.Kirollos.DAL.Models;
using Company.Kirollos.PL.Dtos;
using Company.Kirollos.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Company.Kirollos.PL.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleToReturnDto> roles;

            if (!string.IsNullOrEmpty(SearchInput))
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name,
                }).Where(U => U.Name.ToLower().Contains(SearchInput.ToLower()));
            }
            else
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name,
                });
            }
            return View(roles);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(RoleToReturnDto model)
        {
            if (model is null) return BadRequest();
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(model.Name);
                if (role != null) { return BadRequest("Role Already Exists"); }
                ;

                role = new IdentityRole()
                {
                    Name = model.Name,
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound(new { StatusCode = 404, Message = "Can't Find Role !" });

            var dto = new RoleToReturnDto()
            {
                Id = role.Id,
                Name = role.Name,
            };
            return View(viewName, dto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string? id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest("Invalid Id");

            return await Details(id, "Update");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest(ModelState);

                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) { return BadRequest("Invalid operation"); }

                var oldRole = await _roleManager.FindByNameAsync(model.Name);
                if (oldRole is not null) { return BadRequest("Invalid operation"); }


                role.Name = model.Name;
                var flag = await _roleManager.UpdateAsync(role);
                if (flag.Succeeded)
                {
                    return RedirectToAction(nameof(Index), "Role");
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest(ModelState);

                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) { return BadRequest("Invalid operation"); }

                var flag = await _roleManager.DeleteAsync(role);
                if (flag.Succeeded)
                {
                    return RedirectToAction(nameof(Index), "Role");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) { return NotFound(); }

            ViewData["RoleId"] = role.Id;

            var usersEnrollDto = new List<UsersEnrollDto>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userEnroll = new UsersEnrollDto()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userEnroll.IsSelected = true;
                }
                else
                {
                    userEnroll.IsSelected = false;
                }

                usersEnrollDto.Add(userEnroll);
            }
            return View(usersEnrollDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UsersEnrollDto> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) { return NotFound(); }

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appuser = await _userManager.FindByIdAsync(user.UserId);
                    if (appuser is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appuser , role.Name))
                        {
                            await _userManager.AddToRoleAsync(appuser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appuser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appuser, role.Name);
                        }
                    }
                }
                return RedirectToAction("Update" , new {id = roleId});
            }
            return View(users);
        }
    }
}
