using System.ComponentModel.DataAnnotations;

namespace Integration.BLModels
{
    public class BLUserRegister
    {
        [Required(ErrorMessage = "Username is a required field.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "First nameis a required field.")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is a required field.")]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Passwordis a required field.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirming the password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is a required field.")]
        [EmailAddress(ErrorMessage = "Invalid email.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Phone number format invalid")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Country of residence is a required field.")]
        public int CountryOfResidenceId { get; set; }
    }
}
