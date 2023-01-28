
using System.ComponentModel.DataAnnotations;

namespace MovieTR.Models
{
    public class SignIn
    {

        [EmailAddress]
        [Key]
        [Required(ErrorMessage = "Email Field is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your Password"), DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
