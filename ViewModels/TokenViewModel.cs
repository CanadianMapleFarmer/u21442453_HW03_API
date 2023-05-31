using System.ComponentModel.DataAnnotations;

namespace u21442453_HW03_API.ViewModels
{
    public class TokenViewModel
    {
        [Required]
        public string Token { get; set; }
    }
}
