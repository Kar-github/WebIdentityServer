using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Application.Models
{
    public class ResetPasswordModel
    {
        [Required]
        [StringLength(50,MinimumLength =6 ,ErrorMessage ="The password must contain at least 6 character")]
        public string NewPassword { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
