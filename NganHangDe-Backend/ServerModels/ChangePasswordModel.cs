using System.ComponentModel.DataAnnotations;

namespace NganHangDe_Backend.ServerModels
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; } = null!;

        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string NewPassword { get; set; } = null!;

        [Compare(nameof(NewPassword), ErrorMessage = "New password and confirm password do not match")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
