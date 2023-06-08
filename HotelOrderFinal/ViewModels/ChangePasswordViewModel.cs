using System.ComponentModel.DataAnnotations;

namespace HotelOrderFinal.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "請填寫舊密碼")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "請填寫新密碼")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "請填寫再次填寫新密碼")]
        public string ReNewPassword { get; set; }
    }
}
