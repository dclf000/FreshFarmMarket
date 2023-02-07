using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket.Models
{
    public class Login
    {
        [Required, RegularExpression(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$", ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{12,}$", ErrorMessage = "Password requires minimum 12 characters,\r\nUse combination of lower-case, upper-case, Numbers and special characters")]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }

        public string? Token { set; get; }
        public bool RememberMe { get; set; } = false;
    }
}
