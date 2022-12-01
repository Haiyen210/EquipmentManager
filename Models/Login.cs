using System.ComponentModel.DataAnnotations;

namespace EquipmentManager.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Tên đăng nhập không để trống")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Mật khẩu không để trống")]
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}
