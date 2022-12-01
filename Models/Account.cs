using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace EquipmentManager.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(10)")]
        [DisplayName("Tên đăng nhập")]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(10, ErrorMessage = "Tên đăng nhập không vượt quá 10 ký tự")]
        public string Code { get; set; } = null!;
        [Column(TypeName = "nvarchar(250)")]
        [DisplayName("Họ và tên")]
        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "bit")]
        [DisplayName("Giới tính")]
        public bool? Gender { get; set; }
        [Column(TypeName = "date")]
        [DisplayName("Ngày sinh")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        public DateTime Birthday { get; set; }
        [Column(TypeName = "varchar(10)")]
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Số điện thoại không đúng định dạng")]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
       
        public string Phone { get; set; } = null!;
        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Địa chỉ email không được để trống")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không đúng định dạng")]
        public string? Email { get; set; } 
        [Column(TypeName = "nvarchar(250)")]
        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string Address { get; set; } = null!;
        [Column(TypeName = "bit")]
        [Display(Name = "Trạng thái")]
        public bool? Status { get; set; }
        [Column(TypeName = "varchar(50)")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(8, ErrorMessage = "Mật khẩu không vượt quá 8 ký tự")]
        [MinLength(8, ErrorMessage = "Mật khẩu tối đa 8 ký tự")]
        public string Password { get; set; } = null!;
        [Column(TypeName = "int")]
        [Display(Name = "Phân quyền")]
        public int RoleId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModiredDate { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public Account()
        {
            Favorites = new HashSet<Favorite>();
            Orders = new HashSet<Order>();
            Ratings = new HashSet<Rating>();
        }
        //[AcceptVerbs("GET", "POST")]
        //public IActionResult VerifyPhone(string phone)
        //{
        //    Regex _isPhone = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
        //    if (!_isPhone.IsMatch(phone))
        //    {
        //        return Json($"Số điện thoại {phone} Không đúng định dạng, VD: 0986421127 hoặc 098.421.1127");
        //    }

        //    return Json(true);
        //}

    }
}
