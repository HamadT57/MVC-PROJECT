using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MovieTR.ViewModels
{
    public class ChangePassword
    {
        [Required]
        [Key]
        //[Remote("ChangePassword", "MovieT", ErrorMessage = "Please Recheck your Old Password")]
        public string OldPassword { get; set; }
        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }
        [Compare( "NewPassword")]
        public string ConfirmPassword { get; set;}
    }
}
