using System.Threading.Tasks;
using App.webui.EmailService;
using App.webui.Extensions;
using App.webui.identity;
using App.webui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using web_app.business.Abstract;

namespace App.webui.Controllers
{
    public class AccountController : Controller
    {
        private ICartService _cartService;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        private IEmailSender _sender;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender sender,ICartService cartService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _sender = sender;
            _cartService=cartService;
        }

        public IActionResult Login(string ReturnUrl = null)
        {


            return View(new LoginModel()
            {

                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı Adı ile Hesap Oluşturulmamıştır");
                return View(model);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (result.Succeeded)
            {
                await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                return Redirect("~/");

            }
            if (await _userManager.IsEmailConfirmedAsync(user) == false)
            {
                ModelState.AddModelError("", "Lütfen Email Hesabınızı Doğrulayınız.");
                return View(model);
            }

            ModelState.AddModelError("", " Hatalı Parola ");
            return View(model);

        }


        public IActionResult Register()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email

            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //await _userManager.AddToRoleAsync(user, "customer");
                //generate token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });

                //email
                await _sender.SendEmailAsync(model.Email, "Hesabınızı Onaylayınız", $"Lütfen email hesabınızı onaylamak için linke tıklayınız <a href='http://localhost:5000{url}'>Link</a>");


                return RedirectToAction("Login", "Account");


            }

            return View(model);
        }
        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Geçersiz Token",
                    Message = "Geçersiz Token",
                    AlertType = "warning"
                });



                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                { 
                    _cartService.InitializeCart(user.Id);
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Hesabınız Onaylandı",
                        Message = "Hesabınız Onaylandı",
                        AlertType = "success"
                    });


                }
            }
            TempData.Put("message", new AlertMessage()
            {
                Title = "Hesabınız onaylanmadı.",
                Message = "Hesabınız onaylanmadı.",
                AlertType = "warning"
            });

            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return View();

            }
            var user = await _userManager.FindByEmailAsync(Email);

            if (user != null)
            {

                //generate token
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var url = Url.Action("ResetPassword", "Account", new
                {
                    userId = user.Id,
                    token = token
                });

                //email
                await _sender.SendEmailAsync(Email, "Reset Password", $"Lütfen  parolanızı yenilemek için linke tıklayınız <a href='http://localhost:5000{url}'></a>");

                return View();

            }
            return View();
        }

        public IActionResult ResetPassword(string userId, string token)
        {

            if (userId == null || token == null)
            {

                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordModel()
            {
                Token = token
            };

            return View();


        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(model);


        }




    }
}