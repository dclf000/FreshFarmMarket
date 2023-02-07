using FreshFarmMarket.ViewModels;
using FreshFarmMarket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FreshFarmMarket.Library;
using FreshFarmMarket.Models;

namespace FreshFarmMarket.Pages.UserPage
{
    public class LoginModel : PageModel
    {
		private readonly SignInManager<User> _signInManager;

		private readonly UserManager<User> _userManager;

		private readonly IHttpContextAccessor _httpContextAccessor;

		private readonly GoogleCaptcha _captchaservice;

        [BindProperty]
        public Login LModel { get; set; }

        public LoginModel(SignInManager<User> signInManager, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, GoogleCaptcha captchaservice)
		{
			_signInManager = signInManager;
			_httpContextAccessor = httpContextAccessor;
			_captchaservice = captchaservice;
			_userManager = userManager;
		}


		[HttpGet]
		[AllowAnonymous]
		public void OnGet()
		{

		}

		public async Task<IActionResult> OnPostGoogle()
		{
			return Challenge(_signInManager.ConfigureExternalAuthenticationProperties("Google", "/GoogleLogin"), "Google");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> OnPostInHouse()
		{
			if (!ModelState.IsValid)
			{
				Console.WriteLine("Model is INVALID");
				return Page();
			}

			Console.WriteLine("Signing in");
			var identityResult = await _signInManager.PasswordSignInAsync(LModel.Email, LModel.Password, LModel.RememberMe, true);
			if (identityResult.Succeeded)
			{
				Console.WriteLine("Success");


				_httpContextAccessor.HttpContext.Session.SetString(SessionVariable.UserName, LModel.Email);

				return RedirectToPage("Index");
			}
			else if (identityResult.IsLockedOut)
			{
				ModelState.AddModelError("", "The account is locked out");
				return Page();
			}

			Console.WriteLine("Failed");
			ModelState.AddModelError(nameof(LModel.Email), "Username or Password is incorrect");
			return Page();
		}
	}
}
