
using System.ComponentModel.DataAnnotations;

namespace MovieTR.Models
{
    public class Users
    {
        [Required]
        [Key]
        
    public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email Field is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter a Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string ?Address { get; set; }
        public int PhoneNumber  { get; set; }
        [Range(18, 500, ErrorMessage = "Users below age 18 not allowed")]
        public int Age  { get; set; }
        public int Role { get; set; }
        

    }
}
