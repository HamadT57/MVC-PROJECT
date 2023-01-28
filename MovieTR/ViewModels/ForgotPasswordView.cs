

using System.ComponentModel.DataAnnotations;

namespace MovieTR.ViewModels
{
    public class ForgotPasswordView
    {
        [Key]
        [Required]
       public string Email { get; set; }
    }
}
