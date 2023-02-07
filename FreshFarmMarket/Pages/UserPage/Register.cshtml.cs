using FreshFarmMarket.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.DataProtection;
using FreshFarmMarket.Models;

namespace FreshFarmMarket.Pages.UserPage
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public IFormFile? Upload { get; set; }

        [BindProperty]
        public User RModel { get; set; }

        [BindProperty]
        public Login LModel { get; set; }

        private UserManager<User> userManager { get; }

        private SignInManager<User> signInManager { get; }

        private IWebHostEnvironment _environment;

        private readonly IDataProtector _protector;

        private readonly IHttpContextAccessor _contxt;

        public RegisterModel(IHttpContextAccessor httpContextAccessor, IDataProtectionProvider provider, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _environment = environment;
            _contxt = httpContextAccessor;
            _protector = provider.CreateProtector("credit_card_protector");

        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }

                    var uploadsFolder = "uploads";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    var ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);


                    var protectedCreditCardNumber = _protector.Protect(RModel.CreditCard);
                    Console.WriteLine(protectedCreditCardNumber);


                    var user = new User
                    {
                        UserName = LModel.Email,
                        ImageUrl = ImageURL,
                        FullName = RModel.FullName,
                        Gender = RModel.Gender,
                        Email = LModel.Email,
                        MobilNo = RModel.MobilNo,
                        Address = RModel.Address,
                        CreditCard = protectedCreditCardNumber,
                        AboutMe = RModel.AboutMe
                    };



                    var result = await userManager.CreateAsync(user, LModel.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToPage("Login");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
            }
            return Page();
        }

    }
}
