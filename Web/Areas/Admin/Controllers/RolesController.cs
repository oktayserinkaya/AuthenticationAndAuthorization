using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Areas.Admin.Models;
using Web.Models.Entities;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager) : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return View(roles);
        }

        public IActionResult CreateRole() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen Aşağıdaki Kurallara Uyunuz!...";
                return View(model);
            }

            var roleNameCheck = await _roleManager.Roles.AnyAsync(x => x.Name == model.Name);
            if (roleNameCheck)
            {
                TempData["Error"] = "Bu Rol İsmi Kullanılmaktadır!...";
                return View(model);
            }

            var resultCreate = await _roleManager.CreateAsync(new IdentityRole(model.Name));
            if (resultCreate.Succeeded)
            {
                TempData["Success"] = "Rol Başarılı Bir Şekilde Eklendi!...";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Rol Eklenemedi!...";
            return View(model);
        }

        public async Task<IActionResult> AssignToRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                TempData["Error"] = "Böyle Bir Role Bulunamadı!...";
                return RedirectToAction("Index");
            }

            var model = new AssignToRoleVM() { RoleName = roleName };
            foreach (var user in await _userManager.Users.ToListAsync())
            {
                var list = await _userManager.IsInRoleAsync(user, model.RoleName) ? model.HasRole : model.HasNotRole;
                list.Add(user);
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToRole(AssignToRoleVM model)
        {
            bool result = true;
            foreach (var userId in model.AddIds ?? [])
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var resultAddToRole = await _userManager.AddToRoleAsync(user, model.RoleName);
                    if (!resultAddToRole.Succeeded)
                        result = false;
                }
            }

            foreach (var userId in model.DeleteIds ?? [])
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var resultRemoveFromRole = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                    if (!resultRemoveFromRole.Succeeded)
                        result = false;
                }
            }

            if (!result)
            {
                TempData["Error"] = "İşlem sırasında hata oluştu!";
                return View(model);
            }

            TempData["Success"] = "Rol atamaları başarılı bir şekilde gerçekleşti!";
            return RedirectToAction("Index");
        }
    }
}
