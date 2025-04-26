using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Models.AccountViewModels;
using Web.Models.Entities;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Web.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IPasswordHasher<AppUser> passwordHasher) : Controller
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher = passwordHasher;

        #region Register
        public IActionResult Register() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen Aşağıdaki Kurallara Uyunuz!...";
                return View(model);
            }

            var emailResult = await _userManager.Users.AnyAsync(x => x.Status != Status.Passive && x.Email == model.Email);

            if (emailResult)
            {
                TempData["Error"] = "Bu E-Posta Kullanılmaktadır!...";
                return View(model);
            }

            var user = new AppUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Birthdate = model.Birthdate,
                Email = model.Email,
                UserName = model.UserName
            };

            IdentityResult resultCreate = await _userManager.CreateAsync(user, model.Password);
            if (resultCreate.Succeeded)
            {
                TempData["Success"] = "Kaydınız Yapılmıştır. Giriş Yapabilirsiniz.";
                return RedirectToAction("Login");
            }
            TempData["Error"] = string.Join(",\n", resultCreate.Errors.Select(x => x.Description));
            return View(model);
        }
        #endregion

        #region Login
        public IActionResult Login(string? returnUrl)
        {
            var model = new LoginVM { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen Aşağıdaki Kurallara Uyunuz!...";
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                TempData["Error"] = "Bu Kullanıcı Bulunamadı";
                return View(model);
            }

            SignInResult resultSignIn = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (resultSignIn.Succeeded)
            {
                TempData["Success"] = $"{user.FirstName} {user.LastName} Hoşgeldiniz!...";
                return Redirect(model.ReturnUrl ?? "/Home/Index");
            }

            TempData["Error"] = "Kullanıcı Adı veya Şifre Yanlış!...";
            return View(model);
        }
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["Success"] = "Çıkış Yapıldı!...";
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
