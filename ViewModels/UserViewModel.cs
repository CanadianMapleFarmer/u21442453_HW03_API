using System.ComponentModel.DataAnnotations;

namespace Assignment3_Backend.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public string emailaddress { get; set; }
        [Required]
        public string password { get; set; }
    }
}
