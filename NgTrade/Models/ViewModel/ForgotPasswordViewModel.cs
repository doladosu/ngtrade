using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NgTrade.Models.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [DisplayName("User name")]
        public string UserName { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}