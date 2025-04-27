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

        #region EditUser
        public async Task<IActionResult> Edit()
        {
            if (User.Identity != null && User.Identity.Name != null)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    var model = new EditUserVM
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Birthdate = user.Birthdate,
                        Email = user.Email ?? "",
                        UserName = user.UserName ?? ""
                    };
                    return View(model);
                }
            }
            TempData["Error"] = "Kullanıcı Bulunamadı!...";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen Aşağıdaki Kurallara Uyunuz!...";
                return View(model);
            }

            var emailCheckResult = await _userManager.Users.AnyAsync(x => x.Email == model.Email && x.Id != model.Id && x.Status != Status.Passive);
            if (emailCheckResult)
            {
                TempData["Error"] = "Bu Email Kullanılmaktadır!...";
                return View(model);
            }

            var userNameCheckResult = await _userManager.Users.AnyAsync(x => x.UserName == model.UserName && x.Id != model.Id && x.Status != Status.Passive);
            if (emailCheckResult)
            {
                TempData["Error"] = "Bu Kullanıcı Adı Kullanılmaktadır!...";
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı Bulunamadı!...";
                return View(model);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Birthdate = model.Birthdate;
            user.Email = model.Email;
            user.UserName = model.UserName;

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["Success"] = "Kullanıcı Bilgileri Başarıyla Güncellendi!...";
                return View(model);
            }
            TempData["Error"] = "Kullanıcı Bilgileri Güncellenemedi!...";
            return View(model);
        }
        #endregion

        public IActionResult AccessDenied()
        {
            return View();
        }

        //Şifre değiştirme bölümünü siz yapın
    }
}
