using FreshFarmMarket.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages.UserPage
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _contxt;

        public LogoutModel(IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _contxt = httpContextAccessor;
        }
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                _signInManager.SignOutAsync();
                _contxt.HttpContext.Session.Clear();
            }
            return RedirectToPage("Index");
        }
      
        public async Task<IActionResult> OnPostDontLogoutAsync()
        {
            return RedirectToPage("Index");
        }
    }
}
