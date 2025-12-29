using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JordanTouristGuide.Models;
using JordanTouristGuide.ViewModels;
using System.Security.Claims;

namespace JordanTouristGuide.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new AuthPageViewModel
            {
                ReturnUrl = returnUrl,
                ActiveTab = "login"
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new AuthPageViewModel
            {
                ReturnUrl = returnUrl,
                ActiveTab = "register"
            };
            return View("Login", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthPageViewModel model)
        {
            model.ActiveTab = "login";

            if (string.IsNullOrEmpty(model.Login.Email) || string.IsNullOrEmpty(model.Login.Password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Login.Email,
                model.Login.Password,
                model.Login.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AuthPageViewModel model)
        {
            model.ActiveTab = "register";

            if (string.IsNullOrEmpty(model.Register.FullName) ||
                string.IsNullOrEmpty(model.Register.Email) ||
                string.IsNullOrEmpty(model.Register.Password) ||
                string.IsNullOrEmpty(model.Register.ConfirmPassword))
            {
                ModelState.AddModelError("", "All fields are required.");
                return View("Login", model);
            }

            if (model.Register.Password != model.Register.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View("Login", model);
            }

            if (model.Register.Password.Length < 6)
            {
                ModelState.AddModelError("", "Password must be at least 6 characters.");
                return View("Login", model);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Register.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "An account with this email already exists.");
                return View("Login", model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Register.Email,
                Email = model.Register.Email,
                FullName = model.Register.FullName,
                PhoneNumber = model.Register.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Register.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: true);

                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("Login", model);
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                TempData["Error"] = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["Error"] = "Error loading external login information.";
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);
            var picture = info.Principal.FindFirstValue("picture");

            if (email == null)
            {
                TempData["Error"] = "Email not received from Google. Please try again.";
                return RedirectToAction(nameof(Login));
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = name ?? email,
                    ProfilePictureUrl = picture,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user);

                if (createResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return LocalRedirect(returnUrl);
                }

                TempData["Error"] = "Error creating user account.";
                return RedirectToAction(nameof(Login));
            }

            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: true);
            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
