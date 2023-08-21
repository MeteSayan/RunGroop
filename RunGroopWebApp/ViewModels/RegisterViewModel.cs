using System.ComponentModel.DataAnnotations;

namespace RunGroopWebApp.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; } = null!; // This tells the compiler that you are aware this property hasn't been set but it will be set later before it is read. If the program later tries to read the value before it's set(error situation) it will throw a NullArgumentException.
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
