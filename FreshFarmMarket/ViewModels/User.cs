using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FreshFarmMarket.ViewModels
{
    public class User: IdentityUser
    {
        [Required, MaxLength(50)]
        public string FullName { get; set; }

        //only validate for MasterCard
        [Required, RegularExpression("^5[1-5][0-9]{14}|^(222[1-9]|22[3-9]\\d|2[3-6]\\d{2}|27[0-1]\\d|2720)[0-9]{12}$",
            ErrorMessage = "This is not Valid Master Credit Card")]
        public string CreditCard { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required, RegularExpression(@"^[89][0-9]{7}$", ErrorMessage = "Not Valid Phone Number")]
        public string MobilNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        public string? ImageUrl { get; set; }

        public string? AboutMe { get; set; }
    }
}
